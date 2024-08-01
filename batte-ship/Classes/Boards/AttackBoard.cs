namespace Components.Battle.Ship;

public class AttackBoard: Board<AttackBoard>
{
	public bool IsHit(Cordinate cordinate)
	{
		return board[cordinate.x,cordinate.y] == null;
	}
	
	public bool SetHit(Cordinate cordinate)
	{
		board[cordinate.x,cordinate.y] = new AttackBoard();
		return true;
	}
	
}
