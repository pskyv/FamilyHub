using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace FamilyAgenda.Utils
{
    public static class Helpers
    {
        public static DateTime UnixTimeStampToDateTime(long unixTimeStamp, bool localTime = true)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp);
            
            return localTime ? dtDateTime.ToLocalTime() : dtDateTime;
        }

        public static void ShowToastMessage(string message)
        {
            //var icon = string.Empty;
            System.Drawing.Color color = Color.Black;

            var toastConfig = new ToastConfig(message);
            toastConfig.SetDuration(3000);
            toastConfig.SetBackgroundColor(color);            
            toastConfig.SetMessageTextColor(Color.White);
            //toastConfig.SetIcon(icon);
            UserDialogs.Instance.Toast(toastConfig);
        }
    }
}
