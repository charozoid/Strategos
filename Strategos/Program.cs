using SFML.Graphics;
using SFML.Window;
using SFML.System;

class Strategos
{
    public const int WINDOW_WIDTH = 1600;
    public const int WINDOW_HEIGHT = 900;
    public static Texture tileTexture = new Texture("../../Assets/hex.png");
    public static Texture characterTexture = new Texture("../../Assets/characters.png");
    public const int TILE_WIDTH = 97;
    public const int TILE_HEIGHT = 113;
    public const int CHARACTER_WIDTH = 22;
    public const int CHARACTER_HEIGHT = 52;
    public static Font font = new Font("../../Assets/arial.ttf");
    public const float ROOTOFTHREE = 1.732f;
    public static List<Hex> hexList = new List<Hex>();

    public static float cameraSpeed = 200.0f;
    public static float zoomSpeed = 1.1f;

    public static void Main(string[] args)
    {
        RenderWindow window = new RenderWindow(new VideoMode(WINDOW_WIDTH, WINDOW_HEIGHT), "Strategos");
        VideoMode videoMode = new VideoMode(WINDOW_WIDTH, WINDOW_HEIGHT);
        window.SetFramerateLimit(60);
        window.Closed += (sender, args) => window.Close();
        window.SetVerticalSyncEnabled(true);

        HexStorage hexStorage = new HexStorage();

        View view = new View(new FloatRect(0, 0, WINDOW_WIDTH, WINDOW_HEIGHT));
        window.KeyPressed += (sender, e) => InputHandler.KeyPressed(sender, e, view);
        window.MouseButtonPressed += (sender, e) => InputHandler.MouseButtonPressed(sender, e, hexStorage);
        window.MouseWheelScrolled += (sender, e) => InputHandler.MouseWheelScrolled(sender, e, view);
        window.MouseButtonReleased += (sender, e) => InputHandler.MouseButtonReleased(sender, e, hexStorage);
        int distance = 0;
        Unit soldier = new Unit(new Cube(0, 0, 0));


        CreateHexagonsCircle(hexStorage, 5);
        while (window.IsOpen)
        {
            window.SetView(view);
            window.Clear(Color.Black);

            Vector2f worldPos = window.MapPixelToCoords(Mouse.GetPosition(window), view);
            Cube cube = Hex.PixelToCube(new Vector2f(worldPos.X, worldPos.Y));

            DrawHexagons(window, hexStorage);
            window.DispatchEvents();

            DrawUnits(window, Unit.unitList);
            window.Display();
        }
    }
    public static void CreateHexagonsCircle(HexStorage hexStorage, int radius)
    {

        Hex hex = new Hex(0, 0);
        hexStorage.AddHex(hex);
        hexList.Add(hex);

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
                        hexList.Add(hex);
                    }
                }
            }
        }
        hexStorage.UpdateMinMax();
    }
    public static void DrawUnits(RenderWindow window, List<Unit> units)
    {
        foreach (Unit unit in units)
        {
            unit.Draw(window);
        }
    }
    public static void DrawHexagons(RenderWindow window, HexStorage hexStorage)
    {
        for (int k = hexStorage.minR; k < hexStorage.maxR; k++)
        {
            for (int i = hexStorage.minQ; i < hexStorage.maxQ; i++)
            {
                for (int j = hexStorage.minR; j < hexStorage.maxR; j++)
                {
                    Hex hex = hexStorage.GetHex(i, j, -i - j);
                    if (hex != null && hex.R == k)
                    {
                        hex.Draw(window);
                    }
                }
            }
        }
    }
}
