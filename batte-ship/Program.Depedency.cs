using Components.Battle.Ship;
using Components.Player;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

partial  class Program
{
	static void DepedencyInjection()
	{
		IServiceCollection servicesCollection = new ServiceCollection();
		servicesCollection.AddLogging(logBuilder => 
		{
			logBuilder.ClearProviders();
			logBuilder.SetMinimumLevel(LogLevel.Information);
			logBuilder.AddNLog("NLog.config");
		});
		ServiceProvider serviceProvider = servicesCollection.BuildServiceProvider();
		GameController gm = serviceProvider.GetRequiredService<GameController>();
		
	}
	
	
}
