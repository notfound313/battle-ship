using System.Dynamic;
using System.Runtime.CompilerServices;
using Components.Battle.Ship;
using Components.Player;

public partial class Program
{
	public static void Display(GameController gm, List<IPlayer> players)
	{


		foreach (var py in players)
		{
			ChangeCordinateShip(gm, py);
		}

		Begining();
		Console.WriteLine();
		Thread.Sleep(1000);
		IPlayer player = gm.GetCurrentPlayer();
		TypingTextAnimation($"Player {player.Name.ToUpper()} First");
		Thread.Sleep(2000);

		while (GameStatus.End != _gameStatus)
		{
			player = gm.GetCurrentPlayer();
			DisplayShipBoard(gm, player);
			Thread.Sleep(1000);
			DisplayAttackBoard(gm, player);
			Thread.Sleep(1000);

			TypingTextAnimation($"Player {player.Name} Turn");
			AttackCordinate(out Coordinate attackCordinate);
			if (gm.ProcessShotResult(player, attackCordinate))
			{
				Console.WriteLine($"Ship {gm.GetShipHasHit(attackCordinate).ShipName} is Hit at {attackCordinate.x} {attackCordinate.y}");

				Thread.Sleep(1000);
			}
			else
			{
				Console.WriteLine($"Missed Attack at {attackCordinate.x} {attackCordinate.y}");
				Thread.Sleep(1000);
			}
			if (gm.IsGameOver())
			{
				_gameStatus = GameStatus.End;
				Console.WriteLine();

				TypingTextAnimation($"------Player {player.Name} Win-------");

				Thread.Sleep(1000);
				Closing();
				break;
			}


		}


	}

	static void DisplayTypeOfShip()
	{
		int count = 1;
		foreach (var type in Enum.GetValues(typeof(ShipType)))
		{
			TypingTextAnimation($"{count}. {type} '{_shipSymbol[(ShipType)type]}'");
			count++;
		}
	}

	private static ShipType GetShipType(int index)
	{
		switch (index)
		{
			case 1:
				return ShipType.Battleship;
			case 2:
				return ShipType.Cruiser;
			case 3:
				return ShipType.Destroyer;
			case 4:
				return ShipType.Submarine;
			case 5:
				return ShipType.Carrier;
			default:
				return ShipType.Battleship;
		}

	}

	private static void ChangeCordinateShip(GameController gm, IPlayer player)
	{
		TypingTextAnimation($"Hi {player.Name}, do you want to change the coordinates of your ship?");

		while (true)
		{
			if (GetYesOrNoInput("Enter Y/N: "))
			{
				while (true)
				{
					TypingTextAnimation("Choose Your Ship Type");
					DisplayTypeOfShip();

					int shipType = GetShipTypeFromInput();
					ShipType shipTypeEnum = GetShipType(shipType);

					DisplayShipBoard(gm, player);
					Ship ship = gm.GetOneShip(player, shipTypeEnum);

					Console.WriteLine();
					TypingTextAnimation($"Input Coordinates to fit the size of the ship ({shipTypeEnum}, Size: {ship._sizeShip})");

					Coordinate coordinateFrom = GetCoordinate("Enter Coordinate From: ");
					Coordinate coordinateTo = GetCoordinate("Enter Coordinate To: ");

					if (gm.PlaceShipsOnBoard(player, shipTypeEnum, coordinateFrom, coordinateTo))
					{
						DisplayShipBoard(gm, player);
						Console.WriteLine("Ship is Placed\n");
						TypingTextAnimation("Are you wanna change the coordinates of your ship again?");
					}
					else
					{
						Console.WriteLine("Invalid Coordinate. Try Again.\n");
						Thread.Sleep(2000);
						continue;
					}

					break; // Exit the inner loop if ship placement is successful
				}
			}
			else
			{
				break; // Exit the outer loop if the user doesn't want to change coordinates
			}
		}
	}

	private static bool GetYesOrNoInput(string prompt)
	{
		Console.Write(prompt);
		string input = Console.ReadLine()?.ToLower();
		return input == "y";
	}

	private static int GetShipTypeFromInput()
	{
		TypingTextAnimation("Enter number of your ship type: ");
		Console.Write("> ");
		return int.TryParse(Console.ReadLine(), out int shipType) ? shipType : 0;
	}

	private static Coordinate GetCoordinate(string prompt)
	{
		TypingTextAnimation(prompt);
		AttackCordinate(out Coordinate coordinate);
		return coordinate;
	}


