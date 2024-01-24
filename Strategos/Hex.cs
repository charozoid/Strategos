using SFML.Graphics;
using SFML.System;

public class Hex
{
    public int Q { get; set; }
    public int R { get; set; }
    public int S { get { return -Q - R; } }
    public Sprite Sprite { get; set; }
    public Text text = new Text("", Strategos.font);
    public Hex(int Q, int R)
    {
        this.Q = Q;
        this.R = R;
        Sprite = new Sprite(Strategos.tileTexture);
        Sprite.TextureRect = new IntRect(0, 0, Strategos.TILE_WIDTH, Strategos.TILE_HEIGHT);
        Sprite.Position = HexToPixel(this) - new Vector2f(Strategos.TILE_WIDTH / 2, Strategos.TILE_HEIGHT / 2);
        text = new Text($"Q:{Q} R:{R} S:{S}", Strategos.font, 12);
        text.Position = Sprite.Position + new Vector2f(5, 40);
        text.FillColor = Color.Black;
        text.FillColor = Color.Black;
        
    }
    public static Hex CubeToHex(Cube cube, HexStorage hexStorage)
    {
        Vector2i axial = Cube.CubeToAxial(cube);
        
        return hexStorage.GetHex(axial.X, axial.Y, -axial.X-axial.Y);
    }

    public static Cube PixelToCube(Vector2f pixel)
    {
        float size = Strategos.TILE_HEIGHT / 2;
        float root = Strategos.ROOTOFTHREE;
        float q = (((root / 3f * pixel.X) - (1f / 3f) * pixel.Y) / size);
        float r = ((2f / 3f) * pixel.Y) / size;

        Cube cube = CubeRound(Cube.AxialToCube(new Vector2f(q, r)));

        return cube;
    }

    public static Cube CubeRound(Cube cube)
    {
        float q = MathF.Round(cube.Q);
        float r = MathF.Round(cube.R);
        float s = MathF.Round(cube.S);

        float qDiff = Math.Abs(q - cube.Q);
        float rDiff = Math.Abs(r - cube.R);
        float sDiff = Math.Abs(s - cube.S);

        if (qDiff > rDiff && qDiff > sDiff)
        {
            q = -r - s;
        }
        else if (rDiff > sDiff)
        {
            r = -q - s;
        }
        else
        {
            s = -q - r;
        }
        return new Cube(q, r, s);
    }
    public static Vector2f HexToPixel(Hex hex)
    {
        float x = (Strategos.TILE_HEIGHT / 2f) * (Strategos.ROOTOFTHREE * hex.Q + (Strategos.ROOTOFTHREE / 2 * hex.R)) ;
        float y = Strategos.TILE_HEIGHT / 2f * (3f / 2f * hex.R);
        return new Vector2f(x, y);
    }


    public void Draw(RenderWindow window)
    {     
        window.Draw(Sprite);
        window.Draw(text);
    }
}
