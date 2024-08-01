namespace Components.Battle.Ship;

public class ShipBoard : Board<ShipBoard>
{
	public bool IsAllShinked()
	{
		return ships.All(ship => ship.IsShunk());
	}
	
	public bool IsAnyShipHit(Cordinate cordinate)
	{
		return ships.Any(ship => ship.IsHit(cordinate));
	}
	
	public List<IShip> GetAllShips()
	{	
		return ships;
	}
	public bool AddShip(IShip ship)
	{
		if(ships.Contains(ship))
		{
			return false;
	
		}
		ships.Add(ship);
		return true;
	}
	public bool PlaceShip(IShip ship, Cordinate from, Cordinate to)
	{
		if(ship.GetShipSize() != from.x - to.x + 1)
		{
			return false;
		}
		if(ship.GetShipSize() != from.y - to.y + 1)
		{
			return false;
		}
		ship.setCordinates(new List<Cordinate>(){from,to});
		return AddShip(ship);
	}
	private bool CalculateShipCordinates(Cordinate from, Cordinate to, out List<Cordinate> cordinates)
	{
		cordinates = new List<Cordinate>();
		if(from.x == to.x)
		{
			for(int i = from.y; i <= to.y; i++)
			{
				cordinates.Add(new Cordinate(from.x,i));
			}
			return true;
		}
		if(from.y == to.y)
		{
			for(int i = from.x; i <= to.x; i++)
			{
				cordinates.Add(new Cordinate(i,from.y));
			}
			return true;
		}
		return false;
	}
	private bool IsPlacementValid(Cordinate from, Cordinate to)
	{
		return CalculateShipCordinates(from,to,out List<Cordinate> cordinates) && cordinates.All(cordinate => isOccopied(cordinate));
	}
	
	private bool IsShipCanBePlaced(Cordinate from, Cordinate to)
	{
		return !IsShipPlaced(from,to);
	}
	
	private bool IsShipPlaced(Cordinate from, Cordinate to)
	{
		return ships.Any(ship => ship.GetCordinates().Contains(from) && ship.GetCordinates().Contains(to));
	}
	
	private List<Cordinate> GetCordinates(Cordinate from, Cordinate to)
	{
		return new List<Cordinate>(){from,to};
	}
}
