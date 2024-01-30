using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class MapGenerator
{
    
    private static Dictionary<double, TileType> tileNoiseLookup = new Dictionary<double, TileType>
    {
        {0.80, TileType.Grass},
        {1.00, TileType.Water},
    };

    public static void GenerateHexes(HexStorage hexStorage, int radius)
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
                double noise = Noise.Noise(i, j, rand);
                hex.Type = DetermineTileType(noise);
                hexStorage.AddHex(hex);
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

