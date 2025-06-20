using StakeLimit.Dtos.Common;
using StakeLimit.Dtos.Devices;
using StakeLimit.Dtos.Ticket;
using StakeLimit.Enteties;

namespace StakeLimit.Interfaces
{
    public interface IStakeLimitService
    {
        Task<Status> EvaluateTicketAsync(TicketMessage ticket);
        Task<Device> UpdateDeviceConfigAsync(Guid deviceId, DeviceConfigDto deviceConfig);
        Task<PagedResult<DeviceDto>> GetAllDevicesAsync(DeviceQueryDto queryDto);
    }
}