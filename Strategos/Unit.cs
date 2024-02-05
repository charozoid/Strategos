using SFML.Graphics;
using SFML.System;

public class Unit
{
    public const int CHARACTER_WIDTH = 22;
    public const int CHARACTER_HEIGHT = 52;
    public int ViewRange { get; set; }
    public int MovementRange { get; set; }
    public int Q { get; set; }
    public int R { get; set; }
    public int S { get { return -Q - R; } }
    public int MovementPoints { get; set; }
    public Sprite Sprite { get; set; }
    public static List<Unit> unitList { get; set; }
    private static Vector2i iconSize = new Vector2i(63, 68);
    public static List<Sprite> MovementRangeSprite { get; set; }
    public Unit(int Q, int R, int viewRange, int movementRange)
    {  
        Sprite = new Sprite(Strategos.iconsTexture);
        Sprite.TextureRect = new IntRect(0, 0, iconSize.X, iconSize.Y);
        Sprite.Origin = new Vector2f(0, 0);
        Sprite.Position = Hex.CubeToPixel(new Cube(Q, R, S)) - new Vector2f(iconSize.X / 2, iconSize.Y / 2);
        ViewRange = viewRange;
        MovementRange = movementRange;
        this.Q = Q;
        this.R = R;
        unitList.Add(this);
        MovementPoints = MovementRange;
    }
    static Unit()
    {
        unitList = new List<Unit>();
        MovementRangeSprite = new List<Sprite>();
    }
    public void Draw(RenderWindow window)
    {
        window.Draw(Sprite);
    }
    public void DiscoverTiles(HexStorage hexStorage)
    {
        List<Cube> cubesInViewRange = Cube.CubesInRange(new Cube(Q, R, -Q - R), ViewRange);
        foreach (Cube cube in cubesInViewRange)
        {
            Hex hex = Hex.GetFromCube(cube, hexStorage);
            if (hex != null && !hex.IsDiscoverd)
            {
                hex.IsDiscoverd = true;
            }
        }
    }
    public void CreateMovementRangeSprites(HexStorage hexStorage)
    {
        List<Cube> cubes = Cube.CubesInRange(new Cube(Q, R, S), MovementPoints);
        foreach (Cube cube in cubes)
        {
            Hex hex = hexStorage.GetHex((int)cube.Q, (int)cube.R, (int)cube.S);
            int distance = Cube.Distance(new Cube(Q, R, S), cube);
            List<Cube> path = Cube.FindPath(new Cube(Q, R, S), cube, hexStorage);
            if (hex != null && hex.UnitOnTile != this && Hex.tilePassable[hex.Type] && path != null && path.Count - 1 < MovementPoints)
            {
                Sprite sprite = new Sprite(Strategos.tileTexture);
                sprite.TextureRect = new IntRect(0, 113, Hex.TILE_WIDTH, Hex.TILE_HEIGHT);
                sprite.Origin = new Vector2f(0, 0);
                sprite.Position = Hex.CubeToPixel(new Cube((int)cube.Q, (int)cube.R, (int)cube.S)) - new Vector2f(Hex.TILE_WIDTH / 2, Hex.TILE_HEIGHT / 2);
                MovementRangeSprite.Add(sprite);
            }
        }
    }
    public void SetPosition(HexStorage hexStorage, int Q, int R)
    {
        this.Q = Q;
        this.R = R;
        Cube position = new Cube(Q, R, -Q - R);
        Sprite.Position = Hex.CubeToPixel(position) - new Vector2f(iconSize.X / 2, iconSize.Y / 2);
        Hex destination = hexStorage.GetHex(Q, R, -Q - R);
        destination.UnitOnTile = this;
        DiscoverTiles(hexStorage);
        UpdateFogOfWar(hexStorage, Q, R);
    }
    public void Move(HexStorage hexStorage, int Q, int R)
    {
        Hex start = hexStorage.GetHex(this.Q, this.R, -this.Q - this.R);
        Hex destination = hexStorage.GetHex(Q, R, -Q - R);
        if (destination != null && destination.UnitOnTile == null)
        {
            int distance = Cube.Distance(new Cube(this.Q, this.R, -this.Q - this.R), new Cube(Q, R, -Q - R));
            if (distance <= MovementPoints)
            {
                start.UnitOnTile = null;
                destination.UnitOnTile = this;
                this.Q = Q;
                this.R = R;
                Cube position = new Cube(Q, R, -Q - R);
                Sprite.Position = Hex.CubeToPixel(position) - new Vector2f(iconSize.X / 2, iconSize.Y / 2);
                DiscoverTiles(hexStorage);
                MovementPoints -= distance;
                MovementRangeSprite.Clear();
                CreateMovementRangeSprites(hexStorage);
                Strategos.UpdateFogOfWar(hexStorage);
                
            }
        }
    }
    public void UpdateFogOfWar(HexStorage hexStorage, int oldQ, int oldR)
    {
        List<Cube> oldCubes = Cube.CubesInRange(new Cube(oldQ, oldR, -oldQ -oldR), ViewRange);
        List<Cube> newCubes = Cube.CubesInRange(new Cube(Q, R, -Q - R), ViewRange);
        foreach (Cube cube in oldCubes)
        {
            Hex hex = Hex.GetFromCube(cube, hexStorage);
            if (hex != null)
            {
                hex.Sprite.Color = new Color(200, 200, 200);
            }
        }
        foreach (Cube cube in newCubes)
        {
            Hex hex = Hex.GetFromCube(cube, hexStorage);
            if (hex != null)
            {
                hex.Sprite.Color = Color.White;
            }
        }
    }
}
