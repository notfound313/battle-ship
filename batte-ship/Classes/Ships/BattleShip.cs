using System.Runtime.Serialization;

namespace Components.Battle.Ship;



public class BattleShip : Ship
{
	public BattleShip(string shipName) :
	base(ShipType.Battleship,  4, shipName)
	{
		
	}
	public BattleShip(){}
}
