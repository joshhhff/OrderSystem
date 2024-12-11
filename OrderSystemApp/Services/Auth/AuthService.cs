using CO550WebApp.Utils;
using Microsoft.EntityFrameworkCore;
using OrderSystemApp.Data;
using OrderSystemApp.Factories.UserFactory;

namespace CO550WebApp.Services.Auth;

public sealed class AuthService(SystemContext context, IUserFactory userFactory) : IAuthService
{
    private readonly SystemContext _context = context;
    private readonly IUserFactory _userFactory = userFactory;

    public async Task<Result<bool>> CreateUser(string email, string password)
    {
        try
        {
            var user = _userFactory.Create(email, password);

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Err(ex.Message);
        }
    }

    public async Task<Result<bool>> Login(string email, string password)
    {
        try
        {
            var user = await _context.Users.AsNoTracking()
                .Where(x => x.Email == email)
                .Where(x => x.Password == password)
                .FirstOrDefaultAsync();

            return Result<bool>.Ok(user is not null);
        }
        catch (Exception ex)
        {
            return Result<bool>.Err(ex.Message);
        }
    }

    public async Task<Result<bool>> Logout()
    {
        return Result<bool>.Ok(true);
    }
}
