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
	public OccopationType _occopationType { get; set; }
	public Orientation Orientation { get; set; }
	[DataMember]
	private List<Cordinate> _cordinates;
	[DataMember]
	public string ShipName { get; set; }
	public Dictionary<Cordinate, OccopationType> statusOccaption = new ();

	public Ship(ShipType shipType, OccopationType occopationType, int sizeShip, string shipName)
	{
		_shipType = shipType;
		_occopationType = occopationType;
		_sizeShip = sizeShip;
		ShipName = shipName;
		_hits = 0;

	}
	public Ship(){}

	
	
	public int GetShipSize()
	{
		return _sizeShip;
	}
	public bool setCordinates(List<Cordinate> cordinates)
	{
		if (cordinates.Count != _sizeShip)
		{
			Console.WriteLine("Error: The number of cordinates is not equal to the size of the ship.");
			return false;
		}
		_cordinates = cordinates;
		return true;
	}
	
	private void SetstatusOccaption(Cordinate cordinate , OccopationType occopationType)
	{
		statusOccaption.Add(cordinate, occopationType);
	}

	public bool IsShunk()
	{
		return _hits == _sizeShip;
	}

	public List<Cordinate> GetCordinates()
	{
		return _cordinates;
	}

	public bool IsHit(Cordinate cordinate)
	{
		if (IsCordinateInShip(cordinate))
		{
			_hits++;
			SetstatusOccaption(cordinate, OccopationType.Hit);
			
			return true;
		}
		return false;
	}
	
	private bool IsCordinateInShip(Cordinate cordinate)
	{
		foreach (var cor in _cordinates)
		{
			if (cor.x == cordinate.x && cor.y == cordinate.y)
			{
				return true;
			}
			
		}
		return false;
	}

}
