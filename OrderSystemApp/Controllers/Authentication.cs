using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OrderSystemApp.Data;
using OrderSystemApp.Services.Auth;

[Route("api/[Controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthService _session;
    private readonly SystemContext _context;

    public AuthenticationController(SystemContext context, IAuthService session)
    {
        _context = context;
        _session = session;
    }

    [HttpGet]
    [Route("logout")]
    public IActionResult Logout()
    {
        _session.Logout();
        return Ok(true);
    }

    [HttpGet]
    [Route("permissions")]
    public IActionResult Permissions()
    {
        var validSession = _session.AuthenticateUserSession();    // returns id of the current user of the session

        if (validSession is null)
        {
            return Ok(new PermissionsResponse { RoleID = null, LoggedIn = false });
        }

        var user = _context.User.FirstOrDefault(m => m.ID == int.Parse(validSession));
        if (user is null)
        {
            return NotFound("User not found");
        }

        return Ok(new PermissionsResponse{ RoleID = user.RoleIdString, LoggedIn = true });
    }
}

public class PermissionsResponse
{
    public string? RoleID { get; set; }
    public bool LoggedIn { get; set; }
}