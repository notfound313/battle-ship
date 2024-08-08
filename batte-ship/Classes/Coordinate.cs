using System.Runtime.Serialization;

namespace Components.Battle.Ship;

[DataContract]
public class Coordinate
{
	[DataMember]
	public  int x {get;set;}
	[DataMember]
	public  int y {get;set;}
	
	public Coordinate(int x, int y)
	{
		this.x = x;
		this.y = y;
	}
	public Coordinate(){}
	public override bool Equals(object? obj)
	{
		if (obj is Coordinate coordinate)
		{
			return x == coordinate.x && y == coordinate.y;
		}
		return false;
	}
	public override int GetHashCode()
	{
		return HashCode.Combine(x, y);
	}
}
