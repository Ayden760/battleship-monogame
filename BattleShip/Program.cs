using BattleShip.Features.CreateShips;
using BattleShip.Features.GameOption;
using BattleShip.Features.Game;
using Microsoft.Extensions.DependencyInjection;
using BattleShip;
using BattleShip.GameData;
using BattleShip.Services;
using BattleShip.GameObjects;
using System;
using BattleShip.Functions;
using System.Diagnostics;
using System.IO;


#if DEBUG
if (!RunUnitTests())
{
    Console.WriteLine("Unit tests failed. Aborting game start.");
    return;
}
#endif


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



//GameValidations
services.AddSingleton<GameValidations>();


services.AddSingleton<ShipSetter>();
services.AddSingleton<AiShipSetter>();
services.AddSingleton<ShipPlacer>();



var serviceProvider = services.BuildServiceProvider();

using var game = serviceProvider.GetService<Game1>();
game.Run();

static bool RunUnitTests()
{
    var projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
    var testProjectPath = Path.Combine(projectRoot, "BattleShip.Tests", "BattleShip.Tests.csproj");

    if (!File.Exists(testProjectPath))
    {
        Console.Error.WriteLine($"Test project not found: {testProjectPath}");
        return false;
    }

    var startInfo = new ProcessStartInfo
    {
        FileName = "dotnet",
        Arguments = $"test \"{testProjectPath}\" --nologo --no-build",
        WorkingDirectory = projectRoot,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false,
        CreateNoWindow = true,
    };

    using var process = new Process { StartInfo = startInfo };
    process.OutputDataReceived += (_, e) =>
    {
        if (!string.IsNullOrWhiteSpace(e.Data))
        {
            Console.WriteLine(e.Data);
        }
    };
    process.ErrorDataReceived += (_, e) =>
    {
        if (!string.IsNullOrWhiteSpace(e.Data))
        {
            Console.Error.WriteLine(e.Data);
        }
    };

    process.Start();
    process.BeginOutputReadLine();
    process.BeginErrorReadLine();
    process.WaitForExit();

    return process.ExitCode == 0;
}
