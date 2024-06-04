using System;
using System.IO;
using System.Text.Json;

public class AsciiArtPrinter
{
    public static void PrintCentered(string text)
    {
        string[] lines = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        int windowWidth = Console.WindowWidth;

        foreach (string line in lines)
        {
            int padding = Math.Max(0, (windowWidth - line.Length) / 2);
            Console.WriteLine(line.PadLeft(padding + line.Length));
        }
    }


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
                // Create a list to store movie elements
                List<JsonElement> movies = new List<JsonElement>();

                // Add each movie element to the list
                foreach (JsonElement movie in root.EnumerateArray())
                {
                    movies.Add(movie);
                }

                // Determine the number of posters to print based on screen width
                int numberOfPosters = Console.WindowWidth > 130 ? 4 : 2;

                // Shuffle the list of movies randomly
                Random rnd = new Random();
                movies = movies.OrderBy(x => rnd.Next()).ToList();

                // Print the ASCII art for the selected number of posters
                for (int i = 0; i < Math.Min(numberOfPosters, movies.Count); i += 2)
                {
                    string[] poster1List = movies[i].GetProperty("Ascii").GetString().Split("\n");
                    string[] poster2List = i + 1 < movies.Count ? movies[i + 1].GetProperty("Ascii").GetString().Split("\n") : new string[] { "", "" };

                    for (int j = 0; j < Math.Max(poster1List.Length, poster2List.Length); j++)
                    {
                        if (poster1List.Length > j && poster2List.Length > j)
                        {
                            Console.Write($"{poster1List[j]}");
                            Console.Write(poster2List[j].PadLeft(75 + poster2List[j].Length - poster1List[j].Length, ' '));
                            Console.Write("\n");
                        }
                        else if (poster1List.Length > j)
                            Console.WriteLine($"{poster1List[j] ?? ""}");
                        else
                        {
                            Console.WriteLine(poster2List[j].PadLeft(75 + poster2List[j].Length, ' '));
                        }
                    }

                    // Separate the posters
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
 |_|   |___|_____|_|  |_|____/ ";
        Console.ForegroundColor = ConsoleColor.Yellow;
        PrintCentered(asciiartfilms);
        Console.ResetColor();
    }
    public static void Printasciihm()
    {
        string asciiArthm = @"
  _    _  ____   ____  ______ _____  __  __ ______ _   _ _    _ 
 | |  | |/ __ \ / __ \|  ____|  __ \|  \/  |  ____| \ | | |  | |
 | |__| | |  | | |  | | |__  | |  | | \  / | |__  |  \| | |  | |
 |  __  | |  | | |  | |  __| | |  | | |\/| |  __| | . ` | |  | |
 | |  | | |__| | |__| | |    | |__| | |  | | |____| |\  | |__| |
 |_|  |_|\____/ \____/|_|    |_____/|_|  |_|______|_| \_|\____/ ";
        Console.ForegroundColor = ConsoleColor.Yellow;
        PrintCentered(asciiArthm);
    }

    public static void Totziens()
    {
        string totziens = @"
  _______    _         _                
 |__   __|  | |       (_)               
    | | ___ | |_   _____  ___ _ __  ___ 
    | |/ _ \| __| |_  / |/ _ \ '_ \/ __|
    | | (_) | |_   / /| |  __/ | | \__ \
    |_|\___/ \__| /___|_|\___|_| |_|___/";
        Console.ForegroundColor = ConsoleColor.Yellow;
        PrintCentered(totziens);
    }
    public static void PrintAsciibetaling()
    {
        string betaling = @"
        
        
  ____  ______ _______       _      _____ _   _  _____ 
 |  _ \|  ____|__   __|/\   | |    |_   _| \ | |/ ____|
 | |_) | |__     | |  /  \  | |      | | |  \| | |  __ 
 |  _ <|  __|    | | / /\ \ | |      | | | . ` | | |_ |
 | |_) | |____   | |/ ____ \| |____ _| |_| |\  | |__| |
 |____/|______|  |_/_/    \_\______|_____|_| \_|\_____|
                                                       
