using System;
using System.IO;
using System.Text.Json;

class Program
{
    static void Main()
    {
        Console.Clear();
        Console.WriteLine(TestPosters.MegaBioscoop());
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
                Console.WriteLine("1. Bekijk films\n2. Inloggen\n3. Bekijk reserveringen\n4. Verlaat pagina\n5. Lijst zalen\n6. CinemaHall toevoegen");
            }
            else
            {
                AsciiArtPrinter.Printasciihm();
                Console.WriteLine("1. Bekijk films\n2. profiel\n3. Bekijk reserveringen\n4. Verlaat pagina\n5. Lijst zalen\n6. CinemaHall toevoegen");
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
                    Console.WriteLine("Log in");
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
                    CinemaHall.ReadFromCinemaHall();
                    Console.ReadLine();
                    Console.Clear();
                    break;

                case "6":
                case "cinemaHall toevoegen":
                    Console.Clear();
                    Console.WriteLine("CinemaHall toevoegen");
                    CinemaHall.AddNewCinemaHall();
                    Console.ReadLine();
                    Console.Clear();
                    break;

                default:
                    Console.WriteLine("Ongeldige invoer");
                    break;
            }
        } while (answer != "4" && answer != "verlaat pagina" && answer != "q");

        Environment.Exit(0);
    }    
}

