using System.Text.Json;
using System.Text.Json.Serialization;
public class Seat
{
    [JsonPropertyName("id")]
    public string ID { get; set; }

    [JsonPropertyName("isAvailable")]
    public bool IsAvailable { get; set; }

    [JsonPropertyName("price")]
    public double Price { get; set; }

    public Seat(string id, double price)
    {
        ID = id;
        IsAvailable = true;
        Price = price;
    }
}
