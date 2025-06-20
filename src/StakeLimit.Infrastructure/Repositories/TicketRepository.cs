using StakeLimit.Enteties;
using StakeLimit.Interfaces;
using StakeLimit.Data;
using Microsoft.EntityFrameworkCore;

namespace StakeLimit.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        public readonly ApplicationDBContext _context;

        public TicketRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task AddTicketAsync(Ticket ticket)
        {
           
            if (ticket == null)
            {
                throw new ArgumentNullException(nameof(ticket), "Device cannot be null");
            }

            await _context.Tickets.AddAsync(ticket);
        }

        public async Task<double> GetSumOfStakeForDeviceInDuration(Guid deviceId, DateTime since) =>
            await _context.Tickets
                .Where(t => t.DeviceId == deviceId && t.CreatedAt >= since)
                .SumAsync(t => t.Stake);

        public Task SaveTicketChangesAsync() => _context.SaveChangesAsync();
    }
}