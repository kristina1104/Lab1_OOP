using System;
using System.Collections.Generic;

namespace lab1
{ 
    public class GameAccount // використовується логіка для обмеження рейтингу в межах від 1 до 10
{
    public string UserName { get; set; }
    private int currentRating;
    public int CurrentRating
    {
        get { return currentRating; }
        set
        {
            if (value < 1)
            {
                throw new ArgumentException("Rating cannot be less than 1");
            }
            currentRating = value;
        }
    }
    public int GamesCount { get; set; } // визначає кількість ігор, в яких взяв участь гравець

    private List <GameHistory> gamesHistory;

    public GameAccount(string userName, int initialRating)
    {
        UserName = userName;
        CurrentRating = initialRating;
        GamesCount = 0;
        gamesHistory = new List<GameHistory>();
    }

    public void WinGame(string opponentName, int rating) // метод, що викликається при виграші гравцем гри
    {
        if (rating < 1)
        {
            throw new ArgumentException("Rating cannot be less than 1");
        }

        CurrentRating = Math.Min(CurrentRating + rating, 10);

        GamesCount++;
        gamesHistory.Add(new GameHistory(opponentName, true, rating, GamesCount));
    }

    public void LoseGame(string opponentName, int rating) // метод, що викликається при програші гравцем гри
    {
        if (rating < 1)
        {
            throw new ArgumentException("Rating cannot be less than 1");
        }

        CurrentRating = Math.Max(CurrentRating - rating, 1);

        GamesCount++;
        gamesHistory.Add(new GameHistory(opponentName, false, rating, GamesCount));
    }

    public string GetStats()
    {
        string result = $"Player: {UserName}, Rating: {CurrentRating}\n";
        result += $"{"GameIndex",10}{"OpponentName",15}{"Outcome",10}{"Rating",10}\n";

        if (gamesHistory != null)
        {
            foreach (var game in gamesHistory)
            {
                result += $"{game.GameIndex,10}{game.OpponentName,15}{(game.Won ? "Victory" : "Defeat"),10}{game.Rating,10}\n";
            }
        }

        return result;
    }

     public class GameHistory // вкладений приватний клас для зберігання історії ігор
    {
        public string OpponentName { get; }
        public int Rating { get; }
        public int GameIndex { get; }
        public bool Won { get; }

        public GameHistory(string opponentName, bool won, int rating, int gameIndex)
        {
            OpponentName = opponentName;
            Won = won;
            Rating = rating;
            GameIndex = gameIndex;
        }
    }
}

public class Game
{
    public Guid GameId { get; set; }

    public Game()
    {
        GameId = Guid.NewGuid(); // Генерація нового унікального ідентифікатора
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        GameAccount player1 = new GameAccount("Player1", 1000);
        GameAccount player2 = new GameAccount("Player2", 1200);

        Game game1 = new Game();
        // Імітація ігор та оновлення статистики гравців
        player1.WinGame("Player2", 1700);
        player2.LoseGame("Player1", 1200);

        Game game2 = new Game();
        player1.LoseGame("Player2", 1400);
        player2.WinGame("Player1", 1600);

        // Виведення статистики кожного гравця
        Console.WriteLine(player1.GetStats());
        Console.WriteLine(player2.GetStats());
    }
}
}