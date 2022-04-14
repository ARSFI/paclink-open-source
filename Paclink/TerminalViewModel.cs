using Paclink.UI.Common;
using System;

namespace Paclink
{
    internal class TerminalViewModel : ITerminalBacking
    {
        public int GetComCloseTime { get { return Globals.intComCloseTime; } }

        public bool EditTerminalProperties(TerminalProperties terminalProperties)
        {
            TerminalSettingsViewModel vm = new TerminalSettingsViewModel();
            vm.Properties = terminalProperties;
            UserInterfaceFactory.GetUiSystem().DisplayForm(AvailableForms.TerminalSettings, vm);
            if (vm.DialogResult == DialogFormResult.OK)
            {
                SaveTerminalProperties(vm.Properties);
                return true;
            }
            return false;
        }

        public TerminalProperties LoadTerminalProperties()
        {
            var terminalProperties = new TerminalProperties();
            terminalProperties.Port = Globals.Settings.Get("Terminal", "Port", "COM1");
            terminalProperties.BaudRate = Globals.Settings.Get("Terminal", "BaudRate", 9600);
            terminalProperties.DataBits = Globals.Settings.Get("Terminal", "DataBits", 8);
            terminalProperties.StopBits = Globals.Settings.Get("Terminal", "StopBits", 1);
            terminalProperties.Parity = Globals.Settings.Get("Terminal", "Parity", 0);
            terminalProperties.Handshake = Globals.Settings.Get("Terminal", "Handshake", 2);
            terminalProperties.WriteTimeout = Globals.Settings.Get("Terminal", "WriteTimeout", 1000);
            terminalProperties.RTSEnable = Globals.Settings.Get("Terminal", "RTSEnable", true);
            terminalProperties.DTREnable = Globals.Settings.Get("Terminal", "DTREnable", true);
            terminalProperties.LocalEcho = Globals.Settings.Get("Terminal", "LocalEcho", true);
            terminalProperties.WordWrap = Globals.Settings.Get("Terminal", "WordWrap", false);
            var bufferType = Globals.Settings.Get("Terminal", "BufferType", BufferType.Line.ToString());
            terminalProperties.BufferType = (BufferType)Enum.Parse(typeof(BufferType), bufferType);

            return terminalProperties;
        }

        public void SaveTerminalProperties(TerminalProperties terminalProperties)
        {
            Globals.Settings.Save("Terminal", "Port", terminalProperties.Port);
            Globals.Settings.Save("Terminal", "BaudRate", terminalProperties.BaudRate);
            Globals.Settings.Save("Terminal", "DataBits", terminalProperties.DataBits);
            Globals.Settings.Save("Terminal", "StopBits", terminalProperties.StopBits);
            Globals.Settings.Save("Terminal", "Parity", terminalProperties.Parity);
            Globals.Settings.Save("Terminal", "Handshake", terminalProperties.Handshake);
            Globals.Settings.Save("Terminal", "WriteTimeout", terminalProperties.WriteTimeout);
            Globals.Settings.Save("Terminal", "RTSEnable", terminalProperties.RTSEnable);
            Globals.Settings.Save("Terminal", "DTREnable", terminalProperties.DTREnable);
            Globals.Settings.Save("Terminal", "LocalEcho", terminalProperties.LocalEcho);
            Globals.Settings.Save("Terminal", "WordWrap", terminalProperties.WordWrap);
            Globals.Settings.Save("Terminal", "BufferType", terminalProperties.BufferType.ToString());
        }

        public void FormLoading()
        {
            // empty
        }

        public void FormLoaded()
        {
            // empty
        }

        public void FormClosing()
        {
            // empty
        }

        public void FormClosed()
        {
            // empty
        }

        public void RefreshWindow()
        {
            // empty
        }

        public void CloseWindow()
        {
            // empty
        }

    }
}
