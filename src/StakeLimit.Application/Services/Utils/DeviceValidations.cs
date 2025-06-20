using StakeLimit.Dtos.Devices;
using StakeLimit.Entities;

namespace StakeLimit.Services.Utils
{
    public static class DeviceValidations
    {
        #region Device Limits
        private static readonly int _timeDurationMax = 86400; // 24 hours in seconds
        private static readonly int _timeDurationMin = 300; // 5 minutes in seconds
        private static readonly double _stakeLimitMax = 10000000;
        private static readonly double _stakeLimitMin = 1;
        private static readonly int _hotPercentageMax = 100;
        private static readonly int _hotPercentageMin = 1;
        private static readonly int _restrictionExpiresMin = 60; // 1 minute in seconds
        #endregion

        public static void ValidateDeviceConfig(DeviceConfigDto deviceConfigDto)
        {
            // 5 min = 300 seconds
            // 24 hours = 86,400 seconds
            if (deviceConfigDto.TimeDuration < _timeDurationMin || deviceConfigDto.TimeDuration > _timeDurationMax)
                throw new ArgumentOutOfRangeException(nameof(deviceConfigDto.TimeDuration), "TimeDuration must be between 300 (5 minutes) and 86400 (24 hours)");

            if (deviceConfigDto.StakeLimit < _stakeLimitMin || deviceConfigDto.StakeLimit > _stakeLimitMax)
                throw new ArgumentOutOfRangeException(nameof(deviceConfigDto.StakeLimit), "StakeLimit must be between 1 and 10000000");

            if (deviceConfigDto.HotPercentage < _hotPercentageMin || deviceConfigDto.HotPercentage > _hotPercentageMax)
                throw new ArgumentOutOfRangeException(nameof(deviceConfigDto.HotPercentage), "HotPercentage must be between 1 and 100");

            if (deviceConfigDto.RestrictionExpires != 0 && deviceConfigDto.RestrictionExpires < _restrictionExpiresMin)
                throw new ArgumentOutOfRangeException(nameof(deviceConfigDto.RestrictionExpires),
                    "RestrictionExpires must be 0 (never expires) or at least 60 seconds.");

        }

        /// <summary>
        /// Cheks if the device is unblocked based on the current time and the restriction settings.
        /// </summary>
        /// <returns>True if device is blocked, false if device is unblocked</returns>
        public static bool IsDeviceBlocked(Device device, DateTime currentTime)
        {
            if (!device.IsDeviceBlocked || device.BlockedAt == null)
                return false;

            if (device.RestrictionExpires > 0)
            {
                var shouldUnblockAt = device.BlockedAt.Value.AddSeconds(device.RestrictionExpires);
                if (currentTime >= shouldUnblockAt)
                    return false;

            }

            return true;
        }
    }
}