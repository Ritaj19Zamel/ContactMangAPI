
namespace ContactMangAPI.Services
{
    public interface IAuthService
    {
        Task<ApiResponse> RegisterAsync(RegisterDto model);
        Task<ApiResponse> LoginAsync(LoginDto model);
    }
}
