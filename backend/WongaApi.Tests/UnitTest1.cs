using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WongaApi.Controllers;
using WongaApi.Data;
using WongaApi.Models;
using Xunit;

namespace WongaApi.Tests
{
    public class AuthControllerTests
    {
        private AppDbContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        private IConfiguration GetConfig()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                { "Jwt:Key", "ThisIsASecretKeyForWongaAssessment123!" },
                { "Jwt:Issuer", "WongaApi" },
                { "Jwt:Audience", "WongaClient" }
            };
            return new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings!)
                .Build();
        }

        [Fact]
        public async Task Register_ShouldReturnOk_WhenUserIsNew()
        {
            var context = GetInMemoryContext();
            var controller = new AuthController(context, GetConfig());

            var request = new RegisterRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Password = "password123"
            };

            var result = await controller.Register(request);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Register_ShouldReturnBadRequest_WhenEmailExists()
        {
            var context = GetInMemoryContext();
            var controller = new AuthController(context, GetConfig());

            var request = new RegisterRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Password = "password123"
            };

            await controller.Register(request);
            var result = await controller.Register(request);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Login_ShouldReturnOk_WithValidCredentials()
        {
            var context = GetInMemoryContext();
            var controller = new AuthController(context, GetConfig());

            var registerRequest = new RegisterRequest
            {
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane@example.com",
                Password = "password123"
            };
            await controller.Register(registerRequest);

            var loginRequest = new LoginRequest
            {
                Email = "jane@example.com",
                Password = "password123"
            };

            var result = await controller.Login(loginRequest);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Login_ShouldReturnUnauthorized_WithWrongPassword()
        {
            var context = GetInMemoryContext();
            var controller = new AuthController(context, GetConfig());

            var registerRequest = new RegisterRequest
            {
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane@example.com",
                Password = "password123"
            };
            await controller.Register(registerRequest);

            var loginRequest = new LoginRequest
            {
                Email = "jane@example.com",
                Password = "wrongpassword"
            };

            var result = await controller.Login(loginRequest);

            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task Login_ShouldReturnUnauthorized_WhenUserDoesNotExist()
        {
            var context = GetInMemoryContext();
            var controller = new AuthController(context, GetConfig());

            var loginRequest = new LoginRequest
            {
                Email = "nobody@example.com",
                Password = "password123"
            };

            var result = await controller.Login(loginRequest);

            Assert.IsType<UnauthorizedObjectResult>(result);
        }
    }
}