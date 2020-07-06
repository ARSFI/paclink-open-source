using System;
using System.ComponentModel;
using System.Diagnostics;

namespace RMS_Link_Test.My
{
    internal static partial class MyProject
    {
        internal partial class MyForms
        {
            [EditorBrowsable(EditorBrowsableState.Never)]
            public About m_About;

            public About About
            {
                [DebuggerHidden]
                get
                {
                    m_About = Create__Instance__(m_About);
                    return m_About;
                }

                [DebuggerHidden]
                set
                {
                    if (value == m_About)
                        return;
                    if (value is object)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_About);
                }
            }

            [EditorBrowsable(EditorBrowsableState.Never)]
            public Main m_Main;

            public Main Main
            {
                [DebuggerHidden]
                get
                {
                    m_Main = Create__Instance__(m_Main);
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
        }
    }
}