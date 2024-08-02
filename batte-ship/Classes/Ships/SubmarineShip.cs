namespace Components.Battle.Ship;

public class SubmarineShip:Ship
{
	public SubmarineShip(string shipName) :
	base(ShipType.Submarine, OccopationType.Empty,1, shipName)
	{
		
	}
	public SubmarineShip(){}
}
