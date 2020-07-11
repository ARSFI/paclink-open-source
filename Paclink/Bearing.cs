using System;
using System.Drawing;
using System.Windows.Forms;

namespace Paclink
{
    public partial class Bearing
    {
        public Bearing()
        {
            InitializeComponent();
            _pnlRadar.Name = "pnlRadar";
        }

        private double dblRange; // Range in statute miles
        private double dblBearing; // Bearing in degrees True Great circle
        private RangeBearing objRangeBearing = new RangeBearing();

        private void Bearing_Activated(object sender, EventArgs e)
        {
            DrawRangeBearing();
        }

        private void Bearing_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                Globals.objINIFile.WriteInteger("Bearing", "Top", Top);
                Globals.objINIFile.WriteInteger("Bearing", "Left", Left);
            }
        }

        private void Bearing_Load(object sender, EventArgs e)
        {
            objRangeBearing.ComputeRangeAndBearing(Globals.SiteGridSquare, Globals.strConnectedGridSquare, ref dblRange, ref dblBearing);
            Top = Globals.objINIFile.GetInteger("Bearing", "Top", 100);
            Left = Globals.objINIFile.GetInteger("Bearing", "Left", 100);
            tmrShow.Start();
        }

        private bool blnFirstTime = true;

        private void tmrShow_Tick(object sender, EventArgs e)
        {
            if (blnFirstTime == true)
            {
                blnFirstTime = false;
                tmrShow.Stop();
                DrawRangeBearing();
                tmrShow.Interval = 2000;
                tmrShow.Start();
            }
            else if (Globals.blnEndBearingDisplay == true)
            {
                Globals.blnEndBearingDisplay = false;
                Close();
            }
            else
            {
                DrawRangeBearing();
            }
        }

        private void DrawRangeBearing()
        {
            // Subroutine to draw the radar plot with range rings for range and bearing...
            double dblDispRad; // The radius of the display from the center
            double dblRingRad;
            var graRangeBearing = pnlRadar.CreateGraphics();
            var fnt = DefaultFont;
            try
            {
                // Compute scaling factor using a margin of 10 display units (assume Height is <=  Width)
                double dblMax = (pnlRadar.Height - 10) / (double)2; // This represents 10,800 nm, the longest possible great circle range
                                                                    // Implement a log scale selecting 1sm as the center stating point.
                                                                    // The scale factors below are based on Log(max value) - 1
                                                                    // E.g. max sm value = 10800 nm x 1.15 = 12096 Log10(12096) = 4.08264 
                                                                    // Any value less than 1 sm is plotted in the center
                dblRingRad = dblMax / 4.08264; // 4 rings representing 10, 100, 1000 and 10000sm 
                dblDispRad = Math.Max(Math.Log10(dblRange) * dblRingRad, 0); // the plotting radius for the point
                graRangeBearing.Clear(Color.Black);
                // Draw the rings
                var objPen = new Pen(Color.Lime, 1);
                int intYctr = (int)(pnlRadar.Height / (double)2);
                int intXctr = (int)(pnlRadar.Width / (double)2);
                int intOffset = Convert.ToInt32(dblRingRad);
                graRangeBearing.DrawEllipse(objPen, intXctr - intOffset, intYctr - intOffset, (int)(2 * dblRingRad), (int)(2 * dblRingRad));
                graRangeBearing.DrawEllipse(objPen, intXctr - 2 * intOffset, intYctr - 2 * intOffset, (int)(4 * dblRingRad), (int)(4 * dblRingRad));
                graRangeBearing.DrawEllipse(objPen, intXctr - 3 * intOffset, intYctr - 3 * intOffset, (int)(6 * dblRingRad), (int)(6 * dblRingRad));
                graRangeBearing.DrawEllipse(objPen, intXctr - 4 * intOffset, intYctr - 4 * intOffset, (int)(8 * dblRingRad), (int)(8 * dblRingRad));
                if (dblRange >= 0)
                {
                    // Plot the point
                    objPen.Color = Color.Tomato;
                    objPen.Width = 2;
                    int intXplot = (int)(intXctr + Math.Sin(dblBearing * Math.PI / 180) * dblDispRad);
                    int intYplot = (int)(intYctr - Math.Cos(dblBearing * Math.PI / 180) * dblDispRad);
                    graRangeBearing.DrawEllipse(objPen, intXplot - 3, intYplot - 3, 6, 6);
                    // Draw the line to the point...
                    objPen.Width = 1;
                    objPen.Color = Color.Yellow;
                    graRangeBearing.DrawLine(objPen, intXctr, intYctr, intXplot, intYplot);

                    // Draw the range and bearing strings... 
                    graRangeBearing.DrawString(dblRange.ToString("####0") + " sm", fnt, Brushes.Yellow, 5, 5);
                    graRangeBearing.DrawString(dblBearing.ToString("000") + "T", fnt, Brushes.Yellow, pnlRadar.Width - 40, 5);
                }

                if (!string.IsNullOrEmpty(Globals.strConnectedCallsign)) // plot the callsign in the lower right corner
                {
                    graRangeBearing.DrawString(Globals.strConnectedCallsign, fnt, Brushes.Yellow, pnlRadar.Width - 60, pnlRadar.Height - 15);
                }

                // Plot the UTC time in the lower left...
                string strStartTime = DateTime.UtcNow.ToString("HH:mm:ss") + " UTC";
                graRangeBearing.DrawString(strStartTime, fnt, Brushes.Yellow, 5, pnlRadar.Height - 15);
                objPen.Dispose();
                graRangeBearing.Dispose();
            }
            catch (Exception e)
            {
                Logs.Exception("[Radar.DrawRangeBearing] " + e.Message);
            }
        } // Radar.DrawRangeBearing
    }
}