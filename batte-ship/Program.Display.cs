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
			ChangeCordinateShip(gm,py);	
		}				
		
		Begining();
		Console.WriteLine();
		Thread.Sleep(1000);
		IPlayer player = gm.GetCurrentPlayer();
		Console.WriteLine($"Player {player.Name} First");
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

				Console.WriteLine($"Player {player.Name} Win");

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
			Console.WriteLine($"{count}. {type} '{_shipSymbol[(ShipType)type]}'");
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
		TypingTextAnimation($"Hi {player.Name} Are you wanna Change Cordinate your Ship");
		bool change = false;
		Console.Write("Enter Y/N: ");
		string? input = Console.ReadLine().ToLower();
		if (input == "y")
		{
			change = true;
		}
		
		while(change)
		{
			DisplayShipBoard(gm, player);
			TypingTextAnimation("Choose Your Ship Type");
			DisplayTypeOfShip();
			Console.Write("Enter num Your Ship Type: ");
			int shipType = int.TryParse(Console.ReadLine(), out int shipTypeInt) ? shipTypeInt : 0;
			ShipType shipTypeEnum = GetShipType(shipType);
			Console.WriteLine("Enter Cordinate From: ");
			AttackCordinate(out Cordinate cordinateTo);
			Console.WriteLine("Enter Cordinate To: ");
			AttackCordinate(out Cordinate cordinateFrom);
			bool status = gm.PlaceShipsOnBoard(player, shipTypeEnum, cordinateTo, cordinateFrom);
			if (!status)
			{
				Console.WriteLine("Invalid Cordinate");
				Console.WriteLine("Try Again");
				Console.WriteLine();
				Thread.Sleep(2000);
			}else
			{
				DisplayShipBoard(gm, player);
				Console.WriteLine("Ship is Placed");
				Console.WriteLine();
				Console.WriteLine("Still Wanna Change Cordinate again ?");
				Console.Write("Enter Y/N: ");
				input = Console.ReadLine().ToLower();
				if (input == "y")
				{
					change = true;
				}else change = false;
				
			}
			
		}
	}

	public static void Begining()
	{
		Console.WriteLine("Game Starting");
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
    Console.WriteLine($"\nThe {player.Name} ShipBoard ");

    Ship[,] ships = gm.GetShipBoard(player);

    for (int i = 0; i < ships.GetLength(0); i++)
    {
        for (int j = 0; j < ships.GetLength(1); j++)
        {
            var cellRepresentation = GetCellRepresentationBoard(gm, ships[i, j], new Cordinate(i, j));
            Console.Write(cellRepresentation + " ");
        }
        Console.WriteLine();
    }
}

private static string GetCellRepresentationBoard(GameController gm, Ship ship, Cordinate coord)
{
    if (ship == null)
    {
        return ValidMissAttack(gm, coord) ? "M" : ".";
    }
    else
    {
        if (ship.statusOccaption.ContainsValue(OccopationType.Hit))
        {
            return DisplayHitShip(ship, coord) ? "X" : _shipSymbol[ship._shipType];
        }
        return _shipSymbol[ship._shipType];
    }
}


	public static void DisplayAttackBoard(GameController gm, IPlayer player)
{
    Console.WriteLine($"\nThe {player.Name} AttackBoard ");

    Ship[,] ships = gm.GetAttckBoard(player);

    for (int i = 0; i < ships.GetLength(0); i++)
    {
        for (int j = 0; j < ships.GetLength(1); j++)
        {
            Console.Write(GetCellRepresentation(gm, ships[i, j], new Cordinate(i, j)) + " ");
        }
        Console.WriteLine();
    }
}

private static string GetCellRepresentation(GameController gm, Ship ship, Cordinate coord)
{
    if (ship != null && ship.statusOccaption.ContainsValue(OccopationType.Hit))
    {
        return DisplayHitShip(ship, coord) ? "X" : ".";
    }
    else
    {
        return ValidMissAttack(gm, coord) ? "M" : ".";
    }
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
		attackCordinate = new();
		string cor="";
		do
		{
			Console.Write("Enter Row: ");
			cor = Console.ReadLine();
			if(!IsValidCordinate(cor))
			{
				Console.WriteLine("Enter Valid Cordinate");
			}else 
			{
				attackCordinate.x = int.Parse(cor);
				break;
			}
		}while(!IsValidCordinate(cor));
		
		do
		{
			Console.Write("Enter Col: ");
			cor = Console.ReadLine();
			if (!IsValidCordinate(cor))
			{
				Console.WriteLine("Enter Valid Cordinate");
			}
			else
			{
				attackCordinate.y = int.Parse(cor);
				break;
			}
		}while (!IsValidCordinate(cor));

	}
	
	public static bool IsValidCordinate(string cordinate)
	{
		bool status = int.TryParse(cordinate, out int cordinateInt);
		if(status && cordinateInt < 10 && cordinateInt >= 0)
		{
			return true;
		}
		return false;
			
	}
}