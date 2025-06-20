

using StakeLimit.Dtos.Ticket;
using StakeLimit.Enteties;
using StakeLimit.Presentation.Api.Models;

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