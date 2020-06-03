using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace Paclink
{
    public class DialogAbout : Form
    {

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public DialogAbout() : base()
        {

            /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
            // Shows about box for this application...
            base.Load += About_Load;

            // This call is required by the Windows Form Designer.
            InitializeComponent();

            // Add any initialization after the InitializeComponent() call

        }

        // Form overrides dispose to clean up the component list.
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components is object)
                {
                    components.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.  
        // Do not modify it using the code editor.
        private Label _lblDescription;

        public Label lblDescription
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblDescription;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_lblDescription != null)
                {
                }

                _lblDescription = value;
                if (_lblDescription != null)
                {
                }
            }
        }

        private Label _lblCopyright;

        public Label lblCopyright
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblCopyright;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_lblCopyright != null)
                {
                }

                _lblCopyright = value;
                if (_lblCopyright != null)
                {
                }
            }
        }

        private Label _lblARSFI;

        internal Label lblARSFI
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblARSFI;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_lblARSFI != null)
                {
                }

                _lblARSFI = value;
                if (_lblARSFI != null)
                {
                }
            }
        }

        private PictureBox _PictureBox1;

        internal PictureBox PictureBox1
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _PictureBox1;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_PictureBox1 != null)
                {
                }

                _PictureBox1 = value;
                if (_PictureBox1 != null)
                {
                }
            }
        }

        private LinkLabel _lnkARSFI;

        internal LinkLabel lnkARSFI
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lnkARSFI;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_lnkARSFI != null)
                {
                    _lnkARSFI.LinkClicked -= lnkARSFI_LinkClicked;
                }

                _lnkARSFI = value;
                if (_lnkARSFI != null)
                {
                    _lnkARSFI.LinkClicked += lnkARSFI_LinkClicked;
                }
            }
        }

        private LinkLabel _lnkWL2K;

        internal LinkLabel lnkWL2K
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lnkWL2K;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_lnkWL2K != null)
                {
                    _lnkWL2K.LinkClicked -= lnkWL2K_LinkClicked;
                }

                _lnkWL2K = value;
                if (_lnkWL2K != null)
                {
                    _lnkWL2K.LinkClicked += lnkWL2K_LinkClicked;
                }
            }
        }

        private Label _lblVersion;

        public Label lblVersion
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblVersion;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_lblVersion != null)
                {
                }

                _lblVersion = value;
                if (_lblVersion != null)
                {
                }
            }
        }

        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogAbout));
            _lblDescription = new Label();
            _lblCopyright = new Label();
            _lblVersion = new Label();
            _lblARSFI = new Label();
            _PictureBox1 = new PictureBox();
            _lnkARSFI = new LinkLabel();
            _lnkARSFI.LinkClicked += new LinkLabelLinkClickedEventHandler(lnkARSFI_LinkClicked);
            _lnkWL2K = new LinkLabel();
            _lnkWL2K.LinkClicked += new LinkLabelLinkClickedEventHandler(lnkWL2K_LinkClicked);
            ((System.ComponentModel.ISupportInitialize)_PictureBox1).BeginInit();
            SuspendLayout();
            // 
            // lblDescription
            // 
            _lblDescription.Location = new Point(4, 33);
            _lblDescription.Name = "_lblDescription";
            _lblDescription.Size = new Size(378, 39);
            _lblDescription.TabIndex = 8;
            _lblDescription.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblCopyright
            // 
            _lblCopyright.Location = new Point(62, 103);
            _lblCopyright.Name = "_lblCopyright";
            _lblCopyright.Size = new Size(255, 44);
            _lblCopyright.TabIndex = 6;
            _lblCopyright.Text = "Copyright © 2004 - 2014  Victor Poor, W5SMM (SK), Rick Muething, KN6KB, Phil Sher" + "rod, W4PHS, Peter Woods, N6PRW";
            _lblCopyright.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblVersion
            // 
            _lblVersion.BorderStyle = BorderStyle.FixedSingle;
            _lblVersion.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, Conversions.ToByte(0));
            _lblVersion.Location = new Point(105, 9);
            _lblVersion.Name = "_lblVersion";
            _lblVersion.Size = new Size(176, 24);
            _lblVersion.TabIndex = 5;
            _lblVersion.Text = "Version: 0.0.0.0";
            _lblVersion.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblARSFI
            // 
            _lblARSFI.Location = new Point(23, 165);
            _lblARSFI.Name = "_lblARSFI";
            _lblARSFI.Size = new Size(331, 47);
            _lblARSFI.TabIndex = 9;
            _lblARSFI.Text = "Paclink is made possible through the Amateur Radio Safety Foundation Inc.  Your m" + "embership in and donations to the ARSF Inc make programs like Paclink and the Wi" + "nlink 2000 system possible. ";

            // 
            // PictureBox1
            // 
            _PictureBox1.ErrorImage = (Image)resources.GetObject("PictureBox1.ErrorImage");
            _PictureBox1.Image = (Image)resources.GetObject("PictureBox1.Image");
            _PictureBox1.InitialImage = (Image)resources.GetObject("PictureBox1.InitialImage");
            _PictureBox1.Location = new Point(38, 248);
            _PictureBox1.Name = "_PictureBox1";
            _PictureBox1.Size = new Size(304, 291);
            _PictureBox1.TabIndex = 10;
            _PictureBox1.TabStop = false;
            // 
            // lnkARSFI
            // 
            _lnkARSFI.AutoSize = true;
            _lnkARSFI.BorderStyle = BorderStyle.Fixed3D;
            _lnkARSFI.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, Conversions.ToByte(0));
            _lnkARSFI.Location = new Point(119, 219);
            _lnkARSFI.Name = "_lnkARSFI";
            _lnkARSFI.Size = new Size(120, 18);
            _lnkARSFI.TabIndex = 12;
            _lnkARSFI.TabStop = true;
            _lnkARSFI.Text = "http://www.arsfi.org";
            // 
            // lnkWL2K
            // 
            _lnkWL2K.AutoSize = true;
            _lnkWL2K.BorderStyle = BorderStyle.Fixed3D;
            _lnkWL2K.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, Conversions.ToByte(0));
            _lnkWL2K.Location = new Point(119, 78);
            _lnkWL2K.Name = "_lnkWL2K";
            _lnkWL2K.Size = new Size(134, 18);
            _lnkWL2K.TabIndex = 13;
            _lnkWL2K.TabStop = true;
            _lnkWL2K.Text = "http://www.winlink.org";
            // 
            // DialogAbout
            // 
            AutoScaleBaseSize = new Size(5, 13);
            ClientSize = new Size(386, 565);
            Controls.Add(_lnkWL2K);
            Controls.Add(_lnkARSFI);
            Controls.Add(_PictureBox1);
            Controls.Add(_lblARSFI);
            Controls.Add(_lblDescription);
            Controls.Add(_lblCopyright);
            Controls.Add(_lblVersion);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "DialogAbout";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "About Paclink";
            ((System.ComponentModel.ISupportInitialize)_PictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private void About_Load(object sender, EventArgs e)
        {
            base.Text = "About " + Application.ProductName;
            lblVersion.Text = "Version: " + Application.ProductVersion;
            lblDescription.Text = "Paclink is a user client program which provides a POP3/SMTP " + "interface to standard E-mail clients and Telnet and radio links into the Winlink system.";
            lblARSFI.Text = "Paclink is made possible through the Amateur Radio Safety Foundation Inc.  " + "Your membership in and support for the ARSF make programs like Paclink and the Winlink 2000 system possible.";
            lblARSFI.Enabled = true;
        } // About_Load

        // Close the about box...
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        } // btnClose_Click

        private void lnkARSFI_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(lnkARSFI.Text);
        } // lnkARSFI_LinkClicked

        private void lnkWL2K_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(lnkWL2K.Text);
        } // lnkWL2K_LinkClicked
    }
}