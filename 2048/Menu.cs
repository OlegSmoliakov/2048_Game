class Menu
{
    private static Game game = new Game();
    private static GameState gameState;
    static void Main()
    {
        bool exitProgram = false;
        ShowLoader("Loading the game...", 1);

        do
        {
            gameState = game.gameState.Load();

            if (gameState.size != 0)
            {
                exitProgram = DisplayFullMenu();
            }
            else
            {
                exitProgram = DisplayStartMenu();
            }

            Console.WriteLine(); // Add a new line for better formatting

        } while (!exitProgram);
    }

    static bool DisplayFullMenu()
    {
        Console.Clear();
        Console.WriteLine(" Menu:");
        Console.WriteLine("1. Start New Classic Game");
        Console.WriteLine("2. Resume Game");
        Console.WriteLine("3. Start New Custom Game");
        Console.WriteLine("4. Leaderboard");
        Console.WriteLine("5. Exit");

        Console.Write("Enter your choice (1-5): ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                StartClassicGame();
                return false;

            case "2":
                ResumeGame();
                return false;

            case "3":
                StartCustomGame();
                return false;

            case "4":
                ShowLeaderboard();
                return false;

            case "5":
                return true;

            default:
                Console.WriteLine("Invalid choice. Please enter a valid option.");
                return false;
        }
    }

    static bool DisplayStartMenu()
    {
        Console.Clear();
        Console.WriteLine(" Menu:");
        Console.WriteLine("1. Start New Classic Game");
        Console.WriteLine("2. Start New Custom Game");
        Console.WriteLine("3. Leaderboard");
        Console.WriteLine("4. Exit");

        Console.Write("Enter your choice (1-4): ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                StartClassicGame();
                return false;

            case "2":
                StartCustomGame();
                return false;

            case "3":
                ShowLeaderboard();
                return false;

            case "4":
                return true;

            default:
                Console.WriteLine("Invalid choice. Please enter a valid option.");
                return false;
        }
    }

    static void StartClassicGame()
    {
        Console.WriteLine("Starting Classic Game...");
        game.Run(new GameState());
    }

    static void ResumeGame()
    {
        Console.WriteLine("Resuming Game...");
        game.Run(gameState);
    }

    static void StartCustomGame()
    {
        Console.Clear();
        Console.WriteLine("Starting Custom Game...");
        game.Run(new GameState(), "custom");
    }

    static void ShowLeaderboard()
    {
        Console.WriteLine("Leaderboard...");
        game.leaderboard.DisplayLeaderboard();
    }

    public static void ShowLoader(string message, int delay)
    {
        Console.Clear();
        ShowLogo();
        Console.WriteLine(message);
        // Environment.Exit(0);

        int steps = 100;
        delay *= 1000;
        delay /= steps;

        // Имитация процесса загрузки
        using (var progress = new ProgressBar())
        {
            for (int i = 0; i <= steps; i++)
            {
                progress.Report((double)i / steps);
                Thread.Sleep(delay);
            }
        }
    }

    static void ShowLogo()
    {
        string filePath = @"D:\Users\user\Desktop\Study\C#\Coursework\Code\coursework_c-\2048\banner.txt";

        try
        {
            string fileContent = File.ReadAllText(filePath);
            Console.WriteLine(fileContent);
        }
        catch (IOException e)
        {
            Console.WriteLine($"An error occurred while reading the file: {e.Message}");
        }
    }
}
