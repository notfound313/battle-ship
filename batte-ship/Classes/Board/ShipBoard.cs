namespace Components.Battle.Ship;

public class ShipBoard : Board<Ship>
{
	public bool IsAllShinked()
	{
		return ships.All(ship => ship.IsShunk());
	}
	
	public bool IsAnyShipHit(Cordinate cordinate)
	{
		return ships.Any(ship => ship.IsHit(cordinate));
	}
}
