using SFML.Graphics;
using SFML.Window;
using SFML.System;

class Strategos
{
    public const int WINDOW_WIDTH = 1600;
    public const int WINDOW_HEIGHT = 900;
    public static Texture tileTexture = new Texture("../../Assets/hex.png");
    public const int TILE_WIDTH = 97;
    public const int TILE_HEIGHT = 113;
    public static Hex[,,] hexGrid = new Hex[10, 10, 10];
    private static Font font = new Font("../../Assets/arial.ttf");
    public const float ROOTOFTHREE = 1.732f;
    
    public static void Main(string[] args)
    {
        RenderWindow window = new RenderWindow(new VideoMode(WINDOW_WIDTH, WINDOW_HEIGHT), "Strategos");
        VideoMode videoMode = new VideoMode(WINDOW_WIDTH, WINDOW_HEIGHT);
        window.SetFramerateLimit(60);
        window.Closed += (sender, args) => window.Close();
        window.SetVerticalSyncEnabled(true);
        HexStorage hexStorage = new HexStorage();
        CreateHexagons(hexStorage);
        while (window.IsOpen)
        {
            window.DispatchEvents();
            window.Clear(Color.Black);
            PrintHexagons(window, hexStorage);
            Text text = new Text("Q: R: S:", font, 20);
            text.Position = new Vector2f(1000, 0);
            text.Color = Color.White;
            if (Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                Vector2f mousePosition = new Vector2f(Mouse.GetPosition(window).X, Mouse.GetPosition(window).Y);
                Hex hex = Hex.PixelToHex(mousePosition, hexStorage);
                if (hex != null)
                {
                    text.DisplayedString = "Q: " + hex.Q + " R: " + hex.R + " S: " + hex.S;
                    hex.Sprite.Color = Color.Red;
                }
            }
            window.Draw(text);
            window.Display();
        }
    }
    public static void CreateHexagons(HexStorage hexStorage)
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                Hex hex = new Hex();
                hex.Q = i;
                hex.R = j;
                if (j % 2 == 0)
                {
                    hex.Sprite.Position = new Vector2f(hex.Q * TILE_WIDTH, hex.R * (0.75f * TILE_HEIGHT));
                }
                else
                {
                    hex.Sprite.Position = new Vector2f(hex.Q * TILE_WIDTH + (TILE_WIDTH / 2), hex.R * (0.75f * TILE_HEIGHT));
                }

                hexStorage.AddHex(hex);
            }

        }
    }
    public static void PrintHexagons(RenderWindow window, HexStorage hexStorage)
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                Hex hex = hexStorage.GetHex(i, j, -i - j);
                if (hex != null)
                {
                    window.Draw(hex.Sprite);
                }
            }
        }
    }
}

public class Hex
{
    public int Q { get; set; }
    public int R { get; set; }
    public int S { get { return -Q - R; } }
    public Sprite Sprite { get; set; }
    
    public Hex()
    {
        Sprite = new Sprite(Strategos.tileTexture);
        Sprite.TextureRect = new IntRect(0, 0, Strategos.TILE_WIDTH, Strategos.TILE_HEIGHT);
        
    }
    public static Hex PixelToHex(Vector2f pixel, HexStorage hexStorage)
    { 
        float q = ((Strategos.ROOTOFTHREE / 3f * pixel.X) - (1f / 3f * pixel.Y)) / (Strategos.TILE_HEIGHT / 2);
        float r = ((2f / 3f) * pixel.Y) / (Strategos.TILE_HEIGHT / 2);
        Vector3f roundedVector = HexRound(new Vector3f(q, r, -q-r));
        return hexStorage.GetHex((int)roundedVector.X, (int)roundedVector.Y, (int)roundedVector.Z);
    }

    private static Vector3f HexRound(Vector3f hex)
    {
        float rx = MathF.Round(hex.X);
        float ry = MathF.Round(hex.Y);
        float rz = MathF.Round(hex.Z);

        float x_diff = MathF.Abs(rx - hex.X);
        float y_diff = MathF.Abs(ry - hex.Y);
        float z_diff = MathF.Abs(rz - hex.Z);

        if (x_diff > y_diff && x_diff > z_diff)
        {
            rx = -ry - rz;
        }
        else if (y_diff > z_diff)
        {
            ry = -rx - rz;
        }
        else
        {
            rz = -rx - ry;
        }

        return new Vector3f(rx, ry, rz);
    }
}

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