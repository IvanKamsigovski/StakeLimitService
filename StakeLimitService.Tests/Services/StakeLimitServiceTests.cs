using Xunit;
using Moq;
using api.Services;
using api.Interfaces;
using api.Enteties;
using api.Dtos.Ticket;

public class StakeLimitServiceTests
{
    private readonly Mock<IDeviceRepository> _deviceRepoMock = new();
    private readonly Mock<ITicketRepository> _ticketRepoMock = new();
    private readonly StakeLimitService _service;

    public StakeLimitServiceTests()
    {
        _service = new StakeLimitService(_deviceRepoMock.Object, _ticketRepoMock.Object);
    }

    [Fact]
    public async Task EvaluateTicketAsync_ShouldReturnBlocked_WhenDeviceIsBlocked()
    {
        // Arrange
        var ticket = new TicketMessage
        {
            Id = Guid.NewGuid(),
            DeviceId = Guid.NewGuid(),
            Stake = 200
        };

        var device = new Device
        {
            DeviceId = ticket.DeviceId,
            IsDeviceBlocked = true,
            BlockedAt = DateTime.UtcNow,
            RestrictionExpires = 3600
        };

        _deviceRepoMock.Setup(r => r.GetDeviceByIdAsync(ticket.DeviceId)).ReturnsAsync(device);

        // Act
        var result = await _service.EvaluateTicketAsync(ticket);

        // Assert
        Assert.Equal(Status.BLOCKED, result);
    }
}
