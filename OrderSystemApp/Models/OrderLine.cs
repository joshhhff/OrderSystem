using System.Text.Json.Serialization;

namespace OrderSystemApp.Models;

public class OrderLine
{
    public int ID { get; set; }
    [JsonPropertyName("item")]
    public string Item { get; set; }
    [JsonPropertyName("description")]
    public string Description { get; set; }
    [JsonPropertyName("rate")]
    public string Rate { get; set; }
    [JsonPropertyName("taxRate")]
    public string TaxRate { get; set; }
    [JsonPropertyName("quantity")]
    public string Quantity { get; set; }
    [JsonPropertyName("total")]
    public string Total { get; set; }
    [JsonPropertyName("taxTotal")]
    public string TaxTotal { get; set; }
    [JsonPropertyName("grossAmount")]
    public string GrossAmount { get; set; }

    public int OrderId { get; set; }

    public override string ToString()
    {
        var values = $"Order ID: {OrderId} - Item: {Item} - Description: {Description} - Rate: {Rate} - Tax Rate: {TaxRate} - Quantity: {Quantity} - Total: {Total} - TaxTotal: {TaxTotal} - GrossAmount {GrossAmount}";
        return values;
    }
}