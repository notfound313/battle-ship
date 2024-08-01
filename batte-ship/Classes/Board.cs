using Components.Player;

namespace Components.Battle.Ship;

public abstract class Board<T>:IBoard
{
	public T[,] board = new T[10,10];
	public IPlayer player;
	public List<IShip> ships;
	
	public bool isOccopied(Cordinate cordinate)
	{
		return board[cordinate.x,cordinate.y] == null;
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
	
	public bool SetBoard(T[,] board)
	{
		if(board.Length != getBoardSize())
		{
			return false;
		}
		this.board = board;
		return true;
	}
	
	
	
	public int getBoardSize()
	{
		return board.Length;
	}
}
