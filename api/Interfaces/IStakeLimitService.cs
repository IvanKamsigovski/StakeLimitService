using api.Dtos.Common;
using api.Dtos.Devices;
using api.Dtos.Ticket;
using api.Enteties;

namespace api.Interfaces
{
    public interface IStakeLimitService
    {
        Task<Status> EvaluateTicketAsync(TicketMessage ticket);
        Task<Device> UpdateDeviceConfigAsync(Guid deviceId, DeviceConfigDto deviceConfig);
        Task<PagedResult<DeviceDto>> GetAllDevicesAsync(DeviceQueryDto queryDto);
    }
}