using api.Dtos.Devices;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/devices")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IStakeLimitService _stakeLimitService;

        public DeviceController(IStakeLimitService deviceService)
        {
            _stakeLimitService = deviceService;
        }
        [HttpPut("{deviceId}/config")]
        public async Task<IActionResult> UpdateConfig([FromRoute] Guid deviceId, [FromBody] DeviceConfigDto deviceConfigDto)
        {
            try
            {
                var device = await _stakeLimitService.UpdateDeviceConfigAsync(deviceId, deviceConfigDto);
                return Ok(device.ToConfigDeviceDto());

            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating device config: {ex.Message}");
                return StatusCode(500, new { error = "An unexpected error occurred." });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllDevices([FromQuery] DeviceQueryDto queryDto)
        {
            try
            {
                var devices = await _stakeLimitService.GetAllDevicesAsync(queryDto);
                return Ok(devices);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving devices: {ex.Message}");
                return StatusCode(500, new { error = "An unexpected error occurred." });
            }
        }
    }
}