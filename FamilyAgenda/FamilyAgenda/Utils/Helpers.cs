using System;
using System.Collections.Generic;
using System.Text;

namespace FamilyAgenda.Utils
{
    public static class Helpers
    {
        public static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp);//.ToLocalTime();
            return dtDateTime;
        }
    }
}
