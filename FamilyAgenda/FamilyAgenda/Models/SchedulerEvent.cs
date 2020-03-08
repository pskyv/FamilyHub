using FamilyAgenda.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FamilyAgenda.Models
{
    public class SchedulerEvent
    {
        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("start_time")]
        public long StartTimestamp { get; set; }

        [JsonProperty("end_time")]
        public long EndTimestamp { get; set; }

        [JsonIgnore]
        public DateTime StartDate { get; set; }

        [JsonIgnore]
        public TimeSpan StartTime { get; set; }

        [JsonIgnore]
        public DateTime EndDate { get; set; }

        [JsonIgnore]
        public TimeSpan EndTime { get; set; }

        [JsonIgnore]
        public Color Color { get; set; }

        [JsonIgnore]
        public DateTime SchedulerStartTime
        {
            get
            {
                return Helpers.UnixTimeStampToDateTime(StartTimestamp, false);                
            }
        }

        [JsonIgnore]
        public DateTime SchedulerEndTime
        {
            get
            {
                return Helpers.UnixTimeStampToDateTime(EndTimestamp, false);
            }
        }

        [JsonIgnore]
        public char UserInitials
        {
            get
            {
                return Username[0];
            }
        }
    }
}
