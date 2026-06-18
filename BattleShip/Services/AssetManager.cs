using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;
using Microsoft.Xna.Framework.Content;
namespace BattleShip.Services;


public static class Assets

{
    public static SpriteFont ArialBig { get; private set; }
    public static SpriteFont Arial { get; private set; }
    public static SpriteFont ArialSmall { get; private set; }

    public static Sprite G_CrossSprite { get; private set; }
    public static Sprite R_CrossSprite { get; private set; }
    public static Sprite B_CrossSprite { get; private set; }
    public static Sprite Blue_CrossSprite { get; private set; }

    public static Tilemap Tilemap3x { get; private set; }

    public static void Load(ContentManager content)
    {
        // Fonts
        ArialBig = content.Load<SpriteFont>("fonts/ArialBig");
        Arial = content.Load<SpriteFont>("fonts/Arial");
        ArialSmall = content.Load<SpriteFont>("fonts/ArialSmall");

        // Atlases
        TextureAtlas atlas = TextureAtlas.FromFile(content, "images/atlas-definition.xml");
        TextureAtlas atlas2 = TextureAtlas.FromFile(content, "images/atlas2-definition.xml");
        TextureAtlas atlas3 = TextureAtlas.FromFile(content, "images/atlas3-definition.xml");
        TextureAtlas atlas4 = TextureAtlas.FromFile(content, "images/atlas4-definition.xml");

        G_CrossSprite = atlas.CreateSprite("yellowCross");
        G_CrossSprite.Scale = new Vector2(0.75f, 0.75f);
        R_CrossSprite = atlas2.CreateSprite("redCross");
        R_CrossSprite.Scale = new Vector2(0.75f, 0.75f);
        B_CrossSprite = atlas3.CreateSprite("blackCross");
        B_CrossSprite.Scale = new Vector2(0.75f, 0.75f);
        Blue_CrossSprite = atlas4.CreateSprite("blueCross");
        Blue_CrossSprite.Scale = new Vector2(0.75f, 0.75f);
        // G_CrossSprite4x = atlas.CreateSprite("gelbeCross");
        // G_CrossSprite4x.Scale = new Vector2(1.0f, 1.0f);
        // R_CrossSprite4x = atlas2.CreateSprite("roteCross");
        // R_CrossSprite4x.Scale = new Vector2(1.0f, 1.0f);
        // S_CrossSprite4x = atlas3.CreateSprite("schwarzeCross");
        // S_CrossSprite4x.Scale = new Vector2(1.0f, 1.0f);
        Tilemap3x = Tilemap.FromFile(content, "images/tilemap-definition.xml");
        Tilemap3x.Scale = new Vector2(3.0f, 3.0f);
        // Tilemap4x = Tilemap.FromFile(content, "images/tilemap-definition.xml");
        //Tilemap4x.Scale = new Vector2(4.0f, 4.0f);

    }
}