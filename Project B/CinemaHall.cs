using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.IO;

public class CinemaHall
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("rows")]
    public int Rows { get; set; }

    [JsonPropertyName("columns")]
    public int Columns { get; set; }

    [JsonPropertyName("serial number")]
    public int SerialNumber { get; set; }

    public CinemaHall(string name, int rows, int columns, int serialNumber)
    {
        Name = name;
        Rows = rows;
        Columns = columns;
        SerialNumber = serialNumber;
    }


    public void WriteToCinemaHall(string name, string rows, string columns, string serialNumber)
    {
        int parsedRows = int.Parse(rows);
        int parsedColumns = int.Parse(columns);
        int parsedSerialNumber = int.Parse(serialNumber);

        // Create a new CinemaHall object
        CinemaHall newCinemaHall = new CinemaHall(name, parsedRows, parsedColumns, parsedSerialNumber);

        string directory = Directory.GetCurrentDirectory();
        string filePath = Path.Combine(directory, "cinemaHall.json");

        // Serialize the CinemaHall object to JSON
        string json = JsonSerializer.Serialize(newCinemaHall, new JsonSerializerOptions { WriteIndented = true });

        // Write the JSON string to the file
        File.AppendAllText(filePath, json);
        Console.WriteLine("CinemaHall toegevoegd");
    }

    public static void AddNewCinemaHall()
    {
        Console.WriteLine("Voeg nieuwe zaal toe");

        Console.Write("Zaal naam: ");
        string? name = Console.ReadLine() ?? "";

        Console.Write("Aantal rijen: ");
        int rows = Convert.ToInt32(Console.ReadLine());

        Console.Write("Aantal kolommen: ");
        int columns = Convert.ToInt32(Console.ReadLine());

        // Calculate the next serial number
        int maxSerialNumber = FindMaxSerialNumber();
        int serialNumber = maxSerialNumber + 1;

        // Convert integer values to strings
        string rowsString = rows.ToString();
        string columnsString = columns.ToString();
        string serialNumberString = serialNumber.ToString();

        CinemaHall cinemaHall = new CinemaHall(name, rows, columns, serialNumber);
        cinemaHall.WriteToCinemaHall(name, rowsString, columnsString, serialNumberString);
    }


    static int FindMaxSerialNumber()
    {
        // Read existing CinemaHalls from the JSON file
        List<CinemaHall> cinemaHalls = ReadFromCinemaHall();

        // If there are no existing CinemaHalls, return 0
        if (cinemaHalls.Count == 0)
        {
            return 0;
        }

        // Find the maximum serial number among existing CinemaHalls
        int maxSerialNumber = cinemaHalls.Max(cinemaHall => cinemaHall.SerialNumber);
        return maxSerialNumber;
    }


    public static List<CinemaHall> ReadFromCinemaHall()
    {
        string directory = Directory.GetCurrentDirectory();
        string filePath = Path.Combine(directory, "cinemaHall.json");

        try
        {
            string json = File.ReadAllText(filePath);

            List<CinemaHall>? cinemaHalls = JsonSerializer.Deserialize<List<CinemaHall>>(json);

            if (cinemaHalls != null && cinemaHalls.Count > 0)
            {
                foreach (var cinemaHall in cinemaHalls)
                {
                    Console.WriteLine($"Name: {cinemaHall.Name}, Rows: {cinemaHall.Rows}, Columns: {cinemaHall.Columns}, SerialNumber: {cinemaHall.SerialNumber}");
                }
                return cinemaHalls;
            }
            else
            {
                Console.WriteLine("No cinema halls found or failed to parse JSON.");
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("File not found.");
        }
        catch (JsonException)
        {
            Console.WriteLine("Failed to parse JSON.");
        }
        return new List<CinemaHall>();
    }

}
