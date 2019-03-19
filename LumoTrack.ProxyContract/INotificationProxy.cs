using LumoTrack.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using zorbek.essentials.utilities.Entities;

namespace LumoTrack.ProxyContract
{
    public interface INotificationProxy
    {
        Task<ResponseEntity<List<NotificationDTO>>> GetNotification();
    }
}
