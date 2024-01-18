using System.Reflection.Metadata;
using System.Xml.Serialization;

[Serializable]
public class Player
{
    public string Name { get; set; }
    public int Board_size { get; set; }
    public int Score { get; set; }

    public Player()
    {
        // Default constructor required for XML serialization
    }

    public Player(string name, int score, int board_size)
    {
        Name = name;
        Score = score;
        Board_size = board_size;
    }

    public override string ToString()
    {
        return $"{Name}: {Score}, {Board_size}";
    }
}

[Serializable]
public class Leaderboard
{
    public List<Player> Players { get; set; }

    public Leaderboard()
    {
        Players = new List<Player>();
    }

    public void AddPlayer(Player player)
    {
        Players.Add(player);
    }

    private string CenterText(string text, int width)
    {
        int padding = (width - text.Length) / 2;
        return $"{new string(' ', padding)}{text}{new string(' ', width - text.Length - padding)}";
    }

    public void DisplayLeaderboard()
    {
        bool exitProgram = false;
        int tableWidth = 45;
        do
        {
            Console.Clear();
            Console.WriteLine("Leaderboard:");
            Console.WriteLine(new string('-', tableWidth));
            Console.WriteLine("|         Name         | Score | Board size |");
            Console.WriteLine(new string('-', tableWidth));

            foreach (var player in Players.OrderByDescending(p => p.Score))
            {
                Console.WriteLine($"| {CenterText(player.Name, 20)} | {CenterText(player.Score.ToString(), 5)} | {CenterText(player.Board_size.ToString(), 10)} |");
                Console.WriteLine(new string('-', tableWidth));
            }

            Console.WriteLine("\nPress ESC to exit");
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            if (keyInfo.Key == ConsoleKey.Escape)
            {
                exitProgram = true;
            }

        } while (!exitProgram);
    }

    public void SaveLeaderboardToFile()
    {
        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Leaderboard));

            using (StreamWriter writer = new StreamWriter("leaderboard.xml"))
            {
                serializer.Serialize(writer, this);
                Console.WriteLine("Leaderboard saved successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving leaderboard: {ex.Message}");
        }
    }

    public static Leaderboard LoadLeaderboardFromFile()
    {
        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Leaderboard));

            using (StreamReader reader = new StreamReader("leaderboard.xml"))
            {
                return (Leaderboard)serializer.Deserialize(reader);
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("No leaderboard file found. Creating a new one.");
            return new Leaderboard();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading leaderboard: {ex.Message}");
            return new Leaderboard();
        }
    }
}