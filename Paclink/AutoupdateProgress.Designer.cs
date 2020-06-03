using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace Paclink
{
    [DesignerGenerated()]
    public partial class AutoupdateProgress : Form
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoupdateProgress));
            _Label1 = new Label();
            _Label2 = new Label();
            _txtProgress = new TextBox();
            _tmrUpdate = new Timer(components);
            _tmrUpdate.Tick += new EventHandler(tmrUpdate_Tick);
            SuspendLayout();
            // 
            // Label1
            // 
            _Label1.AutoSize = true;
            _Label1.Location = new Point(33, 23);
            _Label1.Name = "_Label1";
            _Label1.Size = new Size(129, 13);
            _Label1.TabIndex = 0;
            _Label1.Text = "Autoupdate is in progress.";
            _Label1.UseWaitCursor = true;
            // 
            // Label2
            // 
            _Label2.AutoSize = true;
            _Label2.Location = new Point(33, 44);
            _Label2.Name = "_Label2";
            _Label2.Size = new Size(91, 13);
            _Label2.TabIndex = 1;
            _Label2.Text = "Please stand by...";
            _Label2.UseWaitCursor = true;
            // 
            // txtProgress
            // 
            _txtProgress.Location = new Point(33, 72);
            _txtProgress.Name = "_txtProgress";
            _txtProgress.ReadOnly = true;
            _txtProgress.Size = new Size(218, 20);
            _txtProgress.TabIndex = 2;
            _txtProgress.UseWaitCursor = true;
            // 
            // tmrUpdate
            // 
            _tmrUpdate.Enabled = true;
            _tmrUpdate.Interval = 200;
            // 
            // AutoupdateProgress
            // 
            AutoScaleDimensions = new SizeF(6.0F, 13.0F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(284, 127);
            ControlBox = false;
            Controls.Add(_txtProgress);
            Controls.Add(_Label2);
            Controls.Add(_Label1);
            Cursor = Cursors.WaitCursor;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AutoupdateProgress";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Paclink Autoupdate Progress";
            TopMost = true;
            UseWaitCursor = true;
            ResumeLayout(false);
            PerformLayout();
        }

        private Label _Label1;

        internal Label Label1
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Label1;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Label1 != null)
                {
                }

                _Label1 = value;
                if (_Label1 != null)
                {
                }
            }
        }

        private Label _Label2;

        internal Label Label2
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Label2;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Label2 != null)
                {
                }

                _Label2 = value;
                if (_Label2 != null)
                {
                }
            }
        }

        private TextBox _txtProgress;

        internal TextBox txtProgress
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtProgress;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_txtProgress != null)
                {
                }

                _txtProgress = value;
                if (_txtProgress != null)
                {
                }
            }
        }

        private Timer _tmrUpdate;

        internal Timer tmrUpdate
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _tmrUpdate;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_tmrUpdate != null)
                {
                    _tmrUpdate.Tick -= tmrUpdate_Tick;
                }

                _tmrUpdate = value;
                if (_tmrUpdate != null)
                {
                    _tmrUpdate.Tick += tmrUpdate_Tick;
                }
            }
        }
    }
}