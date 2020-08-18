using System;
using System.IO;
using Ionic.Zip;
using NLog;

namespace Paclink
{
    public class FileSupport
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        public void ProcessSupportFile(string strFilename)
        {
            try
            {
                if (!File.Exists(strFilename))
                    return;
                if (!strFilename.EndsWith(".zip"))
                    return;
                using (var objZip = ZipFile.Read(strFilename))
                {
                    foreach (ZipEntry ze in objZip)
                    {
                        string strDecompressedName = ze.FileName.ToLower();
                        if (!string.IsNullOrEmpty(strDecompressedName))
                        {
                            if (strDecompressedName.EndsWith(".doc") | strDecompressedName.EndsWith(".txt") | strDecompressedName.EndsWith(".rtf"))

                            {
                                ze.Extract(Globals.SiteRootDirectory + @"Documentation\", ExtractExistingFileAction.OverwriteSilently);
                                Globals.queChannelDisplay.Enqueue("R  *** File: " + strDecompressedName + " extracted to " + Globals.SiteRootDirectory + "Documentation");
                            }
                            else if (strDecompressedName.EndsWith(".chm"))
                            {
                                ze.Extract(Globals.SiteRootDirectory + @"Help\", ExtractExistingFileAction.OverwriteSilently);
                                Globals.queChannelDisplay.Enqueue("R  *** File: " + strDecompressedName + " extracted to " + Globals.SiteRootDirectory + "Help");
                            }
                            else if (strDecompressedName.EndsWith(".exe") | strDecompressedName.EndsWith(".dll"))
                            {
                                ze.Extract(Globals.SiteRootDirectory + @"Bin\", ExtractExistingFileAction.OverwriteSilently);
                                Globals.queChannelDisplay.Enqueue("R  *** File: " + strDecompressedName + " extracted to " + Globals.SiteRootDirectory + "Bin");
                            }
                            else // All other file extensions from the zip file go to the Data directory
                            {
                                ze.Extract(Globals.SiteRootDirectory + @"Data\", ExtractExistingFileAction.OverwriteSilently);
                                Globals.queChannelDisplay.Enqueue("R  *** File: " + strDecompressedName + " extracted to " + Globals.SiteRootDirectory + "Data");
                            }
                        }
                    }
                }

                File.Delete(strFilename); // Delete support file
            }
            catch (Exception ex)
            {
                _log.Error("[ProcessSupportFile] " + ex.Message);
            }
        } // ProcessSupportFile
    }
}