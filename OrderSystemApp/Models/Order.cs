using Microsoft.VisualStudio.Web.CodeGenerators.Mvc;

namespace OrderSystemApp.Models;

public class Order
{
    public int ID { get; set; }
    public string OrderNumber { get; set; }
    public string Status { get; set; }

    public string CustomerName { get; set; }

    public string Email { get; set; }

    public string Memo { get; set; }

    public string ShippingMethod { get; set; }

    public string Phone { get; set; }

    public string PaymentMethod { get; set; }
    
    public decimal Total { get; set; }
    
    public decimal TaxTotal { get; set; }
    
    public decimal GrossAmount { get; set; }
    
    public ICollection<OrderLine> OrderLines { get; set; }

    public Customer Customer { get; set; }

    public override string ToString()
    {
        return $"{ID} - {OrderNumber} - {Status} - {CustomerName} - {Email} - {Memo} - {ShippingMethod} - {Phone} - {PaymentMethod} - {Total} - {TaxTotal} - {GrossAmount}";
    }

}