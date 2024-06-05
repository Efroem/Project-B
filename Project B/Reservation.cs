using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;
class Reservation
{
    [JsonPropertyName("movieTitle")]
    public string MovieTitle { get; set; }

    [JsonPropertyName("serialNumber")]
    public string JsonSerialNumber
    {
        set
        {
            SerialNumber = Convert.ToInt32(value);
            Hall = AdminFunctions.ReadFromCinemaHall().Find(x => x.SerialNumber == SerialNumber);
        }
    }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("date")]
    public string JsonDate { set => Date = DateTime.ParseExact(value, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture); }

    [JsonPropertyName("seats")]
    public List<string> Seats { get; set; }

    public DateTime Date { get; set; }

    public int SerialNumber { get; set; }

    public AdminFunctions? Hall { get; set; }

    public List<string> Food { get; set; }

    public Reservation(string movieTitle, string serialNumber, string email, string date, List<string> seats, List<string>? food)
    {
        MovieTitle = movieTitle;
        JsonSerialNumber = serialNumber;
        Email = email;
        Date = DateTime.ParseExact(date, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        Seats = seats;
        Food = food ?? new();
    }

    public Reservation() { }

    public override string ToString()
    {
        string hallName = Hall is not null ? Hall.Name : "onbekend";
        return $"Film: {MovieTitle} \nZaal: {hallName} \nStoel: {Seats} \nDatum: {Date}";
    }

    public static void OpenReservationMenu(Account currentUser)
    {
        List<Reservation> reservations = ReadReservationJson();
        reservations = reservations.Where(x => x.Email == currentUser.Email).Where(x => x.Date > DateTime.Now).OrderBy(x => x.Date).ToList();

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
        // Read the JSON file as a string
        string jsonString = File.ReadAllText("reservations.json");

        // Deserialize the JSON string into an object
        return JsonSerializer.Deserialize<List<Reservation>>(jsonString) ?? new();
    }


    public void createReservation(string movieTitle, string hallSerialNumber, string userEmail, string date, List<string> seats, List<string>? food)
    {
        Reservation reservation = new(movieTitle, hallSerialNumber, userEmail, date, seats, food);
        // Retrieves existing accounts
        List<Reservation> reservations = ReadReservationJson();
        // Adds the new account
        reservations.Add(reservation);
        //saves accounts
        JsonSerializerOptions options = new() { WriteIndented = true };
        string jsonString = JsonSerializer.Serialize(reservations, options);
        File.WriteAllText("reservations.json", jsonString);
    }
}