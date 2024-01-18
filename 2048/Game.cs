using System.Xml.Serialization;
class Game
{
    public GameState gameState;
    public Leaderboard leaderboard;
    public Game()
    {
        gameState = new GameState();
        leaderboard = Leaderboard.LoadLeaderboardFromFile();
    }

    public string Run(GameState importGameState, string gamemode = "classic")
    {
        gameState = importGameState;

        if (gameState.size == 0)
        {
            InitializeGame(gamemode);
        }

        while (true)
        {
            DrawBoard();

            if (gamemode != "free run" && IsTargetReached())
            {
                Console.WriteLine("Congratulations! You reached the target value of " + gameState.targetValue);
                return "win";
            }

            if (IsGameOver())
            {
                EndGame();
                return "lose";
            }

            if (ProcessInput() == "escape")
            {

                if (gameState.score > 0)
                {
                    gameState.Save();
                    askToSave();
                }
                else
                {
                    gameState = new GameState();
                    gameState.Save();
                }

                return "escape";
            }
        }
    }

    public void InitializeGame(string gamemode)
    {
        int board_size = 0;
        int targetValue = 0;

        if (gamemode == "custom")
        {
            (board_size, targetValue) = InitializeCustomGame();
        }
        else if (gamemode == "free run")
        {
            (board_size, targetValue) = InitializeFreeRunGame();
        }
        else if (gamemode == "classic")
        {
            board_size = 4;
            targetValue = 2048;
        }

        gameState.size = board_size;
        gameState.board = new int[board_size, board_size];
        gameState.targetValue = targetValue;
        gameState.score = 0;
        gameState.gamemode = gamemode;


        GenerateNewNumber();
        GenerateNewNumber();
    }

    public (int, int) InitializeCustomGame()
    {
        Console.Write("Enter the size of the board (e.g., 4 for 4x4): ");
        int size = int.Parse(Console.ReadLine());
        while (size < 2 || size > 10)
        {
            Console.Write("\rInvalid choice. Please enter a valid option. (between 2 and 10): ");
            size = int.Parse(Console.ReadLine());
        }


        Console.Write("Enter the game target (e.g. 2048): ");
        int target = int.Parse(Console.ReadLine());
        while (target < 1)
        {
            Console.Write("\rInvalid choice. Please enter a valid option.: ");
            size = int.Parse(Console.ReadLine());
        }


        return (size, target);
    }

    public (int, int) InitializeFreeRunGame()
    {
        Console.Write("Enter the size of the board (e.g., 4 for 4x4): ");
        return (int.Parse(Console.ReadLine()), -1);
    }

    public void ResumeGame()
    {
        gameState.Load();
    }

    public void DrawBoard()
    {
        Console.Clear();
        Console.WriteLine("\tBest Game Ever\n");

        for (int row = 0; row < gameState.size; row++)
        {
            for (int col = 0; col < gameState.size; col++)
            {
                Console.Write(gameState.board[row, col].ToString().PadLeft(6));
            }
            Console.WriteLine();
        }
        Console.WriteLine("\nScore: " + gameState.score);
    }

    public string ProcessInput()
    {
        ConsoleKeyInfo keyInfo = Console.ReadKey();
        switch (keyInfo.Key)
        {
            case ConsoleKey.UpArrow:
                Move(Direction.Up);
                return "move";
            case ConsoleKey.DownArrow:
                Move(Direction.Down);
                return "move";
            case ConsoleKey.LeftArrow:
                Move(Direction.Left);
                return "move";
            case ConsoleKey.RightArrow:
                Move(Direction.Right);
                return "move";
            case ConsoleKey.Escape:
                return "escape";
            default:
                return "move";
        }
    }

