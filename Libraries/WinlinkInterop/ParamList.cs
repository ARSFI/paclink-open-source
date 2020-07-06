using System;
using System.Collections.Generic;

namespace WinlinkInterop
{
    public enum ParamType
    {
        ParTyp_String,
        ParTyp_Datetime
    }

    public class ParamEntry
    {
        // 
        // Data members.
        // 
        public string strParam;
        public string strValue;
        public DateTime dttValue;
        public ParamType enmType = ParamType.ParTyp_String;
    }

    public class ParamList
    {
        // 
        // List of parameters for JSON commands.
        // Data members.
        // 
        public List<ParamEntry> lstParams;

        public ParamList(string strCallsign = "", string strAccessCode = "")
        {
            // 
            // Constructor
            // 
            lstParams = new List<ParamEntry>();
            // 
            // See if we should add a callsign entry.
            // 
            if (!string.IsNullOrEmpty(strCallsign))
            {
                Add("Requester", strCallsign);
                Add("Callsign", strCallsign);
            }
            // 
            // See if we should add an entry for the web service access code.
            // 
            if (!string.IsNullOrEmpty(strAccessCode))
            {
                Add("WebServiceAccessCode", strAccessCode);
            }
        }

        public void Add(string strParamName, string strParamValue)
        {
            // 
            // Add a new parameter entry to the list.
            // 
            var objParam = new ParamEntry();
            objParam.strParam = strParamName;
            objParam.strValue = strParamValue;
            objParam.enmType = ParamType.ParTyp_String;
            lstParams.Add(objParam);
            return;
        }

        public void AddDatetime(string strParamName, DateTime dttParamValue)
        {
            // 
            // Add a new parameter entry to the list.
            // 
            var objParam = new ParamEntry();
            objParam.strParam = strParamName;
            objParam.dttValue = dttParamValue;
            objParam.enmType = ParamType.ParTyp_Datetime;
            lstParams.Add(objParam);
            return;
        }
    }
}