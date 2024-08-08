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

	public bool IsAnyShipHit(Coordinate cordinate)
	{
		return ships.Exists(ship => ship.IsHit(cordinate));
	}
	#endregion

	#region Setting the ship in the board
	public override void SetShipInBoard()
	{
		foreach (var ship in ships)
		{
			foreach (var cordinate in ship.GetCordinates())
			{
				board[cordinate.x, cordinate.y] = ship;
			}

		}

	}

	public List<Ship> GetAllShips()
	{
		return ships;
	}
	public bool AddShip(Ship ship)
	{
		if (ships.Contains(ship))
		{
			// Console.WriteLine("Error: Ship already exists in the board.");
			return false;

		}
		ships.Add(ship);
		return true;
	}
	#endregion

	#region Checking if the ship is placed in the board
	public bool PlaceShip(Ship ship, Coordinate from, Coordinate to)
	{
		// calculate the width and height of the ship
		int shipWidth = Math.Abs(from.x - to.x) + 1;
		int shipHeight = Math.Abs(from.y - to.y) + 1;

		// validate the ship size
		if (ship.GetShipSize() != shipWidth && ship.GetShipSize() != shipHeight)
		{
			// Console.WriteLine("Error: Ukuran kapal tidak sesuai.");
			return false;

		}

		// validate the placement
		if (!IsPlacementValid(from, to))
		{
			// Console.WriteLine("Error: Penempatan kapal tidak valid.");
			return false;
		}

		// make sure the ship is not placed on the board
		if (IsShipPlaced(CalculateShipCordinates(from, to)))
		{
			// Console.WriteLine("Error: Kapal sudah diletakkan di papan.");
			
			return false;
		}

		// set coordinates

		if (ship.setCordinates(CalculateShipCordinates(from, to)))
		{
			// Delete ship on the board
			RemoveShipFromBoard(ship);

			// Set ship on the board
			SetShipInBoard();
		}


		return ship.setCordinates(CalculateShipCordinates(from, to));
	}
	private void RemoveShipFromBoard(Ship ship)
	{
		for (int x = 0; x < board.GetLength(0); x++)
		{
			for (int y = 0; y < board.GetLength(1); y++)
			{
				if (board[x, y] == ship)
				{
					board[x, y] = null;
				}
			}
		}
	}


	private List<Coordinate> CalculateShipCordinates(Coordinate from, Coordinate to)
	{
		var cordinates = new List<Coordinate>();

		// make sure the ship is placed in a straight line
		int startX = Math.Min(from.x, to.x);
		int endX = Math.Max(from.x, to.x);
		int startY = Math.Min(from.y, to.y);
		int endY = Math.Max(from.y, to.y);

		if (from.x == to.x)
		{
			// ship placed vertically
			for (int y = startY; y <= endY; y++)
			{
				cordinates.Add(new Coordinate(from.x, y));
			}
		}
		else if (from.y == to.y)
		{
			// ship placed horizontally
			for (int x = startX; x <= endX; x++)
			{
				cordinates.Add(new Coordinate(x, from.y));
			}
		}
		else
		{
			// Coordinates do not form a valid ship placement.
			throw new ArgumentException("Coordinates do not form a valid ship placement.");
		}

		return cordinates;
	}

	private bool IsPlacementValid(Coordinate from, Coordinate to)
	{
		// checking the board boundaries
		if (from.x < 0 || from.y < 0 || to.x < 0 || to.y < 0 ||
			from.x >= GetBoardRowLength() || from.y >= GetBoardColumnLength() ||
			to.x >= GetBoardRowLength() || to.y >= GetBoardColumnLength())
		{
			return false;
		}

		// checking if the ship is placed in a straight line
		if (from.x == to.x)
		{
			// ship placed vertically
			return from.y <= to.y;
		}
		else if (from.y == to.y)
		{
			// ship placed horizontally
			return from.x <= to.x;
		}
		else
		{
			// ship not placed in a straight line or outside the board boundaries
			return false;
		}
	}


	private bool IsShipPlaced(List<Coordinate> fromTo)
	{
		foreach (var coor in fromTo)
		{
			foreach (var ship in ships)
	{
		// Check if any coordinate of the ship matches the 'from' coordinate
		if (ship.GetCordinates().Exists(cordinate => cordinate.Equals(coor)))
		{
			return true; // Found a matching coordinate
		}
	}
	
		}
		
	return false;
	}


	#endregion

	public Ship GetShip(ShipType shipType)
	{
		foreach (var item in ships)
		{
			if (item._shipType == shipType)
			{
				return item;
			}
		}
		return null;
	}
	public void SetMissAttack(Coordinate cordinate)
	{
		missAttacks.Add(cordinate);
	}
}
