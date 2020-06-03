using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace Paclink
{
    [DesignerGenerated()]
    public partial class Bearing : Form
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
            components = new System.ComponentModel.Container();
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(Bearing));
            _pnlRadar = new Panel();
            _tmrShow = new Timer(components);
            _tmrShow.Tick += new EventHandler(tmrShow_Tick);
            SuspendLayout();
            // 
            // pnlRadar
            // 
            _pnlRadar.AutoSize = true;
            _pnlRadar.BackColor = Color.Black;
            _pnlRadar.Dock = DockStyle.Fill;
            _pnlRadar.Enabled = false;
            _pnlRadar.Location = new Point(0, 0);
            _pnlRadar.Name = "_pnlRadar";
            _pnlRadar.Size = new Size(214, 218);
            _pnlRadar.TabIndex = 24;
            // 
            // tmrShow
            // 
            _tmrShow.Interval = 10;
            // 
            // Bearing
            // 
            AutoScaleDimensions = new SizeF(6.0F, 13.0F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(214, 218);
            ControlBox = false;
            Controls.Add(_pnlRadar);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Bearing";
            ShowIcon = false;
            ShowInTaskbar = false;
            Text = "Range and Bearing";
            Activated += new EventHandler(Bearing_Activated);
            FormClosed += new FormClosedEventHandler(Bearing_FormClosed);
            Load += new EventHandler(Bearing_Load);
            ResumeLayout(false);
            PerformLayout();
        }

        private Panel _pnlRadar;

        internal Panel pnlRadar
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _pnlRadar;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_pnlRadar != null)
                {
                }

                _pnlRadar = value;
                if (_pnlRadar != null)
                {
                }
            }
        }

        private Timer _tmrShow;

        internal Timer tmrShow
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _tmrShow;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_tmrShow != null)
                {
                    _tmrShow.Tick -= tmrShow_Tick;
                }

                _tmrShow = value;
                if (_tmrShow != null)
                {
                    _tmrShow.Tick += tmrShow_Tick;
                }
            }
        }
    }
}