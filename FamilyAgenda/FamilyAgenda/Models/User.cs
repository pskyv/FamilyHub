using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FamilyAgenda.Models
{
    public class User
    {
        [JsonIgnore]
        public string UserId { get; set; }

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

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        //[JsonProperty("profile_photo")]        
        [JsonIgnore]
        public ImageSource ProfilePhoto { get; set; }
    }
}
