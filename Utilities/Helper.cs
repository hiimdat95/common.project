/*************************************************************
 * === * ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ *
 *   *  * ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ *
 *              https: *************************************************************/

using System;

namespace SolarLunarCalendar.Core
{
    public static class Helper
    {
        public static double ToNormalizedArc(this double arc)
        {
            double thisAcr = Math.Abs(arc);
            while (thisAcr > (2 * Math.PI))
                thisAcr -= (2 * Math.PI);
            return thisAcr;
        }

        public static double ToRadians(this double degrees)
        {
            return degrees * Math.PI / 180;
        }

        public static double ToDegrees(this double radians)
        {
            return radians * 180 / Math.PI;
        }
    }
}