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
	private OccopationType? _occopationType;
	public Orientation Orientation { get; set; }
	[DataMember]
	private List<Cordinate> _cordinates;
	[DataMember]
	public string ShipName { get; set; }

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
			return false;
		}
		_cordinates = cordinates;
		return true;
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
		if (_cordinates.Contains(cordinate))
		{
			_hits++;
			return true;
		}
		return false;
	}

}
