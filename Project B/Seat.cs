using System.Text.Json;
using System.Text.Json.Serialization;
public class Seat : ISellable
{
    [JsonPropertyName("id")]
    public string ID { get; set; }

    [JsonPropertyName("isAvailable")]
    public bool IsAvailable { get; set; } = true;

    [JsonPropertyName("price")]
    public double Price { get; set; } = 8.99;

    public Seat(string id)
    {
        ID = id;
    }
}
