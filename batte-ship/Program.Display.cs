using System.Dynamic;
using Components.Battle.Ship;
using Components.Player;

public partial class Program
{
	public static void Display(GameController gm, List<IPlayer> players)
	{
		Begining();
		Thread.Sleep(1000);
		// Ship[,] ship = gm.GetAttckBoard(players[0]);
		// List <Cordinate> cor = ship[0,0].GetCordinates();
		// foreach (var item in cor)
		// {
		// 	Console.WriteLine($"{item.x} {item.y}");
			
		// }
		// Cordinate cordinate = new(1, 0);
		// Console.WriteLine(cor.Contains(cordinate));
		
		// Console.ReadLine();
		while (GameStatus.End != _gameStatus)
		{
			IPlayer player = gm.GetCurrentPlayer();
			DisplayShipBoard(gm, player);
			Thread.Sleep(1000);
			DisplayAttackBoard(gm, player);
			Thread.Sleep(1000);
			Console.WriteLine($"Player {player.Name} Turn");
			var attackCordinate = AttackCordinate();
			if(gm.ProcessShotResult(player, attackCordinate))
			{
				Console.WriteLine($"Ship {gm.GetShipHasHit(attackCordinate).ShipName} is Hit at {attackCordinate.x} {attackCordinate.y}");	
				
				Thread.Sleep(1000);
			}else 
			{
				Console.WriteLine("You Miss");
				Thread.Sleep(1000);
			}
			if(gm.IsGameOver())
			{
				_gameStatus = GameStatus.End;
				Console.WriteLine($"Player {player.Name} Win");
				Thread.Sleep(1000);
				break;
			}
			
			
		}
		
		
		
		
			
		
		
	}
	
	public static void Begining()
	{
		Console.WriteLine("Game Starting");
		Thread.Sleep(1000);
		Console.WriteLine("Its Time you show up your skill");
		Thread.Sleep(1000);
		Console.WriteLine("Lets Start Beat them");
		
	}
	
	public static void DisplayShipBoard(GameController gm, IPlayer player)
	{
		
		Console.WriteLine();
		Console.WriteLine($"The {player.Name} ShipBord ");
		
				
		Ship[,] ships =gm.GetShipBoard(player);
		for(int i = 0; i < ships.GetLength(0); i++)
		{
			for(int j = 0; j < ships.GetLength(1); j++)
			{
				if(ships[i,j] == null)
				{
					Console.Write(". ");
				}
				else Console.Write($"{_shipSymbol[ships[i,j]._shipType]} ");
				
			}
			Console.WriteLine();
		}
		
	}
	
	public static void DisplayAttackBoard(GameController gm, IPlayer player)
	{
		Console.WriteLine();
		
		Console.WriteLine($"The {player.Name} Attack Board ");
		
		Ship[,] ships = gm.GetAttckBoard(player);
		for(int i = 0; i < ships.GetLength(0); i++)
		{
			for(int j = 0; j < ships.GetLength(1); j++)
			{
				if(ships[i,j] != null && ships[i,j].statusOccaption.ContainsValue(OccopationType.Hit))
				{
					if(DisplayHitShip(ships[i,j], new Cordinate(i,j)))
					{
						Console.Write("X ");
					}else Console.Write(". ");
					
				}else Console.Write(". ");
				 

			}
			Console.WriteLine();
		}
	}
	
	static bool DisplayHitShip(Ship ship, Cordinate cordinate)
	{
		foreach (var item in ship.statusOccaption.Keys)
		{
			if(item.x == cordinate.x && item.y == cordinate.y)
			{
				return true;
				
			}
		}
		return false;
	}
	
	static Cordinate AttackCordinate()
	{
		Cordinate attackCordinate = new();
		Console.WriteLine("Please choose the cordinate to attack");
		Console.Write("Enter Row: ");
		bool statusRow = int.TryParse(Console.ReadLine(), out int row);
		if(statusRow && row < 10 && row >= 0)
		{
			attackCordinate.x = row;	
		}else
		{
			Console.WriteLine("Invalid Row");
			Thread.Sleep(1000);
		}
		Console.WriteLine();
		Console.Write("Enter Col: ");
		bool statusCol = int.TryParse(Console.ReadLine(), out int col);
		if(statusCol && col < 10 && col >= 0)
		{
			attackCordinate.y = col;
		}else
		{
			Console.WriteLine("Invalid Col");
			Thread.Sleep(1000);
		}
		return attackCordinate;
	
		
		
		
	}
}