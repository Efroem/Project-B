using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;
public class Reservation
{
    [JsonPropertyName("userEmail")]
    public string Email { get; set; }

    [JsonPropertyName("seats")]
    public List<string> Seats { get; set; }

    [JsonPropertyName("scheduleSerialNumber")]
    public int ScheduleSerialNumber
    {
        get => _scheduleSerialNumber; set
        {
            _scheduleSerialNumber = value;
            _schedule = Schedule.ReadScheduleJson().Find(x => x.SerialNumber == value);
        }
    }

    private int _scheduleSerialNumber;

    private Schedule _schedule;
    public List<Product> Food { get; set; }

    [JsonPropertyName("totalPrice")]
    public double TotalPrice { get; set; }

    public Reservation(int scheduleSerialNumber, List<string> seats, List<Product> food, double totalPrice)
    {
        Email = Authentication.User.Email;
        ScheduleSerialNumber = scheduleSerialNumber;
        Seats = seats;
        Food = food;
        TotalPrice = totalPrice;
    }

    public Reservation() { }

    public override string ToString()
    {
        return $"Film: {_schedule.MovieTitle} \nZaal: {_schedule.CinemaHallSerialNumber}\nDatum: {_schedule.Date}\nStoelen: {ListToString(Seats, 5)}\nExtra: {ListToString(Food.Select(x => x.Naam).ToList(), 3)}";
    }

    private string ListToString(List<string> list, int maxLength)
    {
        string returnString = "";
        for (int i = 0; i < list.Count; i++)
        {
            if (i != 0 && i % maxLength == 0)
            {
                returnString += "\n";
            }
            returnString += list[i];
            if (i != list.Count - 1)
                returnString += ", ";
        }
        return returnString;
    }

    public static void OpenReservationMenu(Account currentUser)
    {
        AsciiArtPrinter.PrintAsciiReservering();
        List<Reservation> reservations = ReadReservationJson();
        reservations = reservations.Where(x => x.Email == currentUser.Email).Where(x => x._schedule.Date > DateTime.Now).OrderBy(x => x._schedule.Date).ToList();

        int longestLineLength = 0;
        if (reservations.Count == 0)
        {
            const string noReservations = "U heeft geen reserveringen";
            ProgramFunctions.PrintTextCentered("┌" + new string('─', noReservations.Length + 3) + "┐");
            PrintTextCentered(noReservations, noReservations.Length);
            ProgramFunctions.PrintTextCentered("└" + new string('─', noReservations.Length + 3) + "┘");
        }
        else
        {
            foreach (Reservation reservation in reservations)
            {

                foreach (string line in reservation.ToString().Split('\n'))
                {
                    longestLineLength = longestLineLength < line.Length ? line.Length : longestLineLength;
                }
            }
            ProgramFunctions.PrintTextCentered("┌" + new string('─', longestLineLength + 3) + "┐");
            for (int i = 0; i < reservations.Count; i++)
            {
                PrintTextCentered($"{reservations[i]}", longestLineLength);
                if (i != reservations.Count - 1)
                {
                    ProgramFunctions.PrintTextCentered("├" + new string('─', longestLineLength + 3) + "┤");
                }
            }

            ProgramFunctions.PrintTextCentered("└" + new string('─', longestLineLength + 3) + "┘");
        }

        

        Console.WriteLine();
        ProgramFunctions.PrintColoredTextCentered("Druk op een ", ConsoleColor.White, "knop", ConsoleColor.Magenta, " om terug te gaan", ConsoleColor.White);
        Console.ReadKey();
        return;
    }

    // Modified PrintTextCentered for multiline options
    private static void PrintTextCentered(string text, int longestLongestLineLength)
    {
        string[] textArray = text.Split('\n');
        int windowWidth;
        int leftPadding;
        int longestLineLength = 0;
        foreach (string line in textArray)
        {
            longestLineLength = longestLineLength < line.Length ? line.Length : longestLineLength;
        }

        for (int i = 0; i < textArray.Length; i++)
        {
            windowWidth = Console.WindowWidth;
            leftPadding = (windowWidth - longestLineLength + longestLineLength - longestLongestLineLength) / 2 - 2;
            Console.CursorLeft = leftPadding;

            Console.SetCursorPosition(leftPadding, Console.CursorTop);
            Console.WriteLine($"│ {textArray[i]}" + new string(' ', longestLongestLineLength + 3 - textArray[i].Length - 1) + "│");
        }
    }

    public static List<Reservation> ReadReservationJson()
    {
        string jsonString = File.ReadAllText("reservations.json");

        return JsonSerializer.Deserialize<List<Reservation>>(jsonString) ?? new();
    }

    public static void CreateReservation(int scheduleSerialNumber, HashSet<(int x, int y)> seats, List<Product> products, double totalPrice)
    {
        List<string> seatStrings = seats.Select(seat => $"{seat.x}-{seat.y}").ToList();
        Reservation reservation = new(scheduleSerialNumber, seatStrings, products, totalPrice);
        // Retrieves existing reservations
        List<Reservation> reservations = ReadReservationJson();
        // Adds the new reservation
        reservations.Add(reservation);
        //saves reservations
        JsonSerializerOptions options = new() { WriteIndented = true };
        string jsonString = JsonSerializer.Serialize(reservations, options);
        File.WriteAllText("reservations.json", jsonString);
    }
}