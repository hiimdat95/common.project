using Augustine.VietnameseCalendar.Core.LuniSolarCalendar;
using System;
using System.Globalization;

namespace cm.Utilities.Common
{
    public static class LunarDateUtil
    {
        public static string ConvertToLunarDate(ref string inputDate)
        {
            DateTime temp = DateTime.Now;

            try
            {
                temp = DateTime.Parse(inputDate, CultureInfo.InvariantCulture);
            }
            catch
            {
                throw new NotImplementedException();
            }
            LuniSolarDate<VietnameseLocalInfoProvider> lunarDate = LuniSolarCalendar<VietnameseLocalInfoProvider>.LuniSolarDateFromSolarDate(temp);
            inputDate = temp.ToString("dd/MM/yyyy");
            return lunarDate.FullDayInfo;
        }
    }
}