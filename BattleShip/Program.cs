using BattleShip.Features.CreateShips;
using BattleShip.Features.GameOption;
using BattleShip.Features.Game;
using Microsoft.Extensions.DependencyInjection;
using BattleShip;
using BattleShip.GameData;
using BattleShip.Services;
using BattleShip.GameObjects;
using BattleShip.Functions;


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

//Services
services.AddSingleton<InputHandler>();

//Player AI
services.AddSingleton<Player>();
services.AddSingleton<AI>();

//GameValidations
services.AddSingleton<GameValidations>();


services.AddSingleton<ShipSetter>();
services.AddSingleton<AiShipSetter>();
services.AddSingleton<ShipPlacer>();



var serviceProvider = services.BuildServiceProvider();

using var game = serviceProvider.GetService<Game1>();
game.Run();
