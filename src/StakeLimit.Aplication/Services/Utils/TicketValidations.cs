using StakeLimit.Dtos.Ticket;

namespace StakeLimit.Aplication.Services.Utils
{
    public static class TicketValidations
    {
        public static void ValidateTicket(TicketMessage ticketMessage)
        {
            if (ticketMessage == null)
                throw new ArgumentNullException(nameof(ticketMessage), "Ticket cannot be null");

            if (ticketMessage.Stake <= 0)
                throw new ArgumentOutOfRangeException(nameof(ticketMessage.Stake), "Stake must be greater than 0.");

            if (ticketMessage.DeviceId == Guid.Empty || ticketMessage.Id == Guid.Empty)
                throw new ArgumentOutOfRangeException("DeviceId and Id must be valid GUIDs.");
        }
    }
}