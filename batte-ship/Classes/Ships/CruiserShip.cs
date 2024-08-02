using System.Runtime.Serialization;

namespace Components.Battle.Ship;


public class CruiserShip:Ship
{
	public CruiserShip( string shipName) :
	base(ShipType.Cruiser, OccopationType.Empty, 3, shipName)
	{
		
		
	}
	
	public CruiserShip(){}
}
