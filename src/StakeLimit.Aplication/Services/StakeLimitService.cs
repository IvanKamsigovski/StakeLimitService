using StakeLimit.Dtos.Common;
using StakeLimit.Dtos.Devices;
using StakeLimit.Dtos.Ticket;
using StakeLimit.Enteties;
using StakeLimit.Interfaces;
using StakeLimit.Aplication.Mappers;
using StakeLimit.Services.Utils;

namespace StakeLimit.Services
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
            await _deviceRepository.BeginTransactionAsync();

            try
            {

                var device = await _deviceRepository.GetDeviceByIdAsync(ticketMessage.DeviceId)
                 ?? throw new KeyNotFoundException($"Device with ID {ticketMessage.DeviceId} not found.");


                //BLOCKED logic    
                bool blockExpired = !DeviceValidations.IsDeviceBlocked(device, DateTime.UtcNow);

                if (device.IsDeviceBlocked && blockExpired)
                {
                    device.IsDeviceBlocked = false;
                    device.BlockedAt = null;
                    await _deviceRepository.SaveDeviceChangesAsync();
                }

                // Check if the device is blocked
                if (DeviceValidations.IsDeviceBlocked(device, DateTime.UtcNow))
                {
                    await _deviceRepository.CommitTransactionAsync();
                    return Status.BLOCKED;
                }

                var currentStakeSum = await _ticketRepository.GetSumOfStakeForDeviceInDuration(
                  ticketMessage.DeviceId,
                  DateTime.UtcNow.AddSeconds(-device.TimeDuration));

                var totalStakeSum = currentStakeSum + ticketMessage.Stake;

                //Chek if the stake exceeds the limit, if so return blocked status
                if (totalStakeSum >= device.StakeLimit)
                {
                    device.IsDeviceBlocked = true;
                    device.BlockedAt = DateTime.UtcNow;
                    await _deviceRepository.SaveDeviceChangesAsync();
                    await _deviceRepository.CommitTransactionAsync();
                    return Status.BLOCKED;
                }

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
                await _deviceRepository.CommitTransactionAsync();

                //HOT Logic
                if (totalStakeSum > device.StakeLimit * device.HotPercentage / 100)
                    return Status.HOT;


                return Status.OK;
            }
            catch
            {
                await _deviceRepository.RollbackTransactionAsync();
                throw;
            }

        }

        public async Task<Device> UpdateDeviceConfigAsync(Guid deviceId, DeviceConfigDto deviceConfigDto)
        {
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