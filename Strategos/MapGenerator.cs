using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class MapGenerator
{
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
                        hexStorage.AddHex(hex);
                    }
                }
            }
        }
        hexStorage.UpdateMinMax();
    }
}

