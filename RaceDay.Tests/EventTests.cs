using Microsoft.EntityFrameworkCore;
using RaceDay.API.Controllers;
using RaceDay.API.Data;
using RaceDay.API.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace RaceDay.Tests
{
    public class EventTests
    {
        private RaceDayDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<RaceDayDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new RaceDayDbContext(options);
        }

        [Fact]
        public async Task CreateEvent_ValidEvent_ReturnsCreatedResult()
        {
            var context = GetInMemoryDbContext();
            var controller = new EventsController(context);
            var newEvent = new Event
            {
                EventName = "Soweto Marathon",
                EventDescription = "Annual road race",
                EventDate = DateTime.UtcNow.AddDays(30),
                RegistrationDeadline = DateTime.UtcNow.AddDays(10),
                Location = "Soweto",
                Distance = 42.2m,
                EventType = "Run",
                Status = "Open",
                MaxParticipants = 5000,
                OrganiserID = 1
            };

            var result = await controller.CreateEvent(newEvent);

            Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Single(context.Events);
        }

        [Fact]
        public async Task GetEvents_ReturnsAllEvents()
        {
            var context = GetInMemoryDbContext();
            context.Events.Add(new Event { EventName = "Cape Town Cycle Tour", OrganiserID = 1 });
            context.Events.Add(new Event { EventName = "Two Oceans Marathon", OrganiserID = 1 });
            await context.SaveChangesAsync();

            var controller = new EventsController(context);

            var result = await controller.GetEvents();

            var actionResult = Assert.IsType<ActionResult<IEnumerable<Event>>>(result);
            var events = Assert.IsAssignableFrom<IEnumerable<Event>>(actionResult.Value);
            Assert.Equal(2, events.Count());
        }
    }
}