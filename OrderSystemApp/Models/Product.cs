namespace OrderSystemApp.Models;

public class Product
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Rate { get; set;  }
    public string TaxRateString { get; set; }
    public decimal TaxRateDecimal { get; set; }
}