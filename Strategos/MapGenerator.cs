using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class MapGenerator
{
    private static PerlinNoise Noise = new PerlinNoise(32);
    private static Dictionary<double, TileType> tileNoiseLookup = new Dictionary<double, TileType>
    {
        {0.50, TileType.Grass},
        {0.85, TileType.Water},
        {0.95, TileType.Mountain }
    };

    public static void CreateHexagonsCircle(HexStorage hexStorage, int radius)
    {

        Hex hex = new Hex(0, 0);
        hexStorage.AddHex(hex);

        for (int i = -radius; radius > i; i++)
        {
            for (int j = -radius; radius > j; j++)
            {
                for (int k = -radius; radius > k; k++)
                {
                    if (i + j + k == 0)
                    {
                        hex = new Hex(i, j);
                        double noise = Noise.Noise(j, i);
                        hex.Type = DetermineTileType(noise);
                        hexStorage.AddHex(hex);
                    }
                }
            }
        }
        hexStorage.UpdateMinMax();

    }
    private static TileType DetermineTileType(double noise)
    {
        foreach (var entry in tileNoiseLookup)
        {
            if (noise <= entry.Key)
            {
                return entry.Value;
            }
        }
        return TileType.Grass;
    }
}

