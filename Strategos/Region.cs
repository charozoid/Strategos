using SFML.Graphics;
using SFML.System;

public class Region
{
    public List<Hex> Hexes { get; set; }
    public static string[] RegionNames { get; set; }
    string Name { get; set; }
    public Text Text { get; set; }
    public Region(string name, List<Hex> hexes, HexStorage hexStorage)
    {
        Name = name;
        Hexes = hexes;
        Text = new Text(name, Strategos.font, 48);
        Text.DisplayedString = Name;
        Text.FillColor = Color.Black;
        Text.Position = FindCenter(hexStorage).Sprite.Position;
    }
    static Region()
    {
        RegionNames = new string[] { "America", "Africa", "Malistan", "Europe", "The Center" };
    }
    public static Region GenerateRegion(HexStorage hexStorage, Hex start)
    {
        Queue<Hex> frontier = new Queue<Hex>();
        List<Hex> visited = new List<Hex>();
        List<Hex> regionHexes = new List<Hex>();
        Random random = new Random();

        frontier.Enqueue(start);
        while (frontier.Count > 0)
        {
            Hex current = frontier.Dequeue();
            if (!visited.Contains(current))
            {
                visited.Add(current);
                regionHexes.Add(current);
                foreach (Hex neighbor in current.GetNeighbors(hexStorage))
                {
                    if (!visited.Contains(neighbor) && !frontier.Contains(neighbor) && neighbor.Type != TileType.Water)
                    {
                        frontier.Enqueue(neighbor);
                    }
                }
            }
        }
        Region region = new Region(RegionNames[random.Next(0, RegionNames.Length)], regionHexes, hexStorage);
        foreach (Hex hex in regionHexes)
        {
            hex.Region = region;
        }
        return region;
    }
    public static List<Hex> GetClosestHexesInTwoRegions(HexStorage hexStorage, Region region1, Region region2)
    {
        List<Hex> closestHexes = new List<Hex>();
        int distance = int.MaxValue;
        foreach (Hex hex1 in region1.Hexes)
        {
            foreach (Hex hex2 in region2.Hexes)
            {
                int newDistance = Hex.Distance(hex1, hex2);
                if (newDistance < distance)
                {
                    distance = newDistance;
                    closestHexes.Clear();
                    closestHexes.Add(hex1);
                    closestHexes.Add(hex2);
                }
            }
        }
        return closestHexes;
    }
    public static List<Region> GenerateRegions(HexStorage hexStorage)
    {
        List<Region> regions = new List<Region>();

        foreach (var hex in hexStorage.Hexes.Values)
        {
            if (hex.Type != TileType.Water && hex.Region == null)
            {
                regions.Add(GenerateRegion(hexStorage, hex));
            }
        }
        return regions;
    }
    public Hex FindCenter(HexStorage hexStorage)
    {
        int q = 0;
        int r = 0;
        int count = 0;
        foreach (Hex hex in Hexes)
        {
            q += hex.Q;
            r += hex.R;
            count++;
        }
        q /= count;
        r /= count;
        return hexStorage.GetHex(q, r, -q-r);
    }
    public static void DrawRegionsText(RenderWindow window, List<Region> regionsList)
    {
        foreach (Region region in regionsList)
        {
            window.Draw(region.Text);
        }
    }
}

