﻿using SFML.Graphics;
using SFML.System;
public class Hex
{
    public int Q { get; set; }
    public int R { get; set; }
    public int S { get { return -Q - R; } }
    public TileType TileType { get; set; }
    public Sprite Sprite { get; set; }
    public Text text = new Text("", Strategos.font);
    public static Dictionary<Direction, Cube> directionDictionary { get; set; }
    public static Dictionary<TileType, IntRect> tileIntRect { get; set; }
    public Hex(int Q, int R)
    {
        this.Q = Q;
        this.R = R;
        Sprite = new Sprite(Strategos.tileTexture);
        TileType = TileType.Mountain;
        Sprite.TextureRect = tileIntRect[TileType];
        Sprite.Origin = new Vector2f(0, 0);
        Sprite.Position = CubeToPixel(new Cube(Q, R, S)) - new Vector2f(Strategos.TILE_WIDTH / 2, Strategos.TILE_HEIGHT / 2 );
        text = new Text($"Q:{Q} R:{R} S:{S}", Strategos.font, 12);
        text.Position = Sprite.Position + new Vector2f(5, 40);
        text.FillColor = Color.Black;
        text.FillColor = Color.Black;
        
    }
    static Hex()
    {
        directionDictionary = new Dictionary<Direction, Cube>();
        directionDictionary.Add(Direction.NW, new Cube(0, -1, 1));
        directionDictionary.Add(Direction.NE, new Cube(1, -1, 0));
        directionDictionary.Add(Direction.E, new Cube(1, 0, -1));
        directionDictionary.Add(Direction.SE, new Cube(0, 1, -1));
        directionDictionary.Add(Direction.SW, new Cube(-1, 1, 0));
        directionDictionary.Add(Direction.W, new Cube(-1, 0, 1));

        tileIntRect = new Dictionary<TileType, IntRect>();
        tileIntRect.Add(TileType.Grass, new IntRect(0, 0, Strategos.TILE_WIDTH, Strategos.TILE_HEIGHT + 10));
        tileIntRect.Add(TileType.Mountain, new IntRect(97, 0, Strategos.TILE_WIDTH, Strategos.TILE_HEIGHT + 10));
    }
    public static Hex GetFromCube(Cube cube, HexStorage hexStorage)
    {
        Vector2i axial = Cube.CubeToAxial(cube);
        return hexStorage.GetHex(axial.X, axial.Y, -axial.X - axial.Y);
    }
    public void SetType(TileType tileType)
    {
        TileType = tileType;
        Sprite.TextureRect = tileIntRect[TileType];
    }
    public static Cube PixelToCube(Vector2f pixel)
    {
        float size = Strategos.TILE_HEIGHT / 2;
        float root = Strategos.ROOTOFTHREE;
        float q = (((root / 3f * pixel.X) - (1f / 3f) * pixel.Y) / size);
        float r = ((2f / 3f) * pixel.Y) / size;

        Cube cube = Cube.CubeRound(Cube.AxialToCube(new Vector2f(q, r)));

        return cube;
    }



    public static Vector2f CubeToPixel(Cube hex)
    {
        float x = (Strategos.TILE_HEIGHT / 2f) * (Strategos.ROOTOFTHREE * hex.Q + (Strategos.ROOTOFTHREE / 2 * hex.R)) ;
        float y = Strategos.TILE_HEIGHT / 2f * (3f / 2f * hex.R);
        return new Vector2f(x, y);
    }
    public static int Distance(Hex a, Hex b)
    {
        return (int)((Math.Abs(a.Q - b.Q) + Math.Abs(a.R - b.R) + Math.Abs(a.S - b.S)) / 2);
    }

    public void Draw(RenderWindow window)
    {     
        window.Draw(Sprite);
        //window.Draw(text);
    }
}
