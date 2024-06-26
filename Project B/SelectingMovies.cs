using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;
using System.Collections.Generic;
using System.IO;

public class SelectingMovies : ProgramFunctions
{
    private static List<Movie> movies = new List<Movie>();
    public static void MoviesSelect()
    {
        LoadMovies();

        int selectedIndex = 0;
        bool running = true;

        while (running)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            AsciiArtPrinter.PrintAsciifilms();
            Console.ResetColor();
            Console.WriteLine();
            PrintColoredTextCentered("Gebruik de ", ConsoleColor.White, "pijltjestoetsen", ConsoleColor.Magenta, " om een optie te selecteren en druk op ", ConsoleColor.White, "Enter\n", ConsoleColor.Magenta);

            // Calculate the length of the longest movie title for box width
            int boxWidth = 0;
            foreach (var movie in movies)
            {
                if (movie.Title.Length > boxWidth)
                {
                    boxWidth = movie.Title.Length;
                }
            }
            boxWidth += 5;

            // Calculate the horizontal position to center the box
            int windowWidth = Console.WindowWidth;
            int leftMargin = (windowWidth - boxWidth) / 2;

            string topBorder = "┌" + new string('─', boxWidth) + "┐";
            string bottomBorder = "└" + new string('─', boxWidth) + "┘";

            // Print the box
            Console.WriteLine(new string(' ', leftMargin) + topBorder);
            for (int i = 0; i < movies.Count; i++)
            {
                string title = movies[i].Title;
                if (i == selectedIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    title = $"> {title}";

                }
                else
                {
                    title = $"  {title}";
                }
                Console.WriteLine(new string(' ', leftMargin) + $"│ {title.PadRight(boxWidth - 1)}│");
                Console.ResetColor();
            }

            // Print the option to view the movie schedule
            Console.WriteLine(new string(' ', leftMargin) + $"│ {"> (R) Bekijk filmrooster".PadRight(boxWidth - 3)}< │");

            Console.WriteLine(new string(' ', leftMargin) + $"│ {"> (M) Terug naar het hoofdmenu".PadRight(boxWidth - 3)}< │");

            Console.WriteLine(new string(' ', leftMargin) + bottomBorder);

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

                case ConsoleKey.R:
                    Console.Clear();
                    Schedule.OpenGeneralMenu();
                    Console.Clear();
                    break;
            }
        }
    }



    private static void ShowMovieDetails(Movie movie)
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

            string[] options = { "Bekijk schema van deze film", "Terug naar film beschrijvingen" };
            Console.WriteLine();
            PrintColoredTextCentered("Gebruik de ", ConsoleColor.White, "pijltjestoetsen", ConsoleColor.Magenta, " om een optie te selecteren en druk op ", ConsoleColor.White, "Enter\n", ConsoleColor.Magenta);

            int selectedOption = ShowMenuInline(options);

            if (selectedOption == 0)
            {
                Console.Clear();
                Schedule.OpenSpecificMenu(movie.Title);
            }
            else if (selectedOption == 1)
            {
                return;
            }
        }
    }

    public static int ShowMenuInline(string[] options)
    {
        int selectedOption = 0;

        while (true)
        {
            // Display options
            for (int i = 0; i < options.Length; i++)
            {
                Console.CursorLeft = 41;
                if (i == selectedOption)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"> {options[i]}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"  {options[i]}");
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
        }

        return selectedOption;
    }

    private static void LoadMovies()
    {
        try
        {
            movies = JsonSerializer.Deserialize<List<Movie>>(File.ReadAllText("movies.json"));

            if (Authentication.User is not null && movies.Count != 0) {
                DateTime birthDate = DateTime.ParseExact(Authentication.User.BirthDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                // Get the current date
                DateTime currentDate = DateTime.Now;
                int age = currentDate.Year - birthDate.Year;
                movies = movies.Where(movie => movie.AgeRestricted <= age).ToList();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
