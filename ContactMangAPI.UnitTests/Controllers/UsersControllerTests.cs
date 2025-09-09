using ContactMangAPI.Controllers;
using ContactMangAPI.DTOs;
using ContactMangAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactMangAPI.UnitTests.Controllers
{
    [TestFixture]
    class UsersControllerTests
    {
        private Mock<IAuthService> _mockAuthService;
        private UsersController _controller;

        [SetUp]
        public void Setup()
        {
            _mockAuthService = new Mock<IAuthService>();
            _controller = new UsersController(_mockAuthService.Object);
        }
        [Test]
        public async Task Register_ShouldReturnOk_WhenSuccess()
        {
            var dto = new RegisterDto { FullName = "John Doe", Email = "john@test.com", Password = "Password123" };
            _mockAuthService.Setup(s => s.RegisterAsync(dto))
                            .ReturnsAsync(new ApiResponse { Success = true, Message = "User registered successfully" });

            var result = await _controller.Register(dto);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(true, ((ApiResponse)okResult.Value!).Success);
        }

        [Test]
        public async Task Register_ShouldReturnBadRequest_WhenFails()
        {
            var dto = new RegisterDto { FullName = "John Doe", Email = "john@test.com", Password = "Password123" };
            _mockAuthService.Setup(s => s.RegisterAsync(dto))
                            .ReturnsAsync(new ApiResponse { Success = false, Message = "Email already exists" });

            var result = await _controller.Register(dto);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badResult);
            Assert.AreEqual(false, ((ApiResponse)badResult.Value!).Success);
        }

        [Test]
        public async Task Login_ShouldReturnOk_WhenSuccess()
        {
            var dto = new LoginDto { Email = "john@test.com", Password = "Password123" };
            _mockAuthService.Setup(s => s.LoginAsync(dto))
                            .ReturnsAsync(new ApiResponse { Success = true, Message = "Login successful" });

            var result = await _controller.Login(dto);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(true, ((ApiResponse)okResult.Value!).Success);
        }

        [Test]
        public async Task Login_ShouldReturnBadRequest_WhenFails()
        {
            var dto = new LoginDto { Email = "john@test.com", Password = "WrongPassword" };
            _mockAuthService.Setup(s => s.LoginAsync(dto))
                            .ReturnsAsync(new ApiResponse { Success = false, Message = "Invalid email or password" });

            var result = await _controller.Login(dto);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badResult);
            Assert.AreEqual(false, ((ApiResponse)badResult.Value!).Success);
        }
    }
}
