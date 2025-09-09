

namespace ContactMangAPI.Services
{
    public interface IContactService
    {
        Task<ApiResponse> AddContactAsync(int userId, ContactDto model);
        Task<ApiResponse> GetContactsAsync(int userId, string? sortBy = "FirstName",
                                                    string? sortOrder = "asc", int pageNumber = 1, int pageSize = 10);
        Task<ApiResponse> GetContactByIdAsync(int userId, int contactId);
        Task<ApiResponse> UpdateContactAsync(int userId, int contactId, ContactDto model);
        Task<ApiResponse> DeleteContactAsync(int userId, int contactId);
    }
}
