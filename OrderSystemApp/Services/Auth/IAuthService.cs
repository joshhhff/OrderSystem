using OrderSystemApp.Utils;
using OrderSystemApp.Models;

namespace OrderSystemApp.Services.Auth;

public interface IAuthService
{
    public Task<Result<User>> Login(string email, string password);
    public Task<Result<User>> CreateUser(User user);
    public Result<bool> Logout();
    public string? AuthenticateUserSession();
    public bool? CheckPermissions();
}