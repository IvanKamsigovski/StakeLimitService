
using StakeLimit.Dtos.Devices;
using StakeLimit.Entities;

namespace StakeLimit.Interfaces
{
    public interface IDeviceRepository
    {
        Task<(IEnumerable<Device>, int TotalCount)> GetAllDevicesAsync(DeviceQueryDto queryDto);
        Task<Device?> GetDeviceByIdAsync(Guid deviceId);
        Task AddDeviceAsync(Device device);
        Task SaveDeviceChangesAsync();

        #region Transactions Related
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        #endregion
    }
}