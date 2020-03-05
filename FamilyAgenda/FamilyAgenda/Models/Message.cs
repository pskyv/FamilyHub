using Newtonsoft.Json;

namespace FamilyAgenda.Models
{
    public class Message
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }
    }
}
