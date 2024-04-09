using System;
using System.IO;
using System.Text.Json;

class Program
{
    static void Main()
    {
        // Testascii asciiArt = new Testascii();
        // asciiArt.PrintMovies("movies.json");
        // string posters1 = Testascii.text;
        // Console.WriteLine(posters1);
        string allposters = TestPosters.text1 + TestPosters.text2 + TestPosters.text3;
        System.Console.WriteLine(allposters);
        Console.WriteLine("Welkom bij MegaBios!");

        string answer;
        do
        {
            if (Authentication.User == null)
                Console.WriteLine("1. Bekijk films\n2. Inloggen\n3. Bekijk reserveringen\n4. Verlaat pagina\n5. Lijst zalen");
            else
                Console.WriteLine("1. Bekijk films\n2. profiel\n3. Bekijk reserveringen\n4. Verlaat pagina\n5. Lijst zalen");
            answer = (Console.ReadLine() ?? "").ToLower();

            switch (answer)
            {
                case "1":
                case "bekijk films":
                case "bekijk":
                case "movies":
                    Console.WriteLine("Bekijk films:");
                    PrintMovieTitles("movies.json");
                    break;

                case "2":
                case "inloggen":
                case "profiel":
                    Console.WriteLine("Log in");
                    if (Authentication.User == null)
                        Authentication.Start();
                    else
                        Authentication.ViewProfile();
                    break;

                case "3":
                case "bekijk reserveringen":
                case "reserveringen":
                    Console.WriteLine("Reserveringen");
                    break;

                case "4":
                case "verlaat pagina":
                case "q":
                    Console.WriteLine("Tot ziens!");
                    Environment.Exit(0);
                    break;

                case "5":
                case "lijst zalen":
                    Console.WriteLine("Lijst Zalen");
                    Zaal.ReadFromZaal();
                    break;

                default:
                    Console.WriteLine("Ongeldige invoer");
                    break;
            }
        } while (answer != "4" && answer != "verlaat pagina" && answer != "q");

        Environment.Exit(0);
    }

    static void PrintMovieTitles(string jsonFilePath)
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
                    string title = movie.GetProperty("Title").GetString();
                    Console.WriteLine("|" + title.PadRight(32) + "|");
                }
                Console.WriteLine("+" + new string('-', 32) + "+");
            }
        }
    }
}
