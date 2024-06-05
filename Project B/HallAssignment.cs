using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;

public class File1Entry
{
    [JsonPropertyName("cinemaHallSerialNumber")]
    public int CinemaHallSerialNumber { get; set; }
}

public class File2Entry
{
    [JsonPropertyName("serial number")]
    public int SerialNumber { get; set; }

    [JsonPropertyName("size")]
    public int size { get; set; }
}

public class HallAssignment
{
    public static void FindMatchingSerialNumbers(List<File1Entry> file1Entries, List<File2Entry> file2Entries)
    {
        foreach (var entry1 in file1Entries)
        {
            foreach (var entry2 in file2Entries)
            {
                if (entry1.CinemaHallSerialNumber == entry2.SerialNumber)
                {
                    //CallFunction(entry1, entry2);
                }
            }
        }
    }

    // public static void CallFunction(File1Entry entry1, File2Entry entry2)
    // {
    //     CinemaHall cinemaZaal = new CinemaHall();
    //     TheaterSeatingPrinter seatingPrinter = new TheaterSeatingPrinter();
        

    //     if (entry2.size == 3)
    //     {
    //         //CinemaHall.PrintGridGroteZaal();
    //         cinemaZaal.NavigateGrid();
    //     }
    //     else if (entry2.size == 2)
    //     {
    //         CinemaHall.PrintGridMediumZaal();
    //         cinemaZaal.NavigateGrid();
    //     }
    //     else
    //     {
    //         CinemaHall.PrintGridKleineZaal();
    //         cinemaZaal.NavigateGrid();
    //     }

    // }

    public static void Callfunction2()
    {
        try
        {
            string json1 = File.ReadAllText("schedule.json");
            string json2 = File.ReadAllText("cinemaHall.json");

            List<File1Entry> file1Entries = JsonSerializer.Deserialize<List<File1Entry>>(json1);
            List<File2Entry> file2Entries = JsonSerializer.Deserialize<List<File2Entry>>(json2);

            Console.Clear();
            FindMatchingSerialNumbers(file1Entries, file2Entries);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}