    public void Move(Direction direction)
    {
        bool moved = false;

        switch (direction)
        {
            case Direction.Up:
                for (int col = 0; col < gameState.size; col++)
                {
                    for (int row = 1; row < gameState.size; row++)
                    {
                        if (gameState.board[row, col] != 0)
                        {
                            int currentRow = row;
                            while (currentRow > 0 && (gameState.board[currentRow - 1, col] == 0 || gameState.board[currentRow - 1, col] == gameState.board[row, col]))
                            {
                                currentRow--;
                            }

                            if (currentRow != row)
                            {
                                if (gameState.board[currentRow, col] == gameState.board[row, col])
                                {
                                    Merge(currentRow, col, row, col);
                                }
                                else
                                {
                                    // Move the tile
                                    gameState.board[currentRow, col] = gameState.board[row, col];
                                    gameState.board[row, col] = 0;
                                }

                                moved = true;
                            }
                        }
                    }
                }
                break;

            case Direction.Down:
                for (int col = 0; col < gameState.size; col++)
                {
                    for (int row = gameState.size - 2; row >= 0; row--)
                    {
                        if (gameState.board[row, col] != 0)
                        {
                            int currentRow = row;
                            while (currentRow < gameState.size - 1 && (gameState.board[currentRow + 1, col] == 0 || gameState.board[currentRow + 1, col] == gameState.board[row, col]))
                            {
                                currentRow++;
                            }

                            if (currentRow != row)
                            {
                                if (gameState.board[currentRow, col] == gameState.board[row, col])
                                {
                                    Merge(currentRow, col, row, col);
                                }
                                else
                                {
                                    // Move the tile
                                    gameState.board[currentRow, col] = gameState.board[row, col];
                                    gameState.board[row, col] = 0;
                                }

                                moved = true;
                            }
                        }
                    }
                }
                break;

            case Direction.Left:
                for (int row = 0; row < gameState.size; row++)
                {
                    for (int col = 1; col < gameState.size; col++)
                    {
                        if (gameState.board[row, col] != 0)
                        {
                            int currentCol = col;
                            while (currentCol > 0 && (gameState.board[row, currentCol - 1] == 0 || gameState.board[row, currentCol - 1] == gameState.board[row, col]))
                            {
                                currentCol--;
                            }

                            if (currentCol != col)
                            {
                                if (gameState.board[row, currentCol] == gameState.board[row, col])
                                {
                                    Merge(row, currentCol, row, col);
                                }
                                else
                                {
                                    // Move the tile
                                    gameState.board[row, currentCol] = gameState.board[row, col];
                                    gameState.board[row, col] = 0;
                                }

                                moved = true;
                            }
                        }
                    }
                }
                break;

            case Direction.Right:
                for (int row = 0; row < gameState.size; row++)
                {
                    for (int col = gameState.size - 2; col >= 0; col--)
                    {
                        if (gameState.board[row, col] != 0)
                        {
                            int currentCol = col;
                            while (currentCol < gameState.size - 1 && (gameState.board[row, currentCol + 1] == 0 || gameState.board[row, currentCol + 1] == gameState.board[row, col]))
                            {
                                currentCol++;
                            }

                            if (currentCol != col)
                            {
                                if (gameState.board[row, currentCol] == gameState.board[row, col])
                                {
                                    Merge(row, currentCol, row, col);
                                }
                                else
                                {
                                    // Move the tile
                                    gameState.board[row, currentCol] = gameState.board[row, col];
                                    gameState.board[row, col] = 0;
                                }

                                moved = true;
                            }
                        }
                    }
                }
                break;

            default:
                break;
        }

        if (moved)
        {
            GenerateNewNumber();
        }
    }

    public void Merge(int row1, int col1, int row2, int col2)
    {
        // Implement the logic to merge two tiles and update the score
        gameState.board[row1, col1] *= 2;  // Double the value of the first tile
        gameState.board[row2, col2] = 0;  // Clear the value of the second tile
        gameState.score += gameState.board[row1, col1];  // Update the score
    }

    public bool CanMove()
    {
        // Implement the logic to check if there are any valid moves left

        for (int row = 0; row < gameState.size; row++)
        {
            for (int col = 0; col < gameState.size; col++)
            {
                if (gameState.board[row, col] == 0)
                {
                    return true;  // There is an empty cell, so a move is possible
                }

                // Check if adjacent tiles have the same value (can be merged)
                if (row < gameState.size - 1 && gameState.board[row, col] == gameState.board[row + 1, col])
                {
                    return true;
                }

                if (col < gameState.size - 1 && gameState.board[row, col] == gameState.board[row, col + 1])
                {
                    return true;
                }
            }
        }

        return false;  // No valid moves left
    }


