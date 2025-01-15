using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrderSystemApp.Data;
using OrderSystemApp.Models;
using OrderSystemApp.Services.Auth;

namespace OrderSystemApp.Pages.Customers
{
    public class CreateModel : PageModel
    {
        private readonly OrderSystemApp.Data.SystemContext _context;
        private readonly IAuthService _session;

        public CreateModel(OrderSystemApp.Data.SystemContext context, IAuthService session)
        {
            _context = context;
            _session = session;
        }

        public IActionResult OnGet()
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

            return Page();
        }

        [BindProperty]
        public Customer Customer { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Customer.Add(Customer);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
