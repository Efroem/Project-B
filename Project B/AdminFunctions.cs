using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;

public class AdminFunctions
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("size")]
    public int Size { get; set; }

    [JsonPropertyName("serial number")]
    public int SerialNumber { get; set; }

    public AdminFunctions(string name, int size, int serialNumber)
    {
        Name = name;
        Size = size;
        SerialNumber = serialNumber;
    }

    public static List<AdminFunctions>? ReadFromCinemaHall()
    {
        try
        {
            string jsonContent = File.ReadAllText("cinemaHall.json");

            if (string.IsNullOrWhiteSpace(jsonContent))
            {
                Console.WriteLine("JSON inhoud is leeg.");
                return null;
            }

            List<AdminFunctions>? cinemaHalls = JsonSerializer.Deserialize<List<AdminFunctions>>(jsonContent);
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
        schedules.Sort((s1, s2) => s1.Date.CompareTo(s2.Date));

        JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        string jsonString = JsonSerializer.Serialize(schedules, options);
        File.WriteAllText("schedule.json", jsonString);
    }

    public static void FormatCinemaHalls(List<AdminFunctions> cinemaHalls)
    {
        int maxNameLength = cinemaHalls.Max(hall => hall.Name.Length);
        int maxSerialNumberLength = cinemaHalls.Max(hall => hall.SerialNumber.ToString().Length);
        int maxSizeLength = cinemaHalls.Max(hall => hall.Size.ToString().Length);

        for (int i = 0; i < cinemaHalls.Count; i++)
        {
            var hall = cinemaHalls[i];
            Console.WriteLine($"Serial Number: {hall.SerialNumber.ToString().PadRight(maxSerialNumberLength)}  Name: {hall.Name.PadRight(maxNameLength)}  Grootte: {hall.Size.ToString().PadRight(maxSizeLength)}");
            Console.WriteLine(new string('-', 40));
        }
    }



    public static void PrintCinemaHalls()
    {
        List<AdminFunctions>? cinemaHalls = ReadFromCinemaHall();

        if (cinemaHalls == null || cinemaHalls.Count == 0)
        {
            Console.WriteLine("Geen zalen gevonden.");
            return;
        }

        int maxNameLength = cinemaHalls.Max(hall => hall.Name.Length);
        int maxSerialNumberLength = cinemaHalls.Max(hall => hall.SerialNumber.ToString().Length);
        int maxSizeLength = cinemaHalls.Max(hall => hall.Size.ToString().Length);

        Console.WriteLine("Beschikbare bioscoopzalen:\n");

        for (int i = 0; i < cinemaHalls.Count; i++)
        {
            var hall = cinemaHalls[i];
            Console.WriteLine($"Serial Number: {hall.SerialNumber.ToString().PadRight(maxSerialNumberLength)}  Name: {hall.Name.PadRight(maxNameLength)}  Grootte: {hall.Size.ToString().PadRight(maxSizeLength)}");
            Console.WriteLine(new string('-', 40));

            if (i == cinemaHalls.Count - 1)
            {
                Console.WriteLine();
                ProgramFunctions.PrintColoredTextCentered("Druk op een ", ConsoleColor.White, "knop", ConsoleColor.Magenta, " om verder te gaan", ConsoleColor.White);
                Console.ReadKey();
            }
        }
    }

    private static bool TryAgain()
    {
        while (true)
        {
            Console.WriteLine("1. Probeer opnieuw\n2. Terug naar het hoofdmenu");
            string? choice = Console.ReadLine();
            if (choice == "1")
            {
                return true;
            }
            else if (choice == "2")
            {
                return false;
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Verkeerde input. Kies 1 of 2.");
            }
        }
    }


    public static void AddNewCinemaHall()
    {
        List<AdminFunctions>? cinemaHalls = ReadFromCinemaHall();
        string? name = null;

        if (cinemaHalls == null)
        {
            Console.WriteLine("Gefaald lezen van de data.");
            return;
        }

        while (true)
        {
            Console.Write($"\nNaam van de bioscoopzaal: ");
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Verkeerde naam. Voer een juiste naam in.");
                if (!TryAgain()) return;
                Console.Clear();
                Console.WriteLine("\x1b[3J");
            }
            else
            {
                name = input;
                break;
            }
        }

        int size;
        while (true)
        {
            Console.Write($"\nKies de grootte van de nieuwe bioscoopzaal\n1 - klein: (55 mensen)\n2 - medium: (86 mensen)\n3 - groot(100 mensen):");
            if (!int.TryParse(Console.ReadLine(), out size) || (size < 1 || size > 3))
            {
                if (!TryAgain()) return;
                Console.Clear();
                Console.WriteLine("\x1b[3J");
            }
            else
            {
                break;
            }
        }

        int maxSerialNumber = cinemaHalls.Count > 0 ? cinemaHalls.Max(hall => hall.SerialNumber) : 0;
        int serialNumber = maxSerialNumber + 1;

        AdminFunctions newCinemaHall = new AdminFunctions(name, size, serialNumber);
        cinemaHalls.Add(newCinemaHall);

        try
        {
            string jsonString = JsonSerializer.Serialize(cinemaHalls, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("cinemaHall.json", jsonString);
            Console.Clear();
            Console.WriteLine();
            ProgramFunctions.PrintTextCentered($"Nieuwe bioscoopzaal {name} succesvol toegevoegd");
            ProgramFunctions.PrintColoredTextCentered("Druk op een ", ConsoleColor.White, "knop", ConsoleColor.Magenta, " om verder te gaan", ConsoleColor.White);
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Er is een error ontstaan met het maken van de bioscoopzaal: {ex.Message}");
        }
    }


    public static void RemoveCinemaHall()
    {
        List<AdminFunctions>? cinemaHalls = ReadFromCinemaHall();

        if (cinemaHalls == null)
        {
            Console.WriteLine("Gefaald lezen van de data.");
            return;
        }

        FormatCinemaHalls(cinemaHalls);

        Console.WriteLine("\nWelke zaal wilt u verwijderen? Voer het serienummer in:");

        int serialNumber;
        while (true)
        {
            if (!int.TryParse(Console.ReadLine(), out serialNumber))
            {
                Console.WriteLine("Ongeldige invoer. Voer een geldig serienummer in (Voorbeeld: '3'):");
                if (!TryAgain()) return;
                Console.Clear();
                Console.WriteLine("\x1b[3J");
                FormatCinemaHalls(cinemaHalls);
                Console.WriteLine("\nWelke zaal wilt u verwijderen? Voer het serienummer in:");
            }
            else if (cinemaHalls.Exists(hall => hall.SerialNumber == serialNumber))
            {
                break;
            }
            else
            {
                Console.WriteLine("Bioscoopzaal met het serienummer bestaat niet. Voer een geldig serienummer in (Voorbeeld: '3'):");
                if (!TryAgain()) return;
                Console.Clear();
                Console.WriteLine("\x1b[3J");
                FormatCinemaHalls(cinemaHalls);
                Console.WriteLine("\nWelke zaal wilt u verwijderen? Voer het serienummer in:");
            }
        }

        AdminFunctions? hallToRemove = cinemaHalls.Find(hall => hall.SerialNumber == serialNumber);

        if (hallToRemove == null)
        {
            Console.WriteLine($"Bioscoopzaal met serienummer {serialNumber} niet gevonden.");
            if (!TryAgain()) return;
            Console.Clear();
            Console.WriteLine("\x1b[3J");
            FormatCinemaHalls(cinemaHalls);
            Console.WriteLine("\nWelke zaal wilt u verwijderen? Voer het serienummer in:");
        }
        else
        {
            cinemaHalls.Remove(hallToRemove);

            try
            {
                string jsonString = JsonSerializer.Serialize(cinemaHalls, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText("cinemaHall.json", jsonString);
                Console.Clear();
                Console.WriteLine();
                ProgramFunctions.PrintTextCentered($"Bioscoopzaal met serienummer {serialNumber} succesvol verwijderd.");
                ProgramFunctions.PrintColoredTextCentered("Druk op een ", ConsoleColor.White, "knop", ConsoleColor.Magenta, " om verder te gaan", ConsoleColor.White);
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Er is een error ontstaan met het verwijderen van de bioscoopzaal: {ex.Message}");
            }
        }
    }



    public static void ChangeCinemaHall()
    {
        List<AdminFunctions>? cinemaHalls = ReadFromCinemaHall();

        if (cinemaHalls == null)
        {
            Console.WriteLine("Gefaald lezen van de data.");
            return;
        }

        FormatCinemaHalls(cinemaHalls);

        Console.WriteLine("\nWelke zaal wilt u wijzigen? Voer het serienummer in:");

        int serialNumber;
        while (true)
        {
            if (!int.TryParse(Console.ReadLine(), out serialNumber))
            {
                Console.WriteLine("Ongeldige invoer. Voer een geldig serienummer in (Voorbeeld '3'):");
                if (!TryAgain()) return;
                Console.Clear();
                Console.WriteLine("\x1b[3J");
                FormatCinemaHalls(cinemaHalls);
                Console.WriteLine("\nWelke zaal wilt u wijzigen? Voer het serienummer in:");
            }
            else if (cinemaHalls.Exists(hall => hall.SerialNumber == serialNumber))
            {
                break;
            }
            else
            {
                Console.WriteLine("Bioscoopzaal met het serienummer bestaat niet. Voer een geldig serienummer in (Voorbeeld '3'):");
                if (!TryAgain()) return;
                Console.Clear();
                Console.WriteLine("\x1b[3J");
                FormatCinemaHalls(cinemaHalls);
                Console.WriteLine("\nWelke zaal wilt u wijzigen? Voer het serienummer in:");
            }
        }

        Console.WriteLine("\nWat wilt u wijzigen?\n1. Naam\n2. Grootte");

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
                if (!TryAgain()) return;
                Console.Clear();
                Console.WriteLine("\x1b[3J");
                FormatCinemaHalls(cinemaHalls);
                Console.WriteLine("\nWat wilt u wijzigen?\n1. Naam\n2. Grootte");
            }
        }

        AdminFunctions hallToChange = cinemaHalls.Find(hall => hall.SerialNumber == serialNumber)!;

        switch (choice)
        {
            case 1:
                while (true)
                {
                    Console.Write("Voer de nieuwe naam in: ");
                    string? newName = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newName))
                    {
                        Console.WriteLine("Verkeerde input. Voer een geldige naam in.");
                        if (!TryAgain()) return;
                        Console.Clear();
                        Console.WriteLine("\x1b[3J");
                    }
                    else
                    {
                        hallToChange.Name = newName;
                        break;
                    }
                }
                break;

            case 2:
                int newSize;
                while (true)
                {
                    Console.WriteLine($"Kies de nieuwe grootte van de bioscoopzaal\n1 - klein: (55 mensen)\n2 - medium: (86 mensen)\n3 - groot(100 mensen):");
                    if (!int.TryParse(Console.ReadLine(), out newSize) || (newSize < 1 || newSize > 3))
                    {
                        Console.WriteLine("Verkeerde input. Kies tussen 1, 2 of 3.");
                        if (!TryAgain()) return;
                        Console.Clear();
                        Console.WriteLine("\x1b[3J");
                        FormatCinemaHalls(cinemaHalls);
                    }
                    else
                    {
                        break;
                    }
                }
                hallToChange.Size = newSize;
                break;
        }

        try
        {
            string jsonString = JsonSerializer.Serialize(cinemaHalls, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("cinemaHall.json", jsonString);
            ProgramFunctions.PrintTextCentered($"Wijziging van bioscoopzaal met serienummer {serialNumber} succesvol doorgevoerd.");
            ProgramFunctions.PrintColoredTextCentered("Druk op een ", ConsoleColor.White, "knop", ConsoleColor.Magenta, " om verder te gaan", ConsoleColor.White);
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Er is een error ontstaan met het wijzigen van de bioscoopzaal: {ex.Message}");
        }
    }


    public static void AddNewMovie()
    {
        List<Schedule> schedules = Schedule.ReadScheduleJson();
        List<AdminFunctions>? cinemaHalls = ReadFromCinemaHall();

        if (cinemaHalls == null || cinemaHalls.Count == 0)
        {
            Console.WriteLine("Geen zalen beschikbaar om een film toe te voegen.");
            return;
        }

        int serialNumber;
        while (true)
        {
            FormatCinemaHalls(cinemaHalls);
            Console.WriteLine("\nWelke zaal wilt u selecteren? Voer het serienummer in:");
            if (!int.TryParse(Console.ReadLine(), out serialNumber) || !cinemaHalls.Exists(hall => hall.SerialNumber == serialNumber))
            {
                Console.WriteLine("Ongeldige invoer. Voer een geldig serienummer in.");
                if (!TryAgain()) return;
                Console.Clear();
                Console.WriteLine("\x1b[3J");
                FormatCinemaHalls(cinemaHalls);
                Console.WriteLine("\nWelke zaal wilt u selecteren? Voer het serienummer in:");
            }
            else
            {
                break;
            }
        }

        AdminFunctions selectedHall = cinemaHalls.First(hall => hall.SerialNumber == serialNumber);

        List<Movie> movies = ReadMoviesFromJson();
        if (movies == null || movies.Count == 0)
        {
            Console.WriteLine("Geen films beschikbaar om te selecteren");
            return;
        }

        Console.Clear();
        Console.WriteLine("\x1b[3J");

        for (int i = 0; i < movies.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {movies[i].Title}");
            Console.WriteLine(new string('-', 40));
        }

        int selectedMovieIndex;
        while (true)
        {
            Console.Write($"\nVoer het nummer van de film in: ");
            if (!int.TryParse(Console.ReadLine(), out selectedMovieIndex) || selectedMovieIndex < 1 || selectedMovieIndex > movies.Count)
            {
                Console.WriteLine("Ongeldige invoer. Voer een geldig nummer in.");
                if (!TryAgain()) return;
                Console.Clear();
                Console.WriteLine("\x1b[3J");
                for (int i = 0; i < movies.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {movies[i].Title}");
                    Console.WriteLine(new string('-', 40));
                }
            }
            else
            {
                break;
            }
        }

        string movieTitle = movies[selectedMovieIndex - 1].Title;

        Console.Write($"\nVoer de datum van de film in (dd/MM/yyyy): ");
        DateTime date;
        while (true)
        {
            string? dateString = Console.ReadLine();
            dateString = dateString?.Replace("-", "/");
            if (DateTime.TryParseExact(dateString, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out date))
            {
                break;
            }
            else
            {
                Console.WriteLine("Verkeerde format. Voer een geldige datum in en op de juiste manier: (dd/MM/yyyy)");
                if (!TryAgain()) return;
                Console.Clear();
                Console.WriteLine("\x1b[3J");
                Console.Write($"\nVoer de datum van de film in (dd/MM/yyyy): ");
            }
        }

        Console.Write($"\nVoer het uur in (0-23): ");
        int hour;
        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out hour) || hour < 0 || hour > 23)
            {
                break;
            }
            else
            {
                Console.WriteLine("Verkeerde input. Voer het uur binnen 0 en 23 in");
                if (!TryAgain()) return;
                Console.Clear();
                Console.WriteLine("\x1b[3J");
                Console.Write($"\nVoer het uur in (0-23): ");
            }

        }

        Console.Write($"\nVoer de minuut in (0-59): ");
        int minute;
        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out minute) || minute < 0 || minute > 59)
            {
                break;
            }
            else
            {
                Console.WriteLine("Verkeerde input. Voer de minuut tussen 0 en 59 in");
                if (!TryAgain()) return;
                Console.Clear();
                Console.WriteLine("\x1b[3J");
                Console.Write($"\nVoer de minuut in (0-59): ");
            }
        }

        DateTime movieDateTime = new DateTime(date.Year, date.Month, date.Day, hour, minute, 0);

        Schedule newSchedule = new(movieTitle, serialNumber, movieDateTime);

        schedules.Add(newSchedule);
        WriteSchedulesToJson(schedules);

        Console.Clear();
        ProgramFunctions.PrintTextCentered("Film succesvol aan het rooster toegevoegd");
        ProgramFunctions.PrintColoredTextCentered("Druk op een ", ConsoleColor.White, "knop", ConsoleColor.Magenta, " om verder te gaan", ConsoleColor.White);
        Console.ReadKey();
    }
}