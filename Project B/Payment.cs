public static class Payment
{
    private static bool bestellingGeplaatst;
    private static double totaalKosten = 0;

    // Definieer de prijzen
    private static readonly double[] popcornPrijzen = { 3.50, 5.00, 7.00 };
    private static readonly double[] nachosPrijzen = { 4.00, 6.00, 8.00 };
    private static readonly double[] chipsPrijzen = { 2.00, 3.50, 5.00 };
    private static readonly double[] frisdrankPrijzen = { 3.50, 3.00, 3.50, 3.00, 2.50, 3.00 };
    private static readonly double[] koffiePrijzen = { 2.50, 3.50, 5.00 };
    private static readonly double[] theePrijzen = { 3.00, 3.50, 3.00, 2.50, 4.50 };

    static Payment()
    {
        bestellingGeplaatst = false;
    }

    private static int KiesOptie(string prompt, string[] opties)
    {
        Console.WriteLine(prompt);
        int selectedIndex = 0;

        while (true)
        {
            for (int i = 0; i < opties.Length; i++)
            {
                if (i == selectedIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    PrintTextCentered($"   {opties[i]}");
                    Console.ResetColor();
                }
                else
                {
                    PrintTextCentered($"   {opties[i]}");
                }
            }

            ConsoleKeyInfo key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.UpArrow)
            {
                selectedIndex = (selectedIndex - 1 + opties.Length) % opties.Length;
            }
            else if (key.Key == ConsoleKey.DownArrow)
            {
                selectedIndex = (selectedIndex + 1) % opties.Length;
            }
            else if (key.Key == ConsoleKey.Enter)
            {
                break;
            }
            Console.Clear();
            Console.WriteLine(prompt);
        }

        return selectedIndex;
    }

    public static void BestelMenu()
    {
        string[] opties = { "Eten", "Drinken", "Afrekenen" };
        int selectedIndex = KiesOptie("Wat wilt u bestellen?", opties);

        switch (selectedIndex)
        {
            case 0:
                BestelEten();
                break;
            case 1:
                BestelDrinken();
                break;

        }
    }

    public static void BestelEten()
    {
        string[] opties = { "Popcorn", "Nachos", "Chips", "Terug naar menu" };
        double[][] prijzen = { popcornPrijzen, nachosPrijzen, chipsPrijzen }; // Een array van prijzenarrays voor elk voedseltype
        int selectedIndex = KiesOptie("Wat wilt u bestellen?", opties);

        switch (selectedIndex)
        {
            case 0:
                Popcorn(prijzen[selectedIndex]); // We geven de juiste prijzenarray door aan de Popcorn-methode
                break;
            case 1:
                Nachos(prijzen[selectedIndex]); // We geven de juiste prijzenarray door aan de Nachos-methode
                break;
            case 2:
                Chips(prijzen[selectedIndex]); // We geven de juiste prijzenarray door aan de Chips-methode
                break;
            case 3:
                BestelMenu(); // Terug naar menu
                break;
        }
    }

    public static void BestelDrinken()
    {
        string[] opties = { "Frisdrank", "Thee", "Koffie", "Terug naar menu" };
        double[][] prijzen = { frisdrankPrijzen, theePrijzen, koffiePrijzen }; // Een array van prijzenarrays voor elk dranktype
        int selectedIndex = KiesOptie("Wat wilt u bestellen?", opties);

        switch (selectedIndex)
        {
            case 0:
                Frisdrank(prijzen[selectedIndex]); // We geven de juiste prijzenarray door aan de Frisdrank-methode
                break;
            case 1:
                Thee(prijzen[selectedIndex]); // We geven de juiste prijzenarray door aan de Thee-methode
                break;
            case 2:
                Koffie(prijzen[selectedIndex]); // We geven de juiste prijzenarray door aan de Koffie-methode
                break;
            case 3:
                BestelMenu(); // Terug naar menu
                break;
        }
    }

    public static void WiltMeerBestellen()
    {
        string[] opties = { "Ja, nog meer bestellen", "Ik wil afrekenen" };
        int selectedIndex = KiesOptie("Wilt u nog meer eten/drinken bestellen of afrekenen?", opties);

        if (selectedIndex == 0)
        {
            BestelMenu();
        }
    }

    public static void Popcorn(double[] prijzen)
    {
        string[] popcorn = { "Zoet", "Zout", "Gemixt" };
        string[] opties = { "Klein", "Middel", "Groot" };
        Console.Clear();
        int popcornchoice = KiesOptie("Kies gewenste smaak", popcorn);
        Console.Clear();
        int selectedIndex = KiesOptie("Kies de grootte voor uw popcorn:", opties);
        Console.Clear();
        string gekozenpopcorn = opties[popcornchoice];
        string gekozenGrootte = opties[selectedIndex];
        double gekozenPrijs = prijzen[selectedIndex];
        Console.WriteLine($"\nU heeft gekozen voor {gekozenGrootte} Popcorn {gekozenpopcorn} - €{gekozenPrijs:0.00}");

        // Bereken de totaalkosten
        totaalKosten += gekozenPrijs;

        Console.WriteLine("\nUw bestelling is toegevoegd.");
        WiltMeerBestellen();
    }

    public static void Nachos(double[] prijzen)
    {
        string[] opties = { "Klein", "Middel", "Groot" };
        Console.Clear();
        int selectedIndex = KiesOptie("Kies de grootte voor uw nachos:", opties);
        Console.Clear();
        string gekozenGrootte = opties[selectedIndex];
        double gekozenPrijs = prijzen[selectedIndex];
        Console.WriteLine($"\nU heeft gekozen voor {gekozenGrootte} nachos - €{gekozenPrijs:0.00}");

        // Bereken de totaalkosten
        totaalKosten += gekozenPrijs;

        Console.WriteLine("\nUw bestelling is toegevoegd.");
        WiltMeerBestellen();
    }

    public static void Chips(double[] prijzen)
    {
        string[] chips = { "Naturel", "Paprika" };
        string[] opties = { "Klein", "Middel", "Groot" };
        Console.Clear();
        int chipschoice = KiesOptie("Kies de smaak voor uw chips:", chips);
        Console.Clear();
        int selectedIndex = KiesOptie("Kies de grootte voor uw chips:", opties);
        Console.Clear();
        string gekozenchips = opties[chipschoice];
        string gekozenGrootte = opties[selectedIndex];
        double gekozenPrijs = prijzen[selectedIndex];
        Console.WriteLine($"\nU heeft gekozen voor {gekozenGrootte} {gekozenchips} chips - €{gekozenPrijs:0.00}");

        // Bereken de totaalkosten
        totaalKosten += gekozenPrijs;

        Console.WriteLine("\nUw bestelling is toegevoegd.");
        WiltMeerBestellen();

    }
    public static void Frisdrank(double[] prijzen)
    {
        string[] frisdrank = { "Cola", "Fanta", "Icetea sparkling", "Icetea green", "Cassis", "Fernandes" };
        Console.Clear();
        int frisdrankChoice = KiesOptie("Kies frisdrank naar keuze:", frisdrank);
        Console.Clear();
        string gekozenFrisdrank = frisdrank[frisdrankChoice];
        double gekozenPrijs = prijzen[frisdrankChoice];
        Console.WriteLine($"\nU heeft gekozen voor {gekozenFrisdrank} - €{gekozenPrijs:0.00}");
        totaalKosten += gekozenPrijs;

        Console.WriteLine("\nUw bestelling is toegevoegd.");
        WiltMeerBestellen();

    }

    public static void Thee(double[] prijzen)
    {
        string[] thee = { "Earl grey", "Jasmijn thee", "Groene thee", "Rooibos thee", "Munt thee" };
        Console.Clear();
        int theechoice = KiesOptie("Kies thee naar keuze:", thee);
        Console.Clear();
        string gekozenthee = thee[theechoice];
        double gekozenPrijs = prijzen[theechoice];
        Console.WriteLine($"\nU heeft gekozen voor {gekozenthee} - €{gekozenPrijs:0.00}");

        // Bereken de totaalkosten
        totaalKosten += gekozenPrijs;

        Console.WriteLine("\nUw bestelling is toegevoegd.");
        WiltMeerBestellen();

    }
    public static void Koffie(double[] prijzen)
    {
        string[] opties = { "Klein", "Middel", "Groot" };
        Console.Clear();
        int selectedIndex = KiesOptie("Kies de grootte voor uw Koffie:", opties);
        Console.Clear();
        string gekozenGrootte = opties[selectedIndex];
        double gekozenPrijs = prijzen[selectedIndex];
        Console.WriteLine($"\nU heeft gekozen voor {gekozenGrootte} Koffie - €{gekozenPrijs:0.00}");

        // Bereken de totaalkosten
        totaalKosten += gekozenPrijs;

        Console.WriteLine("\nUw bestelling is toegevoegd.");
        WiltMeerBestellen();

    }

    public static void PrintTextCentered(string text)
    {
        int windowWidth = Console.WindowWidth;
        int leftPadding = (windowWidth - text.Length) / 2;

        Console.SetCursorPosition(leftPadding, Console.CursorTop);
        Console.WriteLine(text);
    }
}
