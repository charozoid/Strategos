public class HexStorage
{
    private Dictionary<(int, int, int), Hex> hexes = new Dictionary<(int, int, int), Hex>();

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
}
