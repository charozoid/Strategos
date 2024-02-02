using SFML.Graphics;
using SFML.System;

public class Hex
{
    public int Q { get; set; }
    public int R { get; set; }
    public int S { get { return -Q - R; } }
    private TileType _type = TileType.Grass;
    public Region Region { get; set; }
    public TileType Type {
        get {return _type; }
        set {SetType(value); } 
    }
    private void SetType(TileType tileType)
    {
        _type = tileType;
        Sprite.TextureRect = tileIntRect[Type];
    }
    public Sprite Sprite { get; set; }
    //public Text DebugText = new Text("", Strategos.font);
    public static Dictionary<Direction, Cube> directionDictionary { get; set; }
    public static Dictionary<TileType, IntRect> tileIntRect { get; set; }
    public static Dictionary<TileType, bool> tilePassable { get; set;}
    public Hex(int Q, int R)
    {
        this.Q = Q;
        this.R = R;
        Sprite = new Sprite(Strategos.tileTexture);
        Sprite.TextureRect = tileIntRect[Type];
        Sprite.Origin = new Vector2f(0, 0);
        Sprite.Position = CubeToPixel(new Cube(Q, R, S)) - new Vector2f(Strategos.TILE_WIDTH / 2f, Strategos.TILE_HEIGHT / 2f);
        
        //DebugText = new Text($"Q:{Q} R:{R} S:{S}", Strategos.font, 12);
        //DebugText.Position = Sprite.Position + new Vector2f(5, 40);
        //DebugText.FillColor = Color.Black;    
    }
    static Hex()
    {
        directionDictionary = new Dictionary<Direction, Cube>();
        directionDictionary[Direction.NW] =  new Cube(0, -1, 1);
        directionDictionary[Direction.NE] = new Cube(1, -1, 0);
        directionDictionary[Direction.E] = new Cube(1, 0, -1);
        directionDictionary[Direction.SE] = new Cube(0, 1, -1);
        directionDictionary[Direction.SW] = new Cube(-1, 1, 0);
        directionDictionary[Direction.W] = new Cube(-1, 0, 1);

        tileIntRect = new Dictionary<TileType, IntRect>();
        tileIntRect[TileType.Grass] = new IntRect(0, 0, Strategos.TILE_WIDTH, Strategos.TILE_HEIGHT);
        tileIntRect[TileType.Mountain] = new IntRect(96, 0, Strategos.TILE_WIDTH, Strategos.TILE_HEIGHT);
        tileIntRect[TileType.Water] = new IntRect(192, 0, Strategos.TILE_WIDTH, Strategos.TILE_HEIGHT);
        tileIntRect[TileType.Beach] = new IntRect(288, 0, Strategos.TILE_WIDTH, Strategos.TILE_HEIGHT);
        tileIntRect[TileType.Snow] = new IntRect(384, 0, Strategos.TILE_WIDTH, Strategos.TILE_HEIGHT);
        tileIntRect[TileType.Bridge] = new IntRect(0, 113, Strategos.TILE_WIDTH, Strategos.TILE_HEIGHT);

        tilePassable = new Dictionary<TileType, bool>();
        tilePassable[TileType.Grass] = true;
        tilePassable[TileType.Mountain] = false;
        tilePassable[TileType.Water] = false;
        tilePassable[TileType.Beach] = true;
        tilePassable[TileType.Snow] = true;
        tilePassable[TileType.Bridge] = true;
    }
    public bool HasNeighbor(Direction direction, HexStorage hexStorage)
    {
        bool success = false;
        Cube cube = Cube.Add(Cube.AxialToCube(new Vector2f(Q, R)), Cube.GetDirection(direction));
        if (GetFromCube(cube, hexStorage) != null)
        {
            return true;
        }
        return success;
    }
    public List<Hex> GetNeighbors(HexStorage hexStorage)
    {
        List<Hex> neighbors = new List<Hex>();
        foreach (var direction in directionDictionary)
        {
            Cube cube = Cube.Add(Cube.AxialToCube(new Vector2f(Q, R)), direction.Value);
            Hex neighbor = GetFromCube(cube, hexStorage);
            if (neighbor != null)
            {
                neighbors.Add(neighbor);
            }
        }
        return neighbors;
    }
    public static Hex GetFromCube(Cube cube, HexStorage hexStorage)
    {
        Vector2i axial = Cube.CubeToAxial(cube);
        return hexStorage.GetHex(axial.X, axial.Y, -axial.X - axial.Y);
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
        double sqrt3 = Math.Sqrt(3.0);
        float x = (float)((sqrt3 * hex.Q + (sqrt3 / 2.0) * hex.R) * (Strategos.TILE_HEIGHT / 2f + -1.4f));
        float y = (Strategos.TILE_HEIGHT / 2f + 0.8f) * (3f / 2f * hex.R);


        return new Vector2f((float)x, (float)y);
    }
    public static int Distance(Hex a, Hex b)
    {
        return (int)((Math.Abs(a.Q - b.Q) + Math.Abs(a.R - b.R) + Math.Abs(a.S - b.S)) / 2);
    }
    public void Draw(RenderWindow window)
    {     
        window.Draw(Sprite);
        //window.Draw(DebugText);
    }

    public Hex GetNeighbor(Direction direction, HexStorage hexStorage)
    {
        Cube cube = Cube.CubeNeighbor(Cube.AxialToCube(new Vector2f(Q, R)), direction);
        return GetFromCube(cube, hexStorage);
    }
}
