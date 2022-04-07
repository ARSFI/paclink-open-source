using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Paclink.UI.Windows
{
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Terminal));
            this._txtKeyboard = new System.Windows.Forms.TextBox();
            this._mnuMain = new System.Windows.Forms.MenuStrip();
            this._mnuClose = new System.Windows.Forms.ToolStripMenuItem();
            this._mnuProperties = new System.Windows.Forms.ToolStripMenuItem();
            this._mnuClearDisplay = new System.Windows.Forms.ToolStripMenuItem();
            this._mnuClearHost = new System.Windows.Forms.ToolStripMenuItem();
            this._mnuClearSCSHostMode = new System.Windows.Forms.ToolStripMenuItem();
            this._mnuClearKantronicsHostMode = new System.Windows.Forms.ToolStripMenuItem();
            this._mnuClearTimewaveHostMode = new System.Windows.Forms.ToolStripMenuItem();
            this._mnuClearKiss = new System.Windows.Forms.ToolStripMenuItem();
            this._txtDisplay = new System.Windows.Forms.TextBox();
            this._tmrTerminal = new System.Windows.Forms.Timer(this.components);
            this._objSerialPort = new System.IO.Ports.SerialPort(components);
            this._mnuMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // _txtKeyboard
            // 
            this._txtKeyboard.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._txtKeyboard.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this._txtKeyboard.Location = new System.Drawing.Point(0, 557);
            this._txtKeyboard.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._txtKeyboard.Name = "_txtKeyboard";
            this._txtKeyboard.Size = new System.Drawing.Size(740, 26);
            this._txtKeyboard.TabIndex = 0;
            this._txtKeyboard.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtKeyboard_KeyPress);
            this._txtKeyboard.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtKeyboard_KeyUp);
            // 
            // _mnuMain
            // 
            this._mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._mnuClose,
            this._mnuProperties,
            this._mnuClearDisplay,
            this._mnuClearHost,
            this._mnuClearKiss});
            this._mnuMain.Location = new System.Drawing.Point(0, 0);
            this._mnuMain.Name = "_mnuMain";
            this._mnuMain.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this._mnuMain.Size = new System.Drawing.Size(740, 24);
            this._mnuMain.TabIndex = 1;
            this._mnuMain.Text = "mnuMain";
            // 
            // _mnuClose
            // 
            this._mnuClose.Name = "_mnuClose";
            this._mnuClose.Size = new System.Drawing.Size(48, 20);
            this._mnuClose.Text = "Close";
            this._mnuClose.Click += new System.EventHandler(this.mnuClose_Click);
            // 
            // _mnuProperties
            // 
            this._mnuProperties.Name = "_mnuProperties";
            this._mnuProperties.Size = new System.Drawing.Size(70, 20);
            this._mnuProperties.Text = "Settings...";
            this._mnuProperties.Click += new System.EventHandler(this.mnuProperties_Click);
            // 
            // _mnuClearDisplay
            // 
            this._mnuClearDisplay.Name = "_mnuClearDisplay";
            this._mnuClearDisplay.Size = new System.Drawing.Size(87, 20);
            this._mnuClearDisplay.Text = "Clear Display";
            this._mnuClearDisplay.Click += new System.EventHandler(this.mnuClearDisplay_Click);
            // 
            // _mnuClearHost
            // 
            this._mnuClearHost.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._mnuClearSCSHostMode,
            this._mnuClearKantronicsHostMode,
            this._mnuClearTimewaveHostMode});
            this._mnuClearHost.Name = "_mnuClearHost";
            this._mnuClearHost.Size = new System.Drawing.Size(108, 20);
            this._mnuClearHost.Text = "Clear Host Mode";
            // 
            // _mnuClearSCSHostMode
            // 
            this._mnuClearSCSHostMode.Name = "_mnuClearSCSHostMode";
            this._mnuClearSCSHostMode.Size = new System.Drawing.Size(154, 22);
            this._mnuClearSCSHostMode.Text = "SCS";
            this._mnuClearSCSHostMode.Click += new System.EventHandler(this.mnuClearSCSHostMode_Click);
            // 
            // _mnuClearKantronicsHostMode
            // 
            this._mnuClearKantronicsHostMode.Name = "_mnuClearKantronicsHostMode";
            this._mnuClearKantronicsHostMode.Size = new System.Drawing.Size(154, 22);
            this._mnuClearKantronicsHostMode.Text = "Kantronics";
            this._mnuClearKantronicsHostMode.Click += new System.EventHandler(this.mnuClearKantronicsHostMode_Click);
            // 
            // _mnuClearTimewaveHostMode
            // 
            this._mnuClearTimewaveHostMode.Name = "_mnuClearTimewaveHostMode";
            this._mnuClearTimewaveHostMode.Size = new System.Drawing.Size(154, 22);
            this._mnuClearTimewaveHostMode.Text = "AEA/Timewave";
            this._mnuClearTimewaveHostMode.Click += new System.EventHandler(this.mnuClearTimewaveHostMode_Click);
            // 
            // _mnuClearKiss
            // 
            this._mnuClearKiss.Name = "_mnuClearKiss";
            this._mnuClearKiss.Size = new System.Drawing.Size(103, 20);
            this._mnuClearKiss.Text = "Clear Kiss Mode";
            this._mnuClearKiss.Click += new System.EventHandler(this.mnuClearKiss_Click);
            // 
            // _txtDisplay
            // 
            this._txtDisplay.BackColor = System.Drawing.Color.White;
            this._txtDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this._txtDisplay.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this._txtDisplay.Location = new System.Drawing.Point(0, 24);
            this._txtDisplay.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._txtDisplay.MaxLength = 0;
            this._txtDisplay.Multiline = true;
            this._txtDisplay.Name = "_txtDisplay";
            this._txtDisplay.ReadOnly = true;
            this._txtDisplay.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._txtDisplay.Size = new System.Drawing.Size(740, 533);
            this._txtDisplay.TabIndex = 2;
            this._txtDisplay.WordWrap = false;
            this._txtDisplay.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtDisplay_KeyUp);
            this._txtDisplay.MouseUp += new System.Windows.Forms.MouseEventHandler(this.txtDisplay_MouseUp);
            // 
            // _tmrTerminal
            // 
            this._tmrTerminal.Tick += new System.EventHandler(this.tmrTerminal_Tick);
            // 
            // Terminal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(740, 583);
            this.Controls.Add(this._txtDisplay);
            this.Controls.Add(this._txtKeyboard);
            this.Controls.Add(this._mnuMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this._mnuMain;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "Terminal";
            this.Text = "Simple Terminal";
            this.Activated += new System.EventHandler(this.Terminal_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Terminal_FormClosed);
            this.Load += new System.EventHandler(this.Terminal_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Terminal_KeyPress);
            this._mnuMain.ResumeLayout(false);
            this._mnuMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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