
using System;

using System.Collections.Generic;

using System.IO;

using System.Text.Json;

using System;

using Newtonsoft.Json.Linq;

using System.IO;

using System.Security.Cryptography.X509Certificates;

using System.Text.Json;
using System.Text.Json.Serialization;

public class TheaterSeatingPrinter
{
    public static bool navigateGridBool;
    private static int userXPosition = 1;
    private static int userYPosition = 1;
    private static List<(int x, int y)> userPositions = new List<(int x, int y)>();

    public void PrintTheaterSeating(List<Schedule> schedules, int scheduleSerialNumber)
    {
        userPositions.Clear();
        try
        {
            var schedule = schedules.FirstOrDefault(s => s.SerialNumber == scheduleSerialNumber);

            if (schedule == null)
            {
                Console.WriteLine("Invalid schedule serial number.");
                return;
            }

            var seats = schedule.Seats;
            int rows = GetMaxRow(seats);
            int columns = GetMaxColumn(seats);
            bool running = true;

            while (running)
            {
                Console.Clear();
                PrintGridHall(rows, columns, seats);
                running = HandleUserInput(rows, columns, schedules, scheduleSerialNumber, this);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }
    }

    private int GetMaxRow(List<Seat> seats)
    {
        return seats.Max(s =>
        {
            string[] parts = s.ID.Split('-');
            return int.Parse(parts[1]);
        });
    }

    private int GetMaxColumn(List<Seat> seats)
    {
        return seats.Max(s =>
        {
            string[] parts = s.ID.Split('-');
            return int.Parse(parts[1]);
        });
    }

    public static void PrintGridHall(int rows, int columns, List<Seat> seats)
    {
        int totalWidth = columns * 4 + 5;
        string topBorder = " " + new string('_', totalWidth - 2) + " ";
        string emptyBorder = "|" + new string(' ', totalWidth - 2) + "|";

        AsciiArtPrinter.AsciiArtPrinterSelecteren();
        Console.WriteLine(topBorder);
        Console.WriteLine(emptyBorder);

        for (int i = 1; i <= rows; i++)
        {
            Console.Write("| " + (char)(i + 64) + " ");

            for (int j = 1; j <= columns; j++)
            {
                string seatId = i + "-" + j;
                var seat = seats.FirstOrDefault(s => s.ID == seatId);

                if (i == userYPosition && j == userXPosition)
                {
                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("@");
                    Console.ResetColor();
                    Console.Write("]");
                }
                else if (userPositions.Contains((j, i)))
                {
                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.Write("O");
                    Console.ResetColor();
                    Console.Write("]");
                }
                else if (seat != null && seat.IsAvailable)
                {
                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("O");
                    Console.ResetColor();
                    Console.Write("]");
                }
                else
                {
                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("X");
                    Console.ResetColor();
                    Console.Write("]");
                }

                if (j < columns)
                {
                    Console.Write(" ");
                }
            }

            Console.WriteLine(" |");
        }

        Console.WriteLine(emptyBorder);

        int padding = (totalWidth - 12) / 2;
        string filmdoekLine = "|" + new string(' ', padding) + "filmdoek" + new string(' ', totalWidth - 12 - padding) + "  |";
        double totalAmount = userPositions.Sum(pos => (pos.x == 1 || pos.x == columns) ? 6.99 : 8.99);

        Console.WriteLine(filmdoekLine);
        Console.WriteLine("|" + new string('_', totalWidth - 2) + "|");
        ProgramFunctions.PrintColoredText("Klik op ", ConsoleColor.White, "Enter", ConsoleColor.Magenta, " om een stoel te selecteren", ConsoleColor.White);
        ProgramFunctions.PrintColoredText("Klik op ", ConsoleColor.White, "Backspace", ConsoleColor.Magenta, " om een stoel te deselecteren", ConsoleColor.White);
        ProgramFunctions.PrintColoredText("Klik op ", ConsoleColor.White, "E", ConsoleColor.Magenta, $" om de stoel(en) te bevestigen (Totaalbedrag: {totalAmount:C})", ConsoleColor.White);
        ProgramFunctions.PrintColoredText("Klik op ", ConsoleColor.White, "Q", ConsoleColor.Magenta, " om naar het hoofdmenu terug te gaan", ConsoleColor.White);

        Console.WriteLine(new string('_', totalWidth));
        Console.WriteLine("Prijzen van de stoelen:");
        Console.WriteLine("Zitplaatsen aan de zijkanten: € 6.99");
        Console.WriteLine("Zitplaatsen in het midden: € 8.99");

    }




    private static bool HandleUserInput(int rows, int columns, List<Schedule> schedules, int scheduleSerialNumber, TheaterSeatingPrinter printer)
    {
        var keyInfo = Console.ReadKey(true);
        var schedule = schedules.FirstOrDefault(s => s.SerialNumber == scheduleSerialNumber);

        switch (keyInfo.Key)
        {
            case ConsoleKey.LeftArrow:
                if (userXPosition > 1) userXPosition--;
                break;

            case ConsoleKey.RightArrow:
                if (userXPosition < columns) userXPosition++;
                break;

            case ConsoleKey.UpArrow:
                if (userYPosition > 1) userYPosition--;
                break;

            case ConsoleKey.DownArrow:
                if (userYPosition < rows) userYPosition++;
                break;

            case ConsoleKey.Enter:
                var userPosition = (userXPosition, userYPosition);
                if (!userPositions.Contains(userPosition))
                {
                    if (schedule != null)
                    {
                        var seat = schedule.Seats.FirstOrDefault(s => s.ID == $"{userYPosition}-{userXPosition}");
                        if (seat != null && seat.IsAvailable)
                        {
                            userPositions.Add(userPosition);
                        }
                    }
                }
                break;

            case ConsoleKey.Backspace:
                if (userPositions.Count > 0)
                {
                    userPositions.RemoveAt(userPositions.Count - 1);
                }
                break;

            case ConsoleKey.E:
                if (userPositions.Count == 0 || !userPositions.Any(pos => schedule.Seats.Any(s => s.ID == $"{pos.y}-{pos.x}" && s.IsAvailable)))
                {
                    ProgramFunctions.PrintTextCentered("Geen stoelen geselecteerd of geselecteerde stoelen zijn niet beschikbaar\nSelecteer beschikbare stoelen voordat u door kunt gaan naar de betaling"); Console.WriteLine("");
                    ProgramFunctions.PrintColoredTextCentered("Druk op een ", ConsoleColor.White, "knop", ConsoleColor.Magenta, " om verder te gaan", ConsoleColor.White);
                    Console.ReadKey();
                    Console.Clear();
                    Console.WriteLine("\x1b[3J");
                    break;
                }

                double seatPrice = printer.ConvertTupleListToJson(schedules, scheduleSerialNumber);
                Console.Clear();
                Payment.AddSeatPrice(seatPrice);
                Payment.AddSelectedSeats(userPositions);
                Payment.scheduleSerialNumber = scheduleSerialNumber;
                Payment.BestelMenu();
                break;

            case ConsoleKey.Q:
                Console.Clear();
                Console.WriteLine("\x1b[3J");

                return false;
        }

        return true;
    }

    public double ConvertTupleListToJson(List<Schedule> schedules, int scheduleSerialNumber)
    {
        var schedule = schedules.FirstOrDefault(s => s.SerialNumber == scheduleSerialNumber);
        if (schedule == null)
        {
            return 0;
        }

        double GeldteBetalen = 0;
        foreach (var position in userPositions)
        {
            foreach (var seat in schedule.Seats)
            {
                string[] parts = seat.ID.Split('-');
                int seatRow = int.Parse(parts[1]);
                int seatColumn = int.Parse(parts[0]);

                if (position.x == seatRow && position.y == seatColumn && seat.IsAvailable)
                {
                    GeldteBetalen += seat.Price;
                    seat.IsAvailable = false;
                }
            }
        }

        JsonSerializerOptions options = new() { WriteIndented = true };
        string jsonString = JsonSerializer.Serialize(schedules, options);
        File.WriteAllText("schedule.json", jsonString);

        // Add selected seats before clearing the list
        Payment.AddSelectedSeats(userPositions);

        userPositions.Clear();
        return GeldteBetalen;
    }
}
