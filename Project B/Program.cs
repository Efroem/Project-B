using System;
using System.IO;
using System.Text.Json;

class Program

{
    
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
    
        Console.Clear();
        TestPosters.MegaBioscoop();
        Console.Write("Druk op een ");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("knop");
        Console.ResetColor();
        Console.Write(" om verder te gaan");
        Console.ReadKey();
        Console.Clear();
    
        AsciiArtPrinter.PrintAscii("movies.json");
        Console.WriteLine("Druk op een knop om verder te gaan");
        Console.ReadKey();
        Console.Clear();
    
        while (true)
        {
            string[] options;
            if (Authentication.User == null)
            {
                AsciiArtPrinter.Printasciihm();
                //AsciiArtPrinter.PrintAsciiMenu();
                options = new string[] { "1.Bekijk Films", "2.Inloggen", "3.Bekijk Reserveringen", "4.Schema", "5.Verlaat Pagina", "6.Lijst Zalen", "7.CinemaHall Toevoegen", "8.Checken" };
            }
            else
            {
                AsciiArtPrinter.Printasciihm();
                //AsciiArtPrinter.PrintAsciiMenu2();
                options = new string[] { "1.Bekijk Films", "2.Profiel", "3.Bekijk Reserveringen", "4.Schema", "5.Verlaat Pagina", "6.Lijst Zalen", "7.CinemaHall Toevoegen", "8.Checken" };
            }
    
            //Console.WriteLine("Gebruik de pijltjestoetsen om een optie te selecteren en druk op Enter.");
            int selectedOption = ShowMenuInline(options, "Gebruik de pijltjestoetsen om een optie te selecteren en druk op Enter.");
    
            switch (selectedOption)
            {
                case 0:
                    Console.Clear();
                    AsciiArtPrinter.PrintAsciifilms();
                    AsciiArtPrinter.PrintMovieTitles("movies.json");
                    SelectingMovies.MoviesSelect();
                    Console.Clear();
                    break;
                case 1:
                    Console.Clear();
                    AsciiArtPrinter.PrintAsciilogin();
                    if (Authentication.User == null)
                        Authentication.Start();
                    else
                        Authentication.ViewProfile();
                    break;
                case 2:
                    Console.Clear();
                    Console.WriteLine("Reserveringen");
                    Console.ReadLine();
                    Console.Clear();
                    break;
                case 3:
                    Console.Clear();
                    Schedule.OpenGeneralMenu();
                    Console.Clear();
                    break;
                case 4:
                    Console.Clear();
                    Console.WriteLine("Tot ziens!");
                    Environment.Exit(0);
                    Console.ReadLine();
                    break;
                case 5:
                    Console.Clear();
                    Console.WriteLine("Lijst Zalen");
                    CinemaHall.PrintCinemaHalls();
                    Console.ReadLine();
                    Console.Clear();
                    break;
                case 6:
                    Console.Clear();
                    Console.WriteLine("CinemaHall Toevoegen");
                    CinemaHall.AddNewCinemaHall();
                    Console.ReadLine();
                    Console.Clear();
                    break;
                case 7:
                    Console.Clear();
                    CinemaZaal printer = new CinemaZaal();
                    printer.PrintGridGroteZaal();
                    printer.NavigateGrid();
                    Console.ReadLine();
                    Console.Clear();
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Ongeldige invoer");
                    break;
            }
        }
    }
 
public static int ShowMenuInline(string[] options, string prompt)
{
    int selectedOption = 0;
 
    string[] promptParts = prompt.Split(" pijltjestoetsen ");
    
    Console.Write(promptParts[0]);
    Console.ForegroundColor = ConsoleColor.Red; 
    Console.Write(" pijltjestoetsen ");
    Console.ResetColor();
    Console.WriteLine(promptParts[1]);
    do
    {
        for (int i = 0; i < options.Length; i++)
        {
            Console.CursorLeft = 41;
            if (i == selectedOption)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"   {options[i]}");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine($"   {options[i]}");
            }
        }
 
        var key = Console.ReadKey(true);
        if (key.Key == ConsoleKey.UpArrow && selectedOption > 0)
        {
            selectedOption--;
        }
        else if (key.Key == ConsoleKey.DownArrow && selectedOption < options.Length - 1)
        {
            selectedOption++;
        }
        else if (key.Key == ConsoleKey.Enter)
        {
            break;
        }
 
        // Erase previous options display
        Console.CursorTop -= options.Length;
    } while (true);
 
    return selectedOption;
}

}

