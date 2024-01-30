
public class HexStorage
{
    private Dictionary<(int, int, int), Hex> hexes = new Dictionary<(int, int, int), Hex>();
    public int maxQ { get; set; }
    public int maxR { get; set; }
    public int maxS { get; set; }
    public int minQ { get; set; }
    public int minR { get; set; }
    public int minS { get; set; }
    public HexStorage()
    {
        maxQ = int.MinValue;
        maxR = int.MinValue;
        maxS = int.MinValue;
        minQ = int.MaxValue;
        minR = int.MaxValue;
        minS = int.MaxValue;
    }
    public void UpdateMinMax()
    {
        foreach(var key in hexes.Keys)
        {
            maxQ = Math.Max(maxQ, key.Item1);
            maxR = Math.Max(maxR, key.Item2);
            maxS = Math.Max(maxS, key.Item3);
            minQ = Math.Min(minQ, key.Item1);
            minR = Math.Min(minR, key.Item2);
            minS = Math.Min(minS, key.Item3);
        }
    }
    public void AddHex(Hex hex)
    {
        hexes[(hex.Q, hex.R, hex.S)] = hex;
    }
    public Hex GetHex(int q, int r, int s)
    {
        if (hexes.TryGetValue((q, r, s), out Hex hex))
        {
            return hex;
        }
        else
        {
            return null;
        }
    }
    public void Clear()
    {
        hexes.Clear();
    }
}
