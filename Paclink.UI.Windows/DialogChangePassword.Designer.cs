using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Paclink.UI.Windows
{
    public partial class DialogChangePassword : Form
    {

        // Form overrides dispose to clean up the component list.
        [DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components is object)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            _Label1 = new Label();
            _txtOldPassword = new TextBox();
            _Label2 = new Label();
            _txtNewPassword = new TextBox();
            _btnChangePassword = new Button();
            _btnChangePassword.Click += new EventHandler(btnChangePassword_Click);
            _btnCancel = new Button();
            _btnCancel.Click += new EventHandler(btnCancel_Click);
            SuspendLayout();
            // 
            // Label1
            // 
            _Label1.AutoSize = true;
            _Label1.Location = new Point(25, 24);
            _Label1.Name = "_Label1";
            _Label1.Size = new Size(74, 13);
            _Label1.TabIndex = 0;
            _Label1.Text = "Old password:";
            // 
            // txtOldPassword
            // 
            _txtOldPassword.Location = new Point(100, 23);
            _txtOldPassword.Name = "_txtOldPassword";
            _txtOldPassword.Size = new Size(172, 20);
            _txtOldPassword.TabIndex = 1;
            // 
            // Label2
            // 
            _Label2.AutoSize = true;
            _Label2.Location = new Point(19, 59);
            _Label2.Name = "_Label2";
            _Label2.Size = new Size(80, 13);
            _Label2.TabIndex = 2;
            _Label2.Text = "New password:";
            // 
            // txtNewPassword
            // 
            _txtNewPassword.Location = new Point(100, 57);
            _txtNewPassword.Name = "_txtNewPassword";
            _txtNewPassword.Size = new Size(172, 20);
            _txtNewPassword.TabIndex = 3;
            // 
            // btnChangePassword
            // 
            _btnChangePassword.Location = new Point(28, 96);
            _btnChangePassword.Name = "_btnChangePassword";
            _btnChangePassword.Size = new Size(117, 24);
            _btnChangePassword.TabIndex = 4;
            _btnChangePassword.Text = "Change Password";
            _btnChangePassword.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            _btnCancel.Location = new Point(169, 96);
            _btnCancel.Name = "_btnCancel";
            _btnCancel.Size = new Size(117, 24);
            _btnCancel.TabIndex = 5;
            _btnCancel.Text = "Cancel";
            _btnCancel.UseVisualStyleBackColor = true;
            // 
            // DialogChangePassword
            // 
            AutoScaleDimensions = new SizeF(6.0F, 13.0F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(314, 133);
            Controls.Add(_btnCancel);
            Controls.Add(_btnChangePassword);
            Controls.Add(_txtNewPassword);
            Controls.Add(_Label2);
            Controls.Add(_txtOldPassword);
            Controls.Add(_Label1);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "DialogChangePassword";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Change Password";
            Load += new EventHandler(DialogChangePassword_Load);
            ResumeLayout(false);
            PerformLayout();
        }

        private Label _Label1;

        internal Label Label1
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Label1;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Label1 != null)
                {
                }

                _Label1 = value;
                if (_Label1 != null)
                {
                }
            }
        }

        private TextBox _txtOldPassword;

        internal TextBox txtOldPassword
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtOldPassword;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_txtOldPassword != null)
                {
                }

                _txtOldPassword = value;
                if (_txtOldPassword != null)
                {
                }
            }
        }

        private Label _Label2;

        internal Label Label2
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Label2;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Label2 != null)
                {
                }

                _Label2 = value;
                if (_Label2 != null)
                {
                }
            }
        }

        private TextBox _txtNewPassword;

        internal TextBox txtNewPassword
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtNewPassword;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_txtNewPassword != null)
                {
                }

                _txtNewPassword = value;
                if (_txtNewPassword != null)
                {
                }
            }
        }

        private Button _btnChangePassword;

        internal Button btnChangePassword
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnChangePassword;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnChangePassword != null)
                {
                    _btnChangePassword.Click -= btnChangePassword_Click;
                }

                _btnChangePassword = value;
                if (_btnChangePassword != null)
                {
                    _btnChangePassword.Click += btnChangePassword_Click;
                }
            }
        }

        private Button _btnCancel;

        internal Button btnCancel
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnCancel;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnCancel != null)
                {
                    _btnCancel.Click -= btnCancel_Click;
                }

                _btnCancel = value;
                if (_btnCancel != null)
                {
                    _btnCancel.Click += btnCancel_Click;
                }
            }
        }
    }
}