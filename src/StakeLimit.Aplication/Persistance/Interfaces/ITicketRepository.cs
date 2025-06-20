using StakeLimit.Enteties;

namespace StakeLimit.Interfaces
{
    public interface ITicketRepository
    {
        // Task<TicketMessage> CreateTicket(TicketMessage ticket);
        Task<double> GetSumOfStakeForDeviceInDuration(Guid deviceId, DateTime since);
        Task AddTicketAsync(Ticket device);
        Task SaveTicketChangesAsync();
    }
}