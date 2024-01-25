using SFML.Graphics;
using SFML.System;

public class Unit
{
    int Hp { get; set; }
    int Attack { get; set; }
    int Defense { get; set; }
    int Movement { get; set; }
    int Range { get; set; }
    Sprite Sprite { get; set; }
    Cube Position { get; set; }
    public static List<Unit> unitList { get; set; }
    public Unit(Cube Position)
    {  
        this.Position = Position;
        Sprite = new Sprite(Strategos.characterTexture);
        Sprite.TextureRect = new IntRect(0, 0, Strategos.CHARACTER_WIDTH, Strategos.CHARACTER_HEIGHT);
        Sprite.Origin = new Vector2f(0, 0);
        Sprite.Position = Hex.CubeToPixel(Position) - new Vector2f(Strategos.CHARACTER_WIDTH / 2, Strategos.CHARACTER_HEIGHT / 2);
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
