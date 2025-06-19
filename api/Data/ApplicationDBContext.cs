using api.Enteties;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
         : base(options)
        {
        }
        // Define DbSets for your entities here
        public DbSet<Device> Devices { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
    }
}