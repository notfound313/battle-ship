using System.Runtime.Serialization;

namespace Components.Battle.Ship;

[DataContract]
public class Cordinate
{
	[DataMember]
	public  int x {get;set;}
	[DataMember]
	public  int y {get;set;}
	
	public Cordinate(int x, int y)
	{
		this.x = x;
		this.y = y;
	}
	public Cordinate(){}
}
