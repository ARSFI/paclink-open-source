using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog;
using Paclink.UI.Common;

namespace Paclink.UI.Windows
{
    public partial class DialogAGWEngine : IAGWEngineWindow
    {
        private IAGWEngineBacking _backingObject;

        public DialogAGWEngine(IAGWEngineBacking backingObject)
        {
            _backingObject = backingObject;

            InitializeComponent();
            _btnCancel.Name = "btnCancel";
            _btnUpdate.Name = "btnUpdate";
            _rdoNotUsed.Name = "rdoNotUsed";
            _rdoLocal.Name = "rdoLocal";
            _rdoRemote.Name = "rdoRemote";
            _Label1.Name = "Label1";
            _txtAGWPath.Name = "txtAGWPath";
            _Label2.Name = "Label2";
            _Label3.Name = "Label3";
            _Label4.Name = "Label4";
            _Label5.Name = "Label5";
            _txtAGWPort.Name = "txtAGWPort";
            _txtAGWHost.Name = "txtAGWHost";
            _txtAGWUserId.Name = "txtAGWUserId";
            _txtAGWPassword.Name = "txtAGWPassword";
            _btnRemote.Name = "btnRemote";
            _btnBrowse.Name = "btnBrowse";
            _btnHelp.Name = "btnHelp";
        }

        public IAGWEngineBacking BackingObject => _backingObject;

        public delegate void SetRemoteButtonStatusCallback(bool enabled, IAGWEngineWindow.ButtonStatus status);

        private void AGWEngine_Load(object sender, EventArgs e)
        {
            var switchExpr = BackingObject.AgwLocation;
            switch (switchExpr)
            {
                case 0:
                    {
                        rdoNotUsed.Checked = true;
                        break;
                    }

                case 1:
                    {
                        rdoLocal.Checked = true;
                        break;
                    }

                case 2:
                    {
                        rdoRemote.Checked = true;
                        break;
                    }

                default:
                    {
                        rdoNotUsed.Checked = true;
                        break;
                    }
            }

            txtAGWPath.Text = BackingObject.AgwPath;
            txtAGWHost.Text = BackingObject.AgwHost;
            txtAGWPort.Text = BackingObject.AgwTcpPort.ToString();
            txtAGWUserId.Text = BackingObject.AgwUserId;
            txtAGWPassword.Text = BackingObject.AgwPassword;
            if (BackingObject.AgwLocation == 0)
            {
                txtAGWPath.Enabled = false;
                txtAGWHost.Enabled = false;
                txtAGWPort.Enabled = false;
                txtAGWUserId.Enabled = false;
                txtAGWPassword.Enabled = false;
            }
        } // AGWEngine_Load

        private void rdoNotUsed_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoNotUsed.Checked)
            {
                txtAGWPath.Enabled = false;
                txtAGWHost.Enabled = false;
                txtAGWPort.Enabled = false;
                txtAGWUserId.Enabled = false;
                txtAGWPassword.Enabled = false;
                btnRemote.Enabled = false;
                btnBrowse.Enabled = false;
            }
        } // rdoNotUsed_CheckedChanged

        private void rdoLocal_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoLocal.Checked)
            {
                txtAGWPath.Enabled = true;
                txtAGWHost.Enabled = true;
                txtAGWPort.Enabled = true;
                txtAGWUserId.Enabled = true;
                txtAGWPassword.Enabled = true;
                btnRemote.Enabled = false;
                btnBrowse.Enabled = true;
            }
        } // rdoLocal_CheckedChanged

        private void rdoRemote_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoRemote.Checked)
            {
                txtAGWPath.Enabled = false;
                txtAGWHost.Enabled = true;
                txtAGWPort.Enabled = true;
                txtAGWUserId.Enabled = true;
                txtAGWPassword.Enabled = true;
                btnRemote.Enabled = true;
                btnBrowse.Enabled = false;
            }
        } // rdoRemote_CheckedChanged

        public void SetRemoteButtonStatus(bool enabled, IAGWEngineWindow.ButtonStatus status)
        {
            Color backColor = Color.Yellow;

            switch(status)
            {
                case IAGWEngineWindow.ButtonStatus.FAILED:
                    backColor = Color.Tomato;
                    break;
                case IAGWEngineWindow.ButtonStatus.IN_PROGRESS:
                    backColor = Color.Yellow;
                    break;
                case IAGWEngineWindow.ButtonStatus.SUCCESS:
                    backColor = Color.Lime;
                    break;
                default:
                    throw new ArgumentException("Unknown button status: " + status);
            }

            if (btnRemote.InvokeRequired)
            {
                var objButtonDelegate = new SetRemoteButtonStatusCallback(SetRemoteButtonStatus);
                Invoke(objButtonDelegate, new object[] { enabled, status });
                return;
            }

            btnRemote.Enabled = Enabled;
            btnRemote.BackColor = BackColor;
        } // SetRemoteButtonStatus

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            var openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"AGWsoft\Packet Engine Pro");
            openFileDialog1.Filter = "exe files |*.exe";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(openFileDialog1.FileName))
                {
                    txtAGWPath.Text = openFileDialog1.FileName.Substring(0, 1 + openFileDialog1.FileName.LastIndexOf(@"\"));
                }
            }
        } // btnBrowse

        private void btnRemote_Click(object sender, EventArgs e)
        {
            SetRemoteButtonStatus(false, IAGWEngineWindow.ButtonStatus.IN_PROGRESS);
            try
            {
                BackingObject.TestProposedSettings(this, txtAGWHost.Text, Convert.ToInt32(txtAGWPort.Text.Trim()), txtAGWUserId.Text, txtAGWPassword.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not connect...Error in AGWPE Address, Port, User or Password Port : " + ex.Message);
                SetRemoteButtonStatus(true, IAGWEngineWindow.ButtonStatus.FAILED);
            }
        } // btnRemote_Click

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int location = 0;
            if (rdoNotUsed.Checked)
            {
                location = 0;
            }
            else if (rdoLocal.Checked)
            {
                location = 1;
            }
            else
            {
                location = 2;
            }

            if (location == 1)
            {
                if (txtAGWPath.Text.Trim().EndsWith(@"\") == false)
                    txtAGWPath.Text = txtAGWPath.Text.Trim() + @"\";
                if (!File.Exists(txtAGWPath.Text + "AGWPE.ini"))
                {
                    MessageBox.Show("AGWPE Path incorrect or AGWPE not yet configured!", "Missing AGWPE.ini File", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            BackingObject.SaveSettings(location, txtAGWPath.Text, txtAGWHost.Text, Convert.ToInt32(txtAGWPort.Text), txtAGWUserId.Text, txtAGWPassword.Text);
            Close();
        } // btnUpdate_Click

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        } // btnCancel_Click

        private void btnHelp_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, BackingObject.SiteRootDirectory + @"\Paclink.chm", HelpNavigator.Topic, @"html\hs70.htm");
        } // btnHelp_Click

        public void RefreshWindow()
        {
            // empty
        }

        public void CloseWindow()
        {
            // empty
        }
    }
} // AGWEngine