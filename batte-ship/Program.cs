using System.Runtime.Serialization;
using System.Collections.Generic;
using Components.Battle.Ship;
class Program
{
	static void Main(string[] args)
	{
		Ship battleShip = new BattleShip("BattleShip");
		Ship cruiserShip = new CruiserShip("CruiserShip");
		Ship destroyerShip = new DestroyerShip("DestroyerShip");
		Ship submarineShip = new SubmarineShip("SubmarineShip");
		Ship carrierShip = new CarrierShip("CarrierShip");
		
		List<Ship> ships = new List<Ship>
		{
			battleShip,
			cruiserShip,
			destroyerShip,
			submarineShip,
			carrierShip
		};
		
		int x = 0;
		foreach (var item in ships)
		{
			List<Cordinate> cordinates = new List<Cordinate>();
			
			for (int i = 0; i < item.GetShipSize(); i++)
			{
				cordinates.Add(new Cordinate(i, x));
				
			}
			x++;
			item.setCordinates(cordinates);
			
		}
		var knowTypes = new List<Type>{typeof(BattleShip), typeof(CruiserShip), typeof(DestroyerShip), typeof(SubmarineShip), typeof(CarrierShip), typeof(Cordinate)};
		DataContractSerializer dataContract = new DataContractSerializer(typeof(List<Ship>), knowTypes);
		using(FileStream fs = new FileStream("ships.xml", FileMode.Create))
		{
			dataContract.WriteObject(fs, ships);
		}
		
		List<Ship> ships2 = new List<Ship>();
		
		using(FileStream fs = new FileStream("ships.xml", FileMode.Open))
		{
			ships2 = (List<Ship>)dataContract.ReadObject(fs);
			
		}
		
		foreach (var item in ships2)
		{
			Console.WriteLine(item.ShipName);
		}
		
		
		
		
		

		
		
		
	}
	
}