using SFML.System;

public struct Cube
{
    public float Q { get; set; }
    public float R { get; set; }
    public float S { get; set; }

    public Cube(float q, float r, float s)
    {
        Q = q;
        R = r;
        S = s;
    }
    public static Vector2i CubeToAxial(Cube cube)
    {
        Vector2i axial = new Vector2i((int)cube.Q, (int)cube.R);
        return axial;
    }
    public static Cube AxialToCube(Vector2f hex)
    {
        float q = hex.X;
        float r = hex.Y;
        float s = -q - r;
        Cube cube = new Cube(q, r, s);
        return cube;
    }
    public static Cube GetDirection(Direction direction)
    {
        return Hex.directionDictionary[direction];
    }
    public static Cube Add(Cube hex, Cube vec)
    {
        return new Cube(hex.Q + vec.Q, hex.R + vec.R, hex.S + vec.S);
    }
    public static Cube Subtract(Cube hex, Cube vec)
    {
        return new Cube(hex.Q - vec.Q, hex.R - vec.R, hex.S - vec.S);
    }
    public static Cube CubeNeighbor(Cube hex, Direction direction)
    {
        return Add(hex, GetDirection(direction));
    }
    public static int Distance(Cube a, Cube b)
    {
        Cube cube = Subtract(a, b);
        return (int)Math.Max(Math.Max(Math.Abs(cube.Q), Math.Abs(cube.R)), Math.Abs(cube.S));
    }
    public static List<Cube> CubesInRange(Cube cube, int range)
    {
        List<Cube> cubes = new List<Cube>();
        for (int x = -range; x <= range; x++)
        {
            for (int y = Math.Max(-range, -x - range); y <= Math.Min(range, -x + range); y++)
            {
                int z = -x - y;
                cubes.Add(Add(cube, new Cube(x, y, z)));
            }
        }
        return cubes;
    }
    public static float FloatLerp(float a, float b, float t)
    {
        return a + (b - a) * t;
    }
    public static Cube CubeLerp(Cube a, Cube b, float t)
    {
        return new Cube(FloatLerp(a.Q, b.Q, t), FloatLerp(a.R, b.R, t), FloatLerp(a.S, b.S, t));
    }
    public static List<Cube> Linedraw(Cube a, Cube b)
    {
        int N = Distance(a, b);
        List<Cube> results = new List<Cube>();
        for (int i = 0; i <= N; i++)
        {
            results.Add(CubeRound(CubeLerp(a, b, 1.0f / N * i)));
        }
        return results;
    }
    public static Cube CubeRound(Cube cube)
    {
        float q = MathF.Round(cube.Q);
        float r = MathF.Round(cube.R);
        float s = MathF.Round(cube.S);

        float qDiff = Math.Abs(q - cube.Q);
        float rDiff = Math.Abs(r - cube.R);
        float sDiff = Math.Abs(s - cube.S);

        if (qDiff > rDiff && qDiff > sDiff)
        {
            q = -r - s;
        }
        else if (rDiff > sDiff)
        {
            r = -q - s;
        }
        else
        {
            s = -q - r;
        }
        return new Cube(q, r, s);
    }
    public static List<Cube> FindPath(Cube start, Cube target, HexStorage hexStorage)
    {
        Queue<Cube> frontier = new Queue<Cube>();
        Dictionary<Cube, Cube> cameFrom = new Dictionary<Cube, Cube>();
        frontier.Enqueue(start);
        cameFrom[start] = start;
        while (frontier.Count > 0)
        {
            Cube current = frontier.Dequeue();
            if (current.Equals(target))
            {
                return ReconstructPath(cameFrom, start, target);
            }
            foreach (Cube next in CubesInRange(current, 1))
            {
                Hex hex = Hex.GetFromCube(next, hexStorage);
                if (hex != null && hex.TileType != TileType.Mountain && !cameFrom.ContainsKey(next))
                {
                    frontier.Enqueue(next);
                    cameFrom[next] = current;
                }
            }
        }
        return null;
    }

    private static List<Cube> ReconstructPath(Dictionary<Cube, Cube> cameFrom, Cube start, Cube target)
    {
        List<Cube> path = new List<Cube>();
        Cube current = target;

        while (!current.Equals(start))
        {
            path.Add(current);
            current = cameFrom[current];
        }

        path.Reverse();
        return path;
    }

    public static List<Cube> ValidCubesInRange(int movement, Cube start, HexStorage hexStorage)
    {
        List<Cube> frontier = new List<Cube>();
        frontier.Add(start);
        Dictionary<int, List<Cube>> fringes = new Dictionary<int, List<Cube>>();

        fringes[0] = new List<Cube>();
        fringes[0].Add(start);
        for (int i = 1; i <= movement; i++)
        {
            fringes.Add(i, new List<Cube>());
            foreach (Cube cube in fringes[i - 1])
            {
                foreach (Cube neighbor in CubesInRange(cube, 1))
                {
                    Hex hex = Hex.GetFromCube(neighbor, hexStorage);
                    if (hex != null)
                    {
                        if (hex.TileType != TileType.Mountain)
                        {
                            if (!fringes[i].Contains(neighbor))
                            {
                                frontier.Add(neighbor);
                                fringes[i].Add(neighbor);
                            }
                        }
                    }
                }
            }
        }
        return frontier;
    }
}
