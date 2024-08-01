namespace Components.Battle.Ship;

public class DestroyerShip : Ship
{
	public DestroyerShip(string shipName) :
	base(ShipType.Destroyer, OccopationType.Empty, 2, shipName)
	{
		
	}
}
