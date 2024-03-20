using System;

class Program
{
    static void Main()
    {
        Console.WriteLine(Testascii.text);
        Thread.Sleep(3000);
        Console.Clear();

        string answer = "";

        Console.WriteLine("Welcome to MegaBios!");

        do
        {
            Console.WriteLine("1. Show the movies\n2. Log in\n3. Show reservations\n4. Quit");
            answer = (Console.ReadLine() ?? "").ToLower();

            if (answer == "1" || answer == "show the movies" || answer == "movies")
            {
                Console.WriteLine("movies");
            }
            else if (answer == "2" || answer == "log in" || answer == "login")
            {
                Console.WriteLine("Log in");
            }
            else if (answer == "3" || answer == "show reservations" || answer == "reservations")
            {
                Console.WriteLine("Reservations");
            }
            else if (answer == "4" || answer == "quit" || answer == "q")
            {
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Wrong input");
            }
        }while(answer != "4" || answer != "q" || answer != "quit");
        Environment.Exit(0);
    }
}
