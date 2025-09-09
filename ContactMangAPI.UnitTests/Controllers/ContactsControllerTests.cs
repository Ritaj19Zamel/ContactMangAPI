using ContactMangAPI.Controllers;
using ContactMangAPI.DTOs;
using ContactMangAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ContactMangAPI.UnitTests.Controllers
{
    [TestFixture]
    class ContactsControllerTests
    {
        private Mock<IContactService> _mockService;
        private ContactsController _controller;
        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<IContactService>();
            _controller = new ContactsController(_mockService.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }
        [Test]
        public async Task AddContact_ShouldReturnOk_WhenSuccess()
        {
            var dto = new ContactDto { FirstName = "A", LastName = "B", Email = "a@b.com", PhoneNumber = "123" };
            _mockService.Setup(s => s.AddContactAsync(1, dto))
                        .ReturnsAsync(new ApiResponse { Success = true, Message = "Added" });

            var result = await _controller.AddContact(dto);

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task AddContact_ShouldReturnBadRequest_WhenFails()
        {
            var dto = new ContactDto { FirstName = "A", LastName = "B", Email = "a@b.com", PhoneNumber = "123" };
            _mockService.Setup(s => s.AddContactAsync(1, dto))
                        .ReturnsAsync(new ApiResponse { Success = false, Message = "Error" });

            var result = await _controller.AddContact(dto);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }
        [Test]
        public async Task GetContacts_ShouldReturnOk_WhenSuccess()
        {
            var responseData = new { TotalRecords = 1, PageNumber = 1, PageSize = 10, Contacts = new List<ContactDto>() };
            _mockService.Setup(s => s.GetContactsAsync(1, "FirstName", "asc", 1, 10))
                        .ReturnsAsync(new ApiResponse { Success = true, Data = responseData });

            var result = await _controller.GetContacts();

            Assert.IsInstanceOf<OkObjectResult>(result);
        }
        [Test]
        public async Task GetContacts_ShouldReturnBadRequest_WhenFail()
        {
            var responseData = new { TotalRecords = 1, PageNumber = 1, PageSize = 10, Contacts = new List<ContactDto>() };
            _mockService.Setup(s => s.GetContactsAsync(1, "FirstName", "asc", 1, 10))
                        .ReturnsAsync(new ApiResponse { Success = true, Data = responseData });

            var result = await _controller.GetContacts();

            Assert.IsInstanceOf<OkObjectResult>(result);
        }
        [Test]
        public async Task GetContactById_ShouldReturnOk_WhenFound()
        {
            var contact = new ContactDto { FirstName = "A" };
            _mockService.Setup(s => s.GetContactByIdAsync(1, 42))
                        .ReturnsAsync(new ApiResponse { Success = true, Data = contact });

            var result = await _controller.GetContactById(42);

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetContactById_ShouldReturnNotFound_WhenNotFound()
        {
            _mockService.Setup(s => s.GetContactByIdAsync(1, 42))
                        .ReturnsAsync(new ApiResponse { Success = false, Message = "Not Found" });

            var result = await _controller.GetContactById(42);

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }
        [Test]
        public async Task UpdateContact_ShouldReturnOk_WhenSuccess()
        {
            var dto = new ContactDto { FirstName = "A" };
            _mockService.Setup(s => s.UpdateContactAsync(1, 42, dto))
                        .ReturnsAsync(new ApiResponse { Success = true, Message = "Updated" });

            var result = await _controller.UpdateContact(42, dto);

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task UpdateContact_ShouldReturnBadRequest_WhenFails()
        {
            var dto = new ContactDto { FirstName = "A" };
            _mockService.Setup(s => s.UpdateContactAsync(1, 42, dto))
                        .ReturnsAsync(new ApiResponse { Success = false, Message = "Error" });

            var result = await _controller.UpdateContact(42, dto);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }
        [Test]
        public async Task DeleteContact_ShouldReturnOk_WhenSuccess()
        {
            _mockService.Setup(s => s.DeleteContactAsync(1, 42))
                        .ReturnsAsync(new ApiResponse { Success = true, Message = "Deleted" });

            var result = await _controller.DeleteContact(42);

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task DeleteContact_ShouldReturnNotFound_WhenFails()
        {
            _mockService.Setup(s => s.DeleteContactAsync(1, 42))
                        .ReturnsAsync(new ApiResponse { Success = false, Message = "Not Found" });

            var result = await _controller.DeleteContact(42);

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

    }
}
