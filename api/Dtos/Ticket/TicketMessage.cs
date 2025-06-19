using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Ticket
{
    public class TicketMessage
    {
        public Guid Id { get; set; }
        public Guid DeviceId { get; set; }
        public double Stake { get; set; }
    }
}