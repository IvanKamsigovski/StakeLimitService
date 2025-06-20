using System.ComponentModel.DataAnnotations;

namespace StakeLimit.Entities
{
    public class Ticket
    {
        [Key]
        public Guid Id { get; set; }
        public Guid DeviceId { get; set; }
        public double Stake { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}