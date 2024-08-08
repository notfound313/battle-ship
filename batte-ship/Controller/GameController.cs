using System.Runtime.Serialization;
using System.Collections.Generic;

using Components.Player;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Components.Battle.Ship;

public class GameController
{
	private List<IPlayer> _players;
	
	private Dictionary<IPlayer, AttackBoard> _attackBoards = new();
	private Dictionary<IPlayer, ShipBoard> _shipBoards = new();
	private IPlayer _currentPlayer;	
	private Dictionary<IPlayer, List<Ship>> _shipsPlayer = new();
	private Dictionary<IPlayer, List<Coordinate>> _cordinates = new();
	



	public GameController(List<IPlayer> players)
	{
		//set players in list
		_players = players;
		//set players for each player
		
		//read cordinates ship from file
		var knowTypes = new List<Type>
	{
		typeof(BattleShip),
		typeof(CruiserShip),
		typeof(DestroyerShip),
		typeof(SubmarineShip),
		typeof(CarrierShip),
		typeof(Coordinate)
	};

		DataContractSerializer dataContract = new DataContractSerializer(typeof(List<Ship>), knowTypes);

		// try
		// {
		// 	// read XML once.
		// 	List<Ship> ship;	
		// 	using (FileStream fs = new FileStream(@".\ships.xml", FileMode.Open))
		// 	{
		// 		ship = (List<Ship>)dataContract.ReadObject(fs);
		// 	}

		// 	// duplicate data
		// 	_ships_p1 = new List<Ship>(ship);
		// 	_ships_p2 = new List<Ship>(ship);


		// }
		// catch (FileNotFoundException ex)
		// {
		// 	// handle the error of file not found.
		// 	throw new ArgumentException($"File tidak ditemukan: {ex.Message}");
		// }
		// catch (SerializationException ex)
		// {
		// 	// handle the error of deserialisasi.
		// 	throw new ArgumentException($"Kesalahan deserialisasi: {ex.Message}");
		// }
		// catch (Exception ex)
		// {
		// 	// handle the error
		// 	throw new ArgumentException($"Terjadi kesalahan: {ex.Message}");
		// }
		List<Ship> _ships_p1 = new();
		List<Ship> _ships_p2 = new();

		using (FileStream fs = new FileStream(@".\ships.xml", FileMode.Open))
		{
			_ships_p1 = (List<Ship>)dataContract.ReadObject(fs);
		}



		using (FileStream fs = new FileStream(@".\ships.xml", FileMode.Open))
		{
			_ships_p2 = (List<Ship>)dataContract.ReadObject(fs);
		}

		//set ships for each player
		_shipsPlayer.Add(_players[0], _ships_p1);
		_shipsPlayer.Add(_players[1], _ships_p2);

		//set attack boards and ship boards for each player
		_attackBoards[_players[0]] = new AttackBoard(_shipsPlayer[_players[1]]);
		_shipBoards[_players[0]] = new ShipBoard(_shipsPlayer[_players[0]]);


		_shipBoards[_players[1]] = new ShipBoard(_shipsPlayer[_players[1]]);
		_attackBoards[_players[1]] = new AttackBoard(_shipsPlayer[_players[0]]);

		//set current player and next player
		_currentPlayer = _players[0];
		
	}




	public bool PlaceShipsOnBoard(IPlayer player, ShipType shipType, Coordinate from, Coordinate to)
	{
		var shipBoard = _shipBoards[player];
		var ships = _shipsPlayer[player];
		foreach (var ship in ships)
		{
			if (ship._shipType == shipType)
			{

				return shipBoard.PlaceShip(ship, from, to);

			}
		}

		return false;
	}
	public bool HasPlayer(IPlayer player)
	{
		return _players.Contains(player);
	}
	public bool IsShotHit(IPlayer player, Coordinate cordinate)
	{
		var attackBoard = _attackBoards[player];
		return attackBoard.IsHit(cordinate);
	}
	public List<Coordinate> GetMissedAttackBoard(IPlayer player)
	{
		var attackBoard = _attackBoards[player];
		return attackBoard.GetMissedAttacks();
	}
	public List<Coordinate> GetMissedShipBoard(IPlayer player)
	{
		var shipBoard = _shipBoards[player];
		return shipBoard.GetMissedAttacks();
	}


	public bool ProcessShotResult(IPlayer player, Coordinate cordinate)
	{
		var attackBoard = _attackBoards[player];
		var shipBoard = _shipBoards[player];
		if (IsShotHit(player, cordinate))
		{

			return attackBoard.SetHit(cordinate);
		}
		SwitchPlayer();
		shipBoard.SetMissAttack(cordinate);
		attackBoard.SetMissAttack(cordinate);
		return false;
	}

	public IPlayer GetCurrentPlayer()
	{
		return _currentPlayer;
	}

	private void SwitchPlayer()
	{
		foreach (var player in _players)
		{
			if (player != _currentPlayer)
			{
				_currentPlayer = player;
				return;
			}
		}
	}


	public List<IPlayer> GetPlayers()
	{
		return _players;
	}

	public Ship[,] GetShipBoard(IPlayer player)
	{
		return _shipBoards[player].GetBoard();

	}

	public Ship GetShipHasHit(Coordinate cordinate)
	{
		var attckBoard = _attackBoards[_currentPlayer];
		return attckBoard.GetShipHited(cordinate);
	}

	public Ship[,] GetAttckBoard(IPlayer player)
	{
		return _attackBoards[player].GetBoard();

	}

	public bool IsAllShipShunk(IPlayer player)
	{
		var shipBoard = _shipBoards[player];
		return shipBoard.IsAllShinked();

	}

	public Ship GetOneShip(IPlayer player, ShipType type)
	{
		var shipBoard = _shipBoards[player];
		return shipBoard.GetShip(type);
	}



	private bool CheckWinner(IPlayer player)
	{
		var shipBoard = _shipBoards[player];
		return shipBoard.IsAllShinked();
	}
	private IPlayer GetWinner()
	{
		foreach (var player in _players)
		{
			if (CheckWinner(player))
			{
				return player;
			}
		}
		return null;
	}

	public bool IsGameOver()
	{
		return GetWinner() != null;
	}

}

