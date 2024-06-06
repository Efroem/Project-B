using System;
using System.IO;
using System.Text.Json;

class Program
{
    public static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        AsciiArtPrinter.MegaBioscoop();
        Console.ResetColor();

        // Combine the text into one line with appropriate colors
        Console.WriteLine();
        ProgramFunctions.PrintColoredTextCentered("Druk op een ", ConsoleColor.White, "knop", ConsoleColor.Magenta, " om verder te gaan", ConsoleColor.White);

        Console.ReadKey();
        Console.Clear();

        AsciiArtPrinter.PrintAscii("movies.json");
        Thread.Sleep(1000);
        Console.Clear();
        Console.WriteLine("\x1b[3J");
        Console.Clear();

        while (true)
        {
            string[] options;
            if (Authentication.User == null)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                AsciiArtPrinter.Printasciihm();
                Console.ResetColor();
                options = new string[] { "1.Aanmelden", "2.Bekijk Films", "3.Bekijk Filmrooster", "4.Verlaat Pagina" };
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                AsciiArtPrinter.Printasciihm();
                Console.ResetColor();
                if (!Authentication.User.IsAdmin)
                {
                    options = new string[] { "1.Profiel Bekijken", "2.Bekijk Films", "3.Bekijk Filmrooster", "4.Bekijk Reserveringen", "5.Verlaat Pagina", "6.Eten Kopen" };
                }
                else
                {
                    options = new string[] { "1.Profiel Bekijken", "2.Bekijk Films", "3.Bekijk Filmrooster", "4.Bekijk Reserveringen", "5.Verlaat Pagina", "6.Lijst Zalen", "7.Zaal Toevoegen", "8.Zaal Verwijderen", "9.Zaal Veranderen", "10.Film toevoegen" };
                }
            }

            Console.WriteLine();
            ProgramFunctions.PrintColoredTextCentered("Gebruik de ", ConsoleColor.White, "pijltjestoetsen", ConsoleColor.Magenta, " om een optie te selecteren en druk op ", ConsoleColor.White, "Enter\n", ConsoleColor.Magenta);

            int selectedOption = ProgramFunctions.ShowMenuInline(options);

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
                        Thread.Sleep(1500);
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
                    if (!Authentication.User.IsAdmin)
                    {
                        Console.Clear();
                        Payment.BestelMenu();
                    }
                    Console.Clear();
                    Console.WriteLine("Lijst Zalen");
                    AdminFunctions.PrintCinemaHalls();
                    Console.ReadLine();
                    Console.Clear();
                    break;
                case 6:
                    Console.Clear();
                    Console.WriteLine("CinemaHall Toevoegen");
                    AdminFunctions.AddNewCinemaHall();
                    Console.ReadLine();
                    Console.Clear();
                    break;
                case 7:
                    Console.Clear();
                    AdminFunctions.RemoveCinemaHall();
                    Console.ReadLine();
                    Console.Clear();
                    break;
                case 8:
                    Console.Clear();
                    AdminFunctions.ChangeCinemaHall();
                    Console.ReadLine();
                    Console.Clear();
                    break;
                case 9:
                    Console.Clear();
                    AdminFunctions.AddNewMovie();
                    Console.Clear();
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Ongeldige invoer");
                    break;
            }
        }
    }
}
