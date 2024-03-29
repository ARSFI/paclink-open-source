﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Paclink.UI.Common
{
    public static class UserInterfaceFactory
    {
        private static object _lockObject = new object();
        private static IUiPlatform _platform;

        // Set-only property for non-Windows platform to initialize itself.
        public static IUiPlatform Platform
        {
            set
            {
                _platform = value;
            }
        }

        public static IUiPlatform GetUiSystem()
        {
            lock (_lockObject)
            {
                Assembly requiredAssembly = null;
                if (_platform == null)
                {
                    if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                    {
                        // Load Paclink.UI.Windows.dll and retrieve the needed IUiPlatform object
                        // from it.
                        requiredAssembly = Assembly.LoadFrom("Paclink.UI.Windows.dll");
                    }
                    else
                    {
                        // Use Avalonia for all non-Windows platforms.
                        // TBD: eventually deprecate WinForms.
                        requiredAssembly = Assembly.LoadFrom("Paclink.UI.Avalonia.dll");
                    }

                    var type = typeof(IUiPlatform);
                    var typeList = requiredAssembly.GetTypes().Where(p => type.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract);

                    if (typeList.Count() > 1)
                    {
                        // Note: only one IUiPlatform object should exist!
                        throw new InvalidProgramException("Paclink.UI.Windows.dll has more than one IUiPlatform object!");
                    }
                    else if (typeList.Count() == 0)
                    {
                        throw new InvalidProgramException("Paclink.UI.Windows.dll does not have any IUiPlatform objects!");
                    }

                    _platform = (IUiPlatform)Activator.CreateInstance(typeList.First());
                }

                return _platform;
            }
        }
    }
}
