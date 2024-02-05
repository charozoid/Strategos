using SFML.System;
using SFML.Graphics;

public class Bridge : Hex
{
    public static Dictionary<int, IntRect> spriteFlagsDict = new Dictionary<int, IntRect>();
    public Bridge(int Q, int R) : base(Q, R)
    {
        Type = TileType.Bridge;
        Sprite = new Sprite(Strategos.bridgesTexture);
        Sprite.Origin = new Vector2f(0, 0);
        Sprite.Position = CubeToPixel(new Cube(Q, R, S)) - new Vector2f(TILE_WIDTH / 2f, TILE_HEIGHT / 2f);
    }
    /*
    0b W SW SE E NE NW 
    */
    static Bridge()
    {
        spriteFlagsDict[0b000101] = new IntRect(0, 113, TILE_WIDTH, TILE_HEIGHT); //NW - E
        spriteFlagsDict[0b001001] = new IntRect(192, 113, TILE_WIDTH, TILE_HEIGHT); //NW - SE
        spriteFlagsDict[0b010010] = new IntRect(192, 0, TILE_WIDTH, TILE_HEIGHT); //NE - SW
        spriteFlagsDict[0b100010] = new IntRect(96, 113, TILE_WIDTH, TILE_HEIGHT); // NE - W
        spriteFlagsDict[0b100100] = new IntRect(288, 0, TILE_WIDTH, TILE_HEIGHT); //W - E
        spriteFlagsDict[0b010100] = new IntRect(0, 0, TILE_WIDTH, TILE_HEIGHT); //SW - E
        spriteFlagsDict[0b101000] = new IntRect(96, 0, TILE_WIDTH, TILE_HEIGHT); //W - SE

        spriteFlagsDict[0b110100] = new IntRect(288, 0, TILE_WIDTH, TILE_HEIGHT);
        spriteFlagsDict[0b100101] = new IntRect(288, 0, TILE_WIDTH, TILE_HEIGHT);
        spriteFlagsDict[0b101100] = new IntRect(288, 0, TILE_WIDTH, TILE_HEIGHT);
        spriteFlagsDict[0b100110] = new IntRect(288, 0, TILE_WIDTH, TILE_HEIGHT);
        spriteFlagsDict[0b010101] = new IntRect(288, 0, TILE_WIDTH, TILE_HEIGHT);
        spriteFlagsDict[0b010101] = new IntRect(288, 0, TILE_WIDTH, TILE_HEIGHT);
        spriteFlagsDict[0b110010] = new IntRect(192, 0, TILE_WIDTH, TILE_HEIGHT);        
        spriteFlagsDict[0b010110] = new IntRect(192, 0, TILE_WIDTH, TILE_HEIGHT);
        spriteFlagsDict[0b011010] = new IntRect(192, 0, TILE_WIDTH, TILE_HEIGHT);
        spriteFlagsDict[0b010011] = new IntRect(192, 0, TILE_WIDTH, TILE_HEIGHT);
        spriteFlagsDict[0b101001] = new IntRect(192, 113, TILE_WIDTH, TILE_HEIGHT);
        spriteFlagsDict[0b001011] = new IntRect(192, 113, TILE_WIDTH, TILE_HEIGHT);
        spriteFlagsDict[0b011001] = new IntRect(192, 113, TILE_WIDTH, TILE_HEIGHT);
        spriteFlagsDict[0b001101] = new IntRect(192, 113, TILE_WIDTH, TILE_HEIGHT);

        spriteFlagsDict[0b001010] = new IntRect(192, 113, TILE_WIDTH, TILE_HEIGHT);

        spriteFlagsDict[0b101101] = new IntRect(192, 0, TILE_WIDTH, TILE_HEIGHT);
        spriteFlagsDict[0b011011] = new IntRect(192, 0, TILE_WIDTH, TILE_HEIGHT);
        spriteFlagsDict[0b010111] = new IntRect(192, 0, TILE_WIDTH, TILE_HEIGHT);
        spriteFlagsDict[0b110101] = new IntRect(288, 0, TILE_WIDTH, TILE_HEIGHT);

        spriteFlagsDict[0b100000] = new IntRect(288, 0, TILE_WIDTH, TILE_HEIGHT); //W
        spriteFlagsDict[0b010000] = new IntRect(0, 0, TILE_WIDTH, TILE_HEIGHT); //SW
        spriteFlagsDict[0b001000] = new IntRect(96, 113, TILE_WIDTH, TILE_HEIGHT); //NE
        spriteFlagsDict[0b000100] = new IntRect(0, 113, TILE_WIDTH, TILE_HEIGHT); //NW
        spriteFlagsDict[0b000010] = new IntRect(192, 0, TILE_WIDTH, TILE_HEIGHT); //SE
        spriteFlagsDict[0b000001] = new IntRect(288, 0, TILE_WIDTH, TILE_HEIGHT); //SW

    }
    public static void UpdateBridgesSprites(HexStorage hexStorage)
    {
        foreach (Hex hex in hexStorage.Hexes.Values)
        {
            if (hex.Type == TileType.Bridge)
            {
                Bridge bridge = (Bridge)hex;
                int flag = bridge.GetBridgeFlag(hexStorage);
                IntRect intRect = new IntRect(576, 0, TILE_WIDTH, TILE_HEIGHT);
                bridge.Sprite.TextureRect = intRect;
                if (spriteFlagsDict.TryGetValue(flag, out IntRect value))
                {
                    bridge.Sprite.TextureRect = value;
                }

            }
        }
    }
    public int GetBridgeFlag(HexStorage hexStorage)
    {
        int flag = 0b000000;

        foreach (Direction direction in Enum.GetValues(typeof(Direction)))
        {
            Hex neighborHex = GetNeighbor(hexStorage, direction);
            if (neighborHex != null && (neighborHex.Type == TileType.Bridge || neighborHex.Type != TileType.Water))
            {
                flag |= 1 << (int)direction;
            }
        }
        return flag;
    }
    public static void GenerateBridgeBetweenRegions(HexStorage hexStorage, Region region1, Region region2)
    {
        List<Hex> hexes = Region.GetClosestHexesInTwoRegions(hexStorage, region1, region2);
        Cube start = Cube.AxialToCube(new Vector2f(hexes[0].Q, hexes[0].R));
        Cube end = Cube.AxialToCube(new Vector2f(hexes[1].Q, hexes[1].R));
        List<Cube> path = Cube.DrawLine(start, end);
        if (path.Count > 10)
        {
            return;
        }
        path.RemoveAt(0);
        path.RemoveAt(path.Count - 1);
        foreach (Cube cube in path)
        {
            Hex hex = GetFromCube(cube, hexStorage);
            if (hex == null || hex.Type != TileType.Water)
            {
                return;
            }
        }
        foreach (Cube cube in path)
        {
            Hex hex = GetFromCube(cube, hexStorage);
            Bridge bridge = new Bridge((int)cube.Q, (int)cube.R);
            hexStorage.Replace(hex, bridge);
        }
    }
    public override void Draw(RenderWindow window)
    {
        base.Draw(window);
    }
    public static void GenerateBridges(HexStorage hexStorage, List<Region> regionList)
    {

        for (int i = 0; i < regionList.Count; i++)
        {
            for (int j = i + 1; j < regionList.Count; j++)
            {
                Region region = regionList[i];
                Region neighbor = regionList[j];

                GenerateBridgeBetweenRegions(hexStorage, region, neighbor);
            }
        }

    }
}

