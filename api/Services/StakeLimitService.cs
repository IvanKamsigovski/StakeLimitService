using api.Dtos.Common;
using api.Dtos.Devices;
using api.Dtos.Ticket;
using api.Enteties;
using api.Interfaces;
using api.Mappers;
using api.Services.Utils;

namespace api.Services
{
    public class StakeLimitService : IStakeLimitService
    {
        #region Dependencies
        private readonly IDeviceRepository _deviceRepository;
        private readonly ITicketRepository _ticketRepository;
        #endregion

        public StakeLimitService(IDeviceRepository deviceRepository, ITicketRepository ticketRepository)
        {
            _deviceRepository = deviceRepository;
            _ticketRepository = ticketRepository;
        }


        public async Task<Status> EvaluateTicketAsync(TicketMessage ticketMessage)
        {
            var device = await _deviceRepository.GetDeviceByIdAsync(ticketMessage.DeviceId)
             ?? throw new KeyNotFoundException($"Device with ID {ticketMessage.DeviceId} not found.");


            //BLOCKED logic    
            // Check if the device is blocked
            if (await DeviceValidations.IsDeviceBlockedAsync(_deviceRepository, device, DateTime.UtcNow) == true)
                return Status.BLOCKED;

            var currentStakeSum = await _ticketRepository.GetSumOfStakeForDeviceInDuration(
              ticketMessage.DeviceId,
              DateTime.UtcNow.AddSeconds(-device.TimeDuration));

            var totalStakeSum = currentStakeSum + ticketMessage.Stake;

            //Chek if the stake exceeds the limit, if so return blocked status
            if (await DeviceValidations.IsStakeOverTheLimit(_deviceRepository, device, totalStakeSum))
               return Status.BLOCKED;

            //Create Ticket Logic
            var ticket = new Ticket
            {
                Id = ticketMessage.Id,
                DeviceId = ticketMessage.DeviceId,
                Stake = ticketMessage.Stake,
                CreatedAt = DateTime.UtcNow
            };
            await _ticketRepository.AddTicketAsync(ticket);
            await _ticketRepository.SaveTicketChangesAsync();

            //HOT Logic
            if (totalStakeSum > device.StakeLimit * device.HotPercentage / 100)
                return Status.HOT;


            return Status.OK;

        }

        public async Task<Device> UpdateDeviceConfigAsync(Guid deviceId, DeviceConfigDto deviceConfigDto)
        {
            DeviceValidations.ValidateDeviceConfig(deviceConfigDto);

            var device = await _deviceRepository.GetDeviceByIdAsync(deviceId);

            // If the device does not exist, create a new one
            if (device == null)
            {
                device = new Device
                {
                    DeviceId = deviceId,
                    TimeDuration = deviceConfigDto.TimeDuration,
                    StakeLimit = deviceConfigDto.StakeLimit,
                    HotPercentage = deviceConfigDto.HotPercentage,
                    RestrictionExpires = deviceConfigDto.RestrictionExpires
                };

                await _deviceRepository.AddDeviceAsync(device);
            }
            else
            {
                // Update the device configuration
                device.TimeDuration = deviceConfigDto.TimeDuration;
                device.StakeLimit = deviceConfigDto.StakeLimit;
                device.HotPercentage = deviceConfigDto.HotPercentage;
                device.RestrictionExpires = deviceConfigDto.RestrictionExpires;
            }

            await _deviceRepository.SaveDeviceChangesAsync();
            return device;
        }


        public async Task<PagedResult<DeviceDto>> GetAllDevicesAsync(DeviceQueryDto queryDto)
        {
            var (devices, TotalCount) = await _deviceRepository.GetAllDevicesAsync(queryDto);

            return new PagedResult<DeviceDto>
            {
                Items = devices.Select(d => d.ToDeviceDto()),
                TotalCount = TotalCount,
                PageNumber = queryDto.PageNumber,
                PageSize = queryDto.PageSize
            };
        }
    }
}