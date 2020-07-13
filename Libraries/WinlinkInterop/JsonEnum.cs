// 
// Do conversion of Enum strings in Json results.
// 
using System.Collections.Generic;
using System.Text;

namespace WinlinkInterop
{
    public class JsonEnum
    {
        // 
        // Data members.
        // 
        private string strItemName = "";
        private Dictionary<string, string> dicEnum;

        public JsonEnum(string strItemNameArg)
        {
            // 
            // Constructor.
            // 
            strItemName = strItemNameArg;
            dicEnum = new Dictionary<string, string>();
        }

        public void Add(string strEnumItem, string strEnumValue = "")
        {
            // 
            // Set a mapping from an e-num string to its associated numeric value.
            // 
            if (string.IsNullOrEmpty(strEnumValue))
            {
                dicEnum.Add(strEnumItem, (dicEnum.Count + 1).ToString());
            }
            else
            {
                dicEnum.Add(strEnumItem, strEnumValue);
            }

            return;
        }

        public string Convert(string strInput)
        {
            // 
            // Convert enum strings to values.  Return the result with integer replacements.
            // 
            var sbOutput = new StringBuilder();
            bool blnFirst = true;
            var aryItems = strInput.Split(',', '"');
            foreach (string strItem in aryItems)
            {
                var item = strItem;
                var aryEntry = strItem.Split(':');
                if (string.IsNullOrEmpty(strItemName) || aryEntry[0].Contains("\"" + strItemName + "\""))
                {
                    foreach (KeyValuePair<string, string> kvp in dicEnum)
                        item = item.Replace("\"" + kvp.Key + "\"", "\"" + kvp.Value + "\"");
                }

                if (blnFirst)
                {
                    sbOutput.Append(item);
                    blnFirst = false;
                }
                else
                {
                    sbOutput.Append("," + item);
                }
            }

            return sbOutput.ToString();
        }
    }
}