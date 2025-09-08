using ContactMangAPI.Data;
using ContactMangAPI.DTOs;
using ContactMangAPI.Models;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ContactMangAPI.Services
{
    public class ContactService : IContactService
    {
        private readonly ApplicationDbContext _context;
        public ContactService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResponse> AddContactAsync(int userId, ContactDto model)
        {
            var exists = await _context.Contacts
                                .AnyAsync(c => c.UserId == userId && c.Email == model.Email);
            if (exists)
            {
                return new ApiResponse { Success = false, Message = "Contact with the same email already exists." };
            }
            if (model.BirthDate > DateTime.UtcNow)
            {
                return new ApiResponse { Success = false, Message = "Birthdate cannot be in the future." };
            }

            var contact = new Contact
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                BirthDate = model.BirthDate,
                UserId = userId
            };
            await _context.Contacts.AddAsync(contact);
            await _context.SaveChangesAsync();
            return new ApiResponse { Success = true, Message = "Contact added successfully.", Data = contact };
        }

        public async Task<ApiResponse> DeleteContactAsync(int userId, int contactId)
        {
            var contact = await _context.Contacts
                            .FirstOrDefaultAsync(c => c.UserId == userId && c.Id == contactId);
            if (contact == null)
            {
                return new ApiResponse { Success = false, Message = "Contact not found." };
            }
            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
            return new ApiResponse { Success = true, Message = "Contact deleted successfully." };

        }

        public async Task<ApiResponse> GetContactByIdAsync(int userId, int contactId)
        {
            var contact = await _context.Contacts
                            .FirstOrDefaultAsync(c => c.UserId == userId && c.Id == contactId);
            if (contact == null)
                return new ApiResponse { Success = false, Message = "Contact not found." };
            return new ApiResponse { Success = true, Data = contact };
        }

        public async Task<ApiResponse> GetContactsAsync(int userId, string? sortBy = "FirstName",   
                                                    string? sortOrder = "asc", int pageNumber = 1, int pageSize = 10)
        {
            var contacts = _context.Contacts.Where(c => c.UserId == userId);
            contacts = sortBy?.ToLower() switch
            {
                "lastname" => sortOrder?.ToLower() == "desc"
                    ? contacts.OrderByDescending(c => c.LastName)
                    : contacts.OrderBy(c => c.LastName),
                "birthdate" => sortOrder?.ToLower() == "desc"
                    ? contacts.OrderByDescending(c => c.BirthDate)
                    : contacts.OrderBy(c => c.BirthDate),
                _ => sortOrder?.ToLower() == "desc"
                    ? contacts.OrderByDescending(c => c.FirstName)
                    : contacts.OrderBy(c => c.FirstName),

            };
            var totalRecords = await contacts.CountAsync();
            if (totalRecords == 0)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "No contacts found"
                };
            }
            var result = await contacts
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return new ApiResponse
            {
                Success = true,
                Data = new
                {
                    TotalRecords = totalRecords,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    Contacts = result
                }
            };
            

        }

        public async Task<ApiResponse> UpdateContactAsync(int userId, int contactId, ContactDto model)
        {
            var contact = await _context.Contacts.FirstOrDefaultAsync(c => c.UserId == userId && c.Id == contactId);
            if (contact == null)
            {
                return new ApiResponse { Success = false, Message = "Contact not found." };
            }
            var exists = await _context.Contacts
                                .AnyAsync(c => c.UserId == userId && c.Email == model.Email && c.Id != contactId);
            if (exists)
            {
                return new ApiResponse { Success = false, Message = "Another contact with the same email already exists." };
            }
            if (model.BirthDate > DateTime.UtcNow)
            {
                return new ApiResponse { Success = false, Message = "Birthdate cannot be in the future." };
            }

            contact.FirstName = model.FirstName;
            contact.LastName = model.LastName;
            contact.Email = model.Email;
            contact.PhoneNumber = model.PhoneNumber;
            contact.BirthDate = model.BirthDate;
            await _context.SaveChangesAsync();
            return new ApiResponse { Success = true, Message = "Contact updated successfully.", Data = contact };
        }
    }
}
