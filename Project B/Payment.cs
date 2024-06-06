public static class Payment
{
    private static List<Product> Purchasedproducts = new List<Product>();

    private static HashSet<(int x, int y)> selectedSeats = new HashSet<(int row, int column)>();


    private static double totaalKosten = 0;

    // Definieer de prijzen
    private static readonly double[] popcornPrijzen = { 3.50, 5.00, 7.00 };
    private static readonly double[] nachosPrijzen = { 4.00, 6.00, 8.00 };
    private static readonly double[] chipsPrijzen = { 2.00, 3.50, 5.00 };
    private static readonly double[] frisdrankPrijzen = { 3.50, 3.00, 3.50, 3.00, 2.50, 3.00 };
    private static readonly double[] koffiePrijzen = { 2.50, 3.50, 5.00 };
    private static readonly double[] theePrijzen = { 3.00, 3.50, 3.00, 2.50, 4.50 };


    private static int KiesOptie(string prompt, string[] opties)
    {

        int selectedOption = 0;
        int optiesLength = opties.Length;

        int longestLineLength = 0;
        foreach (string option in opties)
        {
            longestLineLength = longestLineLength < option.Length ? option.Length : longestLineLength;
        }
        longestLineLength = longestLineLength < prompt.Length ? prompt.Length : longestLineLength;
        longestLineLength += 3;


        do
        {
            Console.Clear();
            AsciiArtPrinter.PrintAsciibetaling();
            Console.WriteLine();
            Console.ResetColor();

            PrintTextCentered("┌" + new string('─', longestLineLength) + "┐");
            PrintTextCentered($"│ {prompt}" + new string(' ', longestLineLength - prompt.Length - 1) + "│");
            for (int i = 0; i < opties.Length; i++)
            {
                if (i == selectedOption)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    PrintTextCentered($"│ {opties[i]}" + new string(' ', longestLineLength - opties[i].Length - 1) + "│");
                    Console.ResetColor();
                }
                else
                {
                    PrintTextCentered($"│ {opties[i]}" + new string(' ', longestLineLength - opties[i].Length - 1) + "│");
                }
            }
            PrintTextCentered("┕" + new string('─', longestLineLength) + "┘");

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
            Console.CursorTop -= (optiesLength + 1);
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
        string[] menuOptions = { "1.Eten", "2.Drinken", "3.Prijzenlijst", "4.Afrekenen" };
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
                string[] priceOptions = { "1.Prijzen eten", "2.Prijzen frisdranken", "3.Prijzen koffie of thee", "4. Terug naar menu" };
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
                    case 3:
                        BestelMenu();
                        break;
                }
                break;
            case 3:
                Afrekenen();
                break;
        }
    }

    public static void BestelEten()
    {
        string[] opties = { "Popcorn", "Nachos", "Chips", "Terug naar menu" };
        double[][] prijzen = { popcornPrijzen, nachosPrijzen, chipsPrijzen };
        // PrintTextCentered("Wat wilt u bestellen?");
        int selectedIndex = KiesOptie("Kies het gewenste eten", opties);

        switch (selectedIndex)
        {
            case 0:
                Popcorn(prijzen[selectedIndex]);
                break;
            case 1:
                Nachos(prijzen[selectedIndex]);
                break;
            case 2:
                Chips(prijzen[selectedIndex]);
                break;
            case 3:
                BestelMenu();
                break;
        }
    }

    public static void BestelDrinken()
    {
        string[] opties = { "Frisdrank", "Thee", "Koffie", "Terug naar menu" };
        double[][] prijzen = { frisdrankPrijzen, theePrijzen, koffiePrijzen };
        int selectedIndex = KiesOptie("Kies het gewenste drinken", opties);

        switch (selectedIndex)
        {
            case 0:
                Frisdrank(prijzen[selectedIndex]);
                break;
            case 1:
                Thee(prijzen[selectedIndex]);
                break;
            case 2:
                Koffie(prijzen[selectedIndex]);
                break;
            case 3:
                BestelMenu();
                break;
        }
    }

    public static void WiltMeerBestellen()
    {
        string[] opties = { "Ja, nog meer bestellen", "Ik wil afrekenen" };
        PrintTextCentered("Wilt u nog meer eten/drinken bestellen of afrekenen?");
        int selectedIndex = KiesOptie("Wilt u nog meer eten/drinken bestellen of afrekenen?", opties);

        switch (selectedIndex)
        {
            case 0:
                BestelMenu();
                break;
            case 1:
                Afrekenen();
                break;
        }
    }
    public static void AddSeatPrice(double seatPrice)
    {
        totaalKosten += seatPrice;
    }
    public static void AddSelectedSeats(List<(int x, int y)> seats)
    {
        foreach (var seat in seats)
        {
            selectedSeats.Add(seat);
        }
    }

    public static void Popcorn(double[] prijzen)
    {
        string[] popcorn = { "Zoet", "Zout", "Gemixt" };
        string[] opties = { "Klein - €3.50", "Middel - €5.00", "Groot €7.00" };
        int popcornchoice = KiesOptie("Kies gewenste smaak", popcorn);
        int selectedIndex = KiesOptie("Kies de grootte voor uw popcorn:", opties);
        string gekozenpopcorn = popcorn[popcornchoice];
        string gekozenGrootte = opties[selectedIndex];
        double gekozenPrijs = prijzen[selectedIndex];
        PrintTextCentered($"\nU heeft gekozen voor {gekozenGrootte} Popcorn {gekozenpopcorn}");
        Purchasedproducts.Add(new Product($"{gekozenGrootte} Popcorn {gekozenpopcorn}", gekozenPrijs));

        // Bereken de totaalkosten
        totaalKosten += gekozenPrijs;

        PrintTextCentered("\nUw bestelling is toegevoegd.");
        Console.WriteLine();
        ProgramFunctions.PrintColoredTextCentered("Druk op een ", ConsoleColor.White, "knop", ConsoleColor.Magenta, " om verder te gaan", ConsoleColor.White);
        Console.ReadKey();
        Console.Clear();
        WiltMeerBestellen();
    }

    public static void Nachos(double[] prijzen)
    {
        string[] opties = { "Klein - €4.00", "Middel - €6.00", "Groot - €8.00" };
        Console.Clear();
        int selectedIndex = KiesOptie("Kies de grootte voor uw nachos:", opties);
        Console.Clear();
        string gekozenGrootte = opties[selectedIndex];
        double gekozenPrijs = prijzen[selectedIndex];

        PrintTextCentered($"\nU heeft gekozen voor {gekozenGrootte} nachos");
        Purchasedproducts.Add(new Product($"{gekozenGrootte} Nachos", gekozenPrijs));

        // Bereken de totaalkosten
        totaalKosten += gekozenPrijs;

        PrintTextCentered("\nUw bestelling is toegevoegd.");
        Console.WriteLine();
        ProgramFunctions.PrintColoredTextCentered("Druk op een ", ConsoleColor.White, "knop", ConsoleColor.Magenta, " om verder te gaan", ConsoleColor.White);
        Console.ReadKey();
        Console.Clear();
        WiltMeerBestellen();
    }

    public static void Chips(double[] prijzen)
    {
        string[] chips = { "Naturel", "Paprika" };
        string[] opties = { "Klein - €2.00", "Middel - €3.50", "Groot - €5.00" };
        int chipschoice = KiesOptie("Kies de smaak voor uw chips:", chips);
        int selectedIndex = KiesOptie("Kies de grootte voor uw chips:", opties);
        string gekozenchips = chips[chipschoice];
        string gekozenGrootte = opties[selectedIndex];
        double gekozenPrijs = prijzen[selectedIndex];
        PrintTextCentered($"\nU heeft gekozen voor {gekozenGrootte} {gekozenchips} chips");
        Purchasedproducts.Add(new Product($"{gekozenGrootte} {gekozenchips} Chips", gekozenPrijs));

        // Bereken de totaalkosten
        totaalKosten += gekozenPrijs;

        PrintTextCentered("\nUw bestelling is toegevoegd.");
        Console.WriteLine();
        ProgramFunctions.PrintColoredTextCentered("Druk op een ", ConsoleColor.White, "knop", ConsoleColor.Magenta, " om verder te gaan", ConsoleColor.White);
        Console.ReadKey();
        Console.Clear();
        WiltMeerBestellen();
    }

    public static void Frisdrank(double[] prijzen)
    {
        string[] frisdrank = { "Cola - €3.50", "Fanta - €3.00", "Icetea sparkling - €3.50", "Icetea green - €3.00", "Cassis - €2.50", "Fernandes - €3.00" };
        int frisdrankChoice = KiesOptie("Kies frisdrank naar keuze:", frisdrank);
        string gekozenFrisdrank = frisdrank[frisdrankChoice];
        double gekozenPrijs = prijzen[frisdrankChoice];
        PrintTextCentered($"\nU heeft gekozen voor {gekozenFrisdrank}");
        Purchasedproducts.Add(new Product($"{gekozenFrisdrank} ", gekozenPrijs));
        totaalKosten += gekozenPrijs;

        PrintTextCentered("\nUw bestelling is toegevoegd.");
        Console.WriteLine();
        ProgramFunctions.PrintColoredTextCentered("Druk op een ", ConsoleColor.White, "knop", ConsoleColor.Magenta, " om verder te gaan", ConsoleColor.White);
        Console.ReadKey();
        Console.Clear();
        WiltMeerBestellen();
    }

    public static void Thee(double[] prijzen)
    {
        string[] thee = { "Earl grey - €3.00", "Jasmijn thee - €3.50", "Groene thee - €3.00", "Rooibos thee - €2.50", "Munt thee - €4.50" };
        int theechoice = KiesOptie("Kies thee naar keuze:", thee);
        string gekozenthee = thee[theechoice];
        double gekozenPrijs = prijzen[theechoice];
        PrintTextCentered($"\nU heeft gekozen voor {gekozenthee}");
        Purchasedproducts.Add(new Product($"{gekozenthee}", gekozenPrijs));

        // Bereken de totaalkosten
        totaalKosten += gekozenPrijs;

        PrintTextCentered("\nUw bestelling is toegevoegd.");
        Console.WriteLine();
        ProgramFunctions.PrintColoredTextCentered("Druk op een ", ConsoleColor.White, "knop", ConsoleColor.Magenta, " om verder te gaan", ConsoleColor.White);
        Console.ReadKey();
        Console.Clear();
        WiltMeerBestellen();
    }

    public static void Koffie(double[] prijzen)
    {
        string[] opties = { "Klein - €2.50", "Middel - €3.50", "Groot - €5.00" };
        int selectedIndex = KiesOptie("Kies de grootte voor uw Koffie:", opties);
        string gekozenGrootte = opties[selectedIndex];
        double gekozenPrijs = prijzen[selectedIndex];
        PrintTextCentered($"\nU heeft gekozen voor {gekozenGrootte} Koffie");
        Purchasedproducts.Add(new Product($"{gekozenGrootte} koffie", gekozenPrijs));

        // Bereken de totaalkosten
        totaalKosten += gekozenPrijs;

        PrintTextCentered("\nUw bestelling is toegevoegd.");
        Console.WriteLine();
        ProgramFunctions.PrintColoredTextCentered("Druk op een ", ConsoleColor.White, "knop", ConsoleColor.Magenta, " om verder te gaan", ConsoleColor.White);
        Console.ReadKey();
        Console.Clear();
        WiltMeerBestellen();
    }
    public static void Afrekenen()
    {
        string[] opties = { "IDEAL", "PayPal", "Creditcard" };
        int selectedIndex = KiesOptie("Kies de gewenste betaalmethode:", opties);

        switch (selectedIndex)
        {
            case 0:
                string[] partnersIDEAL = { "ING", "ABN AMRO", "ASN Bank", "RaboBank", "Revolut" };
                int selectpartner = KiesOptie("Kies de gewenste Bank:", partnersIDEAL);
                ProgramFunctions.PrintTextCentered("\nUw bestellingen:");
                foreach (var product in Purchasedproducts)
                {
                    ProgramFunctions.PrintTextCentered($"{product.Naam}");
                }
                Console.WriteLine();
                PrintTextCentered("\nGeselecteerde stoelen:");
                foreach (var seat in selectedSeats)
                {
                    PrintTextCentered($"Rij: {seat.y}, Stoel: {seat.x}");
                }
                Console.WriteLine();
                PrintTextCentered($"\nUw totale kosten zijn: €{totaalKosten:0.00}");
                ProgramFunctions.PrintColoredTextCentered("Druk op een ", ConsoleColor.White, "knop", ConsoleColor.Magenta, " om verder te gaan", ConsoleColor.White);
                Console.ReadKey();
                Console.Clear();
                Console.WriteLine("\x1b[3J");
                Program.Main();
                break;
            case 1:
                Console.WriteLine("\nUw bestellingen:");
                foreach (var product in Purchasedproducts)
                {
                    Console.WriteLine($"{product.Naam}");
                }
                Console.WriteLine();
                Console.WriteLine("\nGeselecteerde stoelen:");
                foreach (var seat in selectedSeats)
                {
                    Console.WriteLine($"Rij: {seat.y}, Stoel: {seat.x}");
                }
                Console.WriteLine();
                Console.WriteLine($"\nUw totale kosten zijn: €{totaalKosten:0.00}");
                Console.ReadLine();
                Program.Main();
                break;
            case 2:
                Console.WriteLine("\nUw bestellingen:");
                foreach (var product in Purchasedproducts)
                {
                    Console.WriteLine($"{product.Naam}");
                }
                Console.WriteLine();
                Console.WriteLine("\nGeselecteerde stoelen:");
                foreach (var seat in selectedSeats)
                {
                    Console.WriteLine($"Rij: {seat.y}, Stoel: {seat.x}");
                }
                Console.WriteLine();
                Console.WriteLine($"\nUw totale kosten zijn: €{totaalKosten:0.00}");
                Console.ReadLine();
                Program.Main();
                break;

        }
    }

}
