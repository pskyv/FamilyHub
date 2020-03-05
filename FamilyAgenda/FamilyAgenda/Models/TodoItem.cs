using FamilyAgenda.Utils;
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
                return Helpers.UnixTimeStampToDateTime(Timestamp);
            }
        }

        [JsonProperty("completed")]
        public bool Completed { get; set; }
    }
}
