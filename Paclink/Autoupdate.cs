using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Ionic.Zip;
using WinlinkInterop;

namespace Paclink
{
    public class Autoupdate
    {
        public Autoupdate()
        {
            dttNextAutoupdateCheck = DateTime.UtcNow.AddSeconds(intInitialAutoupdateCheck);
            // 
            // Initialize Autoupdate
            // 
            // Set up the timer.  Set the first update check to start after 2 minutes.
            // 
            tmrAutoupdate = new System.Timers.Timer();
            tmrAutoupdate.AutoReset = true;
            tmrAutoupdate.Interval = intAutoupdateTimerInteval * 1000;
            tmrAutoupdate.Start();
            // 
            // Create the log directory if required
            // 
            if (!Directory.Exists(strLogsDirectory))
            {
                Directory.CreateDirectory(strLogsDirectory);
            }
        }

        private bool blnAutoupdateClosing = false;
        private bool blnDownloadComplete = false;
        private bool blnDownloadError = false;
        private bool blnDoAppReset = false;
        private bool blnAutoUpdateSuccessful = false;
        private bool blnHTTPMode = false;
        private bool blnExceptionOccurred = false;
        private bool blnProcessUpdateTestMode = false;

        // Set the application parameters here.  The values here should work for most of the winlink apps

        private string strAppProductName = Application.ProductName;
        private string strAppProductVersion = Application.ProductVersion;
        private string strAutoupdatePath = Path.Combine(Globals.SiteRootDirectory, @"Autoupdate\");
        private string strLogsDirectory = Globals.SiteRootDirectory + @"Log\";
        private Enumerations.CMSInfo objCmsInfo;
        private AutoupdateProgress objAutoupdateProgress = null;

        // 
        // Set auto update check frequency to every 24 hours; Perform the initial check after 20 seconds.
        // If an exception occurred trying to access the uptoupdate site, retry after 1 hour
        // All times are in units of seconds.
        // 
        private int intAutoupdateTimerInteval = 5;            // Autoupdate timer cycle
        private int intAutoupdateInterval = 60 * 60 * 12;     // Normally check for updates twice a day
        private int intAutoupdateErrInterval = 60 * 60;       // Retry interval if an error occurs
        private int intInitialAutoupdateCheck = 10;           // When to do initial check after starting
        private int intAutoupdateConnectionAttempts = 5;      // Number of times to try to connect to autoupdate site
        private DateTime dttNextAutoupdateCheck;
        private System.Timers.Timer _tmrAutoupdate;

        private System.Timers.Timer tmrAutoupdate
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _tmrAutoupdate;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_tmrAutoupdate != null)
                {
                    _tmrAutoupdate.Elapsed -= AutoupdateTimerEvent;
                }

                _tmrAutoupdate = value;
                if (_tmrAutoupdate != null)
                {
                    _tmrAutoupdate.Elapsed += AutoupdateTimerEvent;
                }
            }
        }

        public string strLocalPath;
        public string strUpdateFile = "";
        public bool blnDownloadFlag;
        public bool blnUpdateInProgress = false;

        public void SetAutoudateCheckInterval(int intSeconds)
        {
            // 
            // Set the number of seconds until we perform the next check for pending autoupdates.
            // 
            dttNextAutoupdateCheck = DateTime.UtcNow.AddSeconds(intSeconds);
            return;
        }

        public void Close()
        {
            tmrAutoupdate.Stop();
            blnAutoupdateClosing = true;
            if (objAutoupdateProgress is object)
            {
                objAutoupdateProgress.Close();
                objAutoupdateProgress.Dispose();
                objAutoupdateProgress = null;
            }
        }

        public byte[] GetBytes(string strText)
        {
            // 
            // Converts a text string to a byte array.
            // 
            return ASCIIEncoding.ASCII.GetBytes(strText);
        }

        private void AutoupdateTimerEvent(object s, System.Timers.ElapsedEventArgs e)
        {
            // 
            // This timer fires when it is time to check for program updates on the Winlink FTP server
            // The entire update process is driven off this timer event
            // 
            // Return quickly if we don't need to do a check now.
            // 
            if (DateTime.UtcNow < dttNextAutoupdateCheck | Globals.blnInhibitAutoupdate)
            {
                return;
            }
            // 
            // It's time to do an autoupdate check.
            // 
            tmrAutoupdate.Stop();
            SetAutoudateCheckInterval(intAutoupdateInterval);
            blnExceptionOccurred = false;
            strLocalPath = strAutoupdatePath;
            blnDownloadFlag = false;
            strUpdateFile = "";
            // 
            // Set this flag to True to use download using HTTP.  Otherwise, downloads will use FTP
            // 
            blnHTTPMode = true;
            WriteAutoupdateLog("[Autoupdate] Start Check");
            if (blnAutoupdateClosing)
            {
                // 
                // We've been asked to shut down, so leave the timer off and exit
                // 
                return;
            }
            // 
            // Clean any stale content from the Autoupdate subdirectory
            // 
            CleanupAutoupdate(strLocalPath);
            // 
            // Should we do an autoupdate check
            // 
            if (!blnAutoUpdateSuccessful)
            {
                // 
                // See if there is a pending full release file.
                // 
                strUpdateFile = CheckForUpdateFile(strAppProductName, strAppProductVersion, blnHTTPMode);
                // 
                // Get the most appropriate autoupdate file.  Call returns null string if no update available
                // 
                if (!string.IsNullOrEmpty(strUpdateFile))
                {
                    // 
                    // Update is available.  Initialize a progress box.
                    // 
                    objAutoupdateProgress = new AutoupdateProgress();
                    AutoupdateStatus("Initializing update");
                    objAutoupdateProgress.Show();
                    // 
                    // Initiate a thread to do the update.
                    // 
                    Globals.thrUpdate = new Thread(PerformUpdate);
                    blnUpdateInProgress = true;
                    Globals.thrUpdate.Start();
                    // 
                    // Wait for autoupdate to finish.
                    // 
                    while (blnUpdateInProgress)
                    {
                        Thread.Sleep(50);
                        Application.DoEvents();
                    }
                }
            }
            // 
            // See if the autoupdate was successful.
            // 
            if (blnAutoUpdateSuccessful)
            {
                // 
                // Autoupdate worked.
                // 
                AutoupdateStatus("Autoupdate successful");
                if (Globals.blnAutoupdateForce)
                {
                    // 
                    // Update worked and we are in test mode, so clear the force flag from the ini file
                    // 
                    ClearTestAutoupdateINI(Path.Combine(Globals.SiteBinDirectory, strAppProductName) + ".ini");
                    Globals.blnAutoupdateForce = false;
                }
            }
            // 
            // Remove all files from the Autoupdate subdirectory and delete it.
            // 
            AutoupdateStatus("Removing temp files");
            CleanupAutoupdate(strLocalPath);
            WriteAutoupdateLog("[Autoupdate] Autoupdate check is finished");
            if (blnDoAppReset)
            {
                // 
                // The update was successful and the update specified an app reset.  Add whatever code is
                // required to ensure the application is idle before performing the reset
                // 
                AutoupdateStatus("Restarting Paclink");
                for (int i = 0; i <= 10; i++)
                {
                    Application.DoEvents();
                    Thread.Sleep(100);
                }

                if (objAutoupdateProgress is object)
                {
                    objAutoupdateProgress.Close();
                    objAutoupdateProgress.Dispose();
                    objAutoupdateProgress = null;
                    Thread.Sleep(200);
                }
                // Set flag to tell Main to do an application restart.
                Globals.blnAutoupdateRestart = true;
                // Application.Restart()
            }
            // 
            // Schedule the next autoupdate check.
            // 
            if (!blnAutoUpdateSuccessful & !blnAutoupdateClosing)
            {
                // 
                // Restart the update timer if there was no update or the update failed.
                // 
                if (blnExceptionOccurred)
                {
                    // 
                    // Error accessing autoupdate site, try again in 1 hour.
                    // 
                    SetAutoudateCheckInterval(intAutoupdateErrInterval);
                }
                else
                {
                    // 
                    // Normal rescan
                    // 
                    SetAutoudateCheckInterval(intAutoupdateInterval);
                }
                // Restart the timer that calls this routine
                tmrAutoupdate.Start();
            }

            return;
        }

        private void PerformUpdate()
        {
            // 
            // This procedure runs in a thread to perform the autoupdate.
            // blnUpdateInProgress is set False when the process completes.
            // 
            try
            {
                // 
                // We have an update, so create the Autoupdate directory to deposit the file in
                // 
                if (!Directory.Exists(strLocalPath))
                {
                    Directory.CreateDirectory(strLocalPath);
                }
                // 
                // Download the file from the Winlink server
                // 
                AutoupdateStatus("Downloading update files");
                if (blnHTTPMode)
                {
                    blnDownloadFlag = DownloadFileHTTP(blnProcessUpdateTestMode, strUpdateFile, strLocalPath);
                }
                else
                {
                    // blnDownloadFlag = DownloadFileFTP(strRemotePath, strUpdateFile, strLocalPath)
                }

                if (blnDownloadFlag)
                {
                    // 
                    // File was downloaded, so let's process.
                    // Unzip the archive to the Autoupdate directory
                    // 
                    if (!blnAutoupdateClosing)
                    {
                        AutoupdateStatus("Decompressing update");
                        UnzipFile(Path.Combine(strLocalPath, strUpdateFile));
                    }
                    // 
                    // Update the target files on the machine
                    // 
                    if (!blnAutoupdateClosing)
                    {
                        AutoupdateStatus("Updating files");
                        blnAutoUpdateSuccessful = UpdateTargetFiles(Globals.SiteRootDirectory, strLocalPath);
                    }
                }
            }
            catch (Exception ex)
            {
                WriteAutoupdateLog("[Autoupdate] EXCEPTION: " + ex.Message);
                AutoupdateStatus("Error: See log file");
            }
            // 
            // Finished the update.
            // 
            Globals.thrUpdate = null;
            blnUpdateInProgress = false;
        }

        private string CheckForUpdateFile(string strAppName, string strAppVersion, bool blnUseHTTP = true)
        {
            // 
            // Check to see if an applicable update file is available.
            // Fetch the names of the files in the FTP directory and look for the most suitable update (if any)
            // 
            // First see if there is a released update that we need.
            // 
            blnProcessUpdateTestMode = false;
            string strFileName = CheckForUpdateFileWork(false, strAppName, strAppVersion, blnUseHTTP);
            if (string.IsNullOrEmpty(strFileName))
            {
                // 
                // There isn't a released version pending, see if there is a field-test version.
                // 
                if (Globals.blnAutoupdateTest)
                {
                    blnProcessUpdateTestMode = true;
                    strFileName = CheckForUpdateFileWork(true, strAppName, strAppVersion, blnUseHTTP);
                }
            }
            // 
            // See if we found a suitable update.
            // 
            if (!string.IsNullOrEmpty(strFileName))
            {
                // 
                // A new version of the app is available.
                // 
                WriteAutoupdateLog("[Autoupdate version check] Found update file: " + strFileName);
                Globals.strNewAUVersion = ExtractUpdateVersion(strFileName);
                /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */            // 
                                                                                                                                                                 // Always reset the app if the user gives confirmation of the update
                                                                                                                                                                 // 
                WriteAutoupdateLog("[Autoupdate] Updating to version: " + strFileName);
                blnDoAppReset = true;
            }
            else
            {
                WriteAutoupdateLog("[Autoupdate] No appropriate update found");
            }

            return strFileName;
        }

        private string CheckForUpdateFileWork(bool blnTestMode, string strAppName, string strAppVersion, bool blnUseHTTP = true)
        {
            // 
            // Check to see if an applicable update file is available.
            // Fetch the names of the files in the FTP directory and look for the most suitable update (if any)
            // 
            string[] strVersions;
            string strReturnFilename = "";
            string strPatchVersion = "0.0.0.0";
            string strMinVersion = "0.0.0.0";
            List<string> lstFileNames = null;
            WriteAutoupdateLog("[AutoUpdate] Fetch inforation for:" + strAppName + " from " + Globals.objWL2KInterop.GetAutoupdateURL(blnTestMode));
            if (blnTestMode)
                WriteAutoupdateLog("[AutoUpdate] Autoupdate Test is set");
            if (Globals.blnAutoupdateForce)
                WriteAutoupdateLog("[AutoUpdate] Autoupdate Force is set");
            // 
            // Get the full list of update files on the server.
            // 
            lstFileNames = Globals.objWL2KInterop.GetAutoupdateFileList(blnTestMode);
            // 
            // Now we proces the files we found
            // 
            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
            WriteAutoupdateLog("Current " + strAppName + " version is " + strAppVersion);
            foreach (string sEnt in lstFileNames)
            {
                if (sEnt.ToLower().StartsWith(strAppName.ToLower()) & sEnt.ToLower().Contains(".zip"))
                {
                    // 
                    // Loop through all the possible update files and find the highest version for which the application meets
                    // the minimum version bar
                    // 
                    WriteAutoupdateLog("[Autoupdate version check] Checking update file: " + sEnt);
                    strVersions = GetAutoupdateVersionInfo(sEnt);
                    // 
                    // strVersions(0) = Required minimum current version for update.
                    // strVersions(1) = Version of this update.
                    // 
                    if (CompareVersions(strAppVersion, strVersions[0]) >= 0 & (CompareVersions(strVersions[1], strAppVersion) > 0 | Globals.blnAutoupdateForce) & CompareVersions(strVersions[1], strPatchVersion) >= 0 & CompareVersions(strVersions[0], strMinVersion) >= 0)


                    {
                        // 
                        // Select this patch file if all three of these conditions are met:
                        // 1) The current version of this application is greater than or equal to the min version required for this patch file
                        // 2) The patch file version is greater than the current version of this application
                        // 3) The patch file version is greater than or equal to the one we already found.  (We start at version 0.0.0.0)
                        // 4) The patch file min version is greater than or equal to the one we already found. (We start at version 0.0.0.0)
                        // 
                        // When we are all done, we'll return the most up-to-date patch file(if one exists) whose min version is 
                        // closest to the app version.
                        // 
                        strReturnFilename = sEnt;
                        strMinVersion = strVersions[0];
                        strPatchVersion = strVersions[1];
                    }
                }
            }
            // 
            // Return the best match, or an empty string if no match
            // 
            return strReturnFilename;
        } // End CheckForUpdateFile

        private string Parse(string strIn, string strAppName)
        {

            // 
            // Strip off comments
            // 
            strIn = StripComment(strIn);
            if (strIn.StartsWith(strAppName) & strIn.ToLower().Contains(".zip"))
            {
                // 
                // Found an autoupdate file for this app, so ad it to the list
                // 
                return strIn;
            }
            // 
            // Invalid line, so return empty string
            // 
            return "";
        } // End Parse

        private string StripComment(string strIn)
        {
            int intComment;
            intComment = strIn.IndexOf(";");
            if (intComment >= 0)
            {
                // 
                // Strip off comments
                // 
                strIn = strIn.Substring(0, intComment);
            }

            return strIn.Trim();
        } // End StripComment

        private bool DownloadFileHTTP(bool blnTestMode, string strRemoteFile, string strLocalPath)
        {
            // 
            // Initiate a download of the patch file
            // 
            string strError = Globals.objWL2KInterop.DownloadUpdateFile(blnTestMode, strRemoteFile, strLocalPath);
            if (!string.IsNullOrEmpty(strError))
            {
                WriteAutoupdateLog("Error downloading update file: " + strError);
                return false;
            }

            WriteAutoupdateLog("Download of " + strRemoteFile + " successful.");
            return true;
        } // End DownloadFileHTTP

        private bool UpdateTargetFiles(string strAppPath, string strSourcePath)
        {
            // 
            // Deploy the files in the update to the specified target location.  Information on file deploymentis stored
            // in a file called Target.dat, which should be included in the archive.
            // 
            var lstFileList = new List<FileTarget>();
            string[] strFiles = null;
            string strFile;
            string strSourceFile;
            bool blnInRecovery = false;
            bool blnFoundResetTag = false;
            int intPos = 0;
            try
            {
                // 
                // Read the update instructions
                // 
                strFiles = File.ReadAllLines(Path.Combine(strSourcePath, "Target.dat"));
            }
            catch (Exception ex)
            {
                WriteAutoupdateLog("[Autoupdate.UpdateTargetFile] EXCEPTION: " + ex.Message);
                return false;
            }

            foreach (string strLine in strFiles)
            {
                strFile = StripComment(strLine);
                if (strFile.Length == 0)
                {
                    // 
                    // Ignore blank lines
                    // 
                    continue;
                }

                if (strFile.ToLower().Contains("<restart>"))
                {
                    // 
                    // Set the app reset flag
                    // 
                    blnFoundResetTag = true;
                    WriteAutoupdateLog("[Autoupdate.UpdateTargetFile] Restart flag found " + strFile);
                    continue;
                }

                // 
                // Split the string across the '?'
                // 
                var strEntry = strFile.Split('?');
                if (strEntry.Length != 2)
                {
                    WriteAutoupdateLog("[Autoupdate.UpdateTargetFile] EXCEPTION: Bad file line: " + strFile);
                    return false;
                }

                strSourceFile = Path.Combine(strSourcePath, strEntry[0]);
                if (!File.Exists(strSourceFile))
                {
                    WriteAutoupdateLog("[Autoupdate.UpdateTargetFile] EXCEPTION: Source file not in archive: " + strSourceFile);
                    return false;
                }

                // 
                // Add the file to process to the list
                // 
                File.SetAttributes(strSourceFile, FileAttributes.Normal);
                lstFileList.Add(new FileTarget(strSourceFile, Path.Combine(strAppPath, strEntry[1]), Path.Combine(Path.Combine(strAppPath, strEntry[1]), strEntry[0])));
                WriteAutoupdateLog("[Autoupdate.UpdateTargetFile] Queuing update for: " + Path.Combine(Path.Combine(strAppPath, strEntry[1]), strEntry[0]));
            }

            // 
            // The source list was successfully built, so process it
            // 
            foreach (FileTarget objFileTarget in lstFileList)
            {
                // 
                // Delete .old files
                // 
                try
                {
                    if (File.Exists(objFileTarget.strDestFile + ".old"))
                    {
                        File.SetAttributes(objFileTarget.strDestFile + ".old", FileAttributes.Normal);
                        File.Delete(objFileTarget.strDestFile + ".old");
                    }
                }
                catch (Exception ex)
                {
                    WriteAutoupdateLog("[Autoupdate.UpdateTargetFile] EXCEPTION: " + ex.Message);
                    return false;
                }
            }

            foreach (FileTarget objFileTarget in lstFileList)
            {
                // 
                // Move current files to .old
                // 
                try
                {
                    if (!Directory.Exists(objFileTarget.strDestPath))
                    {
                        Directory.CreateDirectory(objFileTarget.strDestPath);
                    }

                    if (File.Exists(objFileTarget.strDestFile))
                    {
                        File.Move(objFileTarget.strDestFile, objFileTarget.strDestFile + ".old");
                        objFileTarget.blnOK = true;
                    }
                }
                catch (Exception ex)
                {
                    WriteAutoupdateLog("[Autoupdate.UpdateTargetFile] EXCEPTION: " + ex.Message);
                    blnInRecovery = true;
                    break;
                }
            }

            if (!blnInRecovery)
            {
                // 
                // So far so good, so continue with update
                // 
                foreach (FileTarget objFileTarget in lstFileList)
                {
                    // 
                    // Plop down the new file
                    // 
                    try
                    {
                        File.SetAttributes(objFileTarget.strSourceFile, FileAttributes.Normal);
                        File.Copy(objFileTarget.strSourceFile, objFileTarget.strDestFile);
                        objFileTarget.blnOK = true;
                        WriteAutoupdateLog("[Autoupdate.UpdateTargetFile] File successfully deployed:  " + objFileTarget.strDestFile);
                    }
                    catch (Exception ex)
                    {
                        WriteAutoupdateLog("[Autoupdate.UpdateTargetFile] EXCEPTION: " + ex.Message);
                        blnInRecovery = true;
                        break;
                    }
                }
            }

            if (blnInRecovery)
            {
                // 
                // Uh oh!  We hit a snag with the update, unwind the changes
                // 
                WriteAutoupdateLog("[Autoupdate.UpdateTargetFile] Deployment failed.  Attempting to restore original files");
                foreach (FileTarget objFileTarget in lstFileList)
                {
                    // 
                    // Restore original file
                    // 
                    if (objFileTarget.blnOK)
                    {
                        // 
                        // This file was updated, so put it back
                        // 
                        try
                        {
                            if (File.Exists(objFileTarget.strDestFile))
                            {
                                File.SetAttributes(objFileTarget.strDestFile, FileAttributes.Normal);
                                File.Delete(objFileTarget.strDestFile);
                            }

                            if (File.Exists(objFileTarget.strDestFile + ".old"))
                            {
                                File.Move(objFileTarget.strDestFile + ".old", objFileTarget.strDestFile);
                                WriteAutoupdateLog("[Autoupdate.UpdateTargetFile] File restored: " + objFileTarget.strDestFile);
                            }
                        }
                        catch (Exception ex)
                        {
                            WriteAutoupdateLog("[Autoupdate.UpdateTargetFile] Recovery attempt error - EXCEPTION: " + ex.Message);
                        }
                    }
                }
            }
            // 
            // Set the force app reset global if the update was successful and we found a reset tag in the target.dat file
            // 
            blnDoAppReset = blnFoundResetTag & !blnInRecovery;
            return !blnInRecovery;
        } // End UpdateTargetFile

        private string ExtractUpdateVersion(string strFileName)
        {
            // 
            // Extract the update version string from an update file of the form Paclink_x-x-x-x_y-y-y-y.zip
            // 
            // Remove .zip
            strFileName = strFileName.Substring(0, strFileName.Length - 4);
            // Split name and version strings
            var strTok = strFileName.Split('_');
            if (strTok.Length != 3)
                return "";
            // Clean up the update version number
            return strTok[2].Replace('-', '.');
        }

        private int CompareVersions(string strVersionA, string strVersionB)
        {
            // 
            // Compare version number strings 
            // Returns -2 if A or B is invalid
            // Returns -1 if A < B
            // Returns  0 if A = B
            // Returns  1 if A > B
            // 
            var strVA = strVersionA.Split('.');
            var strVB = strVersionB.Split('.');
            int intIndex;
            if (strVA.Length != 4 | strVB.Length != 4)
            {
                return 2;
            }

            try
            {
                var loopTo = strVA.Length - 1;
                for (intIndex = 0; intIndex <= loopTo; intIndex++)
                {
                    if (Convert.ToInt32(strVA[intIndex]) < Convert.ToInt32(strVB[intIndex]))
                    {
                        return -1;
                    }

                    if (Convert.ToInt32(strVA[intIndex]) > Convert.ToInt32(strVB[intIndex]))
                    {
                        return 1;
                    }
                }
            }
            catch
            {
                return 2;
            }
            // 
            // Versions are equal
            // 
            return 0;
        } // End CompareVersions

        private string TimestampEx()
        {
            // 
            // Returns the current time/date formatted
            // 
            return DateTime.UtcNow.ToString(@"yyyy/MM/dd HH:mm:ss");
        }

        private string[] GetAutoupdateVersionInfo(string strFile)
        {
            // 
            // Extract the version number from the Autoupdate zip filename
            // format of filename is Product_w-x-y-z.zip
            // return value would be w.x.y.z
            // 
            // Returned Versions(0) = Required minimum current version for update.
            // Returned Versions(1) = Version of this update.
            // 
            // 
            int intAltURL = strFile.IndexOf("|");
            if (intAltURL > 0)
            {
                strFile = strFile.Substring(0, intAltURL);
            }

            string strVersion = Path.GetFileNameWithoutExtension(strFile);
            var strRetVersion = new string[2] { "", "0.0.0.0" };
            var strTmp = strVersion.Split('_');
            if (strTmp.Length == 3)
            {
                strRetVersion[0] = strTmp[1].Trim().Replace("-", ".");
                strRetVersion[1] = strTmp[2].Trim().Replace("-", ".");
            }

            return strRetVersion;
        } // End GetAutoupdateVersionInfo

        private void UnzipFile(string strFile)
        {
            // 
            // Open the zip archive and extract all files to the target directory
            // 
            var zFile = new ZipFile(strFile);
            zFile.FlattenFoldersOnExtract = true;
            zFile.ExtractAll(Path.GetDirectoryName(strFile), ExtractExistingFileAction.OverwriteSilently);
            zFile.Dispose();
        } // End UnzipAutoupdateFile

        private void WriteAutoupdateLog(string strData)
        {
            // 
            // Writes an entry to the autoupdate log file
            // 
            try
            {
                // Dim strLogPath As String = strExecutionDirectory & "\Log"
                string strLogPath = strLogsDirectory;
                strLogPath += @"\" + strAppProductName + " Autoupdate";
                strLogPath += " " + DateTime.UtcNow.ToString("yyyyMMdd") + ".log";
                File.AppendAllText(strLogPath, TimestampEx() + "  " + strData + Globals.CRLF);
            }
            catch
            {
            }
        } // End WriteAutoupdateLog

        private void ClearTestAutoupdateINI(string strIniFilePath)
        {
            // 
            // Remove the 'Force Autoupdate' key from the ini file
            // 
            Globals.objINIFile.DeleteKey("Main", "Force Autoupdate");
            return;
        } // End ClearTestAutoupdateINI

        public void CleanupAutoupdate(string strPath)
        {
            // 
            // Clean up the autoupdate directory
            // 
            string[] fso;
            if (Directory.Exists(strPath))
            {
                try
                {
                    // 
                    // Make sure no files are set to read-only.
                    // 
                    fso = Directory.GetFiles(strPath, "*.*", SearchOption.AllDirectories);
                    foreach (string fi in fso)
                    {
                        File.SetAttributes(fi, FileAttributes.Normal);
                        File.Delete(fi);
                    }

                    // 
                    // Delete it all
                    // 
                    Directory.Delete(strPath, true);
                }
                catch (Exception ex)
                {
                    WriteAutoupdateLog("[Autoupdate] Directory cleanup EXCEPTION: " + ex.Message);
                    // Error cleaning up the Autoupdate directory
                }
            }
        } // End CleanupAutoupdate

        private void AutoupdateStatus(string strStatus)
        {
            // 
            // Display text in the autoupdate progress box.
            // 
            Globals.strAutoupdateStatus = strStatus;
            return;
        }

        // Class to hold the update files we are processing

        public class FileTarget
        {
            public string strSourceFile;
            public string strDestPath;
            public string strDestFile;
            public bool blnOK = false;

            public FileTarget(string strSrcFile, string strDstPth, string strDstFile)
            {
                strSourceFile = strSrcFile;
                strDestPath = strDstPth;
                strDestFile = strDstFile;
            }
        }
    }
}