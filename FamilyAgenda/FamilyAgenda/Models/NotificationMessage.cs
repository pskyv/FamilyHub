using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FamilyAgenda.Models
{
    public class NotificationMessage
    {
        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
