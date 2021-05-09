namespace StringObfuscator
{
    partial class StringObfuscator
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.CreateButton = new System.Windows.Forms.Button();
            this.InputTextBox = new System.Windows.Forms.TextBox();
            this.InputGroupBox = new System.Windows.Forms.GroupBox();
            this.OutputGroupBox = new System.Windows.Forms.GroupBox();
            this.OutputTextBox = new System.Windows.Forms.TextBox();
            this.InputGroupBox.SuspendLayout();
            this.OutputGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // CreateButton
            // 
            this.CreateButton.Location = new System.Drawing.Point(6, 39);
            this.CreateButton.Name = "CreateButton";
            this.CreateButton.Size = new System.Drawing.Size(75, 23);
            this.CreateButton.TabIndex = 0;
            this.CreateButton.Text = "Create";
            this.CreateButton.UseVisualStyleBackColor = true;
            this.CreateButton.Click += new System.EventHandler(this.CreateButton_Click);
            // 
            // InputTextBox
            // 
            this.InputTextBox.Location = new System.Drawing.Point(6, 13);
            this.InputTextBox.Name = "InputTextBox";
            this.InputTextBox.Size = new System.Drawing.Size(579, 20);
            this.InputTextBox.TabIndex = 1;
            // 
            // InputGroupBox
            // 
            this.InputGroupBox.Controls.Add(this.InputTextBox);
            this.InputGroupBox.Controls.Add(this.CreateButton);
            this.InputGroupBox.Location = new System.Drawing.Point(12, 12);
            this.InputGroupBox.Name = "InputGroupBox";
            this.InputGroupBox.Size = new System.Drawing.Size(591, 73);
            this.InputGroupBox.TabIndex = 2;
            this.InputGroupBox.TabStop = false;
            this.InputGroupBox.Text = "Input String";
            // 
            // OutputGroupBox
            // 
            this.OutputGroupBox.Controls.Add(this.OutputTextBox);
            this.OutputGroupBox.Location = new System.Drawing.Point(12, 105);
            this.OutputGroupBox.Name = "OutputGroupBox";
            this.OutputGroupBox.Size = new System.Drawing.Size(591, 186);
            this.OutputGroupBox.TabIndex = 3;
            this.OutputGroupBox.TabStop = false;
            this.OutputGroupBox.Text = "Output Array";
            // 
            // OutputTextBox
            // 
            this.OutputTextBox.BackColor = System.Drawing.SystemColors.Info;
            this.OutputTextBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OutputTextBox.Location = new System.Drawing.Point(6, 13);
            this.OutputTextBox.Multiline = true;
            this.OutputTextBox.Name = "OutputTextBox";
            this.OutputTextBox.ReadOnly = true;
            this.OutputTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.OutputTextBox.Size = new System.Drawing.Size(579, 167);
            this.OutputTextBox.TabIndex = 1;
            this.OutputTextBox.WordWrap = false;
            // 
            // StringObfuscator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(615, 303);
            this.Controls.Add(this.OutputGroupBox);
            this.Controls.Add(this.InputGroupBox);
            this.Name = "StringObfuscator";
            this.Text = "String Obfuscator";
            this.Load += new System.EventHandler(this.StringObfuscator_Load);
            this.InputGroupBox.ResumeLayout(false);
            this.InputGroupBox.PerformLayout();
            this.OutputGroupBox.ResumeLayout(false);
            this.OutputGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button CreateButton;
        private System.Windows.Forms.TextBox InputTextBox;
        private System.Windows.Forms.GroupBox InputGroupBox;
        private System.Windows.Forms.GroupBox OutputGroupBox;
        private System.Windows.Forms.TextBox OutputTextBox;
    }
}

