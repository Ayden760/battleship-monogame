using Microsoft.Xna.Framework;
using MonoGameLibrary;
using BattleShip.Services;

public static class ShipDraw
{
    public static void DrawHitStart(int pixelX, int pixelY)
    {
        Assets.G_CrossSprite.Draw(Core.SpriteBatch, new Vector2(pixelX, pixelY));
    }
    public static void DrawMiddle(int pixelX, int pixelY)
    {
        Assets.G_CrossSprite.Draw(Core.SpriteBatch, new Vector2(pixelX, pixelY));
    }
    public static void DrawHitEnd(int pixelX, int pixelY)
    {
        Assets.G_CrossSprite.Draw(Core.SpriteBatch, new Vector2(pixelX, pixelY));
    }
    public static void DrawDestroyedStart(int pixelX, int pixelY)
    {
        Assets.R_CrossSprite.Draw(Core.SpriteBatch, new Vector2(pixelX, pixelY));
    }
    public static void DrawDestroyedMiddle(int pixelX, int pixelY)
    {
        Assets.R_CrossSprite.Draw(Core.SpriteBatch, new Vector2(pixelX, pixelY));
    }
    public static void DrawDrestroyedEnd(int pixelX, int pixelY)
    {
        Assets.R_CrossSprite.Draw(Core.SpriteBatch, new Vector2(pixelX, pixelY));
    }
    public static void DrawMiss(int pixelX, int pixelY)
    {
        Assets.Blue_CrossSprite.Draw(Core.SpriteBatch, new Vector2(pixelX, pixelY));
    }
}