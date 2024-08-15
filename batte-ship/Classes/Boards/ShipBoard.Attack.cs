namespace Components.Battle.Ship;

public partial class ShipBoard: Board<Ship>
{
	
	
	
	#region Attack Ship on Board	
	public bool IsHit(Coordinate cordinate)
	{
		if(HasEverHit(cordinate))
		{
			return false;
		}
		return board[cordinate.x,cordinate.y] != null;
	}
	
	private bool HasEverHit(Coordinate cordinate)
	{
		return missAttacks.Exists(coor => coor.Equals(cordinate));
	}
	private Ship GetShipHasHit(Coordinate cordinate)
	{
		return board[cordinate.x,cordinate.y];
	}
	
	public Ship GetShipHited(Coordinate cordinate)
	{
		return GetShipHasHit(cordinate);
	}
	
	public bool SetHit(Coordinate cordinate)
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
		public List<Coordinate> GetMissedAttacks()
	{
		return missAttacks;
	}
}
 