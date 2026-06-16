using BattleShip.Features.CreateShips;
using BattleShip.Features.GameOption;
using Microsoft.Extensions.DependencyInjection;


var services = new ServiceCollection();

//Game
services.AddSingleton<GameOptionScene>();
services.AddSingleton<GameOptionController>();
services.AddSingleton<GameOptionPanel>();

//Create Ships
services.AddSingleton<CreateShipsScene>();
services.AddSingleton<CreateShipsController>();
services.AddSingleton<GameOptionPanel>();

//Options

services.AddSingleton<GameOptionScene>();
services.AddSingleton<GameOptionController>();
services.AddSingleton<GameOptionPanel>();
services.AddSingleton<BattleShip.Game1>();

var serviceProvider = services.BuildServiceProvider();

using var game = serviceProvider.GetService<BattleShip.Game1>();
game.Run();
