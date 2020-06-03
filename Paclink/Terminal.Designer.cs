using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace Paclink
{
    [DesignerGenerated()]
    public partial class Terminal : Form
    {

        // Form overrides dispose to clean up the component list.
        [DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            if (disposing && components is object)
            {
                components.Dispose();
            }

            base.Dispose(disposing);
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(Terminal));
            _txtKeyboard = new TextBox();
            _txtKeyboard.KeyPress += new KeyPressEventHandler(txtKeyboard_KeyPress);
            _txtKeyboard.KeyUp += new KeyEventHandler(txtKeyboard_KeyUp);
            _mnuMain = new MenuStrip();
            _mnuClose = new ToolStripMenuItem();
            _mnuClose.Click += new EventHandler(mnuClose_Click);
            _mnuProperties = new ToolStripMenuItem();
            _mnuProperties.Click += new EventHandler(mnuProperties_Click);
            _mnuClearDisplay = new ToolStripMenuItem();
            _mnuClearDisplay.Click += new EventHandler(mnuClearDisplay_Click);
            _mnuClearHost = new ToolStripMenuItem();
            _mnuClearSCSHostMode = new ToolStripMenuItem();
            _mnuClearSCSHostMode.Click += new EventHandler(mnuClearSCSHostMode_Click);
            _mnuClearKantronicsHostMode = new ToolStripMenuItem();
            _mnuClearKantronicsHostMode.Click += new EventHandler(mnuClearKantronicsHostMode_Click);
            _mnuClearTimewaveHostMode = new ToolStripMenuItem();
            _mnuClearTimewaveHostMode.Click += new EventHandler(mnuClearTimewaveHostMode_Click);
            _mnuClearKiss = new ToolStripMenuItem();
            _mnuClearKiss.Click += new EventHandler(mnuClearKiss_Click);
            _mnuViewLog = new ToolStripMenuItem();
            _mnuViewLog.Click += new EventHandler(mnuViewLog_Click);
            _txtDisplay = new TextBox();
            _txtDisplay.KeyUp += new KeyEventHandler(txtDisplay_KeyUp);
            _txtDisplay.MouseUp += new MouseEventHandler(txtDisplay_MouseUp);
            _tmrTerminal = new Timer(components);
            _tmrTerminal.Tick += new EventHandler(tmrTerminal_Tick);
            _objSerialPort = new System.IO.Ports.SerialPort(components);
            _mnuMain.SuspendLayout();
            SuspendLayout();
            // 
            // txtKeyboard
            // 
            _txtKeyboard.Dock = DockStyle.Bottom;
            _txtKeyboard.Font = new Font("Courier New", 12.0F, FontStyle.Regular, GraphicsUnit.Point, Conversions.ToByte(0));
            _txtKeyboard.Location = new Point(0, 479);
            _txtKeyboard.Name = "_txtKeyboard";
            _txtKeyboard.Size = new Size(634, 26);
            _txtKeyboard.TabIndex = 0;
            // 
            // mnuMain
            // 
            _mnuMain.Items.AddRange(new ToolStripItem[] { _mnuClose, _mnuProperties, _mnuClearDisplay, _mnuClearHost, _mnuClearKiss, _mnuViewLog });
            _mnuMain.Location = new Point(0, 0);
            _mnuMain.Name = "_mnuMain";
            _mnuMain.Size = new Size(634, 24);
            _mnuMain.TabIndex = 1;
            _mnuMain.Text = "mnuMain";
            // 
            // mnuClose
            // 
            _mnuClose.Name = "_mnuClose";
            _mnuClose.Size = new Size(51, 20);
            _mnuClose.Text = "Close";
            // 
            // mnuProperties
            // 
            _mnuProperties.Name = "_mnuProperties";
            _mnuProperties.Size = new Size(78, 20);
            _mnuProperties.Text = "Settings...";
            // 
            // mnuClearDisplay
            // 
            _mnuClearDisplay.Name = "_mnuClearDisplay";
            _mnuClearDisplay.Size = new Size(94, 20);
            _mnuClearDisplay.Text = "Clear Display";
            // 
            // mnuClearHost
            // 
            _mnuClearHost.DropDownItems.AddRange(new ToolStripItem[] { _mnuClearSCSHostMode, _mnuClearKantronicsHostMode, _mnuClearTimewaveHostMode });
            _mnuClearHost.Name = "_mnuClearHost";
            _mnuClearHost.Size = new Size(114, 20);
            _mnuClearHost.Text = "Clear Host Mode";
            // 
            // mnuClearSCSHostMode
            // 
            _mnuClearSCSHostMode.Name = "_mnuClearSCSHostMode";
            _mnuClearSCSHostMode.Size = new Size(176, 22);
            _mnuClearSCSHostMode.Text = "SCS";
            // 
            // mnuClearKantronicsHostMode
            // 
            _mnuClearKantronicsHostMode.Name = "_mnuClearKantronicsHostMode";
            _mnuClearKantronicsHostMode.Size = new Size(176, 22);
            _mnuClearKantronicsHostMode.Text = "Kantronics";
            // 
            // mnuClearTimewaveHostMode
            // 
            _mnuClearTimewaveHostMode.Name = "_mnuClearTimewaveHostMode";
            _mnuClearTimewaveHostMode.Size = new Size(176, 22);
            _mnuClearTimewaveHostMode.Text = "AEA/Timewave";
            // 
            // mnuClearKiss
            // 
            _mnuClearKiss.Name = "_mnuClearKiss";
            _mnuClearKiss.Size = new Size(111, 20);
            _mnuClearKiss.Text = "Clear Kiss Mode";
            // 
            // mnuViewLog
            // 
            _mnuViewLog.Name = "_mnuViewLog";
            _mnuViewLog.Size = new Size(84, 20);
            _mnuViewLog.Text = "View Log...";
            // 
            // txtDisplay
            // 
            _txtDisplay.BackColor = Color.White;
            _txtDisplay.Dock = DockStyle.Fill;
            _txtDisplay.Font = new Font("Courier New", 12.0F, FontStyle.Regular, GraphicsUnit.Point, Conversions.ToByte(0));
            _txtDisplay.Location = new Point(0, 24);
            _txtDisplay.MaxLength = 0;
            _txtDisplay.Multiline = true;
            _txtDisplay.Name = "_txtDisplay";
            _txtDisplay.ReadOnly = true;
            _txtDisplay.ScrollBars = ScrollBars.Both;
            _txtDisplay.Size = new Size(634, 455);
            _txtDisplay.TabIndex = 2;
            _txtDisplay.WordWrap = false;
            // 
            // tmrTerminal
            // 
            // 
            // Terminal
            // 
            AutoScaleDimensions = new SizeF(6.0F, 13.0F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(634, 505);
            Controls.Add(_txtDisplay);
            Controls.Add(_txtKeyboard);
            Controls.Add(_mnuMain);
            Icon = (Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            MainMenuStrip = _mnuMain;
            Name = "Terminal";
            Text = "Simple Terminal";
            _mnuMain.ResumeLayout(false);
            _mnuMain.PerformLayout();
            Load += new EventHandler(Terminal_Load);
            Activated += new EventHandler(Terminal_Activated);
            FormClosed += new FormClosedEventHandler(Terminal_FormClosed);
            KeyPress += new KeyPressEventHandler(Terminal_KeyPress);
            ResumeLayout(false);
            PerformLayout();
        }

        private TextBox _txtKeyboard;

        internal TextBox txtKeyboard
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtKeyboard;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_txtKeyboard != null)
                {
                    _txtKeyboard.KeyPress -= txtKeyboard_KeyPress;
                    _txtKeyboard.KeyUp -= txtKeyboard_KeyUp;
                }

                _txtKeyboard = value;
                if (_txtKeyboard != null)
                {
                    _txtKeyboard.KeyPress += txtKeyboard_KeyPress;
                    _txtKeyboard.KeyUp += txtKeyboard_KeyUp;
                }
            }
        }

        private MenuStrip _mnuMain;

        internal MenuStrip mnuMain
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

        private ToolStripMenuItem _mnuClose;

        internal ToolStripMenuItem mnuClose
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

        private TextBox _txtDisplay;

        internal TextBox txtDisplay
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtDisplay;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_txtDisplay != null)
                {
                    _txtDisplay.KeyUp -= txtDisplay_KeyUp;
                    _txtDisplay.MouseUp -= txtDisplay_MouseUp;
                }

                _txtDisplay = value;
                if (_txtDisplay != null)
                {
                    _txtDisplay.KeyUp += txtDisplay_KeyUp;
                    _txtDisplay.MouseUp += txtDisplay_MouseUp;
                }
            }
        }

        private Timer _tmrTerminal;

        internal Timer tmrTerminal
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _tmrTerminal;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_tmrTerminal != null)
                {
                    _tmrTerminal.Tick -= tmrTerminal_Tick;
                }

                _tmrTerminal = value;
                if (_tmrTerminal != null)
                {
                    _tmrTerminal.Tick += tmrTerminal_Tick;
                }
            }
        }

        private ToolStripMenuItem _mnuClearDisplay;

        internal ToolStripMenuItem mnuClearDisplay
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _mnuClearDisplay;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_mnuClearDisplay != null)
                {
                    _mnuClearDisplay.Click -= mnuClearDisplay_Click;
                }

                _mnuClearDisplay = value;
                if (_mnuClearDisplay != null)
                {
                    _mnuClearDisplay.Click += mnuClearDisplay_Click;
                }
            }
        }

        private System.IO.Ports.SerialPort _objSerialPort;

        internal System.IO.Ports.SerialPort objSerialPort
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _objSerialPort;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_objSerialPort != null)
                {
                }

                _objSerialPort = value;
                if (_objSerialPort != null)
                {
                }
            }
        }

        private ToolStripMenuItem _mnuViewLog;

        internal ToolStripMenuItem mnuViewLog
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _mnuViewLog;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_mnuViewLog != null)
                {
                    _mnuViewLog.Click -= mnuViewLog_Click;
                }

                _mnuViewLog = value;
                if (_mnuViewLog != null)
                {
                    _mnuViewLog.Click += mnuViewLog_Click;
                }
            }
        }

        private ToolStripMenuItem _mnuClearHost;

        internal ToolStripMenuItem mnuClearHost
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _mnuClearHost;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_mnuClearHost != null)
                {
                }

                _mnuClearHost = value;
                if (_mnuClearHost != null)
                {
                }
            }
        }

        private ToolStripMenuItem _mnuClearKiss;

        internal ToolStripMenuItem mnuClearKiss
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _mnuClearKiss;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_mnuClearKiss != null)
                {
                    _mnuClearKiss.Click -= mnuClearKiss_Click;
                }

                _mnuClearKiss = value;
                if (_mnuClearKiss != null)
                {
                    _mnuClearKiss.Click += mnuClearKiss_Click;
                }
            }
        }

        private ToolStripMenuItem _mnuProperties;

        internal ToolStripMenuItem mnuProperties
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _mnuProperties;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_mnuProperties != null)
                {
                    _mnuProperties.Click -= mnuProperties_Click;
                }

                _mnuProperties = value;
                if (_mnuProperties != null)
                {
                    _mnuProperties.Click += mnuProperties_Click;
                }
            }
        }

        private ToolStripMenuItem _mnuClearSCSHostMode;

        internal ToolStripMenuItem mnuClearSCSHostMode
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _mnuClearSCSHostMode;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_mnuClearSCSHostMode != null)
                {
                    _mnuClearSCSHostMode.Click -= mnuClearSCSHostMode_Click;
                }

                _mnuClearSCSHostMode = value;
                if (_mnuClearSCSHostMode != null)
                {
                    _mnuClearSCSHostMode.Click += mnuClearSCSHostMode_Click;
                }
            }
        }

        private ToolStripMenuItem _mnuClearKantronicsHostMode;

        internal ToolStripMenuItem mnuClearKantronicsHostMode
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _mnuClearKantronicsHostMode;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_mnuClearKantronicsHostMode != null)
                {
                    _mnuClearKantronicsHostMode.Click -= mnuClearKantronicsHostMode_Click;
                }

                _mnuClearKantronicsHostMode = value;
                if (_mnuClearKantronicsHostMode != null)
                {
                    _mnuClearKantronicsHostMode.Click += mnuClearKantronicsHostMode_Click;
                }
            }
        }

        private ToolStripMenuItem _mnuClearTimewaveHostMode;

        internal ToolStripMenuItem mnuClearTimewaveHostMode
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _mnuClearTimewaveHostMode;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_mnuClearTimewaveHostMode != null)
                {
                    _mnuClearTimewaveHostMode.Click -= mnuClearTimewaveHostMode_Click;
                }

                _mnuClearTimewaveHostMode = value;
                if (_mnuClearTimewaveHostMode != null)
                {
                    _mnuClearTimewaveHostMode.Click += mnuClearTimewaveHostMode_Click;
                }
            }
        }
    }
}