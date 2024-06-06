using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;
class Reservation
{
    [JsonPropertyName("userEmail")]
    public string Email { get; set; }

    [JsonPropertyName("seats")]
    public List<string> Seats { get; set; }

    [JsonPropertyName("scheduleSerialNumber")]
    public int ScheduleSerialNumber {get => _scheduleSerialNumber; set {
        _scheduleSerialNumber = value;
        _schedule = Schedule.ReadScheduleJson().Find(x => x.SerialNumber == value);
    }}

    private int _scheduleSerialNumber;

    private Schedule _schedule;
    public List<Product> Food { get; set; }

    [JsonPropertyName("totalPrice")]
    public double TotalPrice {get; set;}

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
        return $"Film: {_schedule.MovieTitle} \nZaal: {_schedule.CinemaHallSerialNumber}\nDatum: {_schedule}\nStoelen: {string.Join(", ", Seats)}";
    }

    public static void OpenReservationMenu(Account currentUser)
    {
        List<Reservation> reservations = ReadReservationJson();
        reservations = reservations.Where(x => x.Email == currentUser.Email).Where(x => x._schedule.Date > DateTime.Now).OrderBy(x => x._schedule.Date).ToList();

        while (true)
        {
            Console.Clear();
            reservations.ForEach(x => Console.WriteLine($"{x}\n"));
            Console.WriteLine("1. Terug");
            string userAction = (Console.ReadLine() ?? "").ToLower();
            if (userAction == "1" || userAction == "terug")
            {
                return;
            }
        }
    }

    public static List<Reservation> ReadReservationJson()
    {
        string jsonString = File.ReadAllText("reservations.json");

        return JsonSerializer.Deserialize<List<Reservation>>(jsonString) ?? new();
    }

    public static void CreateReservation(int scheduleSerialNumber, HashSet<(int x, int y)> seats, List<Product> products, double totalPrice) {
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