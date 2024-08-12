using System.Runtime.Serialization;

namespace Components.Battle.Ship;
// [KnownType(typeof(BattleShip))]
// [KnownType(typeof(CarrierShip))]
// [KnownType(typeof(DestroyerShip))]
// [KnownType(typeof(SubmarineShip))]
// [KnownType(typeof(CruiserShip))]
// [KnownType(typeof(Cordinate))]
[DataContract]
public class Ship : IShip
{
	[DataMember]
	private int _hits;
	[DataMember]
	public readonly int _sizeShip;
	[DataMember]
	public readonly ShipType _shipType;

	[DataMember]
	private List<Coordinate> _coordinates;
	[DataMember]
	public string ShipName { get; set; }
	public Dictionary<Coordinate, OccopationType> statusOccaption = new ();

	public Ship(ShipType shipType, int sizeShip, string shipName)
	{
		_shipType = shipType;
		
		_sizeShip = sizeShip;
		ShipName = shipName;
		_hits = 0;

	}
	public Ship(){}

	
	
	public int GetShipSize()
	{
		return _sizeShip;
	}
	public bool setCordinates(List<Coordinate> cordinates)
	{
		if (cordinates.Count != _sizeShip)
		{
			return false;
		}
		_coordinates = cordinates;
		return true;
	}
	
	private void SetstatusOccaption(Coordinate cordinate , OccopationType occopationType)
	{
		statusOccaption.Add(cordinate, occopationType);
	}

	public bool IsShunk()
	{
		return _hits == _sizeShip;
	}

	public List<Coordinate> GetCordinates()
	{
		return _coordinates;
	}

	public bool IsHit(Coordinate cordinate)
	{
		if (IsCordinateInShip(cordinate))
		{
			_hits++;
			SetstatusOccaption(cordinate, OccopationType.Hit);
			
			return true;
		}
		return false;
	}
	
	private bool IsCordinateInShip(Coordinate cordinate)
	{
		foreach (var cor in _coordinates)
		{
			if (cor.x == cordinate.x && cor.y == cordinate.y)
			{
				return true;
			}
			
		}
		return false;
	}

}