        ";
        Console.ForegroundColor = ConsoleColor.Yellow;
        PrintCentered(betaling);
    }
    public static void PrintAsciilogin()
    {
        string login = @"
  _      ____   _____ _____ _   _ 
 | |    / __ \ / ____|_   _| \ | |
 | |   | |  | | |  __  | | |  \| |
 | |   | |  | | | |_ | | | | . ` |
 | |___| |__| | |__| |_| |_| |\  |
 |______\____/ \_____|_____|_| \_|";
        Console.ForegroundColor = ConsoleColor.Yellow;
        PrintCentered(login);
    }
    public static void PrintAsciiInlog()
    {
        string inlog = @"
  _____ _   _ _      ____   _____  _____ ______ _   _ 
 |_   _| \ | | |    / __ \ / ____|/ ____|  ____| \ | |
   | | |  \| | |   | |  | | |  __| |  __| |__  |  \| |
   | | | . ` | |   | |  | | | |_ | | |_ |  __| | . ` |
  _| |_| |\  | |___| |__| | |__| | |__| | |____| |\  |
 |_____|_| \_|______\____/ \_____|\_____|______|_| \_|";
        Console.ForegroundColor = ConsoleColor.Yellow;
        PrintCentered(inlog);
    }
    public static void PrintAsciiRegister()
    {
        string register = @"        
  _____  ______ _____ _____  _____ _______ _____  ______ _____  ______ _   _ 
 |  __ \|  ____/ ____|_   _|/ ____|__   __|  __ \|  ____|  __ \|  ____| \ | |
 | |__) | |__ | |  __  | | | (___    | |  | |__) | |__  | |__) | |__  |  \| |
 |  _  /|  __|| | |_ | | |  \___ \   | |  |  _  /|  __| |  _  /|  __| | . ` |
 | | \ \| |___| |__| |_| |_ ____) |  | |  | | \ \| |____| | \ \| |____| |\  |
 |_|  \_\______\_____|_____|_____/   |_|  |_|  \_\______|_|  \_\______|_| \_|";
        Console.ForegroundColor = ConsoleColor.Yellow;
        PrintCentered(register);
    }
    public static void PrintAsciibeschrijving()
    {
        string asciiArtBeschrijving = @"
 ______ _____ _      __  __      ____  ______  _____  _____ _    _ _____  _____     ___      _______ _   _  _____ 
 |  ____|_   _| |    |  \/  |    |  _ \|  ____|/ ____|/ ____| |  | |  __ \|_   _|   | \ \    / /_   _| \ | |/ ____|
 | |__    | | | |    | \  / |    | |_) | |__  | (___ | |    | |__| | |__) | | |     | |\ \  / /  | | |  \| | |  __ 
 |  __|   | | | |    | |\/| |    |  _ <|  __|  \___ \| |    |  __  |  _  /  | | _   | | \ \/ /   | | | . ` | | |_ |
 | |     _| |_| |____| |  | |    | |_) | |____ ____) | |____| |  | | | \ \ _| || |__| |  \  /   _| |_| |\  | |__| |
 |_|    |_____|______|_|  |_|    |____/|______|_____/ \_____|_|  |_|_|  \_\_____\_____|   \/   |_____|_| \_|\_____|";
        Console.ForegroundColor = ConsoleColor.Yellow;
        PrintCentered(asciiArtBeschrijving);
    }
    public static void MegaBioscoop()
    {
        string Asciiartstart = @"
                
       __          __  _ _                     ____  _ _            
       \ \        / / | | |                   |  _ \(_|_)           
        \ \  /\  / /__| | | _____  _ __ ___   | |_) |_ _            
         \ \/  \/ / _ \ | |/ / _ \| '_ ` _ \  |  _ <| | |           
          \  /\  /  __/ |   < (_) | | | | | | | |_) | | |           
           \/  \/ \___|_|_|\_\___/|_| |_| |_| |____/|_| |           
                                                     _| |
                                                    |__ /
  __  __                    ____                        
 |  \/  |                  |  _ \(_)                           
 | \  / | ___  __ _  __ _  | |_) |_  ___  ___  ___ ___  ___  _ __  
 | |\/| |/ _ \/ _` |/ _` | |  _ <| |/ _ \/ __|/ __/ _ \ / _ \| '_ \ 
 | |  | |  __/ (_| | (_| | | |_) | | (_) \__ \ (_| (_) | (_) | |_) |
 |_|  |_|\___|\__, |\__,_| |____/|_|\___/|___/\___\___/ \___/| .__/ 
               __/ |                                         | |    
              |___/                                          |_|    ";
        Console.ForegroundColor = ConsoleColor.Yellow;
        string centeredAsciiArt = "                    " + Asciiartstart.Replace("\n", "\n                  ");

        Console.WriteLine(centeredAsciiArt);
    }
}
