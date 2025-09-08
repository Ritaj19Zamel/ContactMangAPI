using ContactMangAPI.DTOs;
using ContactMangAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ContactMangAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _contactService;

        private int GetUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(userId ?? throw new Exception("User ID not found in token"));
        }
        public ContactsController(IContactService contactService)
        {
            _contactService = contactService;
        }
        [HttpPost]
        public async Task<IActionResult> AddContact([FromBody] ContactDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var userId = GetUserId();
            var response = await _contactService.AddContactAsync(userId, model);
            if (!response.Success)
                return BadRequest(response);
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetContacts([FromQuery] string? sortBy = "FirstName",
                                                     [FromQuery] string? sortOrder = "asc",
                                                     [FromQuery] int pageNumber = 1,
                                                     [FromQuery] int pageSize = 10)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var userId = GetUserId();
            var response = await _contactService.GetContactsAsync(userId, sortBy, sortOrder, pageNumber, pageSize);
            if (!response.Success)
                return BadRequest(response);
            return Ok(response);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetContactById(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var userId = GetUserId();
            var response = await _contactService.GetContactByIdAsync(userId, id);
            if (!response.Success)
                return NotFound(response);
            return Ok(response);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContact(int id, [FromBody] ContactDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var userId = GetUserId();
            var response = await _contactService.UpdateContactAsync(userId, id, model);
            if (!response.Success)
                return BadRequest(response);
            return Ok(response);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var userId = GetUserId();
            var response = await _contactService.DeleteContactAsync(userId, id);
            if (!response.Success)
                return NotFound(response);
            return Ok(response);
        }


    }
}
