using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Security;

class Program
{
    //Constants
    const string PLAYERS_FILE = "players.txt";
    const string POINTS_FILE = "points.txt";
    const string HISTORY_FILE = "history.txt";
    const int MAX_POINTS = 20;
    
    static void Main()
    {
    string projectFolder = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
    Directory.SetCurrentDirectory(projectFolder);
    string[] players = File.ReadAllLines(PLAYERS_FILE);
    string[] points = File.ReadAllLines(POINTS_FILE);
    string[] history = File.ReadAllLines(HISTORY_FILE);

    Console.WriteLine("=== GAME NIGHT LEADERBOARD ===");

    if (history.Length == 0)
        {
            Console.WriteLine("No nights played yet.");
        
        }
        else
        {
            Console.WriteLine("Nights played: " + history.Length);
            foreach (string entry in history)
            {
                Console.WriteLine(" " + entry);
            }
        }
        int tonightNumber = history.Length + 1;
        Console.WriteLine("Tonight is Night " +tonightNumber + ".");
    //Display the player standings.
    Console.WriteLine("PLAYER".PadRight(11) + "POINTS");

    for (int i = 0; i < players.Length; i++)
        {
            Console.WriteLine(players[i].PadRight(11) + points [i]);
        }
        int leader = LeaderIndex(points);
        Console.WriteLine("Leader: " + players[leader] + " with " + points[leader]);
        int seasonTotal = TotalPoints(points);
        Console.WriteLine ("Season total: " + seasonTotal);
        int bestTonight = -1;
        string winnerTonight = "";
        //Collect tonight's points from each player
        for (int i = 0; i < players.Length; i++)
        {
            int tonight =-1;
            while (tonight < 0 || tonight > MAX_POINTS)
            {
                Console.Write("How many points did " + players[i] + " earn tonight (0-20)? ");
                tonight = Convert.ToInt32(Console.ReadLine());
            }
            int season = Convert.ToInt32(points[i]) + tonight;
            points[i] = season.ToString();
            if (tonight > bestTonight)
            {
                bestTonight = tonight;
                winnerTonight = players[i];
            }
        }
        //Saves the updated season points.
        File.WriteAllLines(POINTS_FILE, points);
        Console.WriteLine("Tonight's winner: " + winnerTonight + " with " + bestTonight + " points");
        Console.WriteLine("Season points saved!");

        string[] newEntry = {HistoryEntry(tonightNumber, winnerTonight, bestTonight) };
        File.AppendAllLines(HISTORY_FILE, newEntry);
        Console.WriteLine("Added to history.txt: " + newEntry[0]);
    }
    //Returns the index of player with the highest score
    static int LeaderIndex(string[] points)
    {
        int leader = 0;
        for (int i = 1; i < points.Length; i++)
        {
            if (Convert.ToInt32(points[i]) > Convert.ToInt32(points[leader]))
            {
                leader = i;
            }
        }
        return leader;
        
    }
    //Returns total points earned by all players
    static int TotalPoints (string[] points)
    {
        int total = 0;
        foreach (string score in points)
        { 
        total += Convert.ToInt32(score);
        } 
        return total;
    }
//Returns one formatted history line.
static string HistoryEntry(int night, string winner, int points)
    {
        return "Night " + night + ": " + winner + " won with " + points + " points";
    }
}