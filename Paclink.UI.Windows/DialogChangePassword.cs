using Paclink.UI.Common;
using System;

namespace Paclink.UI.Windows
{
    public partial class DialogChangePassword : IChangePasswordWindow
    {
        private IChangePasswordBacking _backingObject;
        public IChangePasswordBacking BackingObject => _backingObject;

        private string strNewPassword;

        public DialogChangePassword(IChangePasswordBacking backingObject)
        {
            _backingObject = backingObject;

            InitializeComponent();
            _Label1.Name = "Label1";
            _txtOldPassword.Name = "txtOldPassword";
            _Label2.Name = "Label2";
            _txtNewPassword.Name = "txtNewPassword";
            _btnChangePassword.Name = "btnChangePassword";
            _btnCancel.Name = "btnCancel";
        }

        private void DialogChangePassword_Load(object sender, EventArgs e)
        {
            txtOldPassword.Text = _backingObject.OldPassword;
            return;
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            strNewPassword = txtNewPassword.Text.Trim();
            if (strNewPassword.Length < 6)
            {
                UserInterfaceFactory.GetUiSystem().DisplayModalError("Entry Error", "New password must be six to twelve characters long.");
                txtNewPassword.Focus();
                return;
            }
            else if (strNewPassword.Length > 12)
            {
                UserInterfaceFactory.GetUiSystem().DisplayModalError("Entry Error", "New password must be six to twelve characters long.");
                txtNewPassword.Focus();
                return;
            }

            _backingObject.NewPassword = strNewPassword;
            _backingObject.DialogResult = Paclink.UI.Common.DialogFormResult.OK;

            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _backingObject.DialogResult = Paclink.UI.Common.DialogFormResult.Cancel;

            Close();
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