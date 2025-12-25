using System;
using System.IO;

class Program
{
    static void Main()
    {
        Console.Write("Enter name for Player 1 (X): ");
        string name1 = Console.ReadLine();

        Console.Write("Enter name for Player 2 (O): ");
        string name2 = Console.ReadLine();

        Player p1 = new Player(name1, 'X');
        Player p2 = new Player(name2, 'O');

        Game game = new Game(p1, p2);
        game.Start();

        Console.WriteLine("Game over. Press Enter to exit.");
        Console.ReadLine();
    }
}

class Player
{
    public string Name { get; }
    public char Symbol { get; }

    public Player(string name, char symbol)
    {
        Name = name;
        Symbol = symbol;
    }
}

class Game
{
    private Player player1;
    private Player player2;
    private Board board;
    private string historyFile = "GameHistory.txt";

    public Game(Player p1, Player p2)
    {
        player1 = p1;
        player2 = p2;
        board = new Board();
    }

    public void Start()
    {
        Player current = player1;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("TIC-TAC-TOE");
            board.Display();

            Console.Write($"{current.Name} ({current.Symbol}), choose position (1-9): ");
            string input = Console.ReadLine();

            if (!int.TryParse(input, out int pos) ||
                !board.MakeMove(pos - 1, current.Symbol))
            {
                Console.WriteLine("Invalid move. Press Enter.");
                Console.ReadLine();
                continue;
            }

            if (board.CheckWin(current.Symbol))
            {
                Console.Clear();
                board.Display();
                Console.WriteLine($"{current.Name} wins!");
                SaveResult($"{current.Name} won");
                break;
            }

            if (board.IsFull())
            {
                Console.Clear();
                board.Display();
                Console.WriteLine("Draw!");
                SaveResult("Draw");
                break;
            }

            current = current == player1 ? player2 : player1;
        }
    }

    private void SaveResult(string result)
    {
        File.AppendAllText(
            historyFile,
            $"{DateTime.Now}: {result}{Environment.NewLine}"
        );
    }
}

class Board
{
    private char[] cells = new char[9];

    public Board()
    {
        for (int i = 0; i < 9; i++)
            cells[i] = ' ';
    }

    public void Display()
    {
        Console.WriteLine();
        for (int i = 0; i < 9; i += 3)
        {
            Console.WriteLine($" {cells[i]} | {cells[i + 1]} | {cells[i + 2]} ");
            if (i < 6)
                Console.WriteLine("---+---+---");
        }
        Console.WriteLine();
    }

    public bool MakeMove(int position, char symbol)
    {
        if (position < 0 || position > 8 || cells[position] != ' ')
            return false;

        cells[position] = symbol;
        return true;
    }

    public bool CheckWin(char s)
    {
        int[,] winPatterns =
        {
            {0,1,2},{3,4,5},{6,7,8},
            {0,3,6},{1,4,7},{2,5,8},
            {0,4,8},{2,4,6}
        };

        for (int i = 0; i < 8; i++)
        {
            if (cells[winPatterns[i,0]] == s &&
                cells[winPatterns[i,1]] == s &&
                cells[winPatterns[i,2]] == s)
                return true;
        }

        return false;
    }

    public bool IsFull()
    {
        foreach (char c in cells)
            if (c == ' ')
                return false;

        return true;
    }
}
