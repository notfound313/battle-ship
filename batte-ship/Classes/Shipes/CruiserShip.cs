namespace Components.Battle.Ship;

public class CruiserShip:Ship
{
	public CruiserShip(ShipType shipType, OccopationType occopationType, int sizeShip, string shipName) :
	base(shipType, occopationType, sizeShip, shipName)
	{
		ShipName = "CruiserShip";
	}
}
