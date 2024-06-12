public class Product : ISellable
{
    public string Naam { get; set; }
    public double Price { get; set; }

    public Product(string naam, double price)
    {
        Naam = naam;
        Price = price;
    }
}
