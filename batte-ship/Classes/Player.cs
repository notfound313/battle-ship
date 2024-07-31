using Components.Player;

namespace Components.Battle.Ship;

public class Player : IPlayer
{
	public string Name { get; set;}
	
	public Player (string name)
	{
		Name = name;
	}
	
	
}
