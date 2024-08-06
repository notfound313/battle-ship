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
	private IPlayer _player1, _player2;
	private Dictionary<IPlayer, AttackBoard> _attackBoards = new();
	private Dictionary<IPlayer, ShipBoard> _shipBoards = new();
	private IPlayer _currentPlayer;
	private IPlayer _nextPlayer;
	private Dictionary<IPlayer, List<Ship>> _shipsPlayer = new();
	private Dictionary<IPlayer, List<Cordinate>> _cordinates = new();
	private List<Ship> _ships_p1 = new();
	private List<Ship> _ships_p2 = new();



	public GameController(List<IPlayer> players)
	{
		//set players in list
		_players = players;
		//set players for each player
		_player1 = _players[0];
		_player2 = _players[1];
		
		//read cordinates ship from file
		var knowTypes = new List<Type>
	{
		typeof(BattleShip),
		typeof(CruiserShip),
		typeof(DestroyerShip),
		typeof(SubmarineShip),
		typeof(CarrierShip),
		typeof(Cordinate)
	};

	DataContractSerializer dataContract = new DataContractSerializer(typeof(List<Ship>), knowTypes);

	try
	{
		// read XML once.
		
		using (FileStream fs = new FileStream(@".\ships.xml", FileMode.Open))
		{
			_ships_p1 = (List<Ship>)dataContract.ReadObject(fs);
		}

		// duplicate data
		_ships_p2 = new List<Ship>(_ships_p1);
		
	}
	catch (FileNotFoundException ex)
	{
		// handle the error of file not found.
		throw new ($"File tidak ditemukan: {ex.Message}");
	}
	catch (SerializationException ex)
	{
		// handle the error of deserialisasi.
		throw new ($"Kesalahan deserialisasi: {ex.Message}");
	}
	catch (Exception ex)
	{
		// handle the error
		throw new ($"Terjadi kesalahan: {ex.Message}");
	}

		//set ships for each player
		_shipsPlayer.Add(_player1, _ships_p1);
		_shipsPlayer.Add(_player2, _ships_p2);
		
		//set attack boards and ship boards for each player
		_attackBoards[_player1] = new AttackBoard(_shipsPlayer[_player2]);
		_shipBoards[_player1] = new ShipBoard(_shipsPlayer[_player1]);


		_shipBoards[_player2] = new ShipBoard(_shipsPlayer[_player2]);
		_attackBoards[_player2] = new AttackBoard(_shipsPlayer[_player1]);

		//set current player and next player
		_currentPlayer =_player1;
		_nextPlayer = _player2;
	}




	public bool PlaceShipsOnBoard(IPlayer player, ShipType shipType,Cordinate from, Cordinate to)
	{
		var shipBoard = _shipBoards[player];
		var ships = _shipsPlayer[player];
		foreach (var ship in ships)
		{
			if (ship._shipType == shipType)
			{	
							
				return shipBoard.PlaceShip(ship,from,to);
				
			}
		}
		
		return false;
	}
	public bool Attack(IPlayer player, Cordinate cordinate)
	{
		var attackBoard = _attackBoards[player];
		if (attackBoard.IsHit(cordinate))
		{
			return attackBoard.SetHit(cordinate);
		}
		
		SwitchPlayer();
		return false;
	}
	public bool HasPlayer(IPlayer player)
	{
		return _players.Contains(player);
	}
	public bool IsShotHit(IPlayer player, Cordinate cordinate)
	{
		var attackBoard = _attackBoards[player];
		return attackBoard.IsHit(cordinate);
	}
	public List<Cordinate> GetMissedAttacks(IPlayer player)
	{
		var attackBoard = _attackBoards[player];
		return attackBoard.GetMissedAttacks();
	}

	public bool ProcessShotResult(IPlayer player, Cordinate cordinate)
	{
		var attackBoard = _attackBoards[player];
		if (IsShotHit(player,cordinate))
		{
			
			return attackBoard.SetHit(cordinate);
		}
		SwitchPlayer();
		attackBoard.SetMissAttack(cordinate);
		return false;
	}

	public IPlayer GetCurrentPlayer()
	{
		return _currentPlayer;
	}

	private void SwitchPlayer()
	{
		var temp = _currentPlayer;
		_currentPlayer = _nextPlayer;
		_nextPlayer = temp;
	}

	public void SetCurrentPlayer(IPlayer player)
	{
		_currentPlayer = player;
	}


	public List<IPlayer> GetPlayers()
	{
		return _players;
	}
	
	public  Ship[,] GetShipBoard(IPlayer player)
	{
		return _shipBoards[player].GetBoard();
		
	}
	
	public Ship GetShipHasHit(Cordinate cordinate)
	{
		var attckBoard = _attackBoards[_currentPlayer];
		return attckBoard.GetShipHited(cordinate);
	}
	
	public  Ship[,] GetAttckBoard(IPlayer player)
	{
		return _attackBoards[player].GetBoard();
		
	}

	public bool IsAllShipShunk(IPlayer player)
	{
		var shipBoard = _shipBoards[player];
		return shipBoard.IsAllShinked();

	}

	public Ship GetOneShip(IPlayer player, ShipType type){
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

