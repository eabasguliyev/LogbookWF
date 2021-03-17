using System;
using System.Collections.Generic;
using System.Drawing;
using LogBookTask.Abstracts;
using Newtonsoft.Json;

namespace LogBookTask.Entities
{
    public class User:Human
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public Image UserImage { get; set; }
        public Byte[] ImageBytes { get; set; }
        public string UserLastLogin { get; set; }
    }
}