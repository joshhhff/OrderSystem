using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OrderSystemApp.Models;
using OrderSystemApp.Data;

namespace OrderSystemApp.Pages.Orders
{
    public class EditModel : PageModel
    {
        private readonly OrderSystemApp.Data.SystemContext _context;

        public EditModel(OrderSystemApp.Data.SystemContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Order Order { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewData["Customers"] = new SelectList(_context.Customer, "ID", "FirstName");
            // Populate the shipping methods
            ViewData["EditShippingMethods"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Value = "Standard", Text = "Standard" },
                new SelectListItem { Value = "Premium", Text = "Premium" }
            }, "Value", "Text");
            ViewData["PaymentMethods"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Value = "Credit/Debit", Text = "Credit/Debit" },
                new SelectListItem { Value = "Apple Pay", Text = "Apple Pay" },
                new SelectListItem { Value = "Google Pay", Text = "Google Pay" },
                new SelectListItem { Value = "Klarna", Text = "Klarna" }
            }, "Value", "Text");

            var order = await _context.Order
                .Include(o => o.OrderLines)  // Include OrderLines
                .Include(o => o.Customer)    // Include Customer
                .FirstOrDefaultAsync(m => m.ID == id);
            if (order == null)
            {
                return NotFound();
            }

            Order = order;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            var customerId = int.Parse(Order.CustomerName);
            
            // Get related customer entity from select value
            var customer = await _context.Customer.FirstOrDefaultAsync(c => c.ID == customerId);
            Order.Customer = customer;

            _context.Attach(Order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(Order.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Details", new { id = Order.ID});
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.ID == id);
        }
    }
}
