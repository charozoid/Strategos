using SFML.Graphics;
using SFML.System;

public class Hex
{
    public const int TILE_WIDTH = 96;
    public const int TILE_HEIGHT = 113;
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
        Sprite.Position = CubeToPixel(new Cube(Q, R, S)) - new Vector2f(TILE_WIDTH / 2f, TILE_HEIGHT / 2f);
  
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
        tileIntRect[TileType.Grass] = new IntRect(0, 0, TILE_WIDTH, TILE_HEIGHT);
        tileIntRect[TileType.Mountain] = new IntRect(96, 0, TILE_WIDTH, TILE_HEIGHT);
        tileIntRect[TileType.Water] = new IntRect(192, 0, TILE_WIDTH, TILE_HEIGHT);
        tileIntRect[TileType.Beach] = new IntRect(288, 0, TILE_WIDTH, TILE_HEIGHT);
        tileIntRect[TileType.Snow] = new IntRect(384, 0, TILE_WIDTH, TILE_HEIGHT);
        tileIntRect[TileType.Bridge] = new IntRect(0, 113, TILE_WIDTH, TILE_HEIGHT);

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
    public Hex GetNeighbor(HexStorage hexStorage, Direction direction)
    {
        Cube cube = Cube.Add(Cube.AxialToCube(new Vector2f(Q, R)), Cube.GetDirection(direction));
        return GetFromCube(cube, hexStorage);
    }
    public static Hex GetFromCube(Cube cube, HexStorage hexStorage)
    {
        Vector2i axial = Cube.CubeToAxial(cube);
        return hexStorage.GetHex(axial.X, axial.Y, -axial.X - axial.Y);
    }
    public static Cube PixelToCube(Vector2f pixel)
    {
        float size = TILE_HEIGHT / 2;
        float root = Strategos.ROOTOFTHREE;
        float q = (((root / 3f * pixel.X) - (1f / 3f) * pixel.Y) / size);
        float r = ((2f / 3f) * pixel.Y) / size;

        Cube cube = Cube.CubeRound(Cube.AxialToCube(new Vector2f(q, r)));

        return cube;
    }
    public static Vector2f CubeToPixel(Cube hex)
    {
        double tileRatio = TILE_HEIGHT / TILE_WIDTH;
        double x = tileRatio * (hex.Q + hex.R / 2.0) * TILE_WIDTH;
        double y = (TILE_HEIGHT / 2.0) * hex.R * (3.0 / 2.0);

        return new Vector2f((float)x, (float)y);
    }
    public virtual void Draw(RenderWindow window)
    {     
        window.Draw(Sprite);
    }
    public Hex GetNeighbor(Direction direction, HexStorage hexStorage)
    {
        Cube cube = Cube.CubeNeighbor(Cube.AxialToCube(new Vector2f(Q, R)), direction);
        return GetFromCube(cube, hexStorage);
    }
}
