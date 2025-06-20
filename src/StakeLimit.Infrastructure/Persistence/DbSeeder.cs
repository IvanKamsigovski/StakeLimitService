using StakeLimit.Data;
using StakeLimit.Enteties;

public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDBContext context)
    {
        if (!context.Devices.Any())
        {
            var random = new Random();
            var now = DateTime.UtcNow;

            var devices = Enumerable.Range(1, 10).Select(i =>
                new Device
                {
                    DeviceId = Guid.NewGuid(),
                    TimeDuration = random.Next(300, 86400), // 5 min to 24h
                    StakeLimit = random.Next(500, 5000),
                    HotPercentage = random.Next(60, 95),
                    RestrictionExpires = random.Next(0, 3600), // 0 (never) to 1 hour
                    BlockedAt = null
                }).ToList();

            context.Devices.AddRange(devices);
            await context.SaveChangesAsync();

            // Add tickets for each device
            foreach (var device in devices)
            {
                var tickets = Enumerable.Range(1, 20).Select(i =>
                    new Ticket
                    {
                        Id = Guid.NewGuid(),
                        DeviceId = device.DeviceId,
                        Stake = random.Next(10, 300),
                        CreatedAt = now.AddSeconds(-random.Next(0, device.TimeDuration + 600))
                    }).ToList();

                context.Tickets.AddRange(tickets);
            }

            await context.SaveChangesAsync();
        }
    }
}
