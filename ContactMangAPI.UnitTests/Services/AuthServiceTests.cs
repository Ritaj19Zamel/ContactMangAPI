using ContactMangAPI.Data;
using ContactMangAPI.DTOs;
using ContactMangAPI.Models;
using ContactMangAPI.Services;
using ContactMangAPI.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactMangAPI.UnitTests.Services
{
    [TestFixture]
    class AuthServiceTests
    {
        private ApplicationDbContext _context;
        private AuthService _service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);

            var jwtOptions = Options.Create(new JWT
            {
                Key = "44mcoZbr7A7buBc16YV0GAgFzl1sWOQ/iHHI5Ru3/28=", 
                Issuer = "TestIssuer",
                Audience = "TestAudience",
                DurationInMinutes = 60
            });

            _service = new AuthService(_context, jwtOptions);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }
        [Test]
        public async Task RegisterAsync_ShouldRegisterUser_WhenValid()
        {
            var dto = new RegisterDto
            {
                Email = "john@gmail.com",
                Password = "Password123",
                FullName = "Test User"
            };

            var result = await _service.RegisterAsync(dto);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("User registered successfully", result.Message);
            Assert.IsNotNull(result.Data);
        }
        [Test]
        public async Task RegisterAsync_ShouldFail_WhenEmailExists()
        {
            var user = new User
            {
                Email = "john@gmail.com",
                FullName = "Existing",
                Password = new PasswordHasher<User>().HashPassword(null!, "Password123")
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var dto = new RegisterDto
            {
                Email = "john@gmail.com",
                Password = "Password123",
                FullName = "New User"
            };

            var result = await _service.RegisterAsync(dto);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Email is already registered", result.Message);
        }
        [Test]
        public async Task RegisterAsync_ShouldFail_WhenPasswordLessThan6Characters()
        {

            var dto = new RegisterDto
            {
                Email = "john@gmail.com",
                Password = "123",
                FullName = "New User"
            };

            var result = await _service.RegisterAsync(dto);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Password must be at least 6 characters long", result.Message);
        }
        [Test]
        public async Task LoginAsync_ShouldLoginUser_WhenValid()
        {
            var user = new User
            {
                Email = "john@gmail.com",
                FullName = "John User"
            };
            var hasher = new PasswordHasher<User>();
            user.Password = hasher.HashPassword(user, "Password123");

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var dto = new LoginDto
            {
                Email = "john@gmail.com",
                Password = "Password123"
            };

            var result = await _service.LoginAsync(dto);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Login successful", result.Message);
            Assert.IsNotNull(result.Data);
        }
        [Test]
        public async Task LoginAsync_ShouldFail_WhenInvalidPassword()
        {
            var user = new User
            {
                Email = "John@gmail.com",
                FullName = "John User"
            };
            var hasher = new PasswordHasher<User>();
            user.Password = hasher.HashPassword(user, "Password123");

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var dto = new LoginDto
            {
                Email = "John@gmail.com",
                Password = "123"
            };

            var result = await _service.LoginAsync(dto);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Invalid email or password", result.Message);
        }

    }
}
