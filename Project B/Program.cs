using System;

class Program
{
    static void Main()
    {
        Testascii test = new Testascii();

        test.PrintMovies("Movies.json");

        Console.WriteLine(Testascii.text);
    }
}
