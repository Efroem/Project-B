using System;
using System.IO;
using System.Text.Json;

class Program
{
    static void Main()
    {
        Console.Clear();
        TestPosters.MegaBioscoop();
        Console.WriteLine("Druk op een knop om verder te gaan");
        Console.ReadKey();
        Console.Clear();

        AsciiArtPrinter.PrintAscii("movies.json");
        Console.WriteLine("Druk op een knop om verder te gaan");
        Console.ReadKey();
        Console.Clear();


        string answer;
        do
        {
            if (Authentication.User == null)
            {
                AsciiArtPrinter.Printasciihm();
                AsciiArtPrinter.PrintAsciiMenu();
                Console.WriteLine("Selecteer het gewenste nummer om door te gaan.");
            }
            else
            {
                AsciiArtPrinter.Printasciihm();
                AsciiArtPrinter.PrintAsciiMenu2();
                Console.WriteLine("Selecteer het gewenste nummer om door te gaan.");
            }

            answer = (Console.ReadLine() ?? "").ToLower();

            switch (answer)
            {
                case "1":
                case "bekijk films":
                case "bekijk":
                case "movies":
                    Console.Clear();
                    AsciiArtPrinter.PrintAsciifilms();
                    AsciiArtPrinter.PrintMovieTitles("movies.json");
                    Console.WriteLine("Druk op een knop om terug te gaan");
                    Console.ReadKey();
                    Console.Clear();
                    break;

                case "2":
                case "inloggen":
                case "profiel":
                    Console.Clear();
                    AsciiArtPrinter.PrintAsciilogin();
                    if (Authentication.User == null)
                        Authentication.Start();
                    else
                        Authentication.ViewProfile();
                    break;

                case "3":
                case "bekijk reserveringen":
                case "reserveringen":
                    Console.Clear();
                    Console.WriteLine("Reserveringen");
                    Console.ReadLine();
                    Console.Clear();
                    break;

                case "4":
                case "verlaat pagina":
                case "q":
                    Console.Clear();
                    Console.WriteLine("Tot ziens!");
                    Environment.Exit(0);
                    Console.ReadLine();
                    Console.Clear();
                    break;

                case "5":
                case "lijst zalen":
                    Console.Clear();
                    Console.WriteLine("Lijst Zalen");
                    CinemaHall.PrintCinemaHalls();
                    Console.ReadLine();
                    Console.Clear();
                    break;

                case "6":
                case "cinemaHall toevoegen":
                    Console.Clear();
                    Console.WriteLine("CinemaHall toevoegen");
                    CinemaHall.RemoveCinemaHall();
                    Console.ReadLine();
                    Console.Clear();
                    break;

                default:
                    Console.Clear();
                    Console.WriteLine("Ongeldige invoer");
                    break;
            }
        } while (answer != "4" && answer != "verlaat pagina" && answer != "q");

        Environment.Exit(0);
    }
}

