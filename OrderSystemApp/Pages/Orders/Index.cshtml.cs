using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OrderSystemApp.Models;
using OrderSystemApp.Data;

namespace OrderSystemApp.Pages.Orders
{
    public class IndexModel : PageModel
    {
        private readonly OrderSystemApp.Data.SystemContext _context;

        public IndexModel(OrderSystemApp.Data.SystemContext context)
        {
            _context = context;
        }

        public IList<Order> Order { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Order = await _context.Order
                .Include(t => t.Customer)
                .ToListAsync();
        }
    }
}
