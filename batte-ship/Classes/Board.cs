using Components.Player;

namespace Components.Battle.Ship;

public abstract class Board<T>:IBoard
{
	public  T[,] board = new T[10,10];
	private IPlayer player;
	public List<IShip> ships {get;set;}
	
	
	public bool isOccopied(Cordinate cordinate)
	{
		return ships.Exists(ship => ship.GetCordinates().Contains(cordinate));
	}
	
	
	public bool PlayerIsAvailable(IPlayer  player)
	{
	
		return this.player == null;
	}
	
	public bool SetPlayer(IPlayer player)
	{
		if(!PlayerIsAvailable(player))
		{
			return false;
		}
		this.player = player;
		return true;
	}
	
	
	
	
	
	public int getBoardSize()
	{
		return board.Length;
	}
}
