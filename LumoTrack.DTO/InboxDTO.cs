
using LumoTrack.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LumoTrack.DTO
{
    public class InboxDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime CreationDate { get; set; }

        public string TruckName { get; set; }

        public string TruckId { get; set; }

        public ReportTypesEnum ReportType { get; set; }

        public string Message { get; set; }
        [Required]
        public string Response { get; set; }

        public DateTime? ResponseDate { get; set; }

        public string UserId { get; set; }

        public string UserResponseId { get; set; }
    }
}
