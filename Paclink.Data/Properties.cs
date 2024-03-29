﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using NLog;

namespace Paclink.Data
{
    public class Properties : IProperties
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();
        private readonly IDatabase _database;

        public Properties(IDatabase db)
        {
            _database = db;
        }

        public void DeleteGroup(string group)
        {
            var sql = $"DELETE FROM `Properties` WHERE `Group`='{group}'";
            _database.NonQuery(sql);
        }

        public void Delete(string group, string name)
        {
            var sql = $"DELETE FROM `Properties` WHERE `Group`='{group}' AND `Property`='{name}'";
            _database.NonQuery(sql);
        }

        public string Get(string group, string name, string defaultValue)
        {
            try
            {
                var sql = $"SELECT `Value` FROM `Properties` WHERE `Group`='{group}' AND `Property`='{name}'";
                string result = _database.GetSingleValue(sql);

                //  Return result if no default specified
                if (string.IsNullOrWhiteSpace(defaultValue))
                {
                    return result;
                }

                //  Return result if a value was found 
                if (!string.IsNullOrWhiteSpace(result))
                {
                    return result;
                }

                //add the default value (if property not found)
                Save(group, name, defaultValue);
                return defaultValue;
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return defaultValue;
            }
        }

        public bool Get(string group, string name, bool defaultValue)
        {
            try
            {
                return Convert.ToBoolean(Get(group, name, Convert.ToString(defaultValue)));
            }
            catch (FormatException)
            {
                return defaultValue;
            }
        }

        public int Get(string group, string name, int defaultValue)
        {
            try
            {
                return Convert.ToInt32(Get(group, name, Convert.ToString(defaultValue)));
            }
            catch (FormatException)
            {
                return defaultValue;
            }
        }

        public void Save(string group, string name, string value)
        {
            //var ts = DateTime.UtcNow.ToString("O"); //ISO8601 format
            var sql = $"REPLACE INTO `Properties` (`Timestamp`,`Group`,`Property`,`Value`) VALUES (datetime('now'),'{group}','{name}','{value}')";
            _database.NonQuery(sql);
        }

        public void Save(string group, string name, int value)
        {
            Save(group, name, Convert.ToString(value));
        }

        public void Save(string group, string name, bool value)
        {
            Save(group, name, Convert.ToString(value));
        }

        public string ToIniFileFormat()
        {
            var sb = new StringBuilder();
            //get all properties
            var ds = _database.FillDataSet("SELECT * FROM `Properties` ORDER BY `Group`,`Property`", "Properties");
            var previousSection = "no-previous-section";
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                var section = Convert.ToString(row["Group"]);
                if (!string.IsNullOrWhiteSpace(section) && !section.Equals(previousSection, StringComparison.OrdinalIgnoreCase))
                {
                    //add a section tag
                    sb.AppendLine($"[{section}]");
                    previousSection = section;
                }
                //get property and value
                var property = Convert.ToString(row["Property"]);
                var value = Convert.ToString(row["Value"]);
                sb.AppendLine($"{property}={value}");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Import settings from ini file. Existing settings are overwritten. New settings are added. 
        /// </summary>
        public void FromIniFileFormat(string iniFileContents)
        {
            if (string.IsNullOrWhiteSpace(iniFileContents)) return;

            // Get ini file lines
            string[] lines = iniFileContents.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            // Process sections and entries within sections. Empty string is the default section.
            string section = "";
            foreach (var line in lines)
            {
                if (line.StartsWith("["))
                {
                    // A new section/group
                    section = line.Replace("[", "").Replace("]", "").Trim();
                    continue;
                }
                if (line.Contains("="))
                {
                    var parts = line.Split("=");
                    Save(section, parts[0].Trim(), parts[1].Trim());
                    continue;
                }
            }

        }
    }
}
