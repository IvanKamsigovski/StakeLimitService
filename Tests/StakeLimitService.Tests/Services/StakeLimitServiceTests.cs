using Xunit;
using Moq;
using System;
using System.Threading.Tasks;
using StakeLimit.Services;
using StakeLimit.Interfaces;
using StakeLimit.Entities;
using StakeLimit.Presentation.Api.Models;
using StakeLimit.Dtos.Ticket;

namespace StakeLimitService.Tests.Services
{
    public class StakeLimitServiceTests
    {
        private readonly Mock<IDeviceRepository> _deviceRepoMock = new();
        private readonly Mock<ITicketRepository> _ticketRepoMock = new();
        private readonly IStakeLimitService _service;

        public StakeLimitServiceTests()
        {
            _service = new StakeLimit.Services.StakeLimitService(_deviceRepoMock.Object, _ticketRepoMock.Object);
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
                BlockedAt = DateTime.UtcNow.AddMinutes(-10),
                RestrictionExpires = 3600
            };

            _deviceRepoMock
                .Setup(repo => repo.GetDeviceByIdAsync(ticket.DeviceId))
                .ReturnsAsync(device);

            // Act
            var result = await _service.EvaluateTicketAsync(ticket);

            // Assert
            Assert.Equal(Status.BLOCKED, result);
        }
        [Fact]
        public async Task EvaluateTicketAsync_ShouldThrow_WhenDeviceDoesNotExist()
        {
            // Arrange
            var ticket = new TicketMessage
            {
                Id = Guid.NewGuid(),
                DeviceId = Guid.NewGuid(),
                Stake = 100
            };

            _deviceRepoMock
                .Setup(repo => repo.GetDeviceByIdAsync(ticket.DeviceId))
                .ReturnsAsync((Device)null); // simulate missing device

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.EvaluateTicketAsync(ticket));
        }

        [Fact]
        public async Task EvaluateTicketAsync_ShouldReturnHot_WhenStakeIsAboveHotThreshold()
        {
            // Arrange
            var ticket = new TicketMessage
            {
                Id = Guid.NewGuid(),
                DeviceId = Guid.NewGuid(),
                Stake = 400
            };

            var device = new Device
            {
                DeviceId = ticket.DeviceId,
                TimeDuration = 300,
                StakeLimit = 1000,
                HotPercentage = 40,
                RestrictionExpires = 0,
                IsDeviceBlocked = false
            };

            _deviceRepoMock.Setup(repo => repo.GetDeviceByIdAsync(ticket.DeviceId)).ReturnsAsync(device);
            _ticketRepoMock.Setup(repo =>
                repo.GetSumOfStakeForDeviceInDuration(ticket.DeviceId, It.IsAny<DateTime>())
            ).ReturnsAsync(50); // so total = 450

            // Act
            var result = await _service.EvaluateTicketAsync(ticket);

            // Assert
            Assert.Equal(Status.HOT, result);
        }

        [Fact]
        public async Task EvaluateTicketAsync_ShouldReturnOk_WhenStakeIsSafe()
        {
            // Arrange
            var ticket = new TicketMessage
            {
                Id = Guid.NewGuid(),
                DeviceId = Guid.NewGuid(),
                Stake = 100
            };

            var device = new Device
            {
                DeviceId = ticket.DeviceId,
                TimeDuration = 300,
                StakeLimit = 1000,
                HotPercentage = 50,
                RestrictionExpires = 0,
                IsDeviceBlocked = false
            };

            _deviceRepoMock.Setup(repo => repo.GetDeviceByIdAsync(ticket.DeviceId)).ReturnsAsync(device);
            _ticketRepoMock.Setup(repo =>
                repo.GetSumOfStakeForDeviceInDuration(ticket.DeviceId, It.IsAny<DateTime>())
            ).ReturnsAsync(200); // total = 300

            // Act
            var result = await _service.EvaluateTicketAsync(ticket);

            // Assert
            Assert.Equal(Status.OK, result);
        }

    }
}
