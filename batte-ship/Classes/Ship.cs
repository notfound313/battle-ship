namespace Components.Battle.Ship;

public abstract class  Ship: IShip
{
	private int _hits ;
	private int _sizeShip;
	private ShipType? _shipType;
	private OccopationType? _occopationType;
	private List<Cordinate> _cordinates;
	public string ShipName { get;  set;}
	public  int GetShipSize()
	{
		return _sizeShip;
	}
	public Ship(ShipType shipType, OccopationType occopationType, int sizeShip, string shipName)
	{
		_shipType = shipType;
		_occopationType = occopationType;
		_sizeShip = sizeShip;
		ShipName = shipName;
		_hits = 0;
		
	}
	public void setCordinates(List<Cordinate> cordinate)
	{
		_cordinates = cordinate ;
	}
	
	public bool IsShunk()
	{
		return _hits == _sizeShip;
	}
	
	public List<Cordinate> getCordinates()
	{
		return _cordinates;
	}
	
	public bool IsHit(Cordinate cordinate)
	{
		if(_cordinates.Contains(cordinate))
		{
			_hits++;
			return true;
		}
		return false;
	}
	
}
