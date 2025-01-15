using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OrderSystemApp.Services.Auth;

namespace OrderSystemApp.Pages
{
    public class InvalidPermissionsModel : PageModel
    {
        private readonly IAuthService _session;
        public InvalidPermissionsModel(IAuthService session)
        {
            _session = session;
        }

        public void OnGet()
        {
            var validSession = _session.AuthenticateUserSession();

            if (validSession is null)
            {
                Response.Redirect("../");
            }
        }
    }
}
