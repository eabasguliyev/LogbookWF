using System.Collections.Generic;
using LogBookTask.Abstracts;

namespace LogBookTask.Entities
{
    public class User:Human
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string UserLastLogin { get; set; }
    }
}