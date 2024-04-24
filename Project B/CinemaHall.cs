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
                Console.WriteLine("JSON inhoud is leeg.");
                return null;
            }

            List<CinemaHall>? cinemaHalls = JsonSerializer.Deserialize<List<CinemaHall>>(jsonContent);
            return cinemaHalls;
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Bestand niet gevonden: cinemaHall.json");
            return null;
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Error lezen van JSON inhoud: {ex.Message}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error gevonden: {ex.Message}");
            return null;
        }
    }



    public static void PrintCinemaHalls()
    {
        List<CinemaHall>? cinemaHalls = ReadFromCinemaHall();

        if (cinemaHalls == null || cinemaHalls.Count == 0)
        {
            Console.WriteLine("Geen zalen gevonden.");
            return;
        }

        foreach (var hall in cinemaHalls)
        {
            Console.WriteLine($"Naam: {hall.Name}\nGrootte: {hall.Size}\nSerial Number: {hall.SerialNumber}");
        }
    }



    public static void AddNewCinemaHall()
    {
        List<CinemaHall>? cinemaHalls = ReadFromCinemaHall();

        if (cinemaHalls == null)
        {
            Console.WriteLine("Gefaald lezen van de data.");
            return;
        }

        Console.Write("Naam van de bioscoopzaal: ");
        string? name = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Verkeerde naam. Voer een juiste naam in.");
            return;
        }

        int size;
        do
        {
            Console.WriteLine($"Kies de grootte van de nieuwe bioscoopzaal\n1 - klein: (55 mensen)\n2 - medium: (86 mensen)\n3 - groot(100 mensen):");
            if (!int.TryParse(Console.ReadLine(), out size) || (size < 1 || size > 3))
            {
                Console.WriteLine("Verkeerde input. Kies tussen 1, 2, of 3.");
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
            Console.WriteLine("Nieuwe bioscoopzaal succesvol toegevoegd.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Er is een error ontstaan met het maken van de bioscoopzaal: {ex.Message}");
        }
    }
}
