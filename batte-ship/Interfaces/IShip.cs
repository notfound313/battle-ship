namespace Components.Battle.Ship;

public interface IShip
{
	string ShipName { get; }
	
	int GetShipSize();
	bool IsShunk();
	
	bool IsHit(Coordinate cordinate);
	
	bool SetCordinates(List<Coordinate> cordinates);
	List<Coordinate> GetCordinates();
}
