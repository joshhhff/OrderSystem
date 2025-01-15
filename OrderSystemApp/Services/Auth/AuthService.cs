using OrderSystemApp.Utils;
using Microsoft.EntityFrameworkCore;
using OrderSystemApp.Data;
using OrderSystemApp.Models;
using OrderSystemApp.Factories.UserFactory;
using System.ComponentModel;

namespace OrderSystemApp.Services.Auth;

public sealed class AuthService(SystemContext context, IUserFactory userFactory, IHttpContextAccessor session, IPasswordHasher hasher) : IAuthService
{
    private readonly SystemContext _context = context;
    private readonly IHttpContextAccessor _session = session;
    private readonly IPasswordHasher _hasher = hasher;

    public async Task<Result<User>> CreateUser(User user)
    {
        try
        {
            user.Password = _hasher.HashPassword(user.Password);

            await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();

            return Result<User>.Ok(user);
        }
        catch (Exception ex)
        {
            return Result<User>.Err(ex.Message);
        }
    }

    public async Task<Result<User>> Login(string email, string password)
    {
        try
        {
            var user = await _context.User.AsNoTracking()
                .Where(x => x.Email == email).FirstOrDefaultAsync();
            
            if (user is null) 
                return Result<User>.Ok(null);

            if (_hasher.VerifyPassword(password, user.Password))
            {
                return Result<User>.Ok(user);
            }

            return Result<User>.Ok(null);
        }
        catch (Exception ex)
        {
            return Result<User>.Err(ex.Message);
        }
    }

    public Result<bool> Logout()
    {
        _session.HttpContext.Session.Clear();
        return Result<bool>.Ok(true);
    }
    public string? AuthenticateUserSession()
    {
        var currentUser = _session.HttpContext.Session.GetString("CurrentUserId");
        return currentUser;
    }

    public bool? CheckPermissions()
    {
        var currentUserSession = AuthenticateUserSession();
        if (string.IsNullOrEmpty(currentUserSession) || !int.TryParse(currentUserSession, out var currentUser))
        {
            return null;
        }
        var user = _context.User.AsNoTracking().Where(x => x.ID == currentUser).FirstOrDefault();
        if (user is null)
        {
            return null;
        }
        if (user.RoleIdString == "2")
        {
            return true;
        }
        return false;
    }
}
