using System;

namespace Paclink.UI.Windows
{
    public class RangeBearing
    {
        // Subrooutine to convert 4 or 6 char gridsquare to NMEA Lat/Lon values with rounding
        private void GridSq2NMEALatLon(string strGridSquare, ref string strLat, ref string strLon)
        {

            // Subroutine to convert Grid square (4 or 6 character)to Lat Lon
            // string sLat will be filled in to NMEA lattitude DDmm.mmN/S
            // string sLon will be filled in to NMEA longitude DDDmm.mmE/W
            // Converted from VB6 Code.

            string strAlpha;
            string strUCGridSq;
            double dblLat;
            double dblLon;
            strUCGridSq = strGridSquare.Trim().ToUpper();
            strAlpha = "ABCDEFGHIJKLMNOPQRSTUVWX";
            dblLat = -100 + (10 * (1 + strAlpha.IndexOf(strUCGridSq.Substring(1, 1))) + Convert.ToInt32(strUCGridSq.Substring(3, 1)));
            if (strAlpha.IndexOf(strUCGridSq.Substring(0, 1)) < 9)
            {
                // West lon no rounding
                dblLon = -200 + (20 * (1 + strAlpha.IndexOf(strUCGridSq.Substring(0, 1))) + 2 * Convert.ToInt32(strUCGridSq.Substring(2, 1)));
            }
            else
            {
                // East lon no rounding
                dblLon = 20 * (1 + strAlpha.IndexOf(strUCGridSq.Substring(0, 1))) - 200 + 2 * Convert.ToInt32(strUCGridSq.Substring(2, 1));
            }

            if (strUCGridSq.Length != 6)
            {
                // round to center of grid square
                dblLat = dblLat + 0.5;
                dblLon = dblLon + 1;
            }
            // Now add the minutes correction converted to decimal deg
            else  // This process the 6 char Grid square case for more accuracy.
            {
                ;
                // should work for both N and S lat
                // this checked out 7/28/2002
                dblLat = dblLat + (2.5 * (1 + strAlpha.IndexOf(strUCGridSq.Substring(5, 1))) - 1.25) / 60;
                dblLon = dblLon + (-2.5 + 5 * (1 + strAlpha.IndexOf(strUCGridSq.Substring(4, 1)))) / 60;
            }
            // now convert to NMEA format string
            int intDeg;
            double dblMin;
            if (dblLat < 0)
            {
                intDeg = Convert.ToInt32(Math.Floor(-dblLat));
                dblMin = -(dblLat + intDeg);
                strLat = intDeg.ToString("00") + (60 * dblMin).ToString("00.00") + "S";
            }
            else
            {
                intDeg = Convert.ToInt32(Math.Floor(dblLat));
                dblMin = dblLat - intDeg;
                strLat = intDeg.ToString("00") + (60 * dblMin).ToString("00.00") + "N";
            }

            if (dblLon < 0)
            {
                intDeg = Convert.ToInt32(Math.Floor(-dblLon));
                dblMin = -(dblLon + intDeg);
                strLon = intDeg.ToString("000") + (60 * dblMin).ToString("00.00") + "W";
            }
            else
            {
                intDeg = Convert.ToInt32(Math.Floor(dblLon));
                dblMin = dblLon - intDeg;
                strLon = intDeg.ToString("000") + (60 * dblMin).ToString("00.00") + "E";
            }
        }  // GridSq2NMEALatLon

        // Function to convert Lat/Lon text to deximal degrees (used for computing distance)
        private double NMEALatLon2Dec(string strLatLon)
        {
            double NMEALatLon2DecRet = default;
            try
            {
                // Function to convert the text lat lon string to a decimal degee.
                // N lat and E lon are +, S lat and W lon are -
                // Input Format is NMEA DDmm.mmN/S or DDDmm.mmE/W  ( e.g 08012.34W) 
                string strCompass;
                strCompass = strLatLon.Substring(strLatLon.Length - 1, 1);
                switch (strCompass)
                {
                    case "N":
                        {
                            NMEALatLon2DecRet = Convert.ToDouble(strLatLon.Substring(0, 2)) + Convert.ToDouble(strLatLon.Substring(2, 5)) / 60;
                            break;
                        }

                    case "S":
                        {
                            NMEALatLon2DecRet = -(Convert.ToDouble(strLatLon.Substring(0, 2)) + Convert.ToDouble(strLatLon.Substring(2, 5)) / 60);
                            break;
                        }

                    case "E":
                        {
                            NMEALatLon2DecRet = Convert.ToDouble(strLatLon.Substring(0, 3)) + Convert.ToDouble(strLatLon.Substring(3, 5)) / 60;
                            break;
                        }

                    case "W":
                        {
                            NMEALatLon2DecRet = -(Convert.ToDouble(strLatLon.Substring(0, 3)) + Convert.ToDouble(strLatLon.Substring(3, 5)) / 60);
                            break;
                        }

                    default:
                        {
                            NMEALatLon2DecRet = 0;  // error case
                            break;
                        }
                }
            }
            catch
            {
                return 0;
            }

            return NMEALatLon2DecRet;
        } // NMEALatLon2Dec

