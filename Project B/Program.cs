using System;
using System.IO;
using System.Text.Json;

class Program

{

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        TestPosters.MegaBioscoop();
        Console.ResetColor();
        Console.Write("Druk op een ");
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write("knop");
        Console.ResetColor();
        Console.Write(" om verder te gaan");
        Console.ReadKey();
        Console.Clear();

        AsciiArtPrinter.PrintAscii("movies.json");
        Console.Write("Druk op een ");
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write("knop");
        Console.ResetColor();
        Console.Write(" om verder te gaan");
        Console.ReadKey();
        Console.Clear();

        while (true)
        {
            string[] options;
            if (Authentication.User == null)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                AsciiArtPrinter.Printasciihm();
                Console.ResetColor();
                //AsciiArtPrinter.PrintAsciiMenu();
                options = new string[] { "1.Aanmelden", "2.Bekijk Films", "3.Bekijk Filmrooster", "4.Verlaat Pagina" };
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                AsciiArtPrinter.Printasciihm();
                Console.ResetColor();
                //AsciiArtPrinter.PrintAsciiMenu2();
                if (!Authentication.User.IsAdmin) {
                    options = new string[] { "1.Profiel Bekijken", "2.Bekijk Films", "3.Bekijk Filmrooster", "4.Bekijk Reserveringen", "5.Verlaat Pagina" };
                } else {
                    options = new string[] { "1.Profiel Bekijken", "2.Bekijk Films", "3.Bekijk Filmrooster", "4.Bekijk Reserveringen", "5.Verlaat Pagina", "6.Lijst Zalen", "7.Zaal Toevoegen","8.Zaal Verwijderen","9.Zaal Veranderen"};
                }
                
            }

            //Console.WriteLine("Gebruik de pijltjestoetsen om een optie te selecteren en druk op Enter.");
            int selectedOption = ShowMenuInline(options, "Gebruik de pijltjestoetsen om een optie te selecteren en druk op Enter.");

            switch (selectedOption)
            {
                case 0:
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    AsciiArtPrinter.PrintAsciilogin();
                    Console.ResetColor();
                    if (Authentication.User == null)
                        Authentication.Start();
                    else
                        Authentication.ViewProfile();
                    break;
                case 1:
                    Console.Clear();
                    SelectingMovies.MoviesSelect();
                    Console.Clear();
                    break;
                case 2:
                    Console.Clear();
                    Schedule.OpenGeneralMenu();
                    Console.Clear();
                    break;
                case 3:
                    if (Authentication.User == null)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        AsciiArtPrinter.Totziens();
                        Console.ResetColor();
                        Environment.Exit(0);
                        Console.ReadLine();
                        break;
                    }
                    else
                    {
                        Console.Clear();
                        Reservation.OpenReservationMenu(Authentication.User);
                        Console.Clear();
                        break;
                    }
                case 4:
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    AsciiArtPrinter.Totziens();
                    Console.ResetColor();
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
                    CinemaHall.RemoveCinemaHall();
                    Console.ReadLine();
                    Console.Clear();
                    // Console.Clear();
                    // CinemaZaal printer = new CinemaZaal();
                    // printer.PrintGridGroteZaal();
                    // printer.NavigateGrid();
                    // Console.ReadLine();
                    // Console.Clear();
                    break;
                case 8:
                    Console.Clear();
                    CinemaHall.ChangeCinemaHall();
                    Console.ReadLine();
                    Console.Clear();
                    break;
                // case 9:
                //     Console.Clear();
                //     CinemaHall.AddNewMovie();
                //     Console.ReadLine();
                //     Console.Clear();
                //     break;
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

        // Deel de prompt op rond de woorden die rood moeten worden
        string[] promptParts = prompt.Split(new string[] { " pijltjestoetsen ", " Enter" }, StringSplitOptions.None);

        // Schrijf het eerste deel van de prompt
        Console.Write(promptParts[0]);
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write(" pijltjestoetsen ");
        Console.ResetColor();
        Console.Write(promptParts[1]);
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write(" Enter");
        Console.ResetColor();
        Console.WriteLine(promptParts[2]);
        do
        {
            for (int i = 0; i < options.Length; i++)
            {
                Console.CursorLeft = 41;
                if (i == selectedOption)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
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

