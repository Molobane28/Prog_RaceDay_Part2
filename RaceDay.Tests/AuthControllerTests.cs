using Microsoft.EntityFrameworkCore;
using RaceDay.API.Controllers;
using RaceDay.API.Data;
using RaceDay.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace RaceDay.Tests
{
    public class AuthControllerTests
    {
        private RaceDayDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<RaceDayDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new RaceDayDbContext(options);
        }

        [Fact]
        public async Task Register_WithNewEmail_ReturnsOkResult()
        {
            var context = GetInMemoryDbContext();
            var controller = new AuthController(context);
            var request = new RegisterRequestDto
            {
                FirstName = "Test",
                LastName = "User",
                Email = "test@example.com",
                Password = "Password123",
                Role = "Participant"
            };

            var result = await controller.Register(request);

            Assert.IsType<OkObjectResult>(result);
            Assert.Single(context.Users);
        }

        [Fact]
        public async Task Register_WithExistingEmail_ReturnsBadRequest()
        {
            var context = GetInMemoryDbContext();
            var controller = new AuthController(context);
            var request = new RegisterRequestDto
            {
                FirstName = "Test",
                LastName = "User",
                Email = "duplicate@example.com",
                Password = "Password123",
                Role = "Participant"
            };
            await controller.Register(request);

            var result = await controller.Register(request);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Login_WithInvalidPassword_ReturnsUnauthorized()
        {
            var context = GetInMemoryDbContext();
            var controller = new AuthController(context);
            var request = new RegisterRequestDto
            {
                FirstName = "Login",
                LastName = "Test",
                Email = "login@example.com",
                Password = "CorrectPassword",
                Role = "Participant"
            };
            await controller.Register(request);

            var loginRequest = new LoginRequestDto
            {
                Email = "login@example.com",
                Password = "WrongPassword"
            };

            var result = await controller.Login(loginRequest);

            Assert.IsType<UnauthorizedObjectResult>(result);
        }
    }
}