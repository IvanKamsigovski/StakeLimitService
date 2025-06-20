

using StakeLimit.Dtos.Ticket;
using StakeLimit.Entities;

namespace StakeLimit.Aplication.Mappers
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