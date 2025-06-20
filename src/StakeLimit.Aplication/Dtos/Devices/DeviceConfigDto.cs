
namespace StakeLimit.Dtos.Devices
{
    public class DeviceConfigDto
    {
        public int TimeDuration { get; set; }
        public double StakeLimit { get; set; }
        public int HotPercentage { get; set; }
        public int RestrictionExpires { get; set; }

    }
}