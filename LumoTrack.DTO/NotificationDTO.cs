using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LumoTrack.DTO
{
    public class NotificationDTO
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public bool Important { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-mm-yy}")]
        public DateTime NotificationDate { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-mm-yy}")]
        public DateTime ExpiryDate { get; set; }

        public DateTime CreationDate { get; set; }

        public string UserId { get; set; }
    }
}
