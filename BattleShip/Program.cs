using BattleShip.Features.CreateShips;
using BattleShip.Features.GameOption;
using BattleShip.Features.Game;
using Microsoft.Extensions.DependencyInjection;
using BattleShip;
using BattleShip.GameData;
using BattleShip.Services;
using BattleShip.GameObjects;
using BattleShip.Functions;
using Microsoft.EntityFrameworkCore;


var services = new ServiceCollection();
var dbPath = GameDbContext.GetDatabasePath();
services.AddDbContext<GameDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"), ServiceLifetime.Scoped);
//Game

services.AddTransient<GameScene>();
services.AddSingleton<GameController>();
services.AddTransient<GamePanel>();
services.AddSingleton<GameSceneManager>();

//Create Ships

services.AddTransient<CreateShipsScene>();
services.AddTransient<CreateShipsController>();
services.AddTransient<CreateShipsPanel>();



//Options

services.AddTransient<GameOptionScene>();
services.AddSingleton<GameOptionController>();
services.AddTransient<GameOptionPanel>();

//title
services.AddTransient<TitlePanel>();
services.AddTransient<TitleScene>();
services.AddTransient<GameOptions>();
services.AddSingleton<Game1>();

//Settings/ Session
services.AddSingleton<GameSettings>();
services.AddSingleton<GameSession>();

//Services
services.AddSingleton<InputHandler>();

services.AddTransient<EscPanel>();

//GameValidations
services.AddSingleton<GameValidations>();


services.AddSingleton<ShipSetter>();
services.AddSingleton<AiShipSetter>();
services.AddSingleton<ShipPlacer>();

var serviceProvider = services.BuildServiceProvider();


using (var scope = serviceProvider.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<GameDbContext>();
    db.Database.Migrate();
}


using var game = serviceProvider.GetRequiredService<Game1>();
game.Run();
