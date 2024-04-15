using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

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

    private static bool IsWord(string value)
    {
        return !string.IsNullOrEmpty(value) && Regex.IsMatch(value, @"^[a-zA-Z]+$");
    }

    public void WriteToCinemaHall(string name, string rows, string columns, string serialNumber)
    {
        int parsedRows = int.Parse(rows);
        int parsedColumns = int.Parse(columns);
        int parsedSerialNumber = int.Parse(serialNumber);

        CinemaHall newCinemaHall = new CinemaHall(name, parsedRows, parsedColumns, parsedSerialNumber);

        string directory = Directory.GetCurrentDirectory();
        string filePath = Path.Combine(directory, "cinemaHall.json");

        try
        {
            string json = File.ReadAllText(filePath);
            List<CinemaHall> cinemaHalls = JsonSerializer.Deserialize<List<CinemaHall>>(json) ?? new List<CinemaHall>();

            cinemaHalls.Add(newCinemaHall);

            string updatedJson = JsonSerializer.Serialize(cinemaHalls, new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(filePath, updatedJson);

            Console.WriteLine("CinemaHall toegevoegd");
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("File not found.");
        }
        catch (JsonException)
        {
            Console.WriteLine("Failed to parse JSON.");
        }
    }

    public static void AddNewCinemaHall()
    {
        Console.WriteLine("Voeg nieuwe zaal toe");

        string name;
        do
        {
            Console.Write("Zaal naam: ");
            name = Console.ReadLine() ?? "";
            if (!IsWord(name))
            {
                Console.WriteLine("Ongeldige zaal naam. Probeer opnieuw.");
            }
            name = char.ToUpper(name[0]) + name.Substring(1);
        } while (!IsWord(name));

        int rows;
        do
        {
            Console.Write("Aantal rijen: ");
            string? input = Console.ReadLine();
            if (!int.TryParse(input, out rows) || rows <= 0)
            {
                Console.WriteLine("Ongeldig aantal rijen. Voer een positief geheel getal in.");
            }
        } while (rows <= 0);

        int columns;
        do
        {
            Console.Write("Aantal kolommen: ");
            string? input = Console.ReadLine();
            if (!int.TryParse(input, out columns) || columns <= 0)
            {
                Console.WriteLine("Ongeldig aantal kolommen. Voer een positief geheel getal in.");
            }
        } while (columns <= 0);

        int maxSerialNumber = FindMaxSerialNumber();
        int serialNumber = maxSerialNumber + 1;

        string rowsString = rows.ToString();
        string columnsString = columns.ToString();
        string serialNumberString = serialNumber.ToString();

        CinemaHall cinemaHall = new CinemaHall(name, rows, columns, serialNumber);
        cinemaHall.WriteToCinemaHall(name, rowsString, columnsString, serialNumberString);
    }

    static int FindMaxSerialNumber()
    {
        List<CinemaHall> cinemaHalls = ReadFromCinemaHall();

        if (cinemaHalls.Count == 0)
        {
            return 0;
        }

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
