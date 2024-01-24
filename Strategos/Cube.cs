using SFML.System;

public class Cube
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
    public static Cube CubeNeighbor(Cube hex, Direction direction)
    {
        return Add(hex, GetDirection(direction));
    }
}