using System;

class Program
{
    static void Main()
    {
    Console.WriteLine(Testascii.text);
    Thread.Sleep(3000);
    Console.Clear();

    Console.WriteLine("Welcome to MegaBios!");

    string answer;
        do
        {
            Console.WriteLine("1. Show the movies\n2. Log in\n3. Show reservations\n4. Quit");
            answer = (Console.ReadLine() ?? "").ToLower();

            switch (answer)
            {
                case "1":
                case "show the movies":
                case "movies":
                    Console.WriteLine("movies");
                    break;

                case "2":
                case "log in":
                case "login":
                    Console.WriteLine("Log in");
                    break;

                case "3":
                case "show reservations":
                case "reservations":
                    Console.WriteLine("Reservations");
                    break;

                case "4":
                case "quit":
                case "q":
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("Wrong input");
                    break;
            }
        } while (answer != "4" && answer != "q" && answer != "quit");

        Environment.Exit(0);
    }
}