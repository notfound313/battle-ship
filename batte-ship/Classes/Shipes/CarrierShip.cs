namespace Components.Battle.Ship;

public class CarrierShip:Ship
{
		public CarrierShip(ShipType shipType, OccopationType occopationType, int sizeShip, string shipName) : 
		base(shipType, occopationType, sizeShip, shipName)
	{
		ShipName = "CarrierShip";
	}
}

	
