using StakeLimit.Dtos.Ticket;
using StakeLimit.Interfaces;
using Microsoft.AspNetCore.Mvc;
using StakeLimit.Aplication.Services.Utils;

namespace StakeLimit.Controllers
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

            TicketValidations.ValidateTicket(ticketMessage);

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