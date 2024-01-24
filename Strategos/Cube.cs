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
}