using System.Runtime.Serialization;

namespace Components.Battle.Ship;

[DataContract]
public class Cordinate
{
	[DataMember]
	public readonly int x;
	[DataMember]
	public readonly int y;
	
	public Cordinate(int x, int y)
	{
		this.x = x;
		this.y = y;
	}
	public Cordinate(){}
}
