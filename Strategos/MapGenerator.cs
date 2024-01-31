using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MapGenerator
{

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
    public static void GenerateHexesSplats(HexStorage hexStorage)
    {
        Random random = new Random();

        int numSplats = random.Next(5, 15);
        //int numSplats = 1;
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
        Hex hex = new Hex(r, q);
        hex.Type = TileType.Grass;
        hexStorage.AddHex(hex);

        foreach (Cube cube in cubes)
        {
            hex = new Hex((int)cube.Q, (int)cube.R);
            hex.Type = TileType.Grass;
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
        hexStorage.UpdateMinMax();
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

