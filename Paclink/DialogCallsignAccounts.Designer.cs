﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Paclink
{
    public partial class DialogCallsignAccounts : Form
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
            components = new System.ComponentModel.Container();
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogCallsignAccounts));
            _btnClose = new Button();
            _btnClose.Click += new EventHandler(btnClose_Click);
            _txtPassword = new TextBox();
            _cmbAccount = new ComboBox();
            _cmbAccount.TextChanged += new EventHandler(cmbAccount_TextChanged);
            _cmbAccount.Leave += new EventHandler(cmbAccount_Leave);
            _cmbAccount.SelectedIndexChanged += new EventHandler(cmbAccount_SelectedIndexChanged);
            _Label2 = new Label();
            _Label1 = new Label();
            _btnRemove = new Button();
            _btnRemove.Click += new EventHandler(btnRemove_Click);
            _btnAdd = new Button();
            _btnAdd.Click += new EventHandler(btnAdd_Click);
            _btnInstructions = new Button();
            _btnInstructions.Click += new EventHandler(btnInstructions_Click);
            _ToolTip1 = new ToolTip(components);
            _btnHelp = new Button();
            _btnHelp.Click += new EventHandler(btnHelp_Click);
            _Label9 = new Label();
            SuspendLayout();
            // 
            // btnClose
            // 
            _btnClose.Location = new Point(404, 112);
            _btnClose.Name = "_btnClose";
            _btnClose.Size = new Size(193, 30);
            _btnClose.TabIndex = 7;
            _btnClose.Text = "Close";
            _btnClose.UseVisualStyleBackColor = true;
            // 
            // txtPassword
            // 
            _txtPassword.Location = new Point(223, 65);
            _txtPassword.Name = "_txtPassword";
            _txtPassword.Size = new Size(146, 20);
            _txtPassword.TabIndex = 1;
            _txtPassword.TextAlign = HorizontalAlignment.Center;
            _ToolTip1.SetToolTip(_txtPassword, "Password (case insensitive) for this account ");
            // 
            // cmbAccount
            // 
            _cmbAccount.FormattingEnabled = true;
            _cmbAccount.Location = new Point(222, 31);
            _cmbAccount.Name = "_cmbAccount";
            _cmbAccount.Size = new Size(147, 21);
            _cmbAccount.TabIndex = 0;
            _ToolTip1.SetToolTip(_cmbAccount, "Select account callsign or enter new callsign with optional -ssid.");
            // 
            // Label2
            // 
            _Label2.AutoSize = true;
            _Label2.Location = new Point(12, 68);
            _Label2.Name = "_Label2";
            _Label2.Size = new Size(204, 13);
            _Label2.TabIndex = 18;
            _Label2.Text = "Callsign POP3/SMTP Account Password:";
            // 
            // Label1
            // 
            _Label1.AutoSize = true;
            _Label1.Location = new Point(127, 34);
            _Label1.Name = "_Label1";
            _Label1.Size = new Size(89, 13);
            _Label1.TabIndex = 17;
            _Label1.Text = "Account Callsign:";
            // 
            // btnRemove
            // 
            _btnRemove.Location = new Point(404, 77);
            _btnRemove.Name = "_btnRemove";
            _btnRemove.Size = new Size(193, 30);
            _btnRemove.TabIndex = 5;
            _btnRemove.Text = "Remove A Callsign Account";
            _btnRemove.UseVisualStyleBackColor = true;
            // 
            // btnAdd
            // 
            _btnAdd.Location = new Point(404, 42);
            _btnAdd.Name = "_btnAdd";
            _btnAdd.Size = new Size(193, 30);
            _btnAdd.TabIndex = 4;
            _btnAdd.Text = "Add A Callsign Account";
            _btnAdd.UseVisualStyleBackColor = true;
            // 
            // btnInstructions
            // 
            _btnInstructions.Location = new Point(404, 7);
            _btnInstructions.Name = "_btnInstructions";
            _btnInstructions.Size = new Size(193, 30);
            _btnInstructions.TabIndex = 3;
            _btnInstructions.Text = "Instructions...";
            _btnInstructions.UseVisualStyleBackColor = true;
            // 
            // btnHelp
            // 
            _btnHelp.Location = new Point(404, 147);
            _btnHelp.Name = "_btnHelp";
            _btnHelp.Size = new Size(193, 30);
            _btnHelp.TabIndex = 24;
            _btnHelp.Text = "Help";
            _btnHelp.UseVisualStyleBackColor = true;
            // 
            // Label9
            // 
            _Label9.AutoSize = true;
            _Label9.Location = new Point(257, 88);
            _Label9.Name = "_Label9";
            _Label9.Size = new Size(81, 13);
            _Label9.TabIndex = 321;
            _Label9.Text = "(Case sensitive)";
            // 
            // DialogCallsignAccounts
            // 
            AutoScaleDimensions = new SizeF(6.0F, 13.0F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(612, 184);
            Controls.Add(_Label9);
            Controls.Add(_btnHelp);
            Controls.Add(_btnClose);
            Controls.Add(_txtPassword);
            Controls.Add(_cmbAccount);
            Controls.Add(_Label2);
            Controls.Add(_Label1);
            Controls.Add(_btnRemove);
            Controls.Add(_btnAdd);
            Controls.Add(_btnInstructions);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "DialogCallsignAccounts";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Manage Callsign Accounts";
            Load += new EventHandler(CallsignAccounts_Load);
            ResumeLayout(false);
            PerformLayout();
        }

        private Button _btnClose;

        internal Button btnClose
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnClose;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnClose != null)
                {
                    _btnClose.Click -= btnClose_Click;
                }

                _btnClose = value;
                if (_btnClose != null)
                {
                    _btnClose.Click += btnClose_Click;
                }
            }
        }

        private TextBox _txtPassword;

        internal TextBox txtPassword
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtPassword;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_txtPassword != null)
                {
                }

                _txtPassword = value;
                if (_txtPassword != null)
                {
                }
            }
        }

        private ComboBox _cmbAccount;

        internal ComboBox cmbAccount
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _cmbAccount;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_cmbAccount != null)
                {
                    _cmbAccount.TextChanged -= cmbAccount_TextChanged;
                    _cmbAccount.Leave -= cmbAccount_Leave;
                    _cmbAccount.SelectedIndexChanged -= cmbAccount_SelectedIndexChanged;
                }

                _cmbAccount = value;
                if (_cmbAccount != null)
                {
                    _cmbAccount.TextChanged += cmbAccount_TextChanged;
                    _cmbAccount.Leave += cmbAccount_Leave;
                    _cmbAccount.SelectedIndexChanged += cmbAccount_SelectedIndexChanged;
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

        private Button _btnRemove;

        internal Button btnRemove
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnRemove;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnRemove != null)
                {
                    _btnRemove.Click -= btnRemove_Click;
                }

                _btnRemove = value;
                if (_btnRemove != null)
                {
                    _btnRemove.Click += btnRemove_Click;
                }
            }
        }

        private Button _btnAdd;

        internal Button btnAdd
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnAdd;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnAdd != null)
                {
                    _btnAdd.Click -= btnAdd_Click;
                }

                _btnAdd = value;
                if (_btnAdd != null)
                {
                    _btnAdd.Click += btnAdd_Click;
                }
            }
        }

        private Button _btnInstructions;

        internal Button btnInstructions
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnInstructions;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnInstructions != null)
                {
                    _btnInstructions.Click -= btnInstructions_Click;
                }

                _btnInstructions = value;
                if (_btnInstructions != null)
                {
                    _btnInstructions.Click += btnInstructions_Click;
                }
            }
        }

        private ToolTip _ToolTip1;

        internal ToolTip ToolTip1
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _ToolTip1;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_ToolTip1 != null)
                {
                }

                _ToolTip1 = value;
                if (_ToolTip1 != null)
                {
                }
            }
        }

        private Button _btnHelp;

        internal Button btnHelp
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnHelp;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnHelp != null)
                {
                    _btnHelp.Click -= btnHelp_Click;
                }

                _btnHelp = value;
                if (_btnHelp != null)
                {
                    _btnHelp.Click += btnHelp_Click;
                }
            }
        }

        private Label _Label9;

        internal Label Label9
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Label9;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Label9 != null)
                {
                }

                _Label9 = value;
                if (_Label9 != null)
                {
                }
            }
        }
    }
}