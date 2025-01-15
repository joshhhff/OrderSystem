using OrderSystemApp.Services.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OrderSystemApp.Pages;

public class IndexModel : PageModel
{
    private readonly IAuthService _auth;

    private readonly ILogger<IndexModel> _logger;

    [BindProperty]
    public string Email { get; set; }
    [BindProperty]
    public string Password { get; set; }
    public string Error { get; set; }

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
        if (result.Value is null)
        {
            this.Error = "Invalid email or password";
            return;
        }

        HttpContext.Session.SetString("CurrentUserId", result.Value!.ID.ToString());
        Response.Redirect("/Orders/Index");
    }
}