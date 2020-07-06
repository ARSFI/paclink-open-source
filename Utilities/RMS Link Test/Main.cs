using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using WinlinkInterop;

namespace RMS_Link_Test
{
    public class Main : Form
    {

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public Main() : base()
        {
            base.Load += Main_Load;
            this.FormClosed += Main_FormClosed; // frmMain_FormClosed

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
        private MainMenu _mnuMain;

        internal MainMenu mnuMain
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _mnuMain;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_mnuMain != null)
                {
                }

                _mnuMain = value;
                if (_mnuMain != null)
                {
                }
            }
        }

        private StatusStrip _sbrMain;

        internal StatusStrip sbrMain
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _sbrMain;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_sbrMain != null)
                {
                }

                _sbrMain = value;
                if (_sbrMain != null)
                {
                }
            }
        }

        private ToolStripStatusLabel _lblStatus;

        internal ToolStripStatusLabel lblStatus
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblStatus;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_lblStatus != null)
                {
                }

                _lblStatus = value;
                if (_lblStatus != null)
                {
                }
            }
        }

        private MenuItem _mnuClose;

        internal MenuItem mnuClose
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _mnuClose;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_mnuClose != null)
                {
                    _mnuClose.Click -= mnuClose_Click;
                }

                _mnuClose = value;
                if (_mnuClose != null)
                {
                    _mnuClose.Click += mnuClose_Click;
                }
            }
        }

        private MenuItem _mnuStartTest;

        internal MenuItem mnuStartTest
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _mnuStartTest;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_mnuStartTest != null)
                {
                    _mnuStartTest.Click -= mnuStartTest_Click;
                }

                _mnuStartTest = value;
                if (_mnuStartTest != null)
                {
                    _mnuStartTest.Click += mnuStartTest_Click;
                }
            }
        }

        private MenuItem _mnuAbout;

        internal MenuItem mnuAbout
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _mnuAbout;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_mnuAbout != null)
                {
                    _mnuAbout.Click -= mnuAbout_Click;
                }

                _mnuAbout = value;
                if (_mnuAbout != null)
                {
                    _mnuAbout.Click += mnuAbout_Click;
                }
            }
        }

        private TextBox _txtMain;

        internal TextBox txtMain
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtMain;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_txtMain != null)
                {
                }

                _txtMain = value;
                if (_txtMain != null)
                {
                }
            }
        }

        private System.Windows.Forms.Timer _tmrPoll;

        internal System.Windows.Forms.Timer tmrPoll
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _tmrPoll;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_tmrPoll != null)
                {
                    _tmrPoll.Tick -= tmrPoll_Tick;
                }

                _tmrPoll = value;
                if (_tmrPoll != null)
                {
                    _tmrPoll.Tick += tmrPoll_Tick;
                }
            }
        }

        private ToolStripStatusLabel _lblActivity;

        internal ToolStripStatusLabel lblActivity
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblActivity;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_lblActivity != null)
                {
                }

                _lblActivity = value;
                if (_lblActivity != null)
                {
                }
            }
        }

        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            _mnuMain = new MainMenu(components);
            _mnuStartTest = new MenuItem();
            _mnuStartTest.Click += new EventHandler(mnuStartTest_Click);
            _mnuClose = new MenuItem();
            _mnuClose.Click += new EventHandler(mnuClose_Click);
            _mnuAbout = new MenuItem();
            _mnuAbout.Click += new EventHandler(mnuAbout_Click);
            _sbrMain = new StatusStrip();
            _lblStatus = new ToolStripStatusLabel();
            _lblActivity = new ToolStripStatusLabel();
            _txtMain = new TextBox();
            _tmrPoll = new System.Windows.Forms.Timer(components);
            _tmrPoll.Tick += new EventHandler(tmrPoll_Tick);
            _sbrMain.SuspendLayout();
            SuspendLayout();
            // 
            // mnuMain
            // 
            _mnuMain.MenuItems.AddRange(new MenuItem[] { _mnuStartTest, _mnuClose, _mnuAbout });
            // 
            // mnuStartTest
            // 
            _mnuStartTest.Index = 0;
            _mnuStartTest.Text = "Start-test";
            // 
            // mnuClose
            // 
            _mnuClose.Index = 1;
            _mnuClose.Text = "Close";
            // 
            // mnuAbout
            // 
            _mnuAbout.Index = 2;
            _mnuAbout.Text = "About";
            // 
            // sbrMain
            // 
            _sbrMain.Items.AddRange(new ToolStripItem[] { _lblStatus, _lblActivity });
            _sbrMain.Location = new Point(0, 318);
            _sbrMain.Name = "_sbrMain";
            _sbrMain.Size = new Size(664, 22);
            _sbrMain.TabIndex = 9;
            // 
            // lblStatus
            // 
            _lblStatus.Name = "_lblStatus";
            _lblStatus.Size = new Size(84, 17);
            _lblStatus.Text = "Ready to test...";
            // 
            // lblActivity
            // 
            _lblActivity.Name = "_lblActivity";
            _lblActivity.Size = new Size(0, 17);
            // 
            // txtMain
            // 
            _txtMain.Dock = DockStyle.Fill;
            _txtMain.Font = new Font("Microsoft Sans Serif", 9.0F, FontStyle.Regular, GraphicsUnit.Point, Conversions.ToByte(0));
            _txtMain.Location = new Point(0, 0);
            _txtMain.Multiline = true;
            _txtMain.Name = "_txtMain";
            _txtMain.ScrollBars = ScrollBars.Vertical;
            _txtMain.Size = new Size(664, 318);
            _txtMain.TabIndex = 10;
            _txtMain.WordWrap = false;
            // 
            // tmrPoll
            // 
            // 
            // Main
            // 
            AutoScaleBaseSize = new Size(5, 13);
            BackColor = SystemColors.ButtonFace;
            ClientSize = new Size(664, 340);
            Controls.Add(_txtMain);
            Controls.Add(_sbrMain);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Menu = _mnuMain;
            MinimumSize = new Size(0, 100);
            Name = "Main";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "RMS Link Test";
            _sbrMain.ResumeLayout(false);
            _sbrMain.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        private bool blnUDPSuccess;
        private Thread thrTest;
        private bool blnDoPingTest = false;
        private WinlinkInterop.WinlinkInterop objWL2KInterop;
        private string strCmsUrl = "cms-z.winlink.org";
        private string strWebUrl = "www.winlink.org";

        private void Main_Load(object sender, EventArgs e)
        {
            Text = "RMS Link Test - " + Application.ProductVersion;
            Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));
            Globals.strExecutionDirectory = Application.StartupPath + @"\";

            // Set inital window position and size...
            Top = Globals.objINIFile.GetInteger("Main Form", "Top", 50);
            Left = Globals.objINIFile.GetInteger("Main Form", "Left", 50);
            Width = Globals.objINIFile.GetInteger("Main Form", "Width", 550);
            Height = Globals.objINIFile.GetInteger("Main Form", "Height", 400);
            objWL2KInterop = new WinlinkInterop.WinlinkInterop("W4LINK");
            if (objWL2KInterop.HaveInternetConnection(true) == false)
            {
                txtMain.AppendText("Cannot perform test.  Your computer does not have an Internet connection." + Constants.vbCrLf);
                return;
            }

            txtMain.Text = "Ports used by Winlink: 443, 8772, 8773" + Constants.vbCrLf + Constants.vbCrLf + "Click 'Start-test' to run the link test" + Constants.vbCrLf;
            txtMain.Select(0, 0);
            tmrPoll.Start();
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Ends the program...

            if (thrTest is object)
                thrTest.Abort();
            if (WindowState == FormWindowState.Normal)
            {
                Globals.objINIFile.WriteInteger("Main Form", "Top", Top);
                Globals.objINIFile.WriteInteger("Main Form", "Left", Left);
                Globals.objINIFile.WriteInteger("Main Form", "Width", Width);
                Globals.objINIFile.WriteInteger("Main Form", "Height", Height);
            }

            Globals.objINIFile.Flush();
        }

        private void mnuClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void mnuAbout_Click(object sender, EventArgs e)
        {
            // Show About dialog...
            TopMost = false;
            var dlgAbout = new About();
            dlgAbout.ShowDialog();
            dlgAbout = null;
            TopMost = true;
        }

        private void mnuStartTest_Click(object sender, EventArgs e)
        {
            CMSTest();
            return;
        }

        private void CMSTest()
        {
            // 
            // Perform the test.
            // 
            bool blnError = false;
            var objStopwatch = new Stopwatch();
            int intResponseTime;
            // 
            // Test telnet connection to a CMS.
            // 
            mnuStartTest.Enabled = false;
            txtMain.Clear();
            txtMain.Text = "Test started " + Globals.TimestampEx() + " UTC" + Constants.vbCrLf;
            Log("");
            Log("Testing CMS telnet connection to cms.winlink.org through port 8772...");
            txtMain.Refresh();
            Application.DoEvents();
            objStopwatch.Reset();
            objStopwatch.Start();
            var objCMSconnection = objWL2KInterop.ConnectToServer("", 8772, false, 10);
            intResponseTime = Conversions.ToInteger(objStopwatch.ElapsedMilliseconds);
            objStopwatch.Stop();
            if (objCMSconnection == null)
            {
                Log("  Error: --> Unable to connect to a CMS through port 8772");
                blnError = true;
            }
            else
            {
                Log("  Successfully connected to a CMS through port 8772 in " + intResponseTime.ToString() + " Milliseconds");
                objCMSconnection = null;
            }
            // 
            // Test SSL telnet connection to a CMS.
            // 
            Log("");
            Log("Testing CMS SSL telnet connection to cms.winlink.org through port 8773...");
            txtMain.Refresh();
            Application.DoEvents();
            objStopwatch.Reset();
            objStopwatch.Start();
            objCMSconnection = objWL2KInterop.ConnectToServer("", 8773, true, 10);
            intResponseTime = Conversions.ToInteger(objStopwatch.ElapsedMilliseconds);
            objStopwatch.Stop();
            if (objCMSconnection == null || objCMSconnection.blnSSL == false)
            {
                Log("  Error: --> Unable to connect to a CMS through port 8773");
                blnError = true;
            }
            else
            {
                Log("  Successfully connected to a CMS through port 8773 in " + intResponseTime.ToString() + " Milliseconds");
                objCMSconnection.Close();
                objCMSconnection = null;
            }
            // 
            // Test API service.
            // 
            Log("");
            Log("Testing API service access through port 443 to api.winlink.org...");
            txtMain.Refresh();
            Application.DoEvents();
            objStopwatch.Reset();
            objStopwatch.Start();
            bool blnResult = objWL2KInterop.PingCMS();
            intResponseTime = Conversions.ToInteger(objStopwatch.ElapsedMilliseconds);
            objStopwatch.Stop();
            if (blnResult)
            {
                Log("  Successfully performed API service to api.winlink.org thorugh port 443 in " + intResponseTime.ToString() + " Milliseconds");
            }
            else
            {
                Log("  Error: --> Unable to connect to api.winlink.org through port 443");
                blnError = true;
            }
            // 
            // Test new Autoupdate service.
            // 
            Log("");
            Log("Testing Autoupdate server access through port 443 to autoupdate2.winlink.org...");
            txtMain.Refresh();
            Application.DoEvents();
            objStopwatch.Reset();
            objStopwatch.Start();
            var lstFiles = objWL2KInterop.GetAutoupdateFileList(false);
            intResponseTime = Conversions.ToInteger(objStopwatch.ElapsedMilliseconds);
            objStopwatch.Stop();
            if (lstFiles.Count > 0)
            {
                Log("  Successfully checked autoupdate server thorugh port 443 in " + intResponseTime.ToString() + " Milliseconds");
            }
            else
            {
                Log("  Error: --> Unable to connect to autoupdate2.winlink.org through port 443");
                blnError = true;
            }
            // 
            // Test connection to web site.
            // 
            Log("");
            Log("Testing connection to web site - www.winlink.org:443");
            txtMain.Refresh();
            Application.DoEvents();
            objStopwatch.Reset();
            objStopwatch.Start();
            if (objWL2KInterop.TestConnection("www.winlink.org", 443))
            {
                intResponseTime = Conversions.ToInteger(objStopwatch.ElapsedMilliseconds);
                objStopwatch.Stop();
                Log("  Successfully connected to www.winlink.org thorugh port 443 in " + intResponseTime.ToString() + " Milliseconds");
            }
            else
            {
                Log("  Error: --> Unable to connect to www.winlink.org through port 443");
                blnError = true;
            }
            // 
            // Test connection to SFI site.
            // 
            Log("");
            Log("Testing FTP connection to SFI site - https://services.swpc.noaa.gov/text/sgas.txt");
            txtMain.Refresh();
            Application.DoEvents();
            objStopwatch.Reset();
            objStopwatch.Start();
            var objSFI = new SFIhelper();
            string strSFI = objSFI.GetSFI(Globals.strExecutionDirectory);
            if (!string.IsNullOrEmpty(strSFI))
            {
                intResponseTime = Conversions.ToInteger(objStopwatch.ElapsedMilliseconds);
                objStopwatch.Stop();
                Log("  Successfully connected to https://services.swpc.noaa.gov/text/sgas.txt thorugh port 443 in " + intResponseTime.ToString() + " Milliseconds");
            }
            else
            {
                Log("  Error: --> Unable to connect to https://services.swpc.noaa.gov/text/sgas.txt through port 443");
                blnError = true;
            }
            // 
            // Finished
            // 
            Log("");
            if (blnError)
            {
                Log("Test completed.  Errors were detected.");
            }
            else
            {
                Log("Test completed successfully.");
            }

            mnuStartTest.Enabled = true;
            txtMain.Refresh();
            Application.DoEvents();
            return;
        }

        private bool blnStarted = false;

        private void tmrPoll_Tick(object sender, EventArgs e)
        {
            tmrPoll.Stop();

            if (blnStarted == false)
            {
                blnStarted = true;
            }

            tmrPoll.Start();
        }

        private void Log(string strText, bool blnCrLf = false)
        {
            // 
            // Queue a string to be displayed.
            // 
            if (true | blnCrLf)
            {
                strText += Constants.vbCrLf;
            }

            txtMain.AppendText(strText);
            txtMain.Refresh();
            Application.DoEvents();
            return;
        }
    }
}