using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FamilyAgenda.Models
{
    public class TodoItem
    {
        [JsonIgnore]
        public string TodoItemId { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonIgnore]
        public char UserInitials 
        { 
            get 
            {
                return Username[0];
            }  
        }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonIgnore]
        public DateTime CreatedAt
        {
            get
            {
                return UnixTimeStampToDateTime(Timestamp);
            }
        }

        [JsonProperty("completed")]
        public bool Completed { get; set; }

        private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
