using Paclink.UI.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Paclink.UI.Windows
{
    public class WindowsUiPlatform : IUiPlatform
    {
        public void DisplayForm(AvailableForms form, IFormBacking backingObject)
        {
            Form window = null;

            switch(form)
            {
                case AvailableForms.MainWindow:
                    window = new Main((IMainFormBacking)backingObject);
                    break;
                default:
                    throw new ArgumentException(string.Format("Invalid form: {0}", form));
            }

            window.Show();
        }

        public void DisplayModalError(string message, string title)
        {
            MessageBox.Show(title, message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public void DisplayMainForm(IMainFormBacking backingObject)
        {
            Application.Run(new Main(backingObject));
        }

        public void RunUiEventLoop()
        {
            Application.Run();
        }
    }
}
