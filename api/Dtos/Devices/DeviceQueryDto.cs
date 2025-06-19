
namespace api.Dtos.Devices
{
    public class DeviceQueryDto
    {
        public Guid? DeviceId { get; set; }
        public string? SortBy { get; set; } = "DeviceId";
        public string? SortDirection { get; set; } = "asc";
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}