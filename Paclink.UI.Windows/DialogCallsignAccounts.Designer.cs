﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Paclink.UI.Windows
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogCallsignAccounts));
            this._btnClose = new System.Windows.Forms.Button();
            this._txtPassword = new System.Windows.Forms.TextBox();
            this._cmbAccount = new System.Windows.Forms.ComboBox();
            this._Label2 = new System.Windows.Forms.Label();
            this._Label1 = new System.Windows.Forms.Label();
            this._btnRemove = new System.Windows.Forms.Button();
            this._btnAdd = new System.Windows.Forms.Button();
            this._btnInstructions = new System.Windows.Forms.Button();
            this._ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this._btnHelp = new System.Windows.Forms.Button();
            this._Label9 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _btnClose
            // 
            this._btnClose.Location = new System.Drawing.Point(261, 129);
            this._btnClose.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._btnClose.Name = "_btnClose";
            this._btnClose.Size = new System.Drawing.Size(225, 35);
            this._btnClose.TabIndex = 5;
            this._btnClose.Text = "Close";
            this._btnClose.UseVisualStyleBackColor = true;
            this._btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // _txtPassword
            // 
            this._txtPassword.Location = new System.Drawing.Point(70, 48);
            this._txtPassword.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._txtPassword.Name = "_txtPassword";
            this._txtPassword.Size = new System.Drawing.Size(170, 23);
            this._txtPassword.TabIndex = 1;
            this._ToolTip1.SetToolTip(this._txtPassword, "Password (case insensitive) for this account ");
            // 
            // _cmbAccount
            // 
            this._cmbAccount.FormattingEnabled = true;
            this._cmbAccount.Location = new System.Drawing.Point(70, 15);
            this._cmbAccount.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._cmbAccount.Name = "_cmbAccount";
            this._cmbAccount.Size = new System.Drawing.Size(171, 23);
            this._cmbAccount.TabIndex = 0;
            this._ToolTip1.SetToolTip(this._cmbAccount, "Select account callsign or enter new callsign with optional -ssid.");
            this._cmbAccount.SelectedIndexChanged += new System.EventHandler(this.cmbAccount_SelectedIndexChanged);
            this._cmbAccount.Leave += new System.EventHandler(this.cmbAccount_Leave);
            // 
            // _Label2
            // 
            this._Label2.AutoSize = true;
            this._Label2.Location = new System.Drawing.Point(10, 51);
            this._Label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label2.Name = "_Label2";
            this._Label2.Size = new System.Drawing.Size(60, 15);
            this._Label2.TabIndex = 8;
            this._Label2.Text = "Password:";
            // 
            // _Label1
            // 
            this._Label1.AutoSize = true;
            this._Label1.Location = new System.Drawing.Point(10, 18);
            this._Label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label1.Name = "_Label1";
            this._Label1.Size = new System.Drawing.Size(52, 15);
            this._Label1.TabIndex = 7;
            this._Label1.Text = "Callsign:";
            // 
            // _btnRemove
            // 
            this._btnRemove.Location = new System.Drawing.Point(261, 89);
            this._btnRemove.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._btnRemove.Name = "_btnRemove";
            this._btnRemove.Size = new System.Drawing.Size(225, 35);
            this._btnRemove.TabIndex = 4;
            this._btnRemove.Text = "Remove Callsign Account";
            this._btnRemove.UseVisualStyleBackColor = true;
            this._btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // _btnAdd
            // 
            this._btnAdd.Location = new System.Drawing.Point(261, 48);
            this._btnAdd.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._btnAdd.Name = "_btnAdd";
            this._btnAdd.Size = new System.Drawing.Size(225, 35);
            this._btnAdd.TabIndex = 3;
            this._btnAdd.Text = "Add Callsign Account";
            this._btnAdd.UseVisualStyleBackColor = true;
            this._btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // _btnInstructions
            // 
            this._btnInstructions.Location = new System.Drawing.Point(261, 8);
            this._btnInstructions.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._btnInstructions.Name = "_btnInstructions";
            this._btnInstructions.Size = new System.Drawing.Size(225, 35);
            this._btnInstructions.TabIndex = 2;
            this._btnInstructions.Text = "Instructions...";
            this._btnInstructions.UseVisualStyleBackColor = true;
            this._btnInstructions.Click += new System.EventHandler(this.btnInstructions_Click);
            // 
            // _btnHelp
            // 
            this._btnHelp.Location = new System.Drawing.Point(261, 170);
            this._btnHelp.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._btnHelp.Name = "_btnHelp";
            this._btnHelp.Size = new System.Drawing.Size(225, 35);
            this._btnHelp.TabIndex = 6;
            this._btnHelp.Text = "Help";
            this._btnHelp.UseVisualStyleBackColor = true;
            this._btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // _Label9
            // 
            this._Label9.AutoSize = true;
            this._Label9.Location = new System.Drawing.Point(70, 74);
            this._Label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label9.Name = "_Label9";
            this._Label9.Size = new System.Drawing.Size(163, 15);
            this._Label9.TabIndex = 9;
            this._Label9.Text = "(passwords are case sensitive)";
            // 
            // DialogCallsignAccounts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(493, 212);
            this.Controls.Add(this._Label9);
            this.Controls.Add(this._btnHelp);
            this.Controls.Add(this._btnClose);
            this.Controls.Add(this._txtPassword);
            this.Controls.Add(this._cmbAccount);
            this.Controls.Add(this._Label2);
            this.Controls.Add(this._Label1);
            this.Controls.Add(this._btnRemove);
            this.Controls.Add(this._btnAdd);
            this.Controls.Add(this._btnInstructions);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogCallsignAccounts";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Manage Callsign Accounts";
            this.Load += new System.EventHandler(this.CallsignAccounts_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

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