using SFML.Graphics;
using SFML.System;

public class Unit
{
    public const int CHARACTER_WIDTH = 22;
    public const int CHARACTER_HEIGHT = 52;
    int Hp { get; set; }
    int Attack { get; set; }
    int Defense { get; set; }
    int Movement { get; set; }
    int Range { get; set; }
    Sprite Sprite { get; set; }
    Cube Position { get; set; }
    public static List<Unit> unitList { get; set; }
    private static Vector2i iconSize = new Vector2i(63, 68);
    public Unit(Cube Position)
    {  
        this.Position = Position;
        Sprite = new Sprite(Strategos.iconsTexture);
        Sprite.TextureRect = new IntRect(0, 0, iconSize.X, iconSize.Y);
        Sprite.Origin = new Vector2f(0, 0);
        Sprite.Position = Hex.CubeToPixel(Position) - new Vector2f(iconSize.X / 2, iconSize.Y / 2);
        unitList.Add(this);
    }
    static Unit()
    {
        unitList = new List<Unit>();
    }
    public void Draw(RenderWindow window)
    {
        window.Draw(Sprite);
    }
}
