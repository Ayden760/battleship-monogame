using BattleShip.Features.CreateShips;
using BattleShip.Features.GameOption;
using BattleShip.Features.Game;
using Microsoft.Extensions.DependencyInjection;
using BattleShip;


var services = new ServiceCollection();

//Game
services.AddSingleton<GameScene>();
services.AddSingleton<GameController>();
services.AddSingleton<GamePanel>();
services.AddSingleton<GameSceneManager>();

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
