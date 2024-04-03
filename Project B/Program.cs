using System;

class Program
{
    static void Main()
    {
    // Testascii asciiArt = new Testascii();
    // asciiArt.PrintMovies("movies.json");
    // string posters1 = Testascii.text;
    // Console.WriteLine(posters1);
     string allposters = TestPosters.text1 + TestPosters.text2 + TestPosters.text3;
     System.Console.WriteLine(allposters);
     Console.WriteLine("Welkom bij MegaBios!");

    string answer;
        do
        {
            if (Authentication.User == null)
                Console.WriteLine("1. Bekijk films\n2. Inloggen\n3. Bekijk reserveringen\n4. Verlaat pagina");
            else
                Console.WriteLine("1. Bekijk films\n2. profiel\n3. Bekijk reserveringen\n4. Verlaat pagina");
            answer = (Console.ReadLine() ?? "").ToLower();

            switch (answer)
            {
                case "1":
                case "Bekijk films":
                case "bekijk films":
                case "movies":
                    Console.WriteLine("movies");
                    break;

                case "2":
                case "Inloggen":
                case "inloggen":
                case "profiel":
                    Console.WriteLine("Log in");
                    if (Authentication.User == null)
                        Authentication.Start();
                    else
                        Authentication.ViewProfile();
                    break;

                case "3":
                case "Bekijk reserveringen":
                case "bekijk reserveringen":
                case "bekijk Reserveringen":
                case "reserveringen":
                    Console.WriteLine("Reserveringen");
                    break;

                case "4":
                case "Verlaat pagina":
                case "verlaat pagina":
                case "q":
                    Console.WriteLine("Tot ziens!");
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("Ongeldige invoer");
                    break;
            }
        } while (answer != "4" && answer != "verlaat pagina" && answer != "Verlaat pagina");

        Environment.Exit(0);
    }
}