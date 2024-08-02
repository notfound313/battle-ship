namespace Components.Battle.Ship;

public class ShipBoard : Board<Ship>
{
	
	public ShipBoard(List<Ship> ships) : base()
	{
		SetListShips(ships);
		SetShipInBoard();
		
	}

	#region Checking if the ship is hit
	public bool IsAllShinked()
	{
		return ships.TrueForAll(ship => ship.IsShunk());
	}
	
	public bool IsAnyShipHit(Cordinate cordinate)
	{
		return ships.Exists(ship => ship.IsHit(cordinate));
	}
	#endregion
	
	#region Setting the ship in the board
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
	
	public List<Ship> GetAllShips()
	{	
		return ships;
	}
	public bool AddShip(Ship ship)
	{
		if(ships.Contains(ship))
		{
			return false;
	
		}
		ships.Add(ship);
		return true;
	}
	#endregion
	
	#region Checking if the ship is placed in the board
	public bool PlaceShip(Ship ship, Cordinate from, Cordinate to)
	{
		if(ship.GetShipSize() != from.x - to.x + 1)
		{
			return false;
		}
		if(ship.GetShipSize() != from.y - to.y + 1)
		{
			return false;
		}
		if(!IsPlacementValid(from,to))
		{
			return false;
		}
		if(IsShipPlaced(from,to))
		{
			return false;
		}
		ship.setCordinates(CalculateShipCordinates(from,to));
		return AddShip(ship);
	}
	
	private List<Cordinate> CalculateShipCordinates(Cordinate from, Cordinate to)
	{
		var cordinates = new List<Cordinate>();
		if(from.x == to.x)
		{
			for(int i = from.y; i <= to.y; i++)
			{
				cordinates.Add(new Cordinate(from.x,i));
			}
			
		}
		if(from.y == to.y)
		{
			for(int i = from.x; i <= to.x; i++)
			{
				cordinates.Add(new Cordinate(i,from.y));
			}
			
			
		}
		return cordinates;
	}
	private bool IsPlacementValid(Cordinate from, Cordinate to)
	{
		if(from.x < 0 || from.y < 0 || to.x < 0 || to.y < 0)
		{
			return false;
		}
		if(from.x >= GetBoardRowLength() || from.y >= GetBoardColumnLength())
		{
			return false;
		}
		if(to.x >= GetBoardRowLength() || to.y >= GetBoardColumnLength())
		{
			return false;
		}
		return true;
	}
	
	private bool IsShipPlaced(Cordinate from, Cordinate to)
	{
		return ships.Exists(ship => ship.GetCordinates().Contains(from) && ship.GetCordinates().Contains(to));
	}
	

	#endregion
}
