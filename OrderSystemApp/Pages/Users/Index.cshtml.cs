using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OrderSystemApp.Data;
using OrderSystemApp.Services.Auth;
using OrderSystemApp.Models;

namespace OrderSystemApp.Pages.Users
{
    public class IndexModel : PageModel
    {
        private readonly OrderSystemApp.Data.SystemContext _context;
        private readonly IAuthService _session;

        public IndexModel(OrderSystemApp.Data.SystemContext context, IAuthService session)
        {
            _context = context;
            _session = session;
        }

        public IList<User> User { get;set; } = default!;

        public async Task OnGetAsync()
        {
            var validSession = _session.AuthenticateUserSession();

            if (validSession is null)
            {
                Response.Redirect("../");
            }

            bool? validPermissions = _session.CheckPermissions();

            if (validPermissions == false || validPermissions is null)
            {
                Response.Redirect("../InvalidPermissions");
            }

            User = await _context.User.Include(u => u.Role).ToListAsync();
        }
    }
}
