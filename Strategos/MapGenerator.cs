using SFML.System;
using SFML.Graphics;
public class MapGenerator
{
    private const int POLAR_LIMIT = 18;
    private const int MAP_HEIGHT = 25;
    private const int MAP_WIDTH = 25;
    private static Dictionary<double, TileType> tileNoiseLookup = new Dictionary<double, TileType>
    {
        {0.25, TileType.Grass},
        {1.00, TileType.Water},
    };

    public static void GenerateHexesNoise(HexStorage hexStorage, int radius)
    {
        PerlinNoise Noise = new PerlinNoise(Strategos.noiseRepeat);
        Random rand = new Random(Strategos.noiseSeed);
        Hex hex = new Hex(0, 0);
        hexStorage.AddHex(hex);

        for (int i = -radius; radius > i; i++)
        {
            for (int j = -radius; radius > j; j++)
            {
                hex = new Hex(i, j);
                double noise = Noise.Noise(j * 105, i * 152, rand);
                noise %= 1.0;
                hex.Type = DetermineTileType(noise);
                hexStorage.AddHex(hex);
            }
        }
        hexStorage.UpdateMinMax();
    }
    public static void GenerateMap(HexStorage hexStorage)
    {
        GenerateContinent(hexStorage, 0, -15, 8, 8); 
        GenerateContinent(hexStorage, 5, -15, 8, 4);
        GenerateContinent(hexStorage, -5, 0, 4, 8);
        GenerateContinent(hexStorage, 8, 0, 4, 4);
        GenerateContinent(hexStorage, -2, 15, 4, 4);
        GenerateWaterInCircle(0, 0, hexStorage, 25);

    }
    public static void GenerateContinent(HexStorage hexStorage, int q, int r, int sizeX, int sizeY)
    {
        Random random = new Random();

        int numSplats = random.Next(6, 15);

        for (int i = 0; i < numSplats; i++)
        {
            int radius = random.Next(2, 4);
            int qOffset = random.Next(-sizeX, sizeX);
            int rOffset = random.Next(-sizeY, sizeY);
            CreateHexCircle(qOffset + q, rOffset + r, hexStorage, radius);
        }
    }
    public static void GenerateHexesSplats(HexStorage hexStorage)
    {
        Random random = new Random();

        int numSplats = random.Next(5, 15);
        for (int i = 0; i < numSplats; i++)
        {
            int radius = random.Next(2, 6);
            int qOffset = random.Next(-10, 10);
            int rOffset = random.Next(-5, 5);
            CreateHexCircle(qOffset, rOffset, hexStorage, radius);
        }
        GenerateWaterInCircle(0, 0, hexStorage, 25);
        CreateBeaches(hexStorage);
    }
    public static void GenerateWater(HexStorage hexStorage, int radius)
    {
        PerlinNoise Noise = new PerlinNoise(Strategos.noiseRepeat);
        Random rand = new Random(Strategos.noiseSeed);
        Hex hex = new Hex(0, 0);
        hexStorage.AddHex(hex);
        for (int i = -radius; radius > i; i++)
        {
            for (int j = -radius; radius > j; j++)
            {
                hex = new Hex(i, j);
                hex.Type = TileType.Water;
                hexStorage.AddHex(hex);
            }
        }
       
        hexStorage.UpdateMinMax();
    }
    public static void CreateHexCircle(int q, int r, HexStorage hexStorage, int radius)
    {
        List<Cube> cubes = Cube.CubesInRange(new Cube(q, r, -q -r), radius);
        //Hex hex = new Hex(r, q);
        //hex.Type = TileType.Grass;
        //hexStorage.AddHex(hex);

        foreach (Cube cube in cubes)
        {
            Hex hex = new Hex((int)cube.Q, (int)cube.R);
            hex.Type = TileType.Grass;
            if (Math.Abs(cube.R) > POLAR_LIMIT)
            {
                hex.Type = TileType.Snow;
            }
            
            hexStorage.AddHex(hex);
        }
        hexStorage.UpdateMinMax();
    }
    public static void GenerateWaterInCircle(int q, int r, HexStorage hexStorage, int radius)
    {
        List<Cube> cubes = Cube.CubesInRange(new Cube(q, r, -q - r), radius);
        Hex hex = new Hex(q, r);
        hex.Type = TileType.Water;
        hexStorage.AddHex(hex);
        foreach (Cube cube in cubes)
        {
            if (hexStorage.GetHex((int)cube.Q, (int)cube.R, (int)cube.S) != null)
            {
                continue;
            }
            hex = new Hex((int)cube.Q, (int)cube.R);
            hex.Type = TileType.Water;
            hexStorage.AddHex(hex);
        }
        TrimTopAndBottom(hexStorage);
        //TrimLeftAndRight(hexStorage);
        hexStorage.UpdateMinMax();
    }
    private static void TrimLeftAndRight(HexStorage hexStorage)
    {
        for (int r = 10; r > 0; r--)
        {
            for (int q = 0; q < 10 && r % 2 == 0; q++)
            {
                Hex hexToRemove = hexStorage.GetHex(q, r, -q - r);
                if (hexToRemove != null)
                {
                    hexStorage.RemoveHex(hexToRemove);
                }
            }
        }
    }
    private static void TrimTopAndBottom(HexStorage hexStorage)
    {
        for (int i = MAP_HEIGHT + 10; i > MAP_HEIGHT; i--)
        {
            List<Hex> hexesToRemove = hexStorage.GetHexesWithRCoord(i);
            foreach (Hex hexToRemove in hexesToRemove)
            {
                hexStorage.RemoveHex(hexToRemove);
            }
        }
    }
    private static TileType DetermineTileType(double noise)
    {
        foreach (KeyValuePair<double, TileType> entry in tileNoiseLookup)
        {
            if (noise <= entry.Key)
            {
                return entry.Value;
            }
        }
        return TileType.Water;
    }
    public static void CreateBeaches(HexStorage hexStorage)
    {
        foreach (var key in hexStorage.Hexes.Keys)
        {
            if (hexStorage.Hexes[key].Type == TileType.Water)
            {
                foreach (Direction direction in Enum.GetValues(typeof(Direction)))
                {
                    if (hexStorage.Hexes[key].HasNeighbor(direction, hexStorage))
                    {
                        Hex neighbor = hexStorage.Hexes[key].GetNeighbor(direction, hexStorage);
                        if (neighbor.Type == TileType.Grass)
                        {
                            neighbor.Type = TileType.Beach;
                        }
                    }
                }
            }
        }
    }
}

