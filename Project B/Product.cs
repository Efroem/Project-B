public class Product
{
    public string Naam { get; set; }
    public double Prijs { get; set; }

    public Product(string naam, double prijs)
    {
        Naam = naam;
        Prijs = prijs;
    }
}
