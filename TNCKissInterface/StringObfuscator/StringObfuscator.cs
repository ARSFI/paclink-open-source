using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace StringObfuscator
{
    public partial class StringObfuscator : Form
    {
        public StringObfuscator()
        {
            InitializeComponent();
        }

        Random byteRandom;
        StringBuilder sb;
        String CRLF = Convert.ToString((Char)13) + Convert.ToString((Char)10);

        private void CreateButton_Click(object sender, EventArgs e)
        {
            //
            // Create the output array
            //
            OutputTextBox.Text = "";
            OutputTextBox.Refresh();

            if (InputTextBox.Text.Length == 0)
            {
                return;
            }

            Int32 l = (4 - ((InputTextBox.Text.Length) % 4)) % 4;

            Byte[] tmpB = Encoding.ASCII.GetBytes(InputTextBox.Text +
                sb.ToString().Substring(0, l));
            Int32 f = tmpB.Length;
            Byte[] tmpR = new Byte[f * 2];

            byteRandom.NextBytes(tmpR);

            for (Int32 i = 0; i < tmpB.Length; i++)
            {
                tmpR[i + f] = (Byte)(tmpB[i] ^ tmpR[i]);
            }

            BuildArray(tmpR, 4, "tmpR");
        }

        private void StringObfuscator_Load(object sender, EventArgs e)
        {
            byteRandom = new Random();
            sb = new StringBuilder();
            for (Int32 r = 0; r < 1024; r++)
            {
                sb.Append(" ");
            }
        }

        void BuildArray(Byte[] b, Int32 m, String bn)
        {
            Int32 tmpI = 0;
            StringBuilder tmpSB = new StringBuilder("        Byte [,] " + bn + " = { ");
            for (Int32 i = 0; i < b.Length; i++)
            {
                if (tmpI == 0)
                {
                    tmpSB.Append("{ ");
                }
                tmpI = (tmpI + 1) % m;

                tmpSB.Append("0x" + b[i].ToString("x2"));
                if (tmpI == 0)
                {
                    tmpSB.Append(" }");
                }
                if (i < (b.Length - 1))
                {
                    tmpSB.Append(", ");
                    if (tmpI == 0)
                    {
                        tmpSB.Append(CRLF);
                        tmpSB.Append("                         ");
                    }
                }
            }
            tmpSB.Append(" };" + CRLF);
            OutputTextBox.Text = tmpSB.ToString();
        }
    }
}
