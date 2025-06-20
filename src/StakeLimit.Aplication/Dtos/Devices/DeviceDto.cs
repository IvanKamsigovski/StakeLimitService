
namespace StakeLimit.Dtos.Devices
{
    public class DeviceDto
    {
        public Guid DeviceId { get; set; }
        public int TimeDuration { get; set; }
        public double StakeLimit { get; set; }
        public int HotPercentage { get; set; }
        public int RestrictionExpires { get; set; }
    }
}