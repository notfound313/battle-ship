namespace Components.Battle.Ship;

public class AttackBoard: Board<Ship>
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
		return board[cordinate.x,cordinate.y] != null;
	}
	private Ship GetShipHasHit(Cordinate cordinate)
	{
		return board[cordinate.x,cordinate.y];
	}
	
	public Ship GetShipHited(Cordinate cordinate)
	{
		return GetShipHasHit(cordinate);
	}
	
	public bool SetHit(Cordinate cordinate)
	{
		if(IsHit(cordinate)&&!GetShipHasHit(cordinate).IsShunk())	
		{
			var ship = GetShipHasHit(cordinate);
			
			return ship.IsHit(cordinate);
		}
		SetMissAttack(cordinate);
		return false;
		
	}
	  #endregion
	public void SetMissAttack(Cordinate cordinate)
	{
		missAttacks.Add(cordinate);
	}
	public List<Cordinate> GetMissedAttacks()
	{
		return missAttacks;
	}
}
 