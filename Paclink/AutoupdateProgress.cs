using System;

namespace Paclink
{
    public partial class AutoupdateProgress
    {
        public AutoupdateProgress()
        {
            InitializeComponent();
            _Label1.Name = "Label1";
            _Label2.Name = "Label2";
            _txtProgress.Name = "txtProgress";
        }

        private string strLastStatus = "";

        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            // 
            // Check if the autoupdate status needs to be updated.
            // 
            if ((Globals.strAutoupdateStatus ?? "") != (strLastStatus ?? ""))
            {
                strLastStatus = Globals.strAutoupdateStatus;
                txtProgress.Text = Globals.strAutoupdateStatus;
                txtProgress.Invalidate();
                txtProgress.Update();
            }
        }
    }
}