using api.Dtos.Ticket;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/tickets")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly IStakeLimitService _stakeLimitService;

        public TicketController(IStakeLimitService deviceService)
        {
            _stakeLimitService = deviceService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTicket([FromBody] TicketMessage ticketMessage)
        {
            if (ticketMessage == null)
            {
                return BadRequest(new { error = "Ticket message cannot be null." });
            }

            try
            {
                var status = await _stakeLimitService.EvaluateTicketAsync(ticketMessage);
                return Ok(new { Status = status.ToString() });
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating ticket: {ex.Message}");
                return StatusCode(500, new { error = "An unexpected error occurred." });
            }
        }
    }
}