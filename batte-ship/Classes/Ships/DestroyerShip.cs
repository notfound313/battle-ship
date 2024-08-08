using System.Runtime.Serialization;

namespace Components.Battle.Ship;


public class DestroyerShip : Ship
{
	public DestroyerShip(string shipName) :
	base(ShipType.Destroyer,  2, shipName)
	{
		
	}
	public DestroyerShip(){}
}
