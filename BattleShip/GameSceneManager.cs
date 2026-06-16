using System;
using Microsoft.Extensions.DependencyInjection;
using MonoGameLibrary;
using MonoGameLibrary.Scenes;

namespace BattleShip;

public sealed class GameSceneManager
{
    // The application's service provider used to resolve scene instances and their dependencies.
    private readonly IServiceProvider _serviceProvider;

    public GameSceneManager(IServiceProvider serviceProvider)
    {
        // Store the DI container reference for later scene resolution.
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Directly switches to the given scene instance.
    /// This method assumes the scene is already constructed and ready to use.
    /// </summary>
    /// <param name="scene">The scene instance to activate.</param>
    public void ChangeScene(Scene scene)
    {
        // Delegates the actual scene switch to the core engine.
        Core.ChangeScene(scene);
    }

    /// <summary>
    /// Creates and switches to a scene of the specified type using dependency injection.
    /// The scene and all of its dependencies are resolved automatically from the service container.
    /// </summary>
    /// <typeparam name="TScene">The type of scene to create and activate.</typeparam>
    public void ChangeScene<TScene>() where TScene : Scene      //only Scenes are allowd
    {
        // Resolve the scene instance from the DI container and switch to it.
        // This ensures all constructor dependencies are properly injected.
        ChangeScene(_serviceProvider.GetRequiredService<TScene>());
    }
}
