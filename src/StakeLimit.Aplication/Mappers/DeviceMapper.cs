
using StakeLimit.Dtos.Devices;
using StakeLimit.Enteties;

namespace StakeLimit.Aplication.Mappers
{
    public static class DeviceMapper
    {
        public static DeviceConfigDto ToConfigDeviceDto(this Device device)
        {
            return new DeviceConfigDto
            {
                TimeDuration = device.TimeDuration,
                StakeLimit = device.StakeLimit,
                HotPercentage = device.HotPercentage,
                RestrictionExpires = device.RestrictionExpires
            };
        }

        public static DeviceDto ToDeviceDto(this Device device)
        {
            return new DeviceDto
            {
                DeviceId = device.DeviceId,
                TimeDuration = device.TimeDuration,
                StakeLimit = device.StakeLimit,
                HotPercentage = device.HotPercentage,
                RestrictionExpires = device.RestrictionExpires
            };
        }
    }
}