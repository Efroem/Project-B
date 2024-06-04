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
            Console.Clear(); // Maak het scherm leeg voordat je de opties weergeeft
            AsciiArtPrinter.PrintAsciibetaling();
            Console.ResetColor();
            PrintTextCentered(prompt); // Print de prompt opnieuw nadat het scherm is geleegd
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
        AsciiArtPrinter.PrintAsciibetaling();
        string[] opties = { "Eten", "Drinken", "Afrekenen" };
        // PrintTextCentered("Wat wilt u bestellen?");
        int selectedIndex = KiesOptie("Wat wilt u bestellen?", opties);
        switch (selectedIndex)
        {
            case 0:
                BestelEten();
                break;
            case 1:
                BestelDrinken();
                break;
            case 2:
                break;
        }
    }

    public static void BestelEten()
    {
        AsciiArtPrinter.PrintAsciibetaling();
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
        AsciiArtPrinter.PrintAsciibetaling();
        string[] opties = { "Frisdrank", "Thee", "Koffie", "Terug naar menu" };
        double[][] prijzen = { frisdrankPrijzen, theePrijzen, koffiePrijzen }; // Een array van prijzenarrays voor elk dranktype
        // PrintTextCentered("Wat wilt u bestellen?");
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
        AsciiArtPrinter.PrintAsciibetaling();
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
        AsciiArtPrinter.PrintAsciibetaling();
        string[] popcorn = { "Zoet", "Zout", "Gemixt" };
        string[] opties = { "Klein", "Middel", "Groot" };
        Console.Clear();
        AsciiArtPrinter.PrintAsciibetaling();
        PrintTextCentered("Kies gewenste smaak");
        int popcornchoice = KiesOptie("Kies gewenste smaak", popcorn);
        Console.Clear();
        AsciiArtPrinter.PrintAsciibetaling();
        PrintTextCentered("Kies de grootte voor uw popcorn:");
        int selectedIndex = KiesOptie("Kies de grootte voor uw popcorn:", opties);
        Console.Clear();
        string gekozenpopcorn = popcorn[popcornchoice];
        string gekozenGrootte = opties[selectedIndex];
        double gekozenPrijs = prijzen[selectedIndex];
        AsciiArtPrinter.PrintAsciibetaling();
        PrintTextCentered($"\nU heeft gekozen voor {gekozenGrootte} Popcorn {gekozenpopcorn} - €{gekozenPrijs:0.00}");

        // Bereken de totaalkosten
        totaalKosten += gekozenPrijs;

        PrintTextCentered("\nUw bestelling is toegevoegd.");
        WiltMeerBestellen();
    }

    public static void Nachos(double[] prijzen)
    {
        AsciiArtPrinter.PrintAsciibetaling();
        string[] opties = { "Klein", "Middel", "Groot" };
        Console.Clear();
        AsciiArtPrinter.PrintAsciibetaling();
        PrintTextCentered("Kies de grootte voor uw nachos:");
        int selectedIndex = KiesOptie("Kies de grootte voor uw nachos:", opties);
        Console.Clear();
        AsciiArtPrinter.PrintAsciibetaling();
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
        AsciiArtPrinter.PrintAsciibetaling();
        string[] chips = { "Naturel", "Paprika" };
        string[] opties = { "Klein", "Middel", "Groot" };
        Console.Clear();
        AsciiArtPrinter.PrintAsciibetaling();
        PrintTextCentered("Kies de smaak voor uw chips:");
        int chipschoice = KiesOptie("Kies de smaak voor uw chips:", chips);
        Console.Clear();
        AsciiArtPrinter.PrintAsciibetaling();
        PrintTextCentered("Kies de grootte voor uw chips:");
        int selectedIndex = KiesOptie("Kies de grootte voor uw chips:", opties);
        Console.Clear();
        string gekozenchips = chips[chipschoice];
        string gekozenGrootte = opties[selectedIndex];
        double gekozenPrijs = prijzen[selectedIndex];
        AsciiArtPrinter.PrintAsciibetaling();
        PrintTextCentered($"\nU heeft gekozen voor {gekozenGrootte} {gekozenchips} chips - €{gekozenPrijs:0.00}");

        // Bereken de totaalkosten
        totaalKosten += gekozenPrijs;

        PrintTextCentered("\nUw bestelling is toegevoegd.");
        WiltMeerBestellen();
    }

    public static void Frisdrank(double[] prijzen)
    {
        AsciiArtPrinter.PrintAsciibetaling();
        string[] frisdrank = { "Cola", "Fanta", "Icetea sparkling", "Icetea green", "Cassis", "Fernandes" };
        Console.Clear();
        AsciiArtPrinter.PrintAsciibetaling();
        PrintTextCentered("Kies frisdrank naar keuze:");
        int frisdrankChoice = KiesOptie("Kies frisdrank naar keuze:", frisdrank);
        Console.Clear();
        string gekozenFrisdrank = frisdrank[frisdrankChoice];
        double gekozenPrijs = prijzen[frisdrankChoice];
        AsciiArtPrinter.PrintAsciibetaling();
        PrintTextCentered($"\nU heeft gekozen voor {gekozenFrisdrank} - €{gekozenPrijs:0.00}");
        totaalKosten += gekozenPrijs;

        PrintTextCentered("\nUw bestelling is toegevoegd.");
        WiltMeerBestellen();
    }

    public static void Thee(double[] prijzen)
    {
        AsciiArtPrinter.PrintAsciibetaling();
        string[] thee = { "Earl grey", "Jasmijn thee", "Groene thee", "Rooibos thee", "Munt thee" };
        Console.Clear();
        AsciiArtPrinter.PrintAsciibetaling();
        PrintTextCentered("Kies thee naar keuze:");
        int theechoice = KiesOptie("Kies thee naar keuze:", thee);
        Console.Clear();
        string gekozenthee = thee[theechoice];
        double gekozenPrijs = prijzen[theechoice];
        AsciiArtPrinter.PrintAsciibetaling();
        PrintTextCentered($"\nU heeft gekozen voor {gekozenthee} - €{gekozenPrijs:0.00}");

        // Bereken de totaalkosten
        totaalKosten += gekozenPrijs;

        PrintTextCentered("\nUw bestelling is toegevoegd.");
        WiltMeerBestellen();
    }

    public static void Koffie(double[] prijzen)
    {
        AsciiArtPrinter.PrintAsciibetaling();
        string[] opties = { "Klein", "Middel", "Groot" };
        Console.Clear();
        AsciiArtPrinter.PrintAsciibetaling();
        PrintTextCentered("Kies de grootte voor uw Koffie:");
        int selectedIndex = KiesOptie("Kies de grootte voor uw Koffie:", opties);
        Console.Clear();
        string gekozenGrootte = opties[selectedIndex];
        double gekozenPrijs = prijzen[selectedIndex];
        AsciiArtPrinter.PrintAsciibetaling();
        PrintTextCentered($"\nU heeft gekozen voor {gekozenGrootte} Koffie - €{gekozenPrijs:0.00}");

        // Bereken de totaalkosten
        totaalKosten += gekozenPrijs;

        PrintTextCentered("\nUw bestelling is toegevoegd.");
        WiltMeerBestellen();
    }
}
