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
                if (Console.WindowWidth > 130)
                {
                    List<JsonElement> filteredElements = new List<JsonElement>();
                    foreach (JsonElement element in root.EnumerateArray())
                    {
                        if (element.GetProperty("Ascii").GetString() != "")
                        {
                            filteredElements.Add(element);
                        }
                    }
                    for (int i = 0; i < filteredElements.Count; i += 2)
                    {
                        string[] poster1List = filteredElements[i].GetProperty("Ascii").GetString().Split("\n");
                        string[] poster2List = filteredElements.Count() - 1 > i ? filteredElements[i + 1].GetProperty("Ascii").GetString().Split("\n") : new string[] { "", "" };

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
                    }
                }
                else
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
    }
    public static void PrintAsciifilms()
    {
        string asciiartfilms = @" 
  _____ ___ _     __  __ ____  
 |  ___|_ _| |   |  \/  / ___| 
 | |_   | || |   | |\/| \___ \ 
 |  _|  | || |___| |  | |___) |
 |_|   |___|_____|_|  |_|____/ ";
        PrintCentered(asciiartfilms);
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
        PrintCentered(totziens);
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
        string centeredAsciiArt = "                    " + Asciiartstart.Replace("\n", "\n                  ");

        Console.WriteLine(centeredAsciiArt);
    }
}