        // Subroutine to Calculate Great circle range and bearing 
        private void RangeAndBearing(ref double dblRange, ref double dblBearing, double dblFromLat, double dblFromLon, double dblToLat, double dblToLon)
        {

            // Subroutine to compute the range and bearing from two lat/lon positions
            // Lat and Lon are in decimal degrees. (not degrees, minutes)
            // +lat = N, -lat = S.  +lon = E, -lon = W
            // lat range -90 to +90   lon range -180 to + 180
            // Computes great circle route (shortest route).
            // bearing is in True degrees (0-360)
            // Range in nautical miles
            // Equations from Bowditch Practical Navigator page 1304
            // Written by Rick Muething

            double dblLO;
            double dblRangeCos;
            double dblRad;
            double dblDenom;
            try
            {
                // First compute difference in Longitude dblLO (must be 180 or less. + for E, - for W)
                dblLO = dblToLon - dblFromLon;
                if (dblLO < -180)
                {
                    dblLO = dblLO + 360;
                }
                else if (dblLO > 180)
                {
                    dblLO = dblLO - 360;
                }
                // Now compute distance (60 nm per degree)
                dblRad = Math.PI / 180; // radians per degree
                dblRangeCos = Math.Sin(dblFromLat * dblRad) * Math.Sin(dblToLat * dblRad) + Math.Cos(dblFromLat * dblRad) * Math.Cos(dblToLat * dblRad) * Math.Cos(dblLO * dblRad);

                // Since VB does not have an Arc Cos function use the identy tan(x) = sin(x)/cos(x) and use Atn function
                if (dblRangeCos > 0)  // Range is within +/- 90 deg
                {
                    dblRange = 60 * 180 / Math.PI * Math.Atan(Math.Sqrt(1 - Math.Pow(dblRangeCos, 2)) / dblRangeCos);
                }
                else  // Range is > 90 deg or < -90 deg
                {
                    dblRange = 60 * (90 + 180 / Math.PI * (Math.PI / 2 - Math.Atan(Math.Sqrt(1 - Math.Pow(dblRangeCos, 2)) / -dblRangeCos)));
                }
                // And now compute great circle bearing: From to To
                dblDenom = Math.Cos(dblFromLat * dblRad) * Math.Tan(dblToLat * dblRad) - Math.Sin(dblFromLat * dblRad) * Math.Cos(dblLO * dblRad);
                // Compute the normal bearing calculation using  4 Quadrant  ATAN2
                // This also handles the error case when Denom = 0
                dblBearing = 180 / Math.PI * Math.Atan2(Math.Sin(dblLO * dblRad), dblDenom);
                // Convert bearing to postive if its a - value
                if (dblBearing < 0)
                    dblBearing = 360 + dblBearing;
            }
            catch
            {
                dblRange = -1; // This flags the error condtions (range should always be >=0)
                dblBearing = 0;
            }
        }     // RangeAndBearing

        // Function to convert Lat/Lon text to deximal degrees (used for computing distance)
        private double LatLon2Dec(string strLatLon)
        {
            double LatLon2DecRet = default;
            try
            {
                // Function to convert the text lat lon string to a decimal degee.
                // N lat and E lon are +, S lat and W lon are -
                // Input Format is NMEA DD-mm.mmN/S or DDD-mm.mmE/W  ( e.g 08012.34W) 
                string strCompass;
                strCompass = strLatLon.Right(1);
                switch (strCompass)
                {
                    case "N":
                        {
                            LatLon2DecRet = Convert.ToDouble(strLatLon.Right(2)) + Convert.ToDouble(strLatLon.Substring(3, 5)) / 60;
                            break;
                        }

                    case "S":
                        {
                            LatLon2DecRet = -(Convert.ToDouble(strLatLon.Right(2)) + Convert.ToDouble(strLatLon.Substring(3, 5)) / 60);
                            break;
                        }

                    case "E":
                        {
                            LatLon2DecRet = Convert.ToDouble(strLatLon.Right(3)) + Convert.ToDouble(strLatLon.Substring(4, 5)) / 60;
                            break;
                        }

                    case "W":
                        {
                            LatLon2DecRet = -(Convert.ToDouble(strLatLon.Right(3)) + Convert.ToDouble(strLatLon.Substring(4, 5)) / 60);
                            break;
                        }

                    default:
                        {
                            LatLon2DecRet = 0;  // error case
                            break;
                        }
                }
            }
            catch
            {
                return 0;
            }

            return LatLon2DecRet;
        } // LatLon2Dec

        // Subroutine to compute range and bearing between grid squares and display
        public void ComputeRangeAndBearing(string strLocalGS, string strRemoteGS, ref double Range, ref double Bearing)
        {
            string strLocalLat = null;
            string strLocalLon = null;
            string strRemoteLat = null;
            string strRemoteLon = null;
            GridSq2NMEALatLon(strLocalGS, ref strLocalLat, ref strLocalLon);
            GridSq2NMEALatLon(strRemoteGS, ref strRemoteLat, ref strRemoteLon);
            double dblRemoteLat = NMEALatLon2Dec(strRemoteLat);
            double dblRemoteLon = NMEALatLon2Dec(strRemoteLon);
            double dblLocalLat = NMEALatLon2Dec(strLocalLat);
            double dblLocalLon = NMEALatLon2Dec(strLocalLon);
            double dblRange = default, dblBearing = default;
            RangeAndBearing(ref dblRange, ref dblBearing, dblLocalLat, dblLocalLon, dblRemoteLat, dblRemoteLon);
            Range = 1.15 * dblRange;
            Bearing = dblBearing;
        }  // ComputeRangeAndBearing
    }
}