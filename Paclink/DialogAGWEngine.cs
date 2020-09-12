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

namespace Paclink
{
    public partial class DialogAGWEngine
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        public DialogAGWEngine()
        {
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
        // AGW Properties...
        public static int AGWLocation;
        public static string AGWPath;
        public static int AGWTCPPort;
        public static string AGWHost;
        public static string AGWUserId;
        public static string AGWPassword;
        private TcpClient _objTCPPort;

        private TcpClient objTCPPort
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _objTCPPort;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _objTCPPort = value;
            }
        }

        private byte[] bytTCPData;

        public delegate void SetRemoteButtonStatusCallback(bool Enabled, Color BackColor);

        private void AGWEngine_Load(object sender, EventArgs e)
        {
            var switchExpr = AGWLocation;
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

            txtAGWPath.Text = AGWPath;
            txtAGWHost.Text = AGWHost;
            txtAGWPort.Text = AGWTCPPort.ToString();
            txtAGWUserId.Text = AGWUserId;
            txtAGWPassword.Text = AGWPassword;
            if (AGWLocation == 0)
            {
                txtAGWPath.Enabled = false;
                txtAGWHost.Enabled = false;
                txtAGWPort.Enabled = false;
                txtAGWUserId.Enabled = false;
                txtAGWPassword.Enabled = false;
            }
        } // AGWEngine_Load

        public static void InitializeAGWProperties()
        {
            AGWLocation = Globals.Settings.Get(Application.ProductName, "AGW Location", 0);
            AGWPath = Globals.Settings.Get(Application.ProductName, "AGW Path", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"AGWsoft\Packet Engine Pro\"));
            AGWHost = Globals.Settings.Get(Application.ProductName, "AGW Host", "Localhost");
            AGWTCPPort = Globals.Settings.Get(Application.ProductName, "AGW Port", 8000);
            AGWUserId = Globals.Settings.Get(Application.ProductName, "AGW User Id", "");
            AGWPassword = Globals.Settings.Get(Application.ProductName, "AGW Password", "");
        } // InitializeAGWProperties

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

        private void OnDataIn(byte[] buffer, int length)
        {
            int intDataLength;
            while (true)
            {
                try
                {
                    if (buffer == null)
                        break;
                    Globals.ConcatanateByteArrays(ref bytTCPData, buffer); // Add data to buffer array
                    if (bytTCPData.Length < 36)
                        break; // not a complete frame header
                    intDataLength = Globals.ComputeLengthL(bytTCPData, 28); // get and decode the data length field from the header
                    if (bytTCPData.Length < 36 + intDataLength)
                        break; // not A complete "G" frame...
                    if (((char)bytTCPData[4]).ToString() != "G")
                    {
                        bytTCPData = new byte[0];
                        return;
                    }

                    tmrTimer10sec.Enabled = false;
                    SetRemoteButtonStatus(true, Color.Lime);
                    objTCPPort.Close();
                    objTCPPort = null;
                }
                catch (Exception ex)
                {
                    _log.Error("[AGWEngine, tcpOnDataIn] " + ex.Message);
                    SetRemoteButtonStatus(true, Color.Tomato);
                }
                break;
            }

            Task<int> t = null;
            try
            {
                t = objTCPPort.GetStream().ReadAsync(buffer, 0, 1024);
                t.ContinueWith(k =>
                {
                    OnDataIn(buffer, k.Result);
                });
                t.Wait(0);
            }
            catch (Exception e)
            {
                // empty
            }
        } // objTCPPort_OnDataIn

        private void OnConnected(TcpClient sender)
        {
            if (!string.IsNullOrEmpty(txtAGWUserId.Text.Trim()))
            {
                LoginAGWRemote();  // do a secure AGWPE login 
            }

            bytTCPData = new byte[0];
            RequestAGWPortInfo();    // Request port info from AGWPE
        } // IPWTCPort_OnReadyToSend

        private void LoginAGWRemote()
        {
            // Private Sub to Login to remote AGWPE with UserID and password ("P" Frame)...

            try
            {
                var bytTemp2 = new byte[546];
                var asc = new ASCIIEncoding(); // had to declare this to eliminate an ocasional error
                asc.GetBytes("P", 0, 1, bytTemp2, 4);
                Array.Copy(Globals.ComputeLengthB(255 + 255), 0, bytTemp2, 28, 4);
                asc.GetBytes(txtAGWUserId.Text.Trim(), 0, txtAGWUserId.Text.Trim().Length, bytTemp2, 36);
                asc.GetBytes(txtAGWPassword.Text.Trim(), 0, txtAGWPassword.Text.Trim().Length, bytTemp2, 36 + 255);
                objTCPPort.GetStream().Write(bytTemp2, 0, bytTemp2.Length);;
            }
            catch (Exception ex)
            {
                _log.Error("[AGWEngine, LoginAGWRemote] " + ex.Message);
                tmrTimer10sec.Enabled = false;
                SetRemoteButtonStatus(true, Color.Tomato);
            }
        } // LoginAGWRemote 

        private void RequestAGWPortInfo()
        {
            // Private Sub to Request AGW Port Information ("G" Frame)...

            var bytTemp = new byte[36];
            try
            {
                if (!objTCPPort.Connected)
                    return;
                bytTemp[4] = (byte)Globals.Asc('G');
                objTCPPort.GetStream().Write(bytTemp, 0, bytTemp.Length);;
            }
            catch (Exception ex)
            {
                _log.Error("[AGWEngine, RequestAGWPortInfo] " + ex.Message);
                tmrTimer10sec.Enabled = false;
                SetRemoteButtonStatus(true, Color.Tomato);
            }
        } // RequestAGWPortInfo

        private void tmrTimer10sec_Tick(object sender, EventArgs e)
        {
            tmrTimer10sec.Enabled = false;
            SetRemoteButtonStatus(true, Color.Tomato);
            _log.Error("[AGWEngine]10 sec timeout on remote AGWPE port info Request to " + txtAGWHost.Text);
            try
            {
                objTCPPort.Close();
                objTCPPort = null;
            }
            catch
            {
            }
        } // tmrTimer10sec_Tick

        private void SetRemoteButtonStatus(bool Enabled, Color BackColor)
        {
            if (btnRemote.InvokeRequired)
            {
                var objButtonDelegate = new SetRemoteButtonStatusCallback(SetRemoteButtonStatus);
                Invoke(objButtonDelegate, new object[] { Enabled, BackColor });
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
            SetRemoteButtonStatus(false, Color.Yellow);
            try
            {
                if (objTCPPort != null)
                {
                    objTCPPort.Close();
                    objTCPPort = null;
                }
                tmrTimer10sec.Enabled = true;
                objTCPPort = new TcpClient();
                if (Globals.strLocalIPAddress != "Default")
                {
                    objTCPPort.Client.Bind(new IPEndPoint(IPAddress.Parse(Globals.strLocalIPAddress), 0));
                }
                objTCPPort.ConnectAsync(txtAGWHost.Text.Trim(), Convert.ToInt32(txtAGWPort.Text.Trim())).ContinueWith(t =>
                {
                    OnConnected(objTCPPort);

                    byte[] buffer = new byte[1024];

                    Task<int> task = null;
                    try
                    {
                        task = objTCPPort.GetStream().ReadAsync(buffer, 0, 1024);
                        task.ContinueWith(t =>
                        {
                            OnDataIn(buffer, t.Result);
                        });
                        task.Wait(0);
                    }
                    catch (Exception e)
                    {
                        // empty
                    }
                }).Wait(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not connect...Error in AGWPE Address, Port, User or Password Port : " + ex.Message);
                SetRemoteButtonStatus(true, Color.Tomato);
                tmrTimer10sec.Enabled = false;
            }
        } // btnRemote_Click

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (rdoNotUsed.Checked)
            {
                AGWLocation = 0;
            }
            else if (rdoLocal.Checked)
            {
                AGWLocation = 1;
            }
            else
            {
                AGWLocation = 2;
            }

            if (AGWLocation == 1)
            {
                if (txtAGWPath.Text.Trim().EndsWith(@"\") == false)
                    txtAGWPath.Text = txtAGWPath.Text.Trim() + @"\";
                if (!File.Exists(txtAGWPath.Text + "AGWPE.ini"))
                {
                    MessageBox.Show("AGWPE Path incorrect or AGWPE not yet configured!", "Missing AGWPE.ini File", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            AGWPath = txtAGWPath.Text;
            AGWHost = txtAGWHost.Text;
            AGWTCPPort = Convert.ToInt32(txtAGWPort.Text);
            AGWUserId = txtAGWUserId.Text;
            AGWPassword = txtAGWPassword.Text;
            Globals.Settings.Save(Application.ProductName, "AGW Location", AGWLocation);
            Globals.Settings.Save(Application.ProductName, "AGW Port", AGWTCPPort);
            Globals.Settings.Save(Application.ProductName, "AGW Path", AGWPath);
            Globals.Settings.Save(Application.ProductName, "AGW Host", AGWHost);
            Globals.Settings.Save(Application.ProductName, "AGW User Id", AGWUserId);
            Globals.Settings.Save(Application.ProductName, "AGW Password", AGWPassword);
            Close();
        } // btnUpdate_Click

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        } // btnCancel_Click

        private void btnHelp_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, Globals.SiteRootDirectory + @"Help\Paclink.chm", HelpNavigator.Topic, @"html\hs70.htm");
        } // btnHelp_Click
    }
} // AGWEngine