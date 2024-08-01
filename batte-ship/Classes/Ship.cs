namespace Components.Battle.Ship;

public abstract class Ship : IShip
{
	private int _hits;
	public readonly int _sizeShip;
	private readonly ShipType _shipType;
	private OccopationType? _occopationType;
	public Orientation Orientation { get; set; }
	private List<Cordinate> _cordinates;
	public string ShipName { get; set; }

	public Ship(ShipType shipType, OccopationType occopationType, int sizeShip, string shipName)
	{
		_shipType = shipType;
		_occopationType = occopationType;
		_sizeShip = sizeShip;
		ShipName = shipName;
		_hits = 0;

	}
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
