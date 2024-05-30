using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

public class SelectingMovies
{
    private static List<Movies> movies = new List<Movies>();
    public static void MoviesSelect()
    {
        LoadMovies(); // Laad de films aan het begin van het programma

        int selectedIndex = 0;
        bool running = true;

        while (running)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            AsciiArtPrinter.PrintAsciifilms();
            Console.ResetColor();
            Console.WriteLine("Gebruik de pijltjestoetsen om door de films te bladeren en druk op Enter om een film te selecteren:");
            Console.WriteLine();

            // Calculate the length of the longest movie title for box width
            int boxWidth = 0;
            foreach (var movie in movies)
            {
                if (movie.Title.Length > boxWidth)
                {
                    boxWidth = movie.Title.Length;
                }
            }
            boxWidth += 5; // Adding padding

            // Calculate the horizontal position to center the box
            int windowWidth = Console.WindowWidth;
            int leftMargin = (windowWidth - boxWidth) / 2;

            string topBottomBorder = new string('*', boxWidth);

            // Print the box
            Console.WriteLine(new string(' ', leftMargin) + topBottomBorder);
            for (int i = 0; i < movies.Count; i++)
            {
                string title = movies[i].Title;
                if (i == selectedIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    title = $"> {title}";
                    Console.ResetColor();
                }
                else
                {
                    title = $"  {title}";
                }
                Console.WriteLine(new string(' ', leftMargin) + $"* {title.PadRight(boxWidth - 3)}*");
            }

            // Print the option to view the movie schedule
            Console.WriteLine(new string(' ', leftMargin) + "> (R) Bekijk filmrooster <");

            Console.WriteLine(new string(' ', leftMargin) + "> (M) Terug naar het hoofdmenu <");

            Console.WriteLine(new string(' ', leftMargin) + topBottomBorder);

            // Handle key press
            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    if (selectedIndex > 0)
                    {
                        selectedIndex--;
                    }
                    break;

                case ConsoleKey.DownArrow:
                    if (selectedIndex < movies.Count - 1)
                    {
                        selectedIndex++;
                    }
                    break;

                case ConsoleKey.Enter:
                    ShowMovieDetails(movies[selectedIndex]);
                    break;

                case ConsoleKey.M:
                    running = false;
                    break;

                case ConsoleKey.R: // New case for viewing movie schedule
                    Console.Clear();
                    Schedule.OpenGeneralMenu();
                    Console.Clear();
                    break;
            }
        }
    }



    private static void ShowMovieDetails(Movies movie)
    {
        while (true)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            AsciiArtPrinter.PrintAsciibeschrijving();
            Console.ResetColor();
            Console.WriteLine(new string('*', Console.WindowWidth - 1));
            Console.WriteLine($"{"Jaar van uitgave",-10} : {movie.Released}");
            Console.WriteLine($"{"Leeftijdsgrens",-10} : {movie.AgeRestricted}");
            Console.WriteLine($"{"Genres",-10} : {string.Join(", ", movie.Genres)}");
            Console.WriteLine($"{"Schrijver",-10} : {movie.Writer}");
            Console.WriteLine($"{"Hoofdacteurs",-10} : {string.Join(", ", movie.TopActors)}");
            Console.WriteLine($"{"Voertaal",-10} : {movie.Language}");
            Console.WriteLine($"{"Beoordeling",-10} : {movie.Rating}");
            Console.WriteLine($"{"Beschrijving",-10} : {movie.Description}");
            Console.WriteLine(new string('*', Console.WindowWidth - 1));
            Console.WriteLine();
            Console.WriteLine("1. Bekijk schema van deze film");
            Console.WriteLine("2. Terug naar film beschrijvingen");

            var key = Console.ReadLine();
            if (key == "1")
            {
                Console.Clear();
                Schedule.OpenSpecificMenu(movie.Title);
            }
            else if (key == "2")
            {
                return;
            }
        }
    }



    private static void LoadMovies()
    {
        try
        {
            using (StreamReader r = new StreamReader("movies.json"))
            {
                string json = r.ReadToEnd();
                movies = JsonConvert.DeserializeObject<List<Movies>>(json);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
