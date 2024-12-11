using CO550WebApp.Utils;

namespace CO550WebApp.Services.Auth;

public interface IAuthService
{
    public Task<Result<bool>> Login(string email, string password);
    public Task<Result<bool>> CreateUser(string email, string password);
    public Task<Result<bool>> Logout();
}