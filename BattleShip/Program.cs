using BattleShip.Features.CreateShips;
using BattleShip.Features.GameOption;
using BattleShip.Features.Game;
using Microsoft.Extensions.DependencyInjection;
using BattleShip;
using BattleShip.GameData;


var services = new ServiceCollection();

//Game

services.AddSingleton<GameScene>();
services.AddSingleton<GameController>();
services.AddSingleton<GamePanel>();
services.AddSingleton<GameSceneManager>();

//Create Ships

services.AddSingleton<CreateShipsScene>();
services.AddSingleton<CreateShipsController>();
services.AddSingleton<CreateShipsPanel>();

//Options

services.AddSingleton<GameOptionScene>();
services.AddSingleton<GameOptionController>();
services.AddSingleton<GameOptionPanel>();


services.AddSingleton<GameOptions>();
services.AddSingleton<Game1>();

//Settings/ Session
services.AddSingleton<GameSettings>();
services.AddSingleton<GameSession>();





var serviceProvider = services.BuildServiceProvider();

using var game = serviceProvider.GetService<Game1>();
game.Run();
