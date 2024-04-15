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
                Console.WriteLine("+" + new string('-', 32) + "+");
                foreach (JsonElement movie in root.EnumerateArray())
                {
                    // Print the title of each movie with box
                    string? title = movie.GetProperty("Title").GetString();
                    Console.WriteLine("|" + title?.PadRight(32) + "|");
                }
                Console.WriteLine("+" + new string('-', 32) + "+");
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
}




