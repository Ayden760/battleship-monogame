using System;
using Microsoft.Extensions.DependencyInjection;
using MonoGameLibrary;
using MonoGameLibrary.Scenes;

namespace BattleShip;

public sealed class GameSceneManager
{
    private readonly IServiceProvider _serviceProvider;

    public GameSceneManager(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void ChangeScene(Scene scene)
    {
        Core.ChangeScene(scene);
    }

    public void ChangeScene<TScene>() where TScene : Scene
    {
        ChangeScene(_serviceProvider.GetRequiredService<TScene>());
    }
}
