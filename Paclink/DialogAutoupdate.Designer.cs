using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Paclink
{
    public partial class DialogAutoupdate : Form
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
            _btnAbort = new Button();
            _btnAbort.Click += new EventHandler(btnAbort_Click);
            _BtnUpdate = new Button();
            _BtnUpdate.Click += new EventHandler(BtnUpdate_Click);
            _lblCV = new Label();
            _lblNV = new Label();
            _lblTR = new Label();
            _Label2 = new Label();
            _Label3 = new Label();
            _Label4 = new Label();
            SuspendLayout();
            // 
            // Label1
            // 
            _Label1.AutoSize = true;
            _Label1.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, Convert.ToByte(0));
            _Label1.Location = new Point(13, 26);
            _Label1.Name = "_Label1";
            _Label1.Size = new Size(310, 64);
            _Label1.TabIndex = 0;
            _Label1.Text = "A new version of this program has been released.  " + '\r' + '\n' + "Click 'Update Now' to install" + " the update and restart." + '\r' + '\n' + "Click 'Remind Me Later' if you do not wish to update" + '\r' + '\n' + "at this time.";

            _Label1.TextAlign = ContentAlignment.TopCenter;
            // 
            // btnAbort
            // 
            _btnAbort.Location = new Point(12, 192);
            _btnAbort.Name = "_btnAbort";
            _btnAbort.Size = new Size(113, 28);
            _btnAbort.TabIndex = 1;
            _btnAbort.Text = "Remind Me Later";
            _btnAbort.UseVisualStyleBackColor = true;
            // 
            // BtnUpdate
            // 
            _BtnUpdate.Location = new Point(226, 191);
            _BtnUpdate.Name = "_BtnUpdate";
            _BtnUpdate.Size = new Size(106, 29);
            _BtnUpdate.TabIndex = 2;
            _BtnUpdate.Text = "Update Now" + '\r' + '\n';
            _BtnUpdate.UseVisualStyleBackColor = true;
            // 
            // lblCV
            // 
            _lblCV.AutoSize = true;
            _lblCV.Location = new Point(186, 120);
            _lblCV.Name = "_lblCV";
            _lblCV.Size = new Size(16, 13);
            _lblCV.TabIndex = 3;
            _lblCV.Text = "---";
            // 
            // lblNV
            // 
            _lblNV.AutoSize = true;
            _lblNV.Location = new Point(186, 140);
            _lblNV.Name = "_lblNV";
            _lblNV.Size = new Size(16, 13);
            _lblNV.TabIndex = 4;
            _lblNV.Text = "---";
            // 
            // lblTR
            // 
            _lblTR.AutoSize = true;
            _lblTR.Location = new Point(186, 159);
            _lblTR.Name = "_lblTR";
            _lblTR.Size = new Size(16, 13);
            _lblTR.TabIndex = 5;
            _lblTR.Text = "---";
            _lblTR.Visible = false;
            // 
            // Label2
            // 
            _Label2.AutoSize = true;
            _Label2.Location = new Point(95, 120);
            _Label2.Name = "_Label2";
            _Label2.Size = new Size(85, 13);
            _Label2.TabIndex = 6;
            _Label2.Text = "Current Version: ";
            // 
            // Label3
            // 
            _Label3.AutoSize = true;
            _Label3.Location = new Point(95, 138);
            _Label3.Name = "_Label3";
            _Label3.Size = new Size(70, 13);
            _Label3.TabIndex = 7;
            _Label3.Text = "New Version:";
            // 
            // Label4
            // 
            _Label4.AutoSize = true;
            _Label4.Location = new Point(95, 157);
            _Label4.Name = "_Label4";
            _Label4.Size = new Size(86, 13);
            _Label4.TabIndex = 8;
            _Label4.Text = "Time Remaining:";
            _Label4.Visible = false;
            // 
            // DialogAutoupdate
            // 
            AutoScaleDimensions = new SizeF(6.0F, 13.0F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(344, 232);
            Controls.Add(_Label4);
            Controls.Add(_Label3);
            Controls.Add(_Label2);
            Controls.Add(_lblTR);
            Controls.Add(_lblNV);
            Controls.Add(_lblCV);
            Controls.Add(_BtnUpdate);
            Controls.Add(_btnAbort);
            Controls.Add(_Label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "DialogAutoupdate";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Autoupdate";
            TopMost = true;
            Load += new EventHandler(DialogAutoupdate_Load);
            FormClosing += new FormClosingEventHandler(DialogAutoupdate_FormClosing);
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

        private Button _btnAbort;

        internal Button btnAbort
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnAbort;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnAbort != null)
                {
                    _btnAbort.Click -= btnAbort_Click;
                }

                _btnAbort = value;
                if (_btnAbort != null)
                {
                    _btnAbort.Click += btnAbort_Click;
                }
            }
        }

        private Button _BtnUpdate;

        internal Button BtnUpdate
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _BtnUpdate;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_BtnUpdate != null)
                {
                    _BtnUpdate.Click -= BtnUpdate_Click;
                }

                _BtnUpdate = value;
                if (_BtnUpdate != null)
                {
                    _BtnUpdate.Click += BtnUpdate_Click;
                }
            }
        }

        private Label _lblCV;

        internal Label lblCV
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblCV;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_lblCV != null)
                {
                }

                _lblCV = value;
                if (_lblCV != null)
                {
                }
            }
        }

        private Label _lblNV;

        internal Label lblNV
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblNV;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_lblNV != null)
                {
                }

                _lblNV = value;
                if (_lblNV != null)
                {
                }
            }
        }

        private Label _lblTR;

        internal Label lblTR
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblTR;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_lblTR != null)
                {
                }

                _lblTR = value;
                if (_lblTR != null)
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

        private Label _Label3;

        internal Label Label3
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Label3;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Label3 != null)
                {
                }

                _Label3 = value;
                if (_Label3 != null)
                {
                }
            }
        }

        private Label _Label4;

        internal Label Label4
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Label4;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Label4 != null)
                {
                }

                _Label4 = value;
                if (_Label4 != null)
                {
                }
            }
        }
    }
}