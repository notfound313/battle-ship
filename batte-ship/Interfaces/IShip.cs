namespace Components.Battle.Ship;

public interface IShip
{
	string ShipName { get; }
	
	int GetShipSize();
	bool IsShunk();
	
	bool IsHit(Cordinate cordinate);
}
