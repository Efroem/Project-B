using System;

public class TestPosters
{

    public static void MegaBioscoop()
    {
        string Asciiartstart = @"
                
       __          __  _ _                     ____  _ _            
       \ \        / / | | |                   |  _ \(_|_)           
        \ \  /\  / /__| | | _____  _ __ ___   | |_) |_ _            
         \ \/  \/ / _ \ | |/ / _ \| '_ ` _ \  |  _ <| | |           
          \  /\  /  __/ |   < (_) | | | | | | | |_) | | |           
           \/  \/ \___|_|_|\_\___/|_| |_| |_| |____/|_| |           
                                                     _| |
                                                    |__ /
  __  __                    ____                        
 |  \/  |                  |  _ \(_)                           
 | \  / | ___  __ _  __ _  | |_) |_  ___  ___  ___ ___  ___  _ __  
 | |\/| |/ _ \/ _` |/ _` | |  _ <| |/ _ \/ __|/ __/ _ \ / _ \| '_ \ 
 | |  | |  __/ (_| | (_| | | |_) | | (_) \__ \ (_| (_) | (_) | |_) |
 |_|  |_|\___|\__, |\__,_| |____/|_|\___/|___/\___\___/ \___/| .__/ 
               __/ |                                         | |    
              |___/                                          |_|    

    ";
        string centeredAsciiArt = "                    " + Asciiartstart.Replace("\n", "\n                  ");

        Console.WriteLine(centeredAsciiArt);
    }

    public static string text2 = @"
                                      THE TERMINATOR 1984
                                          <((((((\\\
                                          /      . }\
                                          ;--..--._|}
                       (\                 '--/\--'  )
                        \\                | '-'  :'|
                         \\               . -==- .-|
                          \\               \.__.'   \--._
                          [\\          __.--|       //  _/'--.
                          \ \\       .'-._ ('-----'/ __/      \
                           \ \\     /   __>|      | '--.       |
                            \ \\   |   \   |     /    /       /
                             \ '\ /     \  |     |  _/       /
                              \  \       \ |     | /        /
                               \  \      \        /
        ";
    public static string text3 = @"   
                            STAR WARS THE FORCE AWAKENS 2015                                                               
                    ___________________________________________________
                    |                                                 |                    
                    |                  __                             |
                    |  .-.__      \ .-.  ___  __                      |            
                    | |_|  '--.-.-(   \/\;;\_\.-._______.-.           |
                    |  (-)___     \ \ .-\ \;;\(   \       \ \         |
                    |   Y    '---._\_((Q)) \;;\\ .-\     __(_)        |
                    |   I           __'-' / .--.((Q))---'    \,       |
                    |   I     ___.-:    \|  |   \'-'_          \      |
                    |   A  .-'      \ .-.\   \   \ \ '--.__     '\    |
                    |   |  |____.----((Q))\   \__|--\_      \     '   |
                    |      ( )        '-'  \_  :  \-' '--.___\        |
                    |       Y                \  \  \       \(_)       |
                    |       I                 \  \  \         \,      |
                    |       I                  \  \  \          \     |
                    |       A                   \  \  \          '\   |
                    |       |                    \  \__|           '  |
                    |                             \_:.  \             |
                    |                               \ \  \            |
                    |                                \ \  \           |
                    |                                 \_\_|           |
                    |_________________________________________________|
";
    //     public static void PrintAsciiMenu()
    //     {
    //         string menuText = @"
    //                     ┌─────────────────────────┐
    //                     │          Menu:          │
    //                     ├─────────────────────────┤
    //                     │ 1. Bekijk films         │
    //                     │ 2. Inloggen             │
    //                     │ 3. Bekijk reserveringen │
    //                     | 4. Bekijk schema        |
    //                     │ 5. Verlaat pagina       │
    //                     │ 6. Lijst zalen          │
    //                     │ 7. CinemaHall toevoegen │
    //                     └─────────────────────────┘
    // ";
    //         string centeredAsciiArt = "                    " + menuText.Replace("\n", "\n                  ");
    //         Console.WriteLine(centeredAsciiArt);
    //     }

    //     public static void PrintAsciiMenu2()
    //     {
    //         string menuText2 = @"
    //                     ┌─────────────────────────┐
    //                     │          Menu:          │
    //                     ├─────────────────────────┤
    //                     │ 1. Bekijk films         │
    //                     │ 2. Profiel bekijken     │
    //                     │ 3. Bekijk reserveringen │
    //                     | 4. Bekijk schema        |
    //                     │ 5. Verlaat pagina       │
    //                     │ 6. Lijst zalen          │
    //                     │ 7. CinemaHall toevoegen │
    //                     └─────────────────────────┘
    //         ";
    //         string centeredAsciiArt = "                    " + menuText2.Replace("\n", "\n                  ");
    //         Console.WriteLine(centeredAsciiArt);
    //     }
}
