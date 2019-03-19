using LumoTrack.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using zorbek.essentials.utilities.Entities;

namespace LumoTrack.ProxyContract
{
    public interface IInboxProxy
    {
        Task<ResponseEntity<IEnumerable<InboxDTO>>> GetNotification(string userId);

        Task<ResponseEntity<InboxDTO>> Create(InboxDTO inboxDTO);

        Task<ResponseEntity<string>> GetNotification1(string userId);
    }
}
