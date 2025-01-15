using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using OrderSystemApp.Data;
using OrderSystemApp.Factories.UserFactory;
using OrderSystemApp.Models;
using OrderSystemApp.Services.Auth;

namespace OrderSystemApp.Pages.Users
{
    public class CreateModel : PageModel
    {
        private readonly OrderSystemApp.Data.SystemContext _context;
        private readonly IAuthService _auth;
        private readonly IUserFactory _userFactory;

        public CreateModel(OrderSystemApp.Data.SystemContext context, IAuthService session, IUserFactory userFactory)
        {
            _context = context;
            _auth = session;
            _userFactory = userFactory;
        }

        public IActionResult OnGet()
        {
            var validSession = _auth.AuthenticateUserSession();

            if (validSession is null)
            {
                Response.Redirect("../");
                return null;
            }

            bool? validPermissions = _auth.CheckPermissions();

            if (validPermissions == false || validPermissions is null)
            {
                Response.Redirect("../InvalidPermissions");
                return null;
            }
            ViewData["Customers"] = new SelectList(_context.Customer, "ID", "FirstName");
            ViewData["Roles"] = new SelectList(_context.Role, "ID", "RoleName");
            return Page();
        }

        [BindProperty]
        public User User { get; set; }

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            int roleId = int.Parse(User.RoleIdString);

            int customerId;

            // if new user role is associated with a customer (customer login) then set necessary values
            if (User.CustomerIdString is not null)
            {
                customerId = int.Parse(User.CustomerIdString);
                var customer = await _context.Customer.FirstOrDefaultAsync(m => m.ID == customerId);
                User.Customer = customer;
            }

            User.Role = await _context.Role.FirstOrDefaultAsync(m => m.ID == roleId);
            User.Password = "orderSystem2024!";


            var createUserResult = await _auth.CreateUser(User);
            if (!createUserResult.IsSuccess) 
            { 
                // error
            }

            if (createUserResult.Value is null) 
            { 
                // user not created, user exists already, etc
            }

            return RedirectToPage("./Index");
        }
    }
}