	public static void Begining()
	{
		TypingTextAnimation("Game Starting.......uuuhhhuuuuuy...");
		Thread.Sleep(1000);
		TypingTextAnimation("Its Time you show up your skill");

		Thread.Sleep(1000);
		TypingTextAnimation("Lets Start Beat them");


	}

	private static void Closing()
	{
		string textCongrut = "Congratulation you did it !!!!!!";
		TypingTextAnimation(textCongrut);

	}
	private static void TypingTextAnimation(string text)
	{
		for (int i = 0; i < text.Length; i++)
		{
			Console.Write(text[i]);
			Thread.Sleep(50);
		}
		Console.WriteLine();

	}

	 public static void DisplayShipBoard(GameController gm, IPlayer player)
	{
		Console.WriteLine();
		Console.WriteLine($"The {player.Name} Ship Board");

		Ship[,] ships = gm.GetShipBoard(player);
		DisplayBoard(ships, gm, IsShipBoard: true);
	}

	public static void DisplayAttackBoard(GameController gm, IPlayer player)
	{
		Console.WriteLine();
		Console.WriteLine($"The {player.Name} Attack Board");

		Ship[,] board = gm.GetAttckBoard(player);
		DisplayBoard(board, gm, IsShipBoard: false);
	}

	private static void DisplayBoard(Ship[,] board, GameController gm, bool IsShipBoard)
	{
		int rows = board.GetLength(0);
		int cols = board.GetLength(1);

		// Print column headers
		Console.Write("   ");
		for (int i = 0; i < cols; i++)
		{
			Console.Write($"{i}  ");
		}
		Console.WriteLine();
		
		// Print separator line
		Console.Write("   ");
		for (int i = 0; i < cols; i++)
		{
			Console.Write("-- ");
		}
		Console.WriteLine();

		// Print board content
		for (int row = 0; row < rows; row++)
		{
			Console.Write($"{row} |");
			for (int col = 0; col < cols; col++)
			{
				char symbol = GetBoardSymbol(board[row, col], new Coordinate(row, col), gm, IsShipBoard);
				Console.Write($"{symbol}  ");
			}
			Console.WriteLine();
		}
	}

	private static char GetBoardSymbol(Ship ship, Coordinate coord, GameController gm, bool IsShipBoard)
	{
		if (ship == null)
		{
			return IsShipBoard
				? ValidMissAttack(gm, coord, false) ? 'M' : '.'
				: ValidMissAttack(gm, coord, true) ? 'M' : '.';
		}

		bool isHit = ship.statusOccaption.ContainsValue(OccopationType.Hit);
		if (isHit && DisplayHitShip(ship, coord))
		{
			return 'X';
		}
		return '.';
	}

	static bool DisplayHitShip(Ship ship, Coordinate coordinate)
	{
		foreach (var item in ship.statusOccaption.Keys)
		{
			if (item.Equals(coordinate))
			{
				return true;

			}
		}
		return false;
	}
	static bool ValidMissAttack(GameController gm, Coordinate coordinate, bool isAttackBorad)
	{

		if (isAttackBorad)
		{
			foreach (var item in gm.GetMissedAttackBoard(gm.GetCurrentPlayer()))
			{
				if (item.Equals(gm.GetMissedAttackBoard(gm.GetCurrentPlayer())))
				{
					return true;
				}

			}
			return false;
		}
		foreach (var item in gm.GetMissedShipBoard(gm.GetCurrentPlayer()))
		{
			if (item.x == coordinate.x && item.y == coordinate.y)
			{
				return true;
			}

		}
		return false;

	}

	static void AttackCordinate(out Coordinate attackCordinate)
	{
		attackCordinate = new Coordinate();
		string input;

		while (true)
		{
			Console.Write("Enter Row and Column (e.g., 5 7): ");
			input = Console.ReadLine();

			if (TryParseCoordinates(input, out int row, out int col))
			{
				attackCordinate.x = row;
				attackCordinate.y = col;
				break;
			}
			else
			{
				Console.WriteLine("Enter Valid Coordinates in the format 'Row Col' or index Out Of bound");
			}
		}
	}

	static bool TryParseCoordinates(string input, out int row, out int col)
	{
		row = 0;
		col = 0;
		var parts = input.Split(' ');

		if (parts.Length == 2 &&
			int.TryParse(parts[0], out row) &&
			int.TryParse(parts[1], out col))
		{
			return row >= 0 && row < 10 && col >= 0 && col < 10;
		}

		return false;
	}

	static bool IsValidCordinate(string coordinate)
	{
		bool status = int.TryParse(coordinate, out int coordinateInt);
		if (status && coordinateInt < 10 && coordinateInt >= 0)
		{
			return true;
		}
		return false;

	}
}
