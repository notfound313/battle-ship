using System.Dynamic;
using System.Runtime.CompilerServices;
using Components.Battle.Ship;
using Components.Player;

public partial class Program
{
	public static void Display(GameController gm, List<IPlayer> players)
	{
		Begining();
		Thread.Sleep(1000);

		while (GameStatus.End != _gameStatus)
		{
			IPlayer player = gm.GetCurrentPlayer();
			DisplayShipBoard(gm, player);
			Thread.Sleep(1000);
			DisplayAttackBoard(gm, player);
			Thread.Sleep(1000);
			
			Console.WriteLine($"Player {player.Name} Turn");
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

		Console.WriteLine();
		Console.WriteLine($"The {player.Name} ShipBord ");


		Ship[,] ships = gm.GetShipBoard(player);
		for (int i = 0; i < ships.GetLength(0); i++)
		{
			for (int j = 0; j < ships.GetLength(1); j++)
			{
				if (ships[i, j] == null)
				{
					if (ValidMissAttack(gm, new Cordinate(i, j)))
					{
						Console.Write("M ");
					}else Console.Write(". ");
				}
				else
				{
					if (ships[i, j].statusOccaption.ContainsValue(OccopationType.Hit))
					{
						if (DisplayHitShip(ships[i, j], new Cordinate(i, j)))
						{
							Console.Write("X ");
						}else Console.Write($"{_shipSymbol[ships[i, j]._shipType]} ");
					
					}					
					else
					{
						Console.Write($"{_shipSymbol[ships[i, j]._shipType]} ");
					}
				} 

			}
			Console.WriteLine();
		}

	}

	public static void DisplayAttackBoard(GameController gm, IPlayer player)
	{
		Console.WriteLine();

		Console.WriteLine($"The {player.Name} Attack Board ");

		Ship[,] ships = gm.GetAttckBoard(player);
		for (int i = 0; i < ships.GetLength(0); i++)
		{
			for (int j = 0; j < ships.GetLength(1); j++)
			{
				if (ships[i, j] != null && ships[i, j].statusOccaption.ContainsValue(OccopationType.Hit))
				{
					if (DisplayHitShip(ships[i, j], new Cordinate(i, j)))
					{
						Console.Write("X ");
					}
					else Console.Write(". ");

				}
				else 
				{
					if(ValidMissAttack(gm, new Cordinate(i, j)))
					{
						Console.Write("M ");
					}else Console.Write(". ");
				}


			}
			Console.WriteLine();
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