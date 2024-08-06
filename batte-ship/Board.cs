using System.Collections.Generic;
using Components.Player;

namespace Components.Battle.Ship;

public abstract class Board<T>:IBoard
{
	public  T[,] board = new T[10,10];
	protected IPlayer player;
	protected List<Ship> ships ;
	protected List<Cordinate> missAttacks = new ();
	
	
	public void SetListShips(List<Ship> ships)
	{
		this.ships = ships;
	}
	
	public bool isOccopied(Cordinate cordinate)
	{
		return ships.Exists(ship => ship.GetCordinates().Contains(cordinate));
	}
	
	public abstract void SetShipInBoard();
	
	
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
	
	public T[,] GetBoard()
	{
		return board;
		
	}
	
	public int GetBoardRowLength()
	{
		return board.GetLength(0);
	}
	public int GetBoardColumnLength()
	{
		return board.GetLength(1);
	}
	
	
	
	
	
	public int getBoardSize()
	{
		return board.Length;
	}
}
