using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Paclink.My
{
    internal static partial class MyProject
    {
        internal partial class MyForms
        {
            [EditorBrowsable(EditorBrowsableState.Never)]
            public AutoupdateProgress m_AutoupdateProgress;

            public AutoupdateProgress AutoupdateProgress
            {
                [DebuggerHidden]
                get
                {
                    m_AutoupdateProgress = MyForms.Create__Instance__(m_AutoupdateProgress);
                    return m_AutoupdateProgress;
                }

                [DebuggerHidden]
                set
                {
                    if (value == m_AutoupdateProgress)
                        return;
                    if (value is object)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_AutoupdateProgress);
                }
            }

            [EditorBrowsable(EditorBrowsableState.Never)]
            public Bearing m_Bearing;

            public Bearing Bearing
            {
                [DebuggerHidden]
                get
                {
                    m_Bearing = MyForms.Create__Instance__(m_Bearing);
                    return m_Bearing;
                }

                [DebuggerHidden]
                set
                {
                    if (value == m_Bearing)
                        return;
                    if (value is object)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_Bearing);
                }
            }

            [EditorBrowsable(EditorBrowsableState.Never)]
            public DialogAbout m_DialogAbout;

            public DialogAbout DialogAbout
            {
                [DebuggerHidden]
                get
                {
                    m_DialogAbout = MyForms.Create__Instance__(m_DialogAbout);
                    return m_DialogAbout;
                }

                [DebuggerHidden]
                set
                {
                    if (value == m_DialogAbout)
                        return;
                    if (value is object)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_DialogAbout);
                }
            }

            [EditorBrowsable(EditorBrowsableState.Never)]
            public DialogAGWEngine m_DialogAGWEngine;

            public DialogAGWEngine DialogAGWEngine
            {
                [DebuggerHidden]
                get
                {
                    m_DialogAGWEngine = MyForms.Create__Instance__(m_DialogAGWEngine);
                    return m_DialogAGWEngine;
                }

                [DebuggerHidden]
                set
                {
                    if (value == m_DialogAGWEngine)
                        return;
                    if (value is object)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_DialogAGWEngine);
                }
            }

            [EditorBrowsable(EditorBrowsableState.Never)]
            public DialogAutoupdate m_DialogAutoupdate;

            public DialogAutoupdate DialogAutoupdate
            {
                [DebuggerHidden]
                get
                {
                    m_DialogAutoupdate = MyForms.Create__Instance__(m_DialogAutoupdate);
                    return m_DialogAutoupdate;
                }

                [DebuggerHidden]
                set
                {
                    if (value == m_DialogAutoupdate)
                        return;
                    if (value is object)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_DialogAutoupdate);
                }
            }

            [EditorBrowsable(EditorBrowsableState.Never)]
            public DialogCallsignAccounts m_DialogCallsignAccounts;

            public DialogCallsignAccounts DialogCallsignAccounts
            {
                [DebuggerHidden]
                get
                {
                    m_DialogCallsignAccounts = MyForms.Create__Instance__(m_DialogCallsignAccounts);
                    return m_DialogCallsignAccounts;
                }

                [DebuggerHidden]
                set
                {
                    if (value == m_DialogCallsignAccounts)
                        return;
                    if (value is object)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_DialogCallsignAccounts);
                }
            }

            [EditorBrowsable(EditorBrowsableState.Never)]
            public DialogPacketAGWChannels m_DialogPacketAGWChannels;

            public DialogPacketAGWChannels DialogPacketAGWChannels
            {
                [DebuggerHidden]
                get
                {
                    m_DialogPacketAGWChannels = MyForms.Create__Instance__(m_DialogPacketAGWChannels);
                    return m_DialogPacketAGWChannels;
                }

                [DebuggerHidden]
                set
                {
                    if (value == m_DialogPacketAGWChannels)
                        return;
                    if (value is object)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_DialogPacketAGWChannels);
                }
            }

            [EditorBrowsable(EditorBrowsableState.Never)]
            public DialogPacketTNCChannels m_DialogPacketTNCChannels;

            public DialogPacketTNCChannels DialogPacketTNCChannels
            {
                [DebuggerHidden]
                get
                {
                    m_DialogPacketTNCChannels = MyForms.Create__Instance__(m_DialogPacketTNCChannels);
                    return m_DialogPacketTNCChannels;
                }

                [DebuggerHidden]
                set
                {
                    if (value == m_DialogPacketTNCChannels)
                        return;
                    if (value is object)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_DialogPacketTNCChannels);
                }
            }

            [EditorBrowsable(EditorBrowsableState.Never)]
            public DialogPactorTNCChannels m_DialogPactorTNCChannels;

            public DialogPactorTNCChannels DialogPactorTNCChannels
            {
                [DebuggerHidden]
                get
                {
                    m_DialogPactorTNCChannels = MyForms.Create__Instance__(m_DialogPactorTNCChannels);
                    return m_DialogPactorTNCChannels;
                }

                [DebuggerHidden]
                set
                {
                    if (value == m_DialogPactorTNCChannels)
                        return;
                    if (value is object)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_DialogPactorTNCChannels);
                }
            }

            [EditorBrowsable(EditorBrowsableState.Never)]
            public DialogPolling m_DialogPolling;

            public DialogPolling DialogPolling
            {
                [DebuggerHidden]
                get
                {
                    m_DialogPolling = MyForms.Create__Instance__(m_DialogPolling);
                    return m_DialogPolling;
                }

                [DebuggerHidden]
                set
                {
                    if (value == m_DialogPolling)
                        return;
                    if (value is object)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_DialogPolling);
                }
            }

            [EditorBrowsable(EditorBrowsableState.Never)]
            public DialogSiteProperties m_DialogSiteProperties;

            public DialogSiteProperties DialogSiteProperties
            {
                [DebuggerHidden]
                get
                {
                    m_DialogSiteProperties = MyForms.Create__Instance__(m_DialogSiteProperties);
                    return m_DialogSiteProperties;
                }

                [DebuggerHidden]
                set
                {
                    if (value == m_DialogSiteProperties)
                        return;
                    if (value is object)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_DialogSiteProperties);
                }
            }

            [EditorBrowsable(EditorBrowsableState.Never)]
            public DialogTacticalAccounts m_DialogTacticalAccounts;

            public DialogTacticalAccounts DialogTacticalAccounts
            {
                [DebuggerHidden]
                get
                {
                    m_DialogTacticalAccounts = MyForms.Create__Instance__(m_DialogTacticalAccounts);
                    return m_DialogTacticalAccounts;
                }

                [DebuggerHidden]
                set
                {
                    if (value == m_DialogTacticalAccounts)
                        return;
                    if (value is object)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_DialogTacticalAccounts);
                }
            }

            [EditorBrowsable(EditorBrowsableState.Never)]
            public DialogTelnetChannels m_DialogTelnetChannels;

            public DialogTelnetChannels DialogTelnetChannels
            {
                [DebuggerHidden]
                get
                {
                    m_DialogTelnetChannels = MyForms.Create__Instance__(m_DialogTelnetChannels);
                    return m_DialogTelnetChannels;
                }

                [DebuggerHidden]
                set
                {
                    if (value == m_DialogTelnetChannels)
                        return;
                    if (value is object)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_DialogTelnetChannels);
                }
            }

            [EditorBrowsable(EditorBrowsableState.Never)]
            public Main m_Main;

            public Main Main
            {
                [DebuggerHidden]
                get
                {
                    m_Main = MyForms.Create__Instance__(m_Main);
                    return m_Main;
                }

                [DebuggerHidden]
                set
                {
                    if (value == m_Main)
                        return;
                    if (value is object)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_Main);
                }
            }

            [EditorBrowsable(EditorBrowsableState.Never)]
            public Terminal m_Terminal;

            public Terminal Terminal
            {
                [DebuggerHidden]
                get
                {
                    m_Terminal = MyForms.Create__Instance__(m_Terminal);
                    return m_Terminal;
                }

                [DebuggerHidden]
                set
                {
                    if (value == m_Terminal)
                        return;
                    if (value is object)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_Terminal);
                }
            }

            [EditorBrowsable(EditorBrowsableState.Never)]
            public TerminalSettings m_TerminalSettings;

            public TerminalSettings TerminalSettings
            {
                [DebuggerHidden]
                get
                {
                    m_TerminalSettings = MyForms.Create__Instance__(m_TerminalSettings);
                    return m_TerminalSettings;
                }

                [DebuggerHidden]
                set
                {
                    if (value == m_TerminalSettings)
                        return;
                    if (value is object)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_TerminalSettings);
                }
            }
        }
    }
}