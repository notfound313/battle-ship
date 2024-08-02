namespace Components.Battle.Ship;

public class AttackBoard: Board<IShip>
{
	
	public AttackBoard(List<Ship> ship)
	{
		SetListShips(ship);
		SetShipInBoard();
	}
	
	
	#region Setup Ship On Board
		public override void SetShipInBoard()
	{
		foreach(var ship in ships)
		{
			foreach(var cordinate in ship.GetCordinates())
			{
				board[cordinate.x,cordinate.y] = ship;
			}
			
		}
		
	}
	#endregion
	
	
	#region Attack Ship on Board	
	public bool IsHit(Cordinate cordinate)
	{
		return board[cordinate.x,cordinate.y] == null;
	}
	
	public bool SetHit(Cordinate cordinate)
	{
		if(IsHit(cordinate))
		{
			board[cordinate.x,cordinate.y] = null;
			return true;
		}
		return false;
		
	}
	  #endregion
	
}
 