    public void GenerateNewNumber()
    {
        // Find empty cells
        List<(int, int)> emptyCells = new List<(int, int)>();

        for (int row = 0; row < gameState.size; row++)
        {
            for (int col = 0; col < gameState.size; col++)
            {
                if (gameState.board[row, col] == 0)
                {
                    emptyCells.Add((row, col));
                }
            }
        }

        // Check if there are empty cells to place a new number
        if (emptyCells.Count > 0)
        {
            // Generate a new number (2 or 4)
            int newNumber = (new Random().Next(10) < 9) ? 2 : 4;

            // Choose a random empty cell
            (int row, int col) randomCell = emptyCells[new Random().Next(emptyCells.Count)];

            // Place the new number in the chosen cell
            gameState.board[randomCell.row, randomCell.col] = newNumber;
        }
    }

    public bool IsGameOver()
    {
        // Implement the logic to check if the game is over (no more possible moves)
        return !CanMove();
    }

    public void EndGame()
    {
        Console.WriteLine($"Game over! Your score: {gameState.score}");
        askToSave();
    }

    public void askToSave()
    {
        // Prompt the user to save their result
        int highestScore = leaderboard.Players.OrderByDescending(p => p.Score).FirstOrDefault().Score;
        if (gameState.score <= highestScore)
        {
            return;
        }

        Console.Write($"DDo you want to save your result ({gameState.score}) to the leaderboard? (y/n): ");
        string choice = Console.ReadLine();

        if (choice.ToLower() == "y")
        {
            Console.WriteLine("Enter your name: ");
            string playerName = Console.ReadLine();

            Player currentPlayer = new Player(playerName, gameState.score, gameState.size);
            leaderboard.AddPlayer(currentPlayer);
            Console.WriteLine("Result saved to the leaderboard.");

            leaderboard.SaveLeaderboardToFile();
        }
    }

    public bool IsTargetReached()
    {
        // Implement the logic to check if the target value is reached in classic mode

        for (int row = 0; row < gameState.size; row++)
        {
            for (int col = 0; col < gameState.size; col++)
            {
                if (gameState.board[row, col] == gameState.targetValue)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
}

[Serializable]
public class SerializableGameState
{
    public int score { get; set; }
    public int size { get; set; }
    public int targetValue { get; set; }
    public string gamemode { get; set; }
    public List<List<int>> board { get; set; }

    public SerializableGameState()
    {
        // Default constructor required for serialization
    }

    public SerializableGameState(GameState gameState)
    {
        score = gameState.score;
        size = gameState.size;
        targetValue = gameState.targetValue;
        gamemode = gameState.gamemode;

        // Convert int[,] to List<List<int>> for serialization
        board = new List<List<int>>();
        for (int i = 0; i < size; i++)
        {
            List<int> row = new List<int>();
            for (int j = 0; j < size; j++)
            {
                row.Add(gameState.board[i, j]);
            }
            board.Add(row);
        }
    }
}

[Serializable]
public class GameState
{
    public int score { get; set; }
    public int[,] board { get; set; }
    public int size { get; set; }
    public int targetValue { get; set; }
    public string gamemode { get; set; }

    public GameState()
    {

    }

    public void Save()
    {
        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SerializableGameState));

            using (StreamWriter writer = new StreamWriter("game_save.xml"))
            {
                var serializableGameState = new SerializableGameState(this);
                serializer.Serialize(writer, serializableGameState);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving game: {ex.Message}");
        }
    }

    public GameState Load()
    {
        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SerializableGameState));

            using (StreamReader reader = new StreamReader("game_save.xml"))
            {
                var serializableGameState = (SerializableGameState)serializer.Deserialize(reader);

                // Convert List<List<int>> to int[,] after deserialization
                int size = serializableGameState.size;
                int[,] loadedBoard = new int[size, size];
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        loadedBoard[i, j] = serializableGameState.board[i][j];
                    }
                }

                return new GameState
                {
                    score = serializableGameState.score,
                    size = serializableGameState.size,
                    targetValue = serializableGameState.targetValue,
                    gamemode = serializableGameState.gamemode,
                    board = loadedBoard
                };
            }
        }
        catch (FileNotFoundException)
        {
            return new GameState();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading game: {ex.Message}");
            return new GameState();
        }
    }
}
