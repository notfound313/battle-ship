using System.Collections.Generic;

using Components.Player;
namespace Components.Battle.Ship;

public class GameController
{
	private List<IPlayer> _players;
	private Dictionary<IPlayer, AttackBoard> _attackBoards;
	private Dictionary<IPlayer, ShipBoard> _shipBoards;
	private IPlayer _currentPlayer;
	private IPlayer _nextPlayer;
	private Dictionary<IPlayer,List<IShip>> _ships = new();
	private Dictionary<IPlayer,List<Cordinate>> _cordinates = new();

	private Board<AttackBoard> _attackBoard;
	private Board<ShipBoard> _shipBoard;
	
	public GameController(List<IPlayer> players)
	{
		_players = players;
		_attackBoards = new Dictionary<IPlayer, AttackBoard>();
		_shipBoards = new Dictionary<IPlayer, ShipBoard>();
		_ships = new Dictionary<IPlayer, List<IShip>>();
		_cordinates = new Dictionary<IPlayer, List<Cordinate>>();
		
		

		foreach (var player in _players)
		{
			_shipBoards.Add(player, new ShipBoard());
			_attackBoards.Add(player, new AttackBoard());
			_ships.Add(player, new List<IShip>());
			_cordinates.Add(player, new List<Cordinate>());
		}

		_attackBoard = new AttackBoard();
		_shipBoard = new ShipBoard();

		_currentPlayer = _players[0];
		_nextPlayer = _players[1];
	}
	
	public void StartGame()
	{
		foreach (var player in _players)
		{
			PlaceShips(player);
		}
	}
	
	private void PlaceShips(IPlayer player)
	{
		var shipBoard = _shipBoards[player];
		var ships = _ships[player];
		foreach (var ship in ships)
		{
			var cordinates = _cordinates[player];
			var from = cordinates[0];
			var to = cordinates[1];
			shipBoard.PlaceShip(ship, from, to);
		}
	}
	public bool Attack(IPlayer player, Cordinate cordinate)
	{
		var attackBoard = _attackBoards[player];
		if (attackBoard.IsHit(cordinate))
		{
			attackBoard.SetHit(cordinate);
			return true;
		}
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
	
	public bool ProcessShotResult(IPlayer player, Cordinate cordinate, bool isShotHit)
	{
		var attackBoard = _attackBoards[player];
		if (isShotHit)
		{
			attackBoard.SetHit(cordinate);
			return true;
		}
		return false;
	}
	
	public IPlayer GetCurrentPlayer()
	{
		return _currentPlayer;
	}
	
	public void SwitchPlayer()
	{
		var temp = _currentPlayer;
		_currentPlayer = _nextPlayer;
		_nextPlayer = temp;
	}
	
	public void SetCurrentPlayer(IPlayer player)
	{
		_currentPlayer = player;
	}
	
	public void SetNextPlayer(IPlayer player)
	{
		_nextPlayer = player;
	}
	
	public List<IPlayer> GetPlayers()
	{
		return _players;
	}
	
	public bool IsAllShipShunk(IPlayer player)
	{
		var shipBoard = _shipBoards[player];
		return shipBoard.IsAllShinked();
		
	}
	
	private bool CheckWinner(IPlayer player)
	{
		var shipBoard = _shipBoards[player];
		return shipBoard.IsAllShinked();
	}
	public IPlayer GetWinner()
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
