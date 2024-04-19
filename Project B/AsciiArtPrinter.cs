using System;
using System.IO;
using System.Text.Json;

public class AsciiArtPrinter
{
    public static void PrintMovieTitles(string jsonFilePath)
    {
    // Read the JSON file
    string jsonString = File.ReadAllText(jsonFilePath);

    // Deserialize the JSON to a JsonDocument
    using (JsonDocument document = JsonDocument.Parse(jsonString))
    {
        // Access root element
        JsonElement root = document.RootElement;

        // Check if the root element is an array
        if (root.ValueKind == JsonValueKind.Array)
        {
            // Calculate the width of the terminal
            int terminalWidth = Console.WindowWidth;

            // Calculate the maximum width of the movie titles
            int maxTitleWidth = 0;
            foreach (JsonElement movie in root.EnumerateArray())
            {
                string? title = movie.GetProperty("Title").GetString();
                if (title != null && title.Length > maxTitleWidth)
                {
                    maxTitleWidth = title.Length;
                }
            }

            // Calculate padding for center alignment
            int leftPadding = (terminalWidth - maxTitleWidth - 2) / 2; // Subtract 2 for the padding characters '|'

            // Print top border
            Console.WriteLine("+" + new string('-', terminalWidth - 2) + "+");

            // Print movie titles
            foreach (JsonElement movie in root.EnumerateArray())
            {
                string? title = movie.GetProperty("Title").GetString();
                Console.WriteLine("|" + new string(' ', leftPadding) + title.PadRight(maxTitleWidth) + new string(' ', terminalWidth - leftPadding - maxTitleWidth - 2) + "|");
            }

            // Print bottom border
            Console.WriteLine("+" + new string('-', terminalWidth - 2) + "+");
        }
    }
    }
    public static void PrintAscii(string jsonFilePath)
    {
        // Read the JSON file
        string jsonString = File.ReadAllText(jsonFilePath);

        // Deserialize the JSON to a JsonDocument
        using (JsonDocument document = JsonDocument.Parse(jsonString))
        {
            // Access root element
            JsonElement root = document.RootElement;

            // Check if the root element is an array
            if (root.ValueKind == JsonValueKind.Array)
            {
                foreach (JsonElement movie in root.EnumerateArray())
                {
                    // Print the title of each movie with box
                    Console.WriteLine(movie.GetProperty("Ascii").GetString());
                    Console.WriteLine();
                }
            }
        }
    }
    public static void PrintAsciifilms()
    {
        string asciiartfilms = @" 
  _____ ___ _     __  __ ____  
 |  ___|_ _| |   |  \/  / ___| 
 | |_   | || |   | |\/| \___ \ 
 |  _|  | || |___| |  | |___) |
 |_|   |___|_____|_|  |_|____/ 
                                ";
        string centeredAsciiArt = "                    " + asciiartfilms.Replace("\n", "\n                                      ");
        Console.WriteLine(centeredAsciiArt);
    }
    public static void Printasciihm()
    {
    string asciiArthm = @"
  _    _  ____   ____  ______ _____  __  __ ______ _   _ _    _ 
 | |  | |/ __ \ / __ \|  ____|  __ \|  \/  |  ____| \ | | |  | |
 | |__| | |  | | |  | | |__  | |  | | \  / | |__  |  \| | |  | |
 |  __  | |  | | |  | |  __| | |  | | |\/| |  __| | . ` | |  | |
 | |  | | |__| | |__| | |    | |__| | |  | | |____| |\  | |__| |
 |_|  |_|\____/ \____/|_|    |_____/|_|  |_|______|_| \_|\____/ 
";

    // Voeg handmatig het aantal spaties toe om het in het midden te plaatsen
    string centeredAsciiArt = "                    " + asciiArthm.Replace("\n", "\n                  ");

    // Print de ASCII art
    Console.WriteLine(centeredAsciiArt);
    }
    public static void PrintAsciiMenu()
    {
        string menuText = @"
                    ┌─────────────────────────┐
                    │          Menu:          │
                    ├─────────────────────────┤
                    │ 1. Bekijk films         │
                    │ 2. Inloggen             │
                    │ 3. Bekijk reserveringen │
                    │ 4. Verlaat pagina       │
                    │ 5. Lijst zalen          │
                    │ 6. CinemaHall toevoegen │
                    └─────────────────────────┘
";
        string centeredAsciiArt = "                    " + menuText.Replace("\n", "\n                  ");
        Console.WriteLine(centeredAsciiArt);
    }
    
    public static void PrintAsciiMenu2()
    {
        string menuText2 = @"
                    ┌─────────────────────────┐
                    │          Menu:          │
                    ├─────────────────────────┤
                    │ 1. Bekijk films         │
                    │ 2. Profiel bekijken     │
                    │ 3. Bekijk reserveringen │
                    │ 4. Verlaat pagina       │
                    │ 5. Lijst zalen          │
                    │ 6. CinemaHall toevoegen │
                    └─────────────────────────┘
        ";
        string centeredAsciiArt = "                    " + menuText2.Replace("\n", "\n                  ");
        Console.WriteLine(centeredAsciiArt);
    }


}




