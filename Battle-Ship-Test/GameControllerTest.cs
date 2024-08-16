using Components.Battle.Ship;
using Components.Player;
using Moq;

namespace Battle_Ship_Test;

public class GameControllerTest
{
	private GameController _gameController;
	private Mock<IPlayer> _player1;
	private Mock<IPlayer> _player2;
	
	[SetUp]
	public void Setup()
	{
		_player1 = new Mock<IPlayer>();
		_player2 = new Mock<IPlayer>();
		_player1.SetupProperty(p => p.Name , "test1");
		_player2.SetupProperty(p => p.Name, "test2");
		_gameController = new GameController(new List<IPlayer> { _player1.Object, _player2.Object },null);
	}

	[Test]
	public void GetCurrentPlayer_WhenCalled_ReturnsCurrentPlayer()
	{
		//arrange
		var currentPlayer = _gameController.GetCurrentPlayer();
		//assert
		Assert.IsNotNull(currentPlayer);
	}
	[Test]
	public void GetPlayers_WhenCalled_ReturnsPlayers()
	{
		//arrange
		var players = _gameController.GetPlayers();
		//assert
		Assert.IsNotNull(players);
	}
	[Test]
	public void GetShipBoard_WhenCalled_ReturnsShipBoard()
	{
		//arrange
		var shipBoard = _gameController.GetShipBoard(_player1.Object);
		//assert
		Assert.IsNotNull(shipBoard);
	}
	[Test]
	public void GetShipHasHit_WhenCalled_ReturnsShipHasHit()
	{
		//arrange
		var shipHasHit = _gameController.GetShipHasHit(new Coordinate(0, 0));
		//assert
		Assert.IsNotNull(shipHasHit);
	}
	[Test]
	public void GetAttackBoard_WhenCalled_ReturnsAttackBoard()
	{
		//arrange
		var attackBoard = _gameController.GetAttackBoard(_player1.Object);
		//assert
		Assert.IsNotNull(attackBoard);
	}
	[Test]
	public void IsAllShipShunk_WhenCalled_ReturnsIsAllShipShunk()
	{
		//arrange
		var isAllShipShunk = _gameController.IsAllShipShunk(_player1.Object);
		bool expected = false;
		//assert
		Assert.That(expected, Is.EqualTo(isAllShipShunk));
	}
	[TestCase(0,0)]
	[TestCase(1,0)]
	[TestCase(2,1)]
	[TestCase(3,2)]
	public void ShotShip_WhenHit_ReturnsTrue(int x, int y)
	{
		//arrange
		var isHit = _gameController.ProcessShotResult(_player1.Object, new Coordinate(x, y));
		//assert
		Assert.IsTrue(isHit);
	}
	[TestCase(7,8)]
	[TestCase(8,8)]
	[TestCase(9,8)]
	[TestCase(9,9)]
	public void ShotShip_WhenMiss_ReturnsFalse(int x, int y)
	{
		//arrange
		var isHit = _gameController.ProcessShotResult(_player1.Object, new Coordinate(x, y));
		//assert
		Assert.IsFalse(isHit);
	}
	[Test]
	public void IsGameOver_WhenCalled_ReturnsIsGameOver()
	{
		//arrange
		var isGameOver = _gameController.IsGameOver();
		//assert
		Assert.IsFalse(isGameOver);
	}
	
	
}