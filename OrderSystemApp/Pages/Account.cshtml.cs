using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OrderSystemApp.Services.Auth;
using OrderSystemApp.Data;
using OrderSystemApp.Models;

namespace OrderSystemApp.Pages
{
    public class AccountModel : PageModel
    {
        private readonly OrderSystemApp.Data.SystemContext _context;
        private readonly IAuthService _session;

        public AccountModel(OrderSystemApp.Data.SystemContext context, IAuthService session)
        {
            _context = context;
            _session = session;
        }

        public User User { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var validSession = _session.AuthenticateUserSession();    // returns id of the current user of the session

            if (validSession is null)
            {
                return RedirectToPage("./Index");
            }

            var user = await _context.User
                .Include(t => t.Role)
                .Include(t => t.Customer)
                .FirstOrDefaultAsync(m => m.ID == int.Parse(validSession));
            
            User = user;

            return Page();
        }
    }
}
