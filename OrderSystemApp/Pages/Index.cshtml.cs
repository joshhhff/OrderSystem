using CO550WebApp.Services.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CO550WebApp.Pages;

public class IndexModel : PageModel
{
    private readonly IAuthService _auth;

    private readonly ILogger<IndexModel> _logger;

    [BindProperty]
    public string Email { get; set; }
    [BindProperty]
    public string Password { get; set; }

    public IndexModel(IAuthService auth, ILogger<IndexModel> logger)
    {
        _auth = auth;
        _logger = logger;
    }

    public void OnGet()
    {

    }

    public async Task OnPost()
    {
        var result = await _auth.Login(Email, Password);
        if (!result.IsSuccess) return;

        HttpContext.Session.SetString("TokenAuth", "TestToken");
        Response.Redirect("/Home");
    }
}