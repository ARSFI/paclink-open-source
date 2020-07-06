using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace RMS_Link_Test
{
    public class About : Form
    {

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public About() : base()
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

        private Button _btnClose;

        public Button btnClose
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

                    // Close the about box...
                    _btnClose.Click -= btnClose_Click;
                }

                _btnClose = value;
                if (_btnClose != null)
                {
                    _btnClose.Click += btnClose_Click;
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
            _lblDescription = new Label();
            _btnClose = new Button();
            _btnClose.Click += new EventHandler(btnClose_Click);
            _lblCopyright = new Label();
            _lblVersion = new Label();
            SuspendLayout();
            // 
            // lblDescription
            // 
            _lblDescription.Location = new Point(12, 44);
            _lblDescription.Name = "_lblDescription";
            _lblDescription.Size = new Size(272, 72);
            _lblDescription.TabIndex = 8;
            _lblDescription.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnClose
            // 
            _btnClose.Location = new Point(116, 165);
            _btnClose.Name = "_btnClose";
            _btnClose.Size = new Size(64, 24);
            _btnClose.TabIndex = 7;
            _btnClose.Text = "&Close";
            // 
            // lblCopyright
            // 
            _lblCopyright.Location = new Point(15, 120);
            _lblCopyright.Name = "_lblCopyright";
            _lblCopyright.Size = new Size(295, 28);
            _lblCopyright.TabIndex = 6;
            _lblCopyright.Text = "Copyright © 2008-2020" + '\r' + '\n' + "Victor Poor, W5SMM (SK), and Phil Sherrod, W4PHS";
            _lblCopyright.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblVersion
            // 
            _lblVersion.BorderStyle = BorderStyle.FixedSingle;
            _lblVersion.Location = new Point(56, 16);
            _lblVersion.Name = "_lblVersion";
            _lblVersion.Size = new Size(176, 16);
            _lblVersion.TabIndex = 5;
            _lblVersion.Text = "Version: 0.0.0.0";
            _lblVersion.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // About
            // 
            AutoScaleBaseSize = new Size(5, 13);
            ClientSize = new Size(324, 198);
            Controls.Add(_lblDescription);
            Controls.Add(_btnClose);
            Controls.Add(_lblCopyright);
            Controls.Add(_lblVersion);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "About";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            ResumeLayout(false);
        }

        private void About_Load(object sender, EventArgs e)
        {
            base.Text = "About " + Application.ProductName;
            lblVersion.Text = "Version: " + Application.ProductVersion;
            lblDescription.Text = "This is a simple program for testing link response " + "for paths required by RMS programs. No access credentials are required to run this program.";
        } // About_Load

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        } // btnClose_Click
    }
}