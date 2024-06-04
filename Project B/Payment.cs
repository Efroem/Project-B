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

        int selectedOption = 0;
        int optiesLength = opties.Length;

        do
        {
            Console.Clear();
            AsciiArtPrinter.PrintAsciibetaling();
            Console.WriteLine();
            Console.ResetColor();
            PrintTextCentered(prompt);
            for (int i = 0; i < opties.Length; i++)
            {
                if (i == selectedOption)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    PrintTextCentered(opties[i]);
                    Console.ResetColor();
                }
                else
                {
                    PrintTextCentered(opties[i]);
                }
            }

            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.UpArrow && selectedOption > 0)
            {
                selectedOption--;
            }
            else if (key.Key == ConsoleKey.DownArrow && selectedOption < opties.Length - 1)
            {
                selectedOption++;
            }
            else if (key.Key == ConsoleKey.Enter)
            {
                break;
            }

            // Wis de vorige opties weergave
            Console.CursorTop -= (optiesLength + 1); // +1 om ook de prompt te wissen
        } while (true);

        return selectedOption;
    }

    private static void PrintTextCentered(string text)
    {
        string[] lines = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        int windowWidth = Console.WindowWidth;

        foreach (string line in lines)
        {
            int leftPadding = (windowWidth - line.Length) / 2;
            if (leftPadding < 0)
                leftPadding = 0;

            Console.SetCursorPosition(leftPadding, Console.CursorTop);
            Console.WriteLine(line);
        }
    }

    public static void BestelMenu()
    {
        string[] menuOptions = { "Eten", "Drinken", "Prijzenlijst", "Afrekenen" };
        int selectedIndex = KiesOptie("Wat wilt u bestellen?", menuOptions);

        switch (selectedIndex)
        {
            case 0:
                BestelEten();
                break;
            case 1:
                BestelDrinken();
                break;
            case 2:
                string[] priceOptions = { "Prijzen eten", "Prijzen frisdranken", "Prijzen koffie of thee" };
                int priceChoice = KiesOptie("Welke prijslijst wilt u bekijken?", priceOptions);

                switch (priceChoice)
                {
                    case 0:
                        AsciiArtPrinter.PrintAsciietenprijzen();
                        break;
                    case 1:
                        AsciiArtPrinter.PrintAsciifrisdrank();
                        break;
                    case 2:
                        AsciiArtPrinter.PrintAsciiTheekoffie();
                        break;
                }
                break;
        }
    }

    public static void BestelEten()
    {
        string[] opties = { "Popcorn", "Nachos", "Chips", "Terug naar menu" };
        double[][] prijzen = { popcornPrijzen, nachosPrijzen, chipsPrijzen }; // Een array van prijzenarrays voor elk voedseltype
        // PrintTextCentered("Wat wilt u bestellen?");
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
        PrintTextCentered("Wilt u nog meer eten/drinken bestellen of afrekenen?");
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
        int popcornchoice = KiesOptie("Kies gewenste smaak", popcorn);
        int selectedIndex = KiesOptie("Kies de grootte voor uw popcorn:", opties);
        string gekozenpopcorn = popcorn[popcornchoice];
        string gekozenGrootte = opties[selectedIndex];
        double gekozenPrijs = prijzen[selectedIndex];
        PrintTextCentered($"\nU heeft gekozen voor {gekozenGrootte} Popcorn {gekozenpopcorn} - €{gekozenPrijs:0.00}");

        // Bereken de totaalkosten
        totaalKosten += gekozenPrijs;

        PrintTextCentered("\nUw bestelling is toegevoegd.");
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

        PrintTextCentered($"\nU heeft gekozen voor {gekozenGrootte} nachos - €{gekozenPrijs:0.00}");

        // Bereken de totaalkosten
        totaalKosten += gekozenPrijs;

        PrintTextCentered("\nUw bestelling is toegevoegd.");
        WiltMeerBestellen();
    }

    public static void Chips(double[] prijzen)
    {
        string[] chips = { "Naturel", "Paprika" };
        string[] opties = { "Klein", "Middel", "Groot" };
        int chipschoice = KiesOptie("Kies de smaak voor uw chips:", chips);
        int selectedIndex = KiesOptie("Kies de grootte voor uw chips:", opties);
        string gekozenchips = chips[chipschoice];
        string gekozenGrootte = opties[selectedIndex];
        double gekozenPrijs = prijzen[selectedIndex];
        PrintTextCentered($"\nU heeft gekozen voor {gekozenGrootte} {gekozenchips} chips - €{gekozenPrijs:0.00}");

        // Bereken de totaalkosten
        totaalKosten += gekozenPrijs;

        PrintTextCentered("\nUw bestelling is toegevoegd.");
        WiltMeerBestellen();
    }

    public static void Frisdrank(double[] prijzen)
    {
        string[] frisdrank = { "Cola", "Fanta", "Icetea sparkling", "Icetea green", "Cassis", "Fernandes" };
        int frisdrankChoice = KiesOptie("Kies frisdrank naar keuze:", frisdrank);
        string gekozenFrisdrank = frisdrank[frisdrankChoice];
        double gekozenPrijs = prijzen[frisdrankChoice];
        PrintTextCentered($"\nU heeft gekozen voor {gekozenFrisdrank} - €{gekozenPrijs:0.00}");
        totaalKosten += gekozenPrijs;

        PrintTextCentered("\nUw bestelling is toegevoegd.");
        WiltMeerBestellen();
    }

    public static void Thee(double[] prijzen)
    {
        string[] thee = { "Earl grey", "Jasmijn thee", "Groene thee", "Rooibos thee", "Munt thee" };
        int theechoice = KiesOptie("Kies thee naar keuze:", thee);
        string gekozenthee = thee[theechoice];
        double gekozenPrijs = prijzen[theechoice];
        PrintTextCentered($"\nU heeft gekozen voor {gekozenthee} - €{gekozenPrijs:0.00}");

        // Bereken de totaalkosten
        totaalKosten += gekozenPrijs;

        PrintTextCentered("\nUw bestelling is toegevoegd.");
        WiltMeerBestellen();
    }

    public static void Koffie(double[] prijzen)
    {
        string[] opties = { "Klein", "Middel", "Groot" };
        int selectedIndex = KiesOptie("Kies de grootte voor uw Koffie:", opties);
        string gekozenGrootte = opties[selectedIndex];
        double gekozenPrijs = prijzen[selectedIndex];
        PrintTextCentered($"\nU heeft gekozen voor {gekozenGrootte} Koffie - €{gekozenPrijs:0.00}");

        // Bereken de totaalkosten
        totaalKosten += gekozenPrijs;

        PrintTextCentered("\nUw bestelling is toegevoegd.");
        WiltMeerBestellen();
    }
    public static void afrekenen()
    {
        string[] opties = { "IDEAL", "PayPal", "Creditard" };
        int selectedIndex = KiesOptie("Kies de gewenste betaalmethode::", opties);
    }
}