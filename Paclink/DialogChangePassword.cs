using System;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace Paclink
{
    public partial class DialogChangePassword
    {
        public DialogChangePassword()
        {
            InitializeComponent();
            _Label1.Name = "Label1";
            _txtOldPassword.Name = "txtOldPassword";
            _Label2.Name = "Label2";
            _txtNewPassword.Name = "txtNewPassword";
            _btnChangePassword.Name = "btnChangePassword";
            _btnCancel.Name = "btnCancel";
        }

        public DialogChangePassword(string strOldPasswordArg)
        {
            InitializeComponent();
            strOldPassword = strOldPasswordArg;
            _Label1.Name = "Label1";
            _txtOldPassword.Name = "txtOldPassword";
            _Label2.Name = "Label2";
            _txtNewPassword.Name = "txtNewPassword";
            _btnChangePassword.Name = "btnChangePassword";
            _btnCancel.Name = "btnCancel";
        }

        private string strOldPassword;
        private string strNewPassword;

        public string GetNewPassword()
        {
            // 
            // Get the value of the new password.
            // 
            return strNewPassword;
        }

        public string GetOldPassword()
        {
            // 
            // Get the value of the old password.
            // 
            return strOldPassword;
        }

        private void DialogChangePassword_Load(object sender, EventArgs e)
        {
            // 
            // This form has been loaded.
            // 
            txtOldPassword.Text = strOldPassword;
            return;
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            // 
            // Change the password.
            // 
            strOldPassword = txtOldPassword.Text.Trim();
            strNewPassword = txtNewPassword.Text.Trim();
            if (strNewPassword.Length < 3)
            {
                MessageBox.Show("New password must be at least three characters.");
                txtNewPassword.Focus();
                return;
            }
            else if (strNewPassword.Length > 12)
            {
                MessageBox.Show("New password must be twelve letters or less long.");
                txtNewPassword.Focus();
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
            return;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // 
            // Cancel the password change.
            // 
            DialogResult = DialogResult.Cancel;
            Close();
            return;
        }
    }
}