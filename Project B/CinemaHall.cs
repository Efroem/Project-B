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

    public static List<Movie> ReadMoviesFromJson()
    {
        string jsonString = File.ReadAllText("movies.json");
        return JsonSerializer.Deserialize<List<Movie>>(jsonString);
    }

    private static void WriteSchedulesToJson(List<Schedule> schedules)
    {
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true,
            IgnoreNullValues = true
        };

        string jsonString = JsonSerializer.Serialize(schedules, options);
        File.WriteAllText("schedule.json", jsonString);
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
            Console.WriteLine($"Naam: {hall.Name}\nGrootte: {hall.Size}\nSerial Number: {hall.SerialNumber}\n");
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

    public static void RemoveCinemaHall()
    {
        List<CinemaHall>? cinemaHalls = ReadFromCinemaHall();

        if (cinemaHalls == null)
        {
            Console.WriteLine("Gefaald lezen van de data.");
            return;
        }

        Console.WriteLine("De beschikbare bioscoopzalen:");

        int maxNameLength = cinemaHalls.Max(hall => hall.Name.Length);

        foreach (var hall in cinemaHalls)
        {
            Console.WriteLine($"- Serial number: {hall.SerialNumber.ToString().PadRight(2)}, Name: {hall.Name.PadRight(maxNameLength)}");
        }

        Console.WriteLine("\nWelke zaal wilt u verwijderen? Voer het serienummer in:");

        int serialNumber;
        while (true)
        {
            if (!int.TryParse(Console.ReadLine(), out serialNumber))
            {
                Console.WriteLine("Ongeldige invoer. Voer een geldig serienummer in (Voorbeeld: '3'):");
                continue;
            }

            if (cinemaHalls.Exists(hall => hall.SerialNumber == serialNumber))
            {
                break;
            }
            else
            {
                Console.WriteLine("Bioscoopzaal met het serienummer bestaat niet. Voer een geldig serienummer in (Voorbeeld: '3'):");
            }
        }

        CinemaHall? hallToRemove = cinemaHalls.Find(hall => hall.SerialNumber == serialNumber);

        if (hallToRemove == null)
        {
            Console.WriteLine($"Bioscoopzaal met serienummer {serialNumber} niet gevonden.");
            return;
        }

        cinemaHalls.Remove(hallToRemove);

        try
        {
            string jsonString = JsonSerializer.Serialize(cinemaHalls, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("cinemaHall.json", jsonString);
            Console.WriteLine($"Bioscoopzaal met serienummer {serialNumber} succesvol verwijderd.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Er is een error ontstaan met het verwijderen van de bioscoopzaal: {ex.Message}");
        }
    }

    public static void ChangeCinemaHall()
    {
        List<CinemaHall>? cinemaHalls = ReadFromCinemaHall();

        if (cinemaHalls == null)
        {
            Console.WriteLine("Gefaald lezen van de data.");
            return;
        }

        Console.WriteLine("De beschikbare bioscoopzalen:");

        int maxNameLength = cinemaHalls.Max(hall => hall.Name.Length);

        foreach (var hall in cinemaHalls)
        {
            Console.WriteLine($"- Serial number: {hall.SerialNumber.ToString().PadRight(2)}, Name: {hall.Name.PadRight(maxNameLength)}");
        }

        Console.WriteLine("\nWelke zaal wilt u wijzigen? Voer het serienummer in:");

        int serialNumber;
        while (true)
        {
            if (!int.TryParse(Console.ReadLine(), out serialNumber))
            {
                Console.WriteLine("Ongeldige invoer. Voer een geldig serienummer in (Voorbeeld '3'):");
                continue;
            }

            if (cinemaHalls.Exists(hall => hall.SerialNumber == serialNumber))
            {
                break;
            }
            else
            {
                Console.WriteLine("Bioscoopzaal met het serienummer bestaat niet. Voer een geldig serienummer in (Voorbeeld '3'):");
            }
        }

        Console.WriteLine("Wat wilt u wijzigen?\n1. Naam\n2. Grootte");

        string? choiceInput;
        int choice;
        while (true)
        {
            choiceInput = Console.ReadLine()?.ToLower();
            if (choiceInput == "naam" || choiceInput == "1")
            {
                choice = 1;
                break;
            }
            else if (choiceInput == "grootte" || choiceInput == "2")
            {
                choice = 2;
                break;
            }
            else
            {
                Console.WriteLine($"Ongeldige invoer. Voer 1 ('naam') of 2 ('grootte') in voor uw keuze.");
            }
        }

        CinemaHall hallToChange = cinemaHalls.Find(hall => hall.SerialNumber == serialNumber)!;

        switch (choice)
        {
            case 1:
                Console.Write("Voer de nieuwe naam in: ");
                string? newName = Console.ReadLine();
                if (newName == null)
                {
                    do
                    {
                        Console.WriteLine("verkeerde input");
                    } while (newName == null);

                }
                else
                {
                    hallToChange.Name = newName;
                }
                break;

            case 2:
                int newSize;
                do
                {
                    Console.WriteLine($"Kies de nieuwe grootte van de bioscoopzaal\n1 - klein: (55 mensen)\n2 - medium: (86 mensen)\n3 - groot(100 mensen):");
                    if (!int.TryParse(Console.ReadLine(), out newSize) || (newSize < 1 || newSize > 3))
                    {
                        Console.WriteLine("Verkeerde input. Kies tussen 1, 2 of 3");
                    }
                } while (newSize < 1 || newSize > 3);
                hallToChange.Size = newSize;
                break;
        }

        try
        {
            string jsonString = JsonSerializer.Serialize(cinemaHalls, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("cinemaHall.json", jsonString);
            Console.WriteLine($"Wijziging van bioscoopzaal met serienummer {serialNumber} succesvol doorgevoerd.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Er is een error ontstaan met het wijzigen van de bioscoopzaal: {ex.Message}");
        }
    }

    public static void AddNewMovie()
    {
        List<Schedule> schedules = Schedule.ReadScheduleJson();
        List<CinemaHall>? cinemaHalls = ReadFromCinemaHall();

        if (cinemaHalls == null || cinemaHalls.Count == 0)
        {
            Console.WriteLine("Geen zalen beschikbaar om een film toe te voegen.");
            return;
        }

        Console.WriteLine("Beschikbare bioscoopzalen:");
        int maxNameLength = cinemaHalls.Max(hall => hall.Name.Length);

        foreach (var hall in cinemaHalls)
        {
            Console.WriteLine($"- Serial number: {hall.SerialNumber.ToString().PadRight(2)}, Name: {hall.Name.PadRight(maxNameLength)}");
        }

        Console.WriteLine("\nWelke zaal wilt u selecteren? Voer het serienummer in:");

        int serialNumber;
        while (true)
        {
            if (!int.TryParse(Console.ReadLine(), out serialNumber))
            {
                Console.WriteLine("Ongeldige invoer. Voer een geldig serienummer in (Voorbeeld '3'):");
                continue;
            }

            if (cinemaHalls.Exists(hall => hall.SerialNumber == serialNumber))
            {
                break;
            }
            else
            {
                Console.WriteLine("Bioscoopzaal met het serienummer bestaat niet. Voer een geldig serienummer in (Voorbeeld '3'):");
            }
        }

        Console.WriteLine("Kies een film uit de volgende lijst:\n");
        List<Movie> movies = ReadMoviesFromJson();

        foreach (var movie in movies)
        {
            Console.WriteLine($"- {movie.Title}");
        }

        Console.Write("Voer de titel van de film in:");
        string? movieTitle = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(movieTitle))
        {
            Console.WriteLine("Titel van de film mag niet leeg zijn.");
            return;
        }

        Console.Write("Voer de datum van de film in (dd/MM/yyyy)");
        string? dateString = Console.ReadLine();
        DateTime date;
        while (!DateTime.TryParse(Console.ReadLine(), out date))
        {
            Console.WriteLine("Verkeerde format. Voer de datum op een juiste manier in (dd/MM/yyyy)");
            dateString = Console.ReadLine();
        }

        Console.WriteLine("Voer het uur in (0-23):");
        int hour;
        while (!int.TryParse(Console.ReadLine(), out hour) || hour < 0 || hour > 23)
        {
            Console.WriteLine("Verkeerde input. Voer het uur binnen 0 en 23 in");
        }

        Console.WriteLine("Voer de minuut in (0-59):");
        int minute;
        while (!int.TryParse(Console.ReadLine(), out minute) || minute < 0 || minute > 59)
        {
            Console.WriteLine("Verkeerde input. Voer de minuut tussen 0 en 59 in");
        }

        DateTime movieDateTime = new DateTime(date.Year, date.Month, date.Day, hour, minute, 0);

        Schedule newSchedule = new Schedule
        {
            MovieTitle = movieTitle,
            SerialNumber = serialNumber,
            Date = movieDateTime
        };

        schedules.Add(newSchedule);
        WriteSchedulesToJson(schedules);
    }
}