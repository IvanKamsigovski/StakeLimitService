

using api.Dtos.Ticket;
using api.Enteties;
using api.Models;

namespace api.Mappers
{
    public static class TicketMapper
    {
        public static TicketMessage ToTicketMessage(this Ticket ticketModel)
        {
            return new TicketMessage
            {
                Id = ticketModel.Id,
                DeviceId = ticketModel.DeviceId,
                Stake = ticketModel.Stake,
            };
        }
    }
}