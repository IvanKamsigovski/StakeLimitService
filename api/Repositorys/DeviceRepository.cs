
using api.Data;
using api.Dtos.Devices;
using api.Enteties;
using api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api.Repositorys
{
    public class DeviceRepository : IDeviceRepository
    {
        public readonly ApplicationDBContext _context;

        public DeviceRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Device?> GetDeviceByIdAsync(Guid deviceId) =>
         await _context.Devices
                .FirstOrDefaultAsync(d => d.DeviceId == deviceId);


        public async Task AddDeviceAsync(Device device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device), "Device cannot be null");
            }

            await _context.Devices.AddAsync(device);
        }

        public Task SaveDeviceChangesAsync() => _context.SaveChangesAsync();



        public async Task<(IEnumerable<Device>, int TotalCount)> GetAllDevicesAsync(DeviceQueryDto queryDto)
        {
            var queryable = _context.Devices.AsQueryable();

            if (queryDto.DeviceId.HasValue)
                queryable = queryable.Where(d => d.DeviceId == queryDto.DeviceId.Value);

            var count = await queryable.CountAsync();

            queryable = queryDto.SortBy?.ToLower() switch
            {
                "stakelimit" => queryDto.SortDirection == "desc"
                    ? queryable.OrderByDescending(d => d.StakeLimit)
                    : queryable.OrderBy(d => d.StakeLimit),

                "timeduration" => queryDto.SortDirection == "desc"
                    ? queryable.OrderByDescending(d => d.TimeDuration)
                    : queryable.OrderBy(d => d.TimeDuration),

                _ => queryDto.SortDirection == "desc"
                    ? queryable.OrderByDescending(d => d.DeviceId)
                    : queryable.OrderBy(d => d.DeviceId)
            };

            queryable = queryable
                .Skip((queryDto.PageNumber - 1) * queryDto.PageSize)
                .Take(queryDto.PageSize);

            var devices = await queryable.ToListAsync();

            return (devices, count);

        }
    }
}