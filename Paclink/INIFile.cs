using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Paclink
{
    public class INIFile
    {
        // 
        // Native .net class to manipulate .ini files
        // 
        private static object objINIFileLock = new object();
        private static object objINIFlushLock = new object();
        private bool blnLoading = false;
        private DateTime dttLastIniBackup = DateTime.MinValue;
        private string strFilePath;
        private Dictionary<string, Dictionary<string, string>> dicSections;

        public INIFile()
        {
            // 
            // Constructor
            // 
            if (Application.StartupPath.IndexOf("Source") == -1)
            {
                Globals.SiteBinDirectory = Application.StartupPath + @"\";
            }
            else
            {
                Globals.SiteBinDirectory = Path.Combine(Path.GetPathRoot(My.MyProject.Computer.FileSystem.SpecialDirectories.ProgramFiles), @"Paclink\bin\");
            }

            strFilePath = Globals.SiteBinDirectory + Application.ProductName + ".ini";
            dicSections = new Dictionary<string, Dictionary<string, string>>();
            Load();
        }

        public void Backup(string strFile, bool blnAck)
        {
            lock (objINIFileLock)
                Flush(strFile);
            if (blnAck)
                Interaction.MsgBox("INI file " + strFilePath + " backed up to " + strFile, MsgBoxStyle.Information);
        }

        public bool Restore(string strFile)
        {
            if (Interaction.MsgBox("This will replace your current Paclink settings with the new settings contained in file:" + Constants.vbCrLf + strFile + ". Any connect Script files (.script) will not be affected." + Constants.vbCrLf + "Your existing INI file will be saved to backup file: " + strFilePath + ".bak" + Constants.vbCrLf + "Do you wish to continue?", MsgBoxStyle.YesNo) == Constants.vbNo)


                return false;
            try
            {
                if (File.Exists(strFilePath + ".bak"))
                    File.Delete(strFilePath + ".bak");
                File.Copy(strFilePath, strFilePath + ".bak");
                if ((strFile ?? "") == (strFilePath ?? ""))
                    return default;
                File.Delete(strFilePath);
                File.Copy(strFile, strFilePath);
            }
            catch
            {
                Logs.Exception("Exception in RestoreINI: " + Information.Err().Description);
                Interaction.MsgBox("Error Restoring INI file. See error log for details.", MsgBoxStyle.Critical);
                return false;
            }

            Interaction.MsgBox("File " + strFile + " successfully imported." + Constants.vbCrLf + "Paclink will now quit.  Restart to use new INI File settings.", MsgBoxStyle.Information);
            return true;
        } // Restore

        public void DeleteSection(string strSection)
        {
            // 
            // Deletes a section from an .ini file along with all associated Keys
            // 
            lock (objINIFileLock)
            {
                strSection = FindSection(strSection);
                if (strSection is object)
                {
                    dicSections.Remove(strSection);
                    Flush();
                }
            }
        } // End DeleteSection

        public void DeleteKey(string strSection, string strKey)
        {
            // 
            // Deletes a key from the desired section
            // 
            lock (objINIFileLock)
            {
                strSection = FindSection(strSection);
                if (strSection is object)
                {
                    strKey = FindKey(strSection, strKey);
                    if (strKey is object)
                    {
                        dicSections[strSection].Remove(strKey);
                        Flush();
                    }
                }
            }
        } // End DeleteKey

        public void Load()
        {
            Load(strFilePath);
        }

        public void Load(string strFile)
        {
            // 
            // Load the .ini file parameters
            // 
            lock (objINIFileLock)
            {
                blnLoading = true;
                dicSections.Clear();
                // 
                // Add a common 'Main' section at the top of the INI file
                // 
                dicSections.Add("Main", new Dictionary<string, string>());
                if (File.Exists(strFilePath))
                {
                    string[] strContent;
                    string strLine;
                    string strCurrentSection = "";
                    try
                    {
                        strContent = File.ReadAllLines(strFile);
                        // 
                        // Loop through all the lines inthe ini file
                        // 
                        for (int i = 0, loopTo = strContent.Length - 1; i <= loopTo; i++)
                        {
                            strLine = strContent[i].Trim();
                            if (strLine.StartsWith("[") & strLine.EndsWith("]") & strLine.Length > 2)
                            {
                                // 
                                // Found a section header
                                // 
                                strLine = strLine.Replace("[", "").Replace("]", "");
                                if (FindSection(strLine) == null)
                                {
                                    // 
                                    // The section doesn't exist, so create it.
                                    // 
                                    dicSections.Add(strLine, new Dictionary<string, string>());
                                }

                                strCurrentSection = strLine;
                            }
                            else
                            {
                                var strTokens = strLine.Split('=');
                                if (strTokens.Length == 2)
                                {
                                    strTokens[0] = strTokens[0].Trim();
                                    strTokens[1] = strTokens[1].Trim();
                                    if (!string.IsNullOrEmpty(strTokens[0]))
                                    {
                                        // 
                                        // Save the key.  If the key already exists, it is overwritten
                                        // 
                                        WriteRecord(strCurrentSection, strTokens[0], strTokens[1]);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logs.Exception("[INIFile.Load] " + Information.Err().Description);
                    }
                }

                blnLoading = false;
            }

            return;
        } // End Load

        public void Flush()
        {
            Flush(strFilePath);
        }

        public void Flush(string strFile)
        {
            // 
            // Flush the contents of the structure back to the file
            // 
            if (blnLoading)
                return;
            lock (objINIFlushLock)
            {
                var sbdContent = new StringBuilder();
                foreach (string strSection in dicSections.Keys)
                {
                    sbdContent.AppendLine(Constants.vbCrLf + "[" + strSection + "]");
                    foreach (string strKey in dicSections[strSection].Keys)
                    {
                        string strValue = dicSections[strSection][strKey];
                        sbdContent.AppendLine(strKey + "=" + strValue);
                    }
                }

                try
                {
                    File.WriteAllText(strFile, sbdContent.ToString());
                }
                catch (Exception ex)
                {
                    Logs.Exception("[INIFile.Flush] " + Information.Err().Description);
                }
            }

            return;
        } // ENd Flush

        // 
        // Read routines
        // 
        public bool GetBoolean(string strSection, string strKey, bool blnDefault)
        {
            bool blnResult;
            try
            {
                blnResult = Conversions.ToBoolean(GetRecord(strSection, strKey, blnDefault.ToString()));
            }
            catch
            {
                blnResult = blnDefault;
            }

            return blnResult;
        } // End GetBoolean

        public int GetInteger(string strSection, string strKey, int intDefault)
        {
            int intResult;
            try
            {
                intResult = Conversions.ToInteger(GetRecord(strSection, strKey, intDefault.ToString()));
            }
            catch
            {
                intResult = intDefault;
            }

            return intResult;
        } // End GetInteger

        public string GetString(string strSection, string strKey, string strDefault)
        {
            return GetRecord(strSection, strKey, strDefault);
        } // End GetString

        private string GetRecord(string strSection, string strKey, string strDefault)
        {
            string GetRecordRet = default;
            // 
            // Common read funtion
            // 
            GetRecordRet = strDefault;
            lock (objINIFileLock)
            {
                strSection = FindSection(strSection);
                if (strSection is object)
                {
                    strKey = FindKey(strSection, strKey);
                    if (strKey is object)
                    {
                        if (dicSections[strSection][strKey] is object)
                        {
                            if (dicSections[strSection][strKey].Length > 0)
                            {
                                GetRecordRet = dicSections[strSection][strKey];
                            }
                        }
                    }
                }
            }

            return GetRecordRet;
        } // End GetRecord

        // 
        // Write routines
        // 
        public void WriteBoolean(string strSection, string strKey, bool blnValue)
        {
            WriteRecord(strSection, strKey, blnValue.ToString());
        }  // End WriteBoolean

        public void WriteInteger(string strSection, string strKey, int intValue)
        {
            WriteRecord(strSection, strKey, intValue.ToString());
        } // End WriteInteger

        public void WriteString(string strSection, string strKey, string strValue)
        {
            WriteRecord(strSection, strKey, strValue);
        } // End WriteString

        private void WriteRecord(string strSection, string strKey, string strValue)
        {
            // 
            // Common write routine
            // 
            string strExactSection;
            string strExactKey;
            lock (objINIFileLock)
            {
                // 
                // Locate the exact section name
                // 
                strExactSection = FindSection(strSection);
                if (strExactSection == null)
                {
                    strExactSection = strSection;
                    dicSections.Add(strExactSection, new Dictionary<string, string>());
                }
                // 
                // Locate the exact key name
                // 
                strExactKey = FindKey(strExactSection, strKey);
                if (strExactKey == null)
                {
                    strExactKey = strKey;
                    dicSections[strExactSection].Add(strExactKey, strValue);
                }
                else
                {
                    var dicRecord = dicSections[strExactSection];
                    dicRecord.Remove(strExactKey);
                    dicRecord.Add(strExactKey, strValue);
                }

                Flush();
            }
        } // End WriteRecord

        // 
        // Helper routines to locate section and key names in a case-insensitive manner.
        // 
        private string FindSection(string strSection)
        {
            string FindSectionRet = default;
            // 
            // Return the key of the desired section if it exists, otherwise Null
            // 
            FindSectionRet = null;
            strSection = strSection.ToLower();
            foreach (KeyValuePair<string, Dictionary<string, string>> dicObj in dicSections)
            {
                if ((dicObj.Key.ToLower() ?? "") == (strSection ?? ""))
                {
                    FindSectionRet = dicObj.Key;
                    return FindSectionRet;
                }
            }

            return FindSectionRet;
        }

        private string FindKey(string strSection, string strKey)
        {
            string FindKeyRet = default;
            // 
            // Return the key of the desired section if it exists, otherwise Null
            // 
            FindKeyRet = null;
            strKey = strKey.ToLower();
            strSection = FindSection(strSection);
            if (strSection is object)
            {
                var dicObj = dicSections[strSection];
                foreach (KeyValuePair<string, string> keyObj in dicObj)
                {
                    if ((keyObj.Key.ToLower() ?? "") == (strKey ?? ""))
                    {
                        FindKeyRet = keyObj.Key;
                        return FindKeyRet;
                    }
                }
            }

            return FindKeyRet;
        }

        public void CheckBackupIni()
        {
            // 
            // Check to see if we need to backup the .ini file.
            // If we've done the check recently, don't do it again.
            // 
            if (blnLoading)
                return;
            double dblBackupHours = (DateTime.UtcNow - dttLastIniBackup).TotalHours;
            if (dblBackupHours < 24)
                return;
            // 
            // Time to do a check.
            // 
            lock (objINIFlushLock)
            {
                // 
                // Check the backup files and see if we need to create a new backup.
                // 
                var dttMostRecentFile = PurgeIniBackups();
                if ((DateTime.UtcNow - dttMostRecentFile).TotalHours >= 24)
                {
                    // Time to do a backup.
                    BackupIniFile();
                    dttLastIniBackup = DateTime.UtcNow;
                }
                else
                {
                    // Not time for a backup yet.
                    dttLastIniBackup = dttMostRecentFile;
                }
            }

            return;
        }

        private void BackupIniFile()
        {
            // 
            // Make a backup copy of the .ini file.
            // 
            string strIniBackupFolder = Globals.SiteBinDirectory + @"iniBackup\";
            // 
            // Make sure the folder exists.
            // 
            try
            {
                if (Directory.Exists(strIniBackupFolder) == false)
                    Directory.CreateDirectory(strIniBackupFolder);
            }
            catch
            {
                // Exceptions("[BackupIniFile] Unable to create iniBackup folder. " & Err.Description)
                return;
            }
            // 
            // Make a backup copy of the .ini file.
            // 
            string strBackupDate = Globals.FormatNetTime();
            string strSourceFile = Globals.SiteBinDirectory + Application.ProductName + ".ini";
            string strBackupName = strIniBackupFolder + Application.ProductName + "_" + strBackupDate + ".ini";
            try
            {
                FileSystem.FileCopy(strSourceFile, strBackupName);
            }
            catch (Exception ex)
            {
                // Exception("[BackupIniFile] " & Err.Description)
            }
            // 
            // Finished
            // 
            return;
        }
        // 
        // Purge old backup ini files.  Return the date of the most
        // 
        private int intBackupExpiration = 7;
        private DateTime PurgeIniBackups()           // Number of days to keep old backup files
        {
            var dttMostRecentBackup = DateTime.MinValue;
            var dttOldestToKeep = DateTime.UtcNow.AddDays((double)-intBackupExpiration);
            string strIniBackupFolder = Globals.SiteBinDirectory + @"iniBackup\";
            try
            {
                int intStart;
                string strFileDate;
                if (Directory.Exists(strIniBackupFolder))
                {
                    // Get list of files in the backup folder.
                    var strFiles = Directory.GetFiles(strIniBackupFolder);
                    foreach (string strFile in strFiles)
                    {
                        intStart = strFile.IndexOf("_");
                        if (intStart > 0)
                        {
                            // Get backup date-time from the file name and get date-time of of oldest allowed file.
                            strFileDate = strFile.Substring(intStart + 1, 14);
                            var dttFileDate = Globals.ParseNetworkDate(strFileDate);
                            if (dttFileDate > dttMostRecentBackup)
                            {
                                dttMostRecentBackup = dttFileDate;
                            }

                            string strExpire = Strings.Format(dttOldestToKeep, "yyyyMMddHHmmss");
                            // See if this file has expired.
                            if (strFileDate.CompareTo(strExpire) < 0)
                            {
                                // This file is expired.  Delete it.
                                try
                                {
                                    File.Delete(strFile);
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
            }

            return dttMostRecentBackup;
        }
    }
}