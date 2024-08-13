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
	private int _hits = 0;
	[DataMember]
	public readonly int _sizeShip;
	[DataMember]
	public readonly ShipType _shipType;

	[DataMember]
	public string ShipName { get; set; }
	public Dictionary<Coordinate, OccopationType> statusCoorOccaption = new();

	public Ship(ShipType shipType, int sizeShip, string shipName)
	{
		_shipType = shipType;
		_sizeShip = sizeShip;
		ShipName = shipName;

	}
	public Ship() { }



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
		foreach (var cordinate in cordinates)
		{
			SetstatusOccaption(cordinate);
		}
		return true;
	}

	private void SetstatusOccaption(Coordinate cordinate)
	{
		statusCoorOccaption.Add(cordinate, OccopationType.Empty);
	}

	public bool IsShunk()
	{
		return _hits == _sizeShip;
	}

	public List<Coordinate> GetCordinates()
	{
		return new List<Coordinate>(statusCoorOccaption.Keys);
	}

	public bool IsHit(Coordinate cordinate)
	{
		if (IsCordinateInShip(cordinate))
		{
			_hits++;
			statusCoorOccaption[cordinate] = OccopationType.Hit;
			return true;
		}
		return false;
	}

	private bool IsCordinateInShip(Coordinate cordinate)
	{
		return statusCoorOccaption.Keys.Any(cor => cor.Equals(cordinate));
	}


	public virtual IShip DeepCopy()
	{
		return (IShip)MemberwiseClone();
	}

}
