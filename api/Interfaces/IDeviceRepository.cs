
using api.Dtos.Devices;
using api.Enteties;

namespace api.Interfaces
{
    public interface IDeviceRepository
    {
        Task<(IEnumerable<Device>, int TotalCount)> GetAllDevicesAsync(DeviceQueryDto queryDto);
        Task<Device?> GetDeviceByIdAsync(Guid deviceId);
        Task AddDeviceAsync(Device device);
        Task SaveDeviceChangesAsync();
    }
}