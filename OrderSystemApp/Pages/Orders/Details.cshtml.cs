using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OrderSystemApp.Data;
using OrderSystemApp.Models;

namespace OrderSystemApp.Pages.Orders
{
    public class DetailsModel : PageModel
    {
        private readonly OrderSystemApp.Data.SystemContext _context;

        public DetailsModel(OrderSystemApp.Data.SystemContext context)
        {
            _context = context;
        }

        public Order Order { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

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
    }
}