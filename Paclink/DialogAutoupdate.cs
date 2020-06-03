using System;
using System.Windows.Forms;

namespace Paclink
{
    public partial class DialogAutoupdate
    {
        public DialogAutoupdate()
        {
            InitializeComponent();
            _Label1.Name = "Label1";
            _btnAbort.Name = "btnAbort";
            _BtnUpdate.Name = "BtnUpdate";
            _lblCV.Name = "lblCV";
            _lblNV.Name = "lblNV";
            _lblTR.Name = "lblTR";
            _Label2.Name = "Label2";
            _Label3.Name = "Label3";
            _Label4.Name = "Label4";
        }

        private System.Timers.Timer tmrTimeout;
        private int intTimeout = 30;

        private void DialogAutoupdate_Load(object sender, EventArgs e)
        {
            lblCV.Text = Application.ProductVersion;
            lblNV.Text = Globals.strNewAUVersion;
            CheckForIllegalCrossThreadCalls = false;
            tmrTimeout = new System.Timers.Timer(1000);

            // Dim tmpTop As Integer = objMain.Top + (objMain.Height \ 2)
            // Dim tmpLeft As Integer = objMain.Left + (objMain.Width \ 2)
            // Me.Top = tmpTop - (Me.Height \ 2)
            // Me.Left = tmpLeft - (Me.Width \ 2)

            tmrTimeout.Elapsed += UpdateTimeout;
            tmrTimeout.AutoReset = true;
            lblTR.Text = "30";
            BtnUpdate.Focus();
            Activate();
            TopLevel = true;
            // 
            // Don't timeout the UI for now
            // 
            // tmrTimeout.Start()
        }

        private void UpdateTimeout(object s, System.Timers.ElapsedEventArgs e)
        {
            intTimeout = intTimeout - 1;
            lblTR.Text = intTimeout.ToString();
            if (intTimeout <= 0)
            {
                Close();
            }
        }

        private void btnAbort_Click(object sender, EventArgs e)
        {
            // blnAbortAU = True
            Close();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            Globals.blnAbortAU = false;
            Close();
        }

        private void DialogAutoupdate_FormClosing(object sender, FormClosingEventArgs e)
        {
            tmrTimeout.Stop();
        }
    }
}