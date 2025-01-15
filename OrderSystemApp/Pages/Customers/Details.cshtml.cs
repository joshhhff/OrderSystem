using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OrderSystemApp.Data;
using OrderSystemApp.Models;
using OrderSystemApp.Services.Auth;

namespace OrderSystemApp.Pages.Customers
{
    public class DetailsModel : PageModel
    {
        private readonly OrderSystemApp.Data.SystemContext _context;
        private readonly IAuthService _session;

        public DetailsModel(OrderSystemApp.Data.SystemContext context, IAuthService session)
        {
            _context = context;
            _session = session;
        }

        public Customer Customer { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var validSession = _session.AuthenticateUserSession();

            if (validSession is null)
            {
                Response.Redirect("../");
                return null;
            }

            bool? validPermissions = _session.CheckPermissions();

            if (validPermissions == false || validPermissions is null)
            {
                Response.Redirect("../InvalidPermissions");
                return null;
            }

            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .Include(s => s.Orders)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (customer == null)
            {
                return NotFound();
            }
            else
            {
                Customer = customer;
            }
            return Page();
        }
    }
}
