using System.Runtime.Serialization;

namespace Components.Battle.Ship;


public class CarrierShip:Ship
{
		public CarrierShip( string shipName) : 
		base(ShipType.Carrier, 5, shipName)
	{
		
	}
	public CarrierShip(){}
}

	
