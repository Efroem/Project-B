using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

public class SelectingMovies
{
    private static List<Movies> movies;

    public static void MoviesSelect()
    {
        Console.WriteLine("1. Film beschrijving");
        Console.WriteLine("2. Terug naar het hoofdmenu");
        LoadMovies(); // Laad de films eenmaal aan het begin van het programma
        bool running = true;
        while (running)
        {
            string choice = Console.ReadLine().ToLower();

            if (choice == "1" || choice == "film beschrijving" || choice == "beschrijving")
            {
                Console.Clear();
                bool found = false;
                while (!found)
                {
                    AsciiArtPrinter.PrintAsciibeschrijving();
                    AsciiArtPrinter.PrintMovieTitles("movies.json");
                    Console.Write("Voer de gewenste titel in voor meer informatie: ");
                    string title = Console.ReadLine();

                    // Zoek de film op titel
                    foreach (var movie in movies)
                    {
                        if (movie.Title.Equals(title, StringComparison.OrdinalIgnoreCase))
                        {
                            Console.Clear();
                            AsciiArtPrinter.PrintAsciibeschrijving();
                            Console.WriteLine(new string('*', 133));
                            Console.WriteLine($"{"Jaar van uitgave",-10} : {movie.Released}");
                            Console.WriteLine($"{"Leeftijdsgrens",-10} : {movie.AgeRestricted}");
                            Console.WriteLine($"{"Genres",-10} : {string.Join(", ", movie.Genres)}");
                            Console.WriteLine($"{"Schrijver",-10} : {movie.Writer}");
                            Console.WriteLine($"{"Hoofdacteurs",-10} : {string.Join(", ", movie.TopActors)}");
                            Console.WriteLine($"{"Voertaal",-10} : {movie.Language}");
                            Console.WriteLine($"{"Beoordeling",-10} : {movie.Rating}");
                            Console.WriteLine($"{"Beschrijving",-10} : {movie.Description}");
                            Console.WriteLine(new string('*', 133));
                            found = true;
                            Console.WriteLine("Druk op een willekeurige knop om terug te gaan naar het hoofdmenu");
                            Console.ReadKey(); // True om de ingedrukte toets weer te geven
                            Console.Clear();
                            running = false;
                            break;
                        }
                    }

                    if (!found)
                    {
                        Console.WriteLine("Film niet gevonden. Wil je een andere film proberen? (ja/nee)");
                        string retryChoice = Console.ReadLine().ToLower();
                        if (retryChoice != "ja" && retryChoice != "j")
                        {
                            running = false; // Stop de lus als de gebruiker geen andere film wil proberen
                            break;
                        }
                        else
                        {
                            Console.Clear(); // Wis het scherm om een nieuwe titel in te voeren
                        }
                    }
                }
            }
            else if (choice == "2" || choice == "terug" || choice == "terug naar hoofdmenu")
            {
                running = false;
            }
            else
            {
                Console.WriteLine("Ongeldige keuze. Probeer opnieuw.");
            }
        }
    }

    private static void LoadMovies()
    {
        // JSON-bestand laden
        using (StreamReader r = new StreamReader("movies.json"))
        {
            string json = r.ReadToEnd();
            movies = JsonConvert.DeserializeObject<List<Movies>>(json);
        }
    }
}
