
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LumoTrack.DTO
{
    public class UserDTO
    {
        public string Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string SecondName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string MotherLastName { get; set; }

        [Required]
        public string Email { get; set; }

        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }

        public DateTime? LockoutEndDateUtc { get; set; }

        public string PhoneNumber { get; set; }
        [Required]
        public string Roles { get; set; }


        public string Fullname
        {
            get { return FirstName +" "+SecondName + " " + LastName + " " + MotherLastName; }
        }
    }
}
