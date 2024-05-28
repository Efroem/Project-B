using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

public class SelectingMovies
{
    private static List<Movies> movies = new List<Movies>();
    public static void MoviesSelect()
    {
        LoadMovies(); // Load the movies at the start of the program

        int selectedIndex = 0;
        bool running = true;
        bool backToMainMenuSelected = false; // To keep track of whether "Back to Main Menu" option is selected

        while (running)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            AsciiArtPrinter.PrintAsciifilms();
            Console.ResetColor();
            Console.Write("Gebruik de");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(" pijltjestoetsen ");
            Console.ResetColor();
            Console.Write("om door de films te bladeren en druk op");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(" Enter ");
            Console.ResetColor();
            Console.Write("om een film te selecteren:");
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
            int leftMargin = (windowWidth - boxWidth - 10) / 2; // Adjusted for 10 more paddings

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

            // Print the option to go back to the main menu
            if (backToMainMenuSelected)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            Console.WriteLine(new string(' ', leftMargin) + (backToMainMenuSelected ? "> (M) Ga terug naar het hoofdmenu <" : "(M) Ga terug naar het hoofdmenu"));
            Console.ResetColor();
            Console.WriteLine(new string(' ', leftMargin) + topBottomBorder);

            // Handle key press
            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    if (selectedIndex == 0 && !backToMainMenuSelected)
                    {
                        selectedIndex = movies.Count;
                        backToMainMenuSelected = true;
                    }
                    else if (selectedIndex > 0)
                    {
                        selectedIndex--;
                    }
                    break;

                case ConsoleKey.DownArrow:
                    if (backToMainMenuSelected)
                    {
                        selectedIndex = 0;
                        backToMainMenuSelected = false;
                    }
                    else if (selectedIndex < movies.Count - 1)
                    {
                        selectedIndex++;
                    }
                    break;

                case ConsoleKey.Enter:
                    if (backToMainMenuSelected)
                    {
                        // Go back to the main menu
                        return;
                    }
                    else
                    {
                        ShowMovieDetails(movies[selectedIndex]);
                    }
                    break;

                case ConsoleKey.Escape:
                    running = false;
                    break;

                case ConsoleKey.M:
                    // Go back to the main menu
                    return;
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
            Console.WriteLine("Druk op een knop om terug te gaan naar het menu.");

            var key = Console.ReadKey(true).Key;
            if (key != ConsoleKey.Escape)
            {
                return; // Return to movie selection menu
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
