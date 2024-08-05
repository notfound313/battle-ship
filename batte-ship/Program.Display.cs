using System.Dynamic;
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
			DisplayAttackBoard(gm, player);
			Thread.Sleep(1000);
			Console.WriteLine($"Player {player.Name} Turn");
			var attackCordinate = AttackCordinate();
			if(gm.Attack(player, attackCordinate))
			{
				DisplayAttackBoard(gm, player);
				Console.WriteLine("You Hit");
			}else 
			{
				Console.WriteLine("You Miss");
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
				Console.Write(". ");

			}
			Console.WriteLine();
		}
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