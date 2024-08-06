namespace Components.Battle.Ship;

public class ShipBoard : Board<Ship>
{
	
	public ShipBoard(List<Ship> ships) : base()
	{
		SetListShips(ships);
		SetShipInBoard();
		
	}

	#region Checking if the ship is hit
	public bool IsAllShinked()
	{
		return ships.TrueForAll(ship => ship.IsShunk());
	}
	
	public bool IsAnyShipHit(Cordinate cordinate)
	{
		return ships.Exists(ship => ship.IsHit(cordinate));
	}
	#endregion
	
	#region Setting the ship in the board
	public override void SetShipInBoard()
	{
		foreach(var ship in ships)
		{
			foreach(var cordinate in ship.GetCordinates())
			{
				board[cordinate.x,cordinate.y] = ship;
			}
			
		}
		
	}
	
	public List<Ship> GetAllShips()
	{	
		return ships;
	}
	public bool AddShip(Ship ship)
	{
		if(ships.Contains(ship))
		{
			Console.WriteLine("Error: Ship already exists in the board.");
			return false;
	
		}
		ships.Add(ship);
		return true;
	}
	#endregion
	
	#region Checking if the ship is placed in the board
	public bool PlaceShip(Ship ship, Cordinate from, Cordinate to)
{
	// Hitung jarak antara 'from' dan 'to'
	int shipWidth = Math.Abs(from.x - to.x) + 1; // Termasuk titik awal
	int shipHeight = Math.Abs(from.y - to.y) + 1; // Termasuk titik awal

	// Validasi ukuran kapal
	if (ship.GetShipSize() != shipWidth && ship.GetShipSize() != shipHeight)
	{
		Console.WriteLine("Error: Ukuran kapal tidak sesuai.");
		return false;
		
	}

	// Validasi penempatan di papan
	if (!IsPlacementValid(from, to))
	{
		Console.WriteLine("Error: Penempatan kapal tidak valid.");
		return false;
	}

	// Pastikan tidak ada kapal lain yang bertabrakan
	if (IsShipPlaced(from, to))
	{
		Console.WriteLine("Error: Kapal sudah diletakkan di papan.");
		return false;
	}

	// Atur koordinat kapal
	

	return ship.setCordinates(CalculateShipCordinates(from, to));
}

	
	private List<Cordinate> CalculateShipCordinates(Cordinate from, Cordinate to)
{
	var cordinates = new List<Cordinate>();

	// Pastikan koordinat dari ke ke (to) diurutkan dengan benar
	int startX = Math.Min(from.x, to.x);
	int endX = Math.Max(from.x, to.x);
	int startY = Math.Min(from.y, to.y);
	int endY = Math.Max(from.y, to.y);

	if (from.x == to.x)
	{
		// Kapal diletakkan secara vertikal
		for (int y = startY; y <= endY; y++)
		{
			cordinates.Add(new Cordinate(from.x, y));
		}
	}
	else if (from.y == to.y)
	{
		// Kapal diletakkan secara horizontal
		for (int x = startX; x <= endX; x++)
		{
			cordinates.Add(new Cordinate(x, from.y));
		}
	}
	else
	{
		// Koordinat tidak valid untuk kapal
		throw new ArgumentException("Coordinates do not form a valid ship placement.");
	}

	return cordinates;
}

	private bool IsPlacementValid(Cordinate from, Cordinate to)
{
	// Memeriksa apakah koordinat berada dalam batas papan
	if (from.x < 0 || from.y < 0 || to.x < 0 || to.y < 0 ||
		from.x >= GetBoardRowLength() || from.y >= GetBoardColumnLength() ||
		to.x >= GetBoardRowLength() || to.y >= GetBoardColumnLength())
	{
		return false;
	}

	// Memeriksa apakah koordinat membentuk garis horizontal atau vertikal
	if (from.x == to.x)
	{
		// Kapal diletakkan secara vertikal
		return (from.y <= to.y);
	}
	else if (from.y == to.y)
	{
		// Kapal diletakkan secara horizontal
		return (from.x <= to.x);
	}
	else
	{
		// Kapal tidak diletakkan secara horizontal atau vertikal
		return false;
	}
}

	
	private bool IsShipPlaced(Cordinate from, Cordinate to)
	{
		return ships.Exists(ship => ship.GetCordinates().Contains(from) && ship.GetCordinates().Contains(to));
	}
	

	#endregion
}
