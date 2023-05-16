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
            this._Label1 = new System.Windows.Forms.Label();
            this._txtOldPassword = new System.Windows.Forms.TextBox();
            this._Label2 = new System.Windows.Forms.Label();
            this._txtNewPassword = new System.Windows.Forms.TextBox();
            this._btnChangePassword = new System.Windows.Forms.Button();
            this._btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _Label1
            // 
            this._Label1.AutoSize = true;
            this._Label1.Location = new System.Drawing.Point(29, 28);
            this._Label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label1.Name = "_Label1";
            this._Label1.Size = new System.Drawing.Size(82, 15);
            this._Label1.TabIndex = 0;
            this._Label1.Text = "Old password:";
            // 
            // _txtOldPassword
            // 
            this._txtOldPassword.Location = new System.Drawing.Point(117, 27);
            this._txtOldPassword.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._txtOldPassword.Name = "_txtOldPassword";
            this._txtOldPassword.Size = new System.Drawing.Size(200, 23);
            this._txtOldPassword.TabIndex = 1;
            // 
            // _Label2
            // 
            this._Label2.AutoSize = true;
            this._Label2.Location = new System.Drawing.Point(22, 68);
            this._Label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label2.Name = "_Label2";
            this._Label2.Size = new System.Drawing.Size(87, 15);
            this._Label2.TabIndex = 2;
            this._Label2.Text = "New password:";
            // 
            // _txtNewPassword
            // 
            this._txtNewPassword.Location = new System.Drawing.Point(117, 66);
            this._txtNewPassword.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._txtNewPassword.Name = "_txtNewPassword";
            this._txtNewPassword.Size = new System.Drawing.Size(200, 23);
            this._txtNewPassword.TabIndex = 3;
            // 
            // _btnChangePassword
            // 
            this._btnChangePassword.Location = new System.Drawing.Point(33, 111);
            this._btnChangePassword.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._btnChangePassword.Name = "_btnChangePassword";
            this._btnChangePassword.Size = new System.Drawing.Size(136, 28);
            this._btnChangePassword.TabIndex = 4;
            this._btnChangePassword.Text = "Change Password";
            this._btnChangePassword.UseVisualStyleBackColor = true;
            this._btnChangePassword.Click += new System.EventHandler(this.btnChangePassword_Click);
            // 
            // _btnCancel
            // 
            this._btnCancel.Location = new System.Drawing.Point(197, 111);
            this._btnCancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size(136, 28);
            this._btnCancel.TabIndex = 5;
            this._btnCancel.Text = "Cancel";
            this._btnCancel.UseVisualStyleBackColor = true;
            this._btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // DialogChangePassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(366, 153);
            this.Controls.Add(this._btnCancel);
            this.Controls.Add(this._btnChangePassword);
            this.Controls.Add(this._txtNewPassword);
            this.Controls.Add(this._Label2);
            this.Controls.Add(this._txtOldPassword);
            this.Controls.Add(this._Label1);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogChangePassword";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Change Password";
            this.Load += new System.EventHandler(this.DialogChangePassword_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

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