using System.ComponentModel.DataAnnotations;
namespace StakeLimit.Entities
{
    public class Device
    {
        [Key]
        //Rename to Id
        public Guid DeviceId { get; set; }
        public int TimeDuration { get; set; }
        public double StakeLimit { get; set; }
        public int HotPercentage { get; set; }
        public int RestrictionExpires { get; set; }
        public bool IsDeviceBlocked { get; set; }
        public DateTime? BlockedAt { get; set; }

    }
}