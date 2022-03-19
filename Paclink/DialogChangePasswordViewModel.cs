using Paclink.UI.Common;

namespace Paclink
{
    internal class DialogChangePasswordViewModel : IChangePasswordBacking
    {
        public string SiteRootDirectory { get; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public DialogFormResult DialogResult { get; set; }

        public void CloseWindow()
        {
            // empty
        }

        public void FormClosed()
        {
            // empty
        }

        public void FormClosing()
        {
            // empty
        }

        public void FormLoaded()
        {
            // empty
        }

        public void FormLoading()
        {
            // empty
        }

        public void RefreshWindow()
        {
            // empty
        }
    }
}
