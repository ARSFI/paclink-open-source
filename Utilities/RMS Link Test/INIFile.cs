using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace RMS_Link_Test
{
    public class INIFile
    {
        private static object objINIFileLock = new object();
        private string strFilePath;
        private Dictionary<string, Dictionary<string, string>> dicSections;

        public void DeleteKey(string strSection, string strKey)
        {
            if (dicSections.ContainsKey(strSection) == false)
                return;
            if (dicSections[strSection].ContainsKey(strKey) == false)
                return;
            var dicRecord = dicSections[strSection];
            dicRecord.Remove(strKey);
            Flush();
        }

        public void Load()
        {
            if (My.MyProject.Computer.FileSystem.FileExists(strFilePath))
            {
                string strContent = My.MyProject.Computer.FileSystem.ReadAllText(strFilePath);
                string strCurrentSection = "";
                var objStringReader = new StringReader(strContent);
                dicSections.Clear();
                do
                {
                    string strLine = "EOF";
                    try
                    {
                        strLine = objStringReader.ReadLine().Trim();
                    }
                    catch
                    {
                        break;
                    }

                    if (strLine == null || strLine == "EOF")
                        break;
                    if (strLine.StartsWith("[") & strLine.EndsWith("]"))
                    {
                        strLine = strLine.Replace("[", "").Replace("]", "");
                        dicSections.Add(strLine, new Dictionary<string, string>());
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
                                dicSections[strCurrentSection].Add(strTokens[0].Trim(), strTokens[1].Trim());
                            }
                        }
                    }
                }
                while (true);
            }
        }

        public void Flush()
        {
            lock (objINIFileLock)
            {
                try
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

                    My.MyProject.Computer.FileSystem.WriteAllText(strFilePath, sbdContent.ToString(), false);
                }
                catch
                {
                }
            }
        }

        public bool GetBoolean(string strSection, string strKey, bool blnDefault)
        {
            bool blnResult;
            lock (objINIFileLock)
            {
                try
                {
                    blnResult = Conversions.ToBoolean(GetRecord(strSection, strKey, blnDefault.ToString()));
                }
                catch
                {
                    blnResult = blnDefault;
                }
            }

            return blnResult;
        }

        public int GetInteger(string strSection, string strKey, int intDefault)
        {
            int intResult;
            lock (objINIFileLock)
            {
                try
                {
                    intResult = Conversions.ToInteger(GetRecord(strSection, strKey, intDefault.ToString()));
                }
                catch
                {
                    intResult = intDefault;
                }
            }

            return intResult;
        }

        public string GetString(string strSection, string strKey, string strDefault)
        {
            string strResult;
            lock (objINIFileLock)
                strResult = GetRecord(strSection, strKey, strDefault);
            return strResult;
        }

        public INIFile()
        {
            if (Application.StartupPath.Contains("Source"))
            {
                Globals.strExecutionDirectory = Path.Combine(Path.GetPathRoot(My.MyProject.Computer.FileSystem.SpecialDirectories.ProgramFiles), @"RMS\RMS Link Test\");
                strFilePath = Globals.strExecutionDirectory + Application.ProductName + ".ini";
            }
            else
            {
                strFilePath = Application.StartupPath + @"\" + Application.ProductName + ".ini";
            }

            dicSections = new Dictionary<string, Dictionary<string, string>>();
            Load();
        }

        public void WriteBoolean(string strSection, string strKey, bool blnValue)
        {
            lock (objINIFileLock)
                WriteRecord(strSection, strKey, blnValue.ToString());
        }

        public void WriteInteger(string strSection, string strKey, int intValue)
        {
            lock (objINIFileLock)
                WriteRecord(strSection, strKey, intValue.ToString());
        }

        public void WriteString(string strSection, string strKey, string strValue)
        {
            lock (objINIFileLock)
                WriteRecord(strSection, strKey, strValue);
        }

        private string GetRecord(string strSection, string strKey, string strDefault)
        {
            string strValue;
            try
            {
                strValue = dicSections[strSection][strKey];
            }
            catch
            {
                return strDefault;
            }

            if (string.IsNullOrEmpty(strValue))
                return strDefault;
            return strValue;
        }

        private void WriteRecord(string strSection, string strKey, string strValue)
        {
            if (dicSections.ContainsKey(strSection) == false)
            {
                dicSections.Add(strSection, new Dictionary<string, string>());
            }

            if (dicSections[strSection].ContainsKey(strKey) == false)
            {
                dicSections[strSection].Add(strKey, strValue);
            }
            else
            {
                var dicRecord = dicSections[strSection];
                dicRecord.Remove(strKey);
                dicRecord.Add(strKey, strValue);
            }
        }
    }
}