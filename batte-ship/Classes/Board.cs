using Components.Player;

namespace Components.Battle.Ship;

public class Board<T>:IBoard
{
	public T[][] board;
	public IPlayer player;
	public List<IShip> ships;
	
	public bool isOccopied(Cordinate cordinate)
	{
		return board[cordinate.x][cordinate.y] == null;
	}
	
	
	public bool PlayerIsAvailable(IPlayer  player)
	{
		return this.player == null;
	}
	
	public void setPlayer(IPlayer player)
	{
		this.player = player;
	}
	
	public void setBoard(T[][] board)
	{
		this.board = board;
	}
	
	
	
	public int getBoardSize()
	{
		return board.Length;
	}
}
