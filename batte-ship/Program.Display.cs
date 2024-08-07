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
			AttackCordinate(out Cordinate attackCordinate);
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

					Cordinate cordinateFrom = GetCoordinate("Enter Coordinate From: ");
					Cordinate cordinateTo = GetCoordinate("Enter Coordinate To: ");

					if (gm.PlaceShipsOnBoard(player, shipTypeEnum, cordinateFrom, cordinateTo))
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

	private static Cordinate GetCoordinate(string prompt)
	{
		TypingTextAnimation(prompt);
		AttackCordinate(out Cordinate coordinate);
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
		Console.WriteLine($"\nThe {player.Name} Ship Board");
		DisplayBoard(gm.GetShipBoard(player), gm, true);
	}

	public static void DisplayAttackBoard(GameController gm, IPlayer player)
	{
		Console.WriteLine($"\nThe {player.Name} Attack Board");
		DisplayBoard(gm.GetShipBoard(player), gm, false);
	}

	private static void DisplayBoard(Ship[,] board, GameController gm, bool isShipBoard)
	{
		// Print column headers
		Console.Write("    ");
		for (int y = 0; y < board.GetLength(1); y++)
		{
			Console.Write($"{y,2} "); // Align columns

		}
		Console.Write("\n");

		// Print top border
		Console.Write("    ");
		for (int x = 0; x < board.GetLength(1); x++)
		{
			Console.Write("-- ");
		}
		Console.Write("\n");

		// Print board rows
		for (int i = 0; i < board.GetLength(0); i++)
		{
			Console.Write($"{i,2} | ");
			for (int j = 0; j < board.GetLength(1); j++)
			{
				var coord = new Cordinate(i, j);
				Console.Write((isShipBoard ? GetCellRepresentationBoard(gm, board[i, j], coord) : GetCellRepresentation(gm, board[i, j], coord)) + "  ");
			}
			Console.WriteLine();
		}
	}

	private static string GetCellRepresentationBoard(GameController gm, Ship ship, Cordinate coord)
	{
		if (ship == null)
		{
			// Return "M" if valid miss attack, otherwise "."
			return ValidMissAttack(gm, coord) ? "M" : ".";
		}

		// Check if the ship has been hit
		bool isHit = ship.statusOccaption.ContainsValue(OccopationType.Hit);

		// Determine the symbol to return based on whether the ship is hit
		if (isHit)
		{
			bool displayHit = DisplayHitShip(ship, coord);
			return displayHit ? "X" : _shipSymbol[ship._shipType];
		}

		// Return the symbol for the ship type if not hit
		return _shipSymbol[ship._shipType];
	}


	private static string GetCellRepresentation(GameController gm, Ship ship, Cordinate coord)
	{
		if (ship != null && ship.statusOccaption.ContainsValue(OccopationType.Hit))
		{
			return DisplayHitShip(ship, coord) ? "X" : ".";
		}
		return ValidMissAttack(gm, coord) ? "M" : ".";
	}


	static bool DisplayHitShip(Ship ship, Cordinate cordinate)
	{
		foreach (var item in ship.statusOccaption.Keys)
		{
			if (item.x == cordinate.x && item.y == cordinate.y)
			{
				return true;

			}
		}
		return false;
	}
	static bool ValidMissAttack(GameController gm, Cordinate cordinate)
	{
		foreach (var item in gm.GetMissedAttacks(gm.GetCurrentPlayer()))
		{
			if (item.x == cordinate.x && item.y == cordinate.y)
			{
				return true;
			}

		}
		return false;

	}

	static void AttackCordinate(out Cordinate attackCordinate)
	{
		attackCordinate = new Cordinate();
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

	private static bool TryParseCoordinates(string input, out int row, out int col)
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

	public static bool IsValidCordinate(string cordinate)
	{
		bool status = int.TryParse(cordinate, out int cordinateInt);
		if (status && cordinateInt < 10 && cordinateInt >= 0)
		{
			return true;
		}
		return false;

	}
}