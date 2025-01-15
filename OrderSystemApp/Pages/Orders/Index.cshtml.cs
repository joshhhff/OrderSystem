using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OrderSystemApp.Services.Auth;
using OrderSystemApp.Models;
using OrderSystemApp.Data;

namespace OrderSystemApp.Pages.Orders
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

        public IList<Order> Order { get;set; } = default!;

        public async Task OnGetAsync()
        {
            var validSession = _session.AuthenticateUserSession();

            if (validSession is null)
            {
                Response.Redirect("../");
                return;
            }

            var user = await _context.User.FirstOrDefaultAsync(m => m.ID == int.Parse(validSession));

            if (user.RoleIdString == "1")
            {
                Order = await _context.Order
                    .Include(t => t.Customer)
                    .Where(t => t.CustomerName == user.CustomerIdString)
                    .ToListAsync();
            } else
            {
                Order = await _context.Order
                .Include(t => t.Customer)
                .ToListAsync();
            }
            
        }
    }
}
