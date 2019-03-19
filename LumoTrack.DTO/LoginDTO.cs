using System;
using System.Collections.Generic;
using System.Text;

namespace LumoTrack.DTO
{
    public class LoginDTO
    {
        public string Password { get; set; }
        public string Username { get; set; }
        public bool RememberMe { get; set; }
    }
}
