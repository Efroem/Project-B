using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

public class Testascii
{
    public static string text = "";

    public void PrintMovies(string jsonFilePath)
    {
        if (jsonFilePath == null)
            throw new ArgumentNullException(nameof(jsonFilePath), "JSON file path cannot be null.");

        string jsonString = File.ReadAllText(jsonFilePath);

        List<Movie>? movies = JsonSerializer.Deserialize<List<Movie>>(jsonString);

        if (movies == null)
            throw new InvalidOperationException("Deserialization failed. No movies found.");

        List<string> popularMovieTitles = movies.Take(5).Select(movie => movie.Title).ToList();

        List<string> recentlyAddedMovieTitles = movies.Skip(5).Select(movie => movie.Title).ToList();

        text = @"
    ___  ___                  ______ _                                 
    |  \/  |                  | ___ (_)                                
    | .  . | ___  __ _  __ _  | |_/ /_  ___  ___  ___ ___   ___  _ __  
    | |\/| |/ _ \/ _` |/ _` | | ___ \ |/ _ \/ __|/ __/ _ \ / _ \| '_ \ 
    | |  | |  __/ (_| | (_| | | |_/ / | (_) \__ \ (_| (_) | (_) | |_) |
    \_|  |_/\___|\__, |\__,_| \____/|_|\___/|___/\___\___/ \___/| .__/ 
                __/ |                                         | |    
                |___/                                         |_|   
                                                                
    _____________________________________________________________
    |                                                               |
    |  Popular Movies to Watch:                                     |
    |  ----------------------------------------------               |";

        for (int i = 0; i < popularMovieTitles.Count; i++)
        {
            text += $"    |  | {i + 1}. {popularMovieTitles[i],-30} |";
            if (i < popularMovieTitles.Count - 1)
                text += "\n";
        }

        text += @"
    |  ----------------------------------------------               |
    |                                                               |
    |  Recently Added:                                               |
    |   ---------------------------------------                       |";

        for (int i = 0; i < recentlyAddedMovieTitles.Count; i++)
        {
            text += $"   |   | {i + 1}. {recentlyAddedMovieTitles[i],-30} |";
            if (i < recentlyAddedMovieTitles.Count - 1)
                text += "\n";
        }

        text += @"
    |   ---------------------------------------                       |
    |_______________________________________________________________|";
    }
}

public class Movie
{
    public string Title { get; set; } = "";
}


//public static string text = @"
    // ___  ___                  ______ _                                 
    // |  \/  |                  | ___ (_)                                
    // | .  . | ___  __ _  __ _  | |_/ /_  ___  ___  ___ ___   ___  _ __  
    // | |\/| |/ _ \/ _` |/ _` | | ___ \ |/ _ \/ __|/ __/ _ \ / _ \| '_ \ 
    // | |  | |  __/ (_| | (_| | | |_/ / | (_) \__ \ (_| (_) | (_) | |_) |
    // \_|  |_/\___|\__, |\__,_| \____/|_|\___/|___/\___\___/ \___/| .__/ 
    //             __/ |                                         | |    
    //             |___/                                         |_|   
                

    //                                                     _____________________________________________________________
    //                                                 |                                                               |
    //                                                 |  Popular Movies to Watch:                                     |
    //                                                 |  ----------------------------------------------               |
    //                                                 |  | 1. " + movie + @"                         |                |
    //                                                 |  | 2. The Crown                              |                |
    //                                                 |  | 3. Money Heist                            |                |
    //                                                 |  | 4. Black Mirror                           |                |
    //                                                 |  | 5. The Witcher                            |                |
    //                                                 |  | ... and more!                             |                |
    //                                                 |  ----------------------------------------------               |
    //                                                 |______________________________________________________________ |
                                                                
                
    //              _____________________________________________________________
    //             |                                                             |
    //             |               Recently Added:                               |
    //             |   ---------------------------------------                   |
    //             |   | 1. Lupin                          |                     |
    //             |   | 2. Bridgerton                     |                     |
    //             |   | 3. Cobra Kai                      |                     |
    //             |   | 4. The Queen's Gambit             |                     |
    //             |   | 5. The Irishman                   |                     |
    //             |   ---------------------------------------                   |
    //             |_____________________________________________________________|
    //             ";




