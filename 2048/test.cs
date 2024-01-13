using System;

class Test
{
    static int[,] board;
    static int score;
    static int size;
    static int targetValue = 2048;

    static void Main()
    {
        InitializeGame();

        while (true)
        {
            DrawBoard();

            if (IsTargetReached())
            {
                Console.WriteLine("Congratulations! You reached the target value of " + targetValue);
                break;
            }

            if (IsGameOver())
            {
                Console.WriteLine("Game Over! Your score: " + score);
                break;
            }

            ProcessInput();
        }
    }

    static void InitializeGame()
    {
        Console.WriteLine("2048 Console Game");

        Console.Write("Enter the size of the board (e.g., 4 for 4x4): ");
        size = int.Parse(Console.ReadLine());

        board = new int[size, size];
        score = 0;

        GenerateNewNumber();
        GenerateNewNumber();
    }

    static void DrawBoard()
    {
        Console.Clear();
        Console.WriteLine("\tBest Game Ever\n");

        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                Console.Write(board[row, col].ToString().PadLeft(6));
            }
            Console.WriteLine();
        }
        Console.WriteLine("\nScore: " + score);
    }

    static void ProcessInput()
    {
        ConsoleKeyInfo keyInfo = Console.ReadKey();
        Console.WriteLine(); // Move to the next line after reading the key

        switch (keyInfo.Key)
        {
            case ConsoleKey.UpArrow:
                Move(Direction.Up);
                break;
            case ConsoleKey.DownArrow:
                Move(Direction.Down);
                break;
            case ConsoleKey.LeftArrow:
                Move(Direction.Left);
                break;
            case ConsoleKey.RightArrow:
                Move(Direction.Right);
                break;
        }
    }

    static void Move(Direction direction)
    {
        bool moved = false;

        switch (direction)
        {
            case Direction.Up:
                for (int col = 0; col < size; col++)
                {
                    for (int row = 1; row < size; row++)
                    {
                        if (board[row, col] != 0)
                        {
                            int currentRow = row;
                            while (currentRow > 0 && (board[currentRow - 1, col] == 0 || board[currentRow - 1, col] == board[row, col]))
                            {
                                currentRow--;
                            }

                            if (currentRow != row)
                            {
                                if (board[currentRow, col] == board[row, col])
                                {
                                    Merge(currentRow, col, row, col);
                                }
                                else
                                {
                                    // Move the tile
                                    board[currentRow, col] = board[row, col];
                                    board[row, col] = 0;
                                }

                                moved = true;
                            }
                        }
                    }
                }
                break;

            case Direction.Down:
                for (int col = 0; col < size; col++)
                {
                    for (int row = size - 2; row >= 0; row--)
                    {
                        if (board[row, col] != 0)
                        {
                            int currentRow = row;
                            while (currentRow < size - 1 && (board[currentRow + 1, col] == 0 || board[currentRow + 1, col] == board[row, col]))
                            {
                                currentRow++;
                            }

                            if (currentRow != row)
                            {
                                if (board[currentRow, col] == board[row, col])
                                {
                                    Merge(currentRow, col, row, col);
                                }
                                else
                                {
                                    // Move the tile
                                    board[currentRow, col] = board[row, col];
                                    board[row, col] = 0;
                                }

                                moved = true;
                            }
                        }
                    }
                }
                break;

            case Direction.Left:
                for (int row = 0; row < size; row++)
                {
                    for (int col = 1; col < size; col++)
                    {
                        if (board[row, col] != 0)
                        {
                            int currentCol = col;
                            while (currentCol > 0 && (board[row, currentCol - 1] == 0 || board[row, currentCol - 1] == board[row, col]))
                            {
                                currentCol--;
                            }

                            if (currentCol != col)
                            {
                                if (board[row, currentCol] == board[row, col])
                                {
                                    Merge(row, currentCol, row, col);
                                }
                                else
                                {
                                    // Move the tile
                                    board[row, currentCol] = board[row, col];
                                    board[row, col] = 0;
                                }

                                moved = true;
                            }
                        }
                    }
                }
                break;

            case Direction.Right:
                for (int row = 0; row < size; row++)
                {
                    for (int col = size - 2; col >= 0; col--)
                    {
                        if (board[row, col] != 0)
                        {
                            int currentCol = col;
                            while (currentCol < size - 1 && (board[row, currentCol + 1] == 0 || board[row, currentCol + 1] == board[row, col]))
                            {
                                currentCol++;
                            }

                            if (currentCol != col)
                            {
                                if (board[row, currentCol] == board[row, col])
                                {
                                    Merge(row, currentCol, row, col);
                                }
                                else
                                {
                                    // Move the tile
                                    board[row, currentCol] = board[row, col];
                                    board[row, col] = 0;
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


    static void Merge(int row1, int col1, int row2, int col2)
    {
        // Implement the logic to merge two tiles and update the score
        board[row1, col1] *= 2;  // Double the value of the first tile
        board[row2, col2] = 0;  // Clear the value of the second tile
        score += board[row1, col1];  // Update the score
    }


    static bool CanMove()
    {
        // Implement the logic to check if there are any valid moves left

        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                if (board[row, col] == 0)
                {
                    return true;  // There is an empty cell, so a move is possible
                }

                // Check if adjacent tiles have the same value (can be merged)
                if (row < size - 1 && board[row, col] == board[row + 1, col])
                {
                    return true;
                }

                if (col < size - 1 && board[row, col] == board[row, col + 1])
                {
                    return true;
                }
            }
        }

        return false;  // No valid moves left
    }


    static void GenerateNewNumber()
    {
        // Find empty cells
        List<(int, int)> emptyCells = new List<(int, int)>();

        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                if (board[row, col] == 0)
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
            board[randomCell.row, randomCell.col] = newNumber;
        }
    }

    static bool IsGameOver()
    {
        // Implement the logic to check if the game is over (no more possible moves)
        return !CanMove();
    }

    static bool IsTargetReached()
    {
        // Implement the logic to check if the target value is reached in classic mode

        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                if (board[row, col] == targetValue)
                {
                    return true;
                }
            }
        }

        return false;
    }


    enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
}
