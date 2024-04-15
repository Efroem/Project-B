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
                Console.WriteLine("+" + new string('-', 40) + "+");
                foreach (JsonElement movie in root.EnumerateArray())
                {
                    // Print the title of each movie with box
                    string? title = movie.GetProperty("Title").GetString();
                    Console.WriteLine("|" + title?.PadRight(40) + "|");
                }
                Console.WriteLine("+" + new string('-',40) + "+");
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
        Console.WriteLine(@"  _____ ___ _     __  __ ____  
 |  ___|_ _| |   |  \/  / ___| 
 | |_   | || |   | |\/| \___ \ 
 |  _|  | || |___| |  | |___) |
 |_|   |___|_____|_|  |_|____/ 
                                ");
    }
    public static void Printasciihm()
    {
        Console.WriteLine(@"
  _    _  ____   ____  ______ _____  __  __ ______ _   _ _    _ 
 | |  | |/ __ \ / __ \|  ____|  __ \|  \/  |  ____| \ | | |  | |
 | |__| | |  | | |  | | |__  | |  | | \  / | |__  |  \| | |  | |
 |  __  | |  | | |  | |  __| | |  | | |\/| |  __| | . ` | |  | |
 | |  | | |__| | |__| | |    | |__| | |  | | |____| |\  | |__| |
 |_|  |_|\____/ \____/|_|    |_____/|_|  |_|______|_| \_|\____/ 
");
    }

}




