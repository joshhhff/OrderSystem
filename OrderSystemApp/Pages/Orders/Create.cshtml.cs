using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrderSystemApp.Data;
using Microsoft.EntityFrameworkCore;
using OrderSystemApp.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OrderSystemApp.Pages.Orders
{
    public class CreateModel : PageModel
    {
        private readonly OrderSystemApp.Data.SystemContext _context;
        
        [BindProperty]
        public string StringifiedItems { get; set; }

        public CreateModel(OrderSystemApp.Data.SystemContext context)
        {
            _context = context;
        }
        
        public async Task<string> GenerateOrderNumberAsync()
        {
            // Get the latest order number from the database
            var latestOrder = await _context.Order
                .OrderByDescending(o => o.ID)
                .Select(o => o.OrderNumber)
                .FirstOrDefaultAsync();

            // Extract the numeric part and increment it
            var nextValue = latestOrder != null 
                ? long.Parse(latestOrder.Substring(2)) + 1 
                : 1;

            // Format the number
            return $"PO{nextValue:D9}";
        }
        
        private class OrderJson<T>
        {
            [JsonPropertyName("total")]
            public required string Total { get; set; }
            [JsonPropertyName("taxTotal")]
            public required string TaxTotal { get; set; }
            [JsonPropertyName("grossAmount")]
            public required string GrossAmount { get; set; }
            [JsonPropertyName("lines")]
            public required T[] Items { get; set; }

            public override string ToString()
            {
                return $"Items: {Items}";
            }
        }

        public IActionResult OnGet()
        {
            ViewData["Customers"] = new SelectList(_context.Customer, "ID", "FirstName");
            ViewData["Products"] = new SelectList(_context.Product, "ID", "Name");
            ViewData["ShippingMethods"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Value = "Standard", Text = "Standard" },
                new SelectListItem { Value = "Premium", Text = "Premium" }
            }, "Value", "Text");
            ViewData["PaymentMethods"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Value = "Credit/Debit", Text = "Standard" },
                new SelectListItem { Value = "Apple Pay", Text = "Apple Pay" },
                new SelectListItem { Value = "Google Pay", Text = "Google Pay" },
                new SelectListItem { Value = "Klarna", Text = "Klarna" }
            }, "Value", "Text");
            
            return Page();
        }

        [BindProperty]
        public Order Order { get; set; } = default!;

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
            
            var order = JsonSerializer.Deserialize<OrderJson<OrderLine>>(StringifiedItems);    // Parse JSON into classes
            var newOrderNumber = await GenerateOrderNumberAsync();

            Order.Total = Decimal.Parse(order.Total);
            Order.TaxTotal = Decimal.Parse(order.TaxTotal);
            Order.GrossAmount = Decimal.Parse(order.GrossAmount);
            Order.OrderNumber = newOrderNumber.ToString();
            Order.Status = "Not Shipped";    // default to not shipped for new orders
            
            Console.WriteLine("Shipping Method" + Order.PaymentMethod);
            
            _context.Order.Add(Order);    // save order to get id
            await _context.SaveChangesAsync();
            
            var lines = order.Items;
            
            if (lines!.Length > 0)
            {
                Order.OrderLines = lines;    // set lines on Order model
                foreach (var line in lines)
                {
                    line.OrderId = Order.ID;   // Set new order id to relate item line to order
                    _context.OrderLine.Add(line);
                }
            }
            
            await _context.SaveChangesAsync();    // save new changes

            return RedirectToPage("./Details", new { id = Order.ID});    // redirect to details page of new order
        }
    }
}
