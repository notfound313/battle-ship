using System.Runtime.Serialization;
using System.Collections.Generic;

using Components.Player;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Text.Json.Serialization;
using NLog;
using Microsoft.Extensions.Logging;
namespace Components.Battle.Ship;

public class GameController
{
	private ILogger<GameController>? _logger ;
	private List<IPlayer>? _players;
	private int maxPlayers = 2;
	

	private Dictionary<IPlayer, ShipBoard> _shipBoards = new();
	private IPlayer _currentPlayer;	
	private Dictionary<IPlayer, List<Ship>> _shipsPlayer = new();
	
	



	public GameController(List<IPlayer> players, ILogger<GameController>? logger = null)
	{
		//set players in list		
		if (players.Count > maxPlayers)
		{
			throw new ArgumentException("Max players is 2");
		}
		_players = players;
		
		//read cordinates ship from file
		
		List<Ship> ships_p1 = new(); 
		List<Ship> ships_p2 = new();
	
		var knowTypes = new List<Type> { typeof(BattleShip), typeof(CruiserShip), typeof(DestroyerShip), typeof(SubmarineShip), typeof(CarrierShip), typeof(Coordinate) };
		DataContractSerializer dataContract = new DataContractSerializer(typeof(List<Ship>), knowTypes);

		using (FileStream fs = new FileStream(@".\ships.xml", FileMode.Open))
		{

			ships_p1 = (List<Ship>)dataContract.ReadObject(fs);
			
		}
			using (FileStream fs = new FileStream(@".\ships.xml", FileMode.Open))
		{

			ships_p2 = (List<Ship>)dataContract.ReadObject(fs);
			
		}
	
		
		
		//set ships for each player
		_shipsPlayer.Add(players[0], ships_p1);
		_shipsPlayer.Add(players[1], ships_p2);
		
		//set attack boards and ship boards for each player
	
		_shipBoards[players[0]] = new ShipBoard(_shipsPlayer[players[0]]);
		_shipBoards[players[1]] = new ShipBoard(_shipsPlayer[players[1]]);
		

		//set current player and next player
		_currentPlayer =players[0];
		
		//set Logger
		_logger = logger;
		
	}




	public bool PlaceShipsOnBoard(IPlayer player, ShipType shipType, Coordinate from, Coordinate to)
	{
		var shipBoard = _shipBoards[player];
		var ships = _shipsPlayer[player];
		foreach (var ship in ships)
		{
			if (ship._shipType == shipType)
			{
				_logger?.LogInformation($"Ship type: {shipType} reaplaced From: {from} To: {to}");
				return shipBoard.PlaceShip(ship, from, to);

			}
		}
		
		return false;
	}
	public bool HasPlayer(IPlayer player)
	{
		return _players.Contains(player);
	}
	public bool IsShotHit(IPlayer player, Coordinate coordinate)
	{
		var attackBoard = _shipBoards[GetNextPlayer()];
		_logger?.LogInformation($"Shot hit: {coordinate}");
		return attackBoard.IsHit(coordinate);
	}
	public List<Coordinate> GetMissedAttackBoard()
	{
		var attackBoard = _shipBoards[GetNextPlayer()];
		return attackBoard.GetMissedAttacks();
	}
	public List<Coordinate> GetMissedShipBoard(IPlayer player)
	{
		var shipBoard = _shipBoards[player];
		return shipBoard.GetMissedAttacks();
	}


	public bool ProcessShotResult(IPlayer player, Coordinate coordinate)
	{
		var attackBoard = _shipBoards[GetNextPlayer()];
	   
		if (IsShotHit(player, coordinate) && !attackBoard.IsShipAnyHit(coordinate))
		{
			_logger?.LogInformation($"Shoot on Cordinate x {coordinate.x} and y {coordinate.y} ");													
			return attackBoard.SetHit(coordinate);
		}
		_logger?.LogInformation("Switch Player");
		SwitchPlayer();
		_logger?.LogInformation("Miss shooting");		
		attackBoard.SetMissAttack(coordinate);
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
	
	private IPlayer GetNextPlayer()
	{
		foreach (var player in _players)
		{
			if (player != _currentPlayer)
			{
				return player;
			}
		}
		return null;
	}


	public List<IPlayer> GetPlayers()
	{
		return _players;
	}

	public Ship[,] GetShipBoard(IPlayer player)
	{
		return _shipBoards[player].GetBoard();

	}

	public Ship GetShipHasHit(Coordinate coordinate)
	{
		var attackBoard = _shipBoards[_currentPlayer];
		return attackBoard.GetShipHited(coordinate);
	}

	public Ship[,] GetAttackBoard(IPlayer player)
	{
		if (player != GetNextPlayer()){
		return _shipBoards[GetNextPlayer()].GetBoard();
		}
		return null;

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
				_logger?.LogInformation($"Player {player} won");
				return player;
			}
		}
		return null;
	}

	public bool IsGameOver()
	{
		_logger?.LogInformation("Game over");
		return GetWinner() != null;
	}

}

