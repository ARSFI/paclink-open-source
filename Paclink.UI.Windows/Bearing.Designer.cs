using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Paclink.UI.Windows
{
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Bearing));
            this._pnlRadar = new System.Windows.Forms.Panel();
            this._tmrShow = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // _pnlRadar
            // 
            this._pnlRadar.AutoSize = true;
            this._pnlRadar.BackColor = System.Drawing.Color.Black;
            this._pnlRadar.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pnlRadar.Enabled = false;
            this._pnlRadar.Location = new System.Drawing.Point(0, 0);
            this._pnlRadar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._pnlRadar.Name = "_pnlRadar";
            this._pnlRadar.Size = new System.Drawing.Size(241, 243);
            this._pnlRadar.TabIndex = 24;
            // 
            // _tmrShow
            // 
            this._tmrShow.Interval = 10;
            this._tmrShow.Tick += new System.EventHandler(this.tmrShow_Tick);
            // 
            // Bearing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(241, 243);
            this.ControlBox = false;
            this.Controls.Add(this._pnlRadar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Bearing";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Range and Bearing";
            this.Activated += new System.EventHandler(this.Bearing_Activated);
            this.ResumeLayout(false);
            this.PerformLayout();

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