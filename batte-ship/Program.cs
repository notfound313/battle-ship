
using Components.Battle.Ship;
using Components.Player;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

public partial class Program
{
	private static GameStatus _gameStatus = GameStatus.Start;
	private static Dictionary<ShipType, char> _shipSymbol = new()

	{
		{ShipType.Battleship,'B'},
		{ShipType.Cruiser, 'C'},
		{ShipType.Destroyer,'D'},
		{ShipType.Submarine,'S'},
		{ShipType.Carrier,'A'}

	};

	static void Main(string[] args)
	{
		var loggerFactory = LoggerFactory.Create(b =>
					{
						b.ClearProviders();
						b.SetMinimumLevel(LogLevel.Information);
						b.AddNLog("nlog.config");
					});
		ILogger<GameController> logger = loggerFactory.CreateLogger<GameController>();
		
		Console.WriteLine("Welcome to Battle Ship Game");

		List<IPlayer> players = CreateNewPlayer();


		GameController gm = new(players, logger);
		Display(gm, players);


	}

	public static List<IPlayer> CreateNewPlayer()
	{
		Console.WriteLine("Set Your Player Name to Start the Game");

		string[] namePlayer = new string[2];
		for (int i = 0; i < namePlayer.Length; i++)
		{
			Console.WriteLine($"Enter Player Name {i + 1}");
			Console.Write("> ");
			namePlayer[i] = Console.ReadLine();

		}
		List<IPlayer> players = new();
		namePlayer.ToList().ForEach(playerName => players.Add(new Player(playerName)));
		return players;


	}
	public static void SetGameStatus(GameStatus status)
	{
		_gameStatus = status;
	}
	public static GameStatus GetGameStatus()
	{
		return _gameStatus;
	}

	public enum GameStatus
	{
		Start,
		End
	}

}