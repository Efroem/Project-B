using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

public class CinemaHall
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("size")]
    public int Size { get; set; }

    [JsonPropertyName("serial number")]
    public int SerialNumber { get; set; }

    public CinemaHall(string name, int size, int serialNumber)
    {
        Name = name;
        Size = size;
        SerialNumber = serialNumber;
    }

    public static List<CinemaHall>? ReadFromCinemaHall()
    {
        try
        {
            string jsonContent = File.ReadAllText("cinemaHall.json");

            if (string.IsNullOrWhiteSpace(jsonContent))
            {
                Console.WriteLine("JSON content is empty.");
                return null;
            }

            List<CinemaHall>? cinemaHalls = JsonSerializer.Deserialize<List<CinemaHall>>(jsonContent);
            return cinemaHalls;
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("File not found: cinemaHall.json");
            return null;
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Error reading JSON content: {ex.Message}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return null;
        }
    }



    public static void PrintCinemaHalls()
    {
        List<CinemaHall>? cinemaHalls = ReadFromCinemaHall();

        if (cinemaHalls == null || cinemaHalls.Count == 0)
        {
            Console.WriteLine("No cinema halls found.");
            return;
        }

        foreach (var hall in cinemaHalls)
        {
            Console.WriteLine($"Name: {hall.Name}\nSize: {hall.Size}\nSerial Number: {hall.SerialNumber}");
        }
    }



    public static void AddNewCinemaHall()
    {
        List<CinemaHall>? cinemaHalls = ReadFromCinemaHall();

        if (cinemaHalls == null)
        {
            Console.WriteLine("Failed to read the cinema hall data.");
            return;
        }

        Console.Write("Enter the name of the new cinema hall: ");
        string? name = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Invalid name. Name cannot be empty.");
            return;
        }

        int size;
        do
        {
            Console.WriteLine("Enter the size of the new cinema hall (1, 2, or 3):");
            if (!int.TryParse(Console.ReadLine(), out size) || (size < 1 || size > 3))
            {
                Console.WriteLine("Invalid input. Size must be 1, 2, or 3.");
            }
        } while (size < 1 || size > 3);

        int maxSerialNumber = cinemaHalls.Count > 0 ? cinemaHalls.Max(hall => hall.SerialNumber) : 0;
        int serialNumber = maxSerialNumber + 1;

        CinemaHall newCinemaHall = new CinemaHall(name, size, serialNumber);
        cinemaHalls.Add(newCinemaHall);

        try
        {
            string jsonString = JsonSerializer.Serialize(cinemaHalls, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("cinemaHall.json", jsonString);
            Console.WriteLine("New cinema hall successfully added.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while saving the cinema hall data: {ex.Message}");
        }
    }
}
