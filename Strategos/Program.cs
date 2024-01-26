using SFML.Graphics;
using SFML.Window;
using SFML.System;

class Strategos
{
    public const int WINDOW_WIDTH = 1600;
    public const int WINDOW_HEIGHT = 900;

    public const int TILE_WIDTH = 97;
    public const int TILE_HEIGHT = 113;

    public const int CHARACTER_WIDTH = 22;
    public const int CHARACTER_HEIGHT = 52;

    public const float ROOTOFTHREE = 1.732050807f;
    public const float CAMERA_SPEED = 200.0f;
    public const float ZOOM_SPEED = 1.1f;

    public static Texture tileTexture = new Texture("../../Assets/hex.png");
    public static Texture characterTexture = new Texture("../../Assets/characters.png");
    public static Font font = new Font("../../Assets/arial.ttf");
    public static void Main(string[] args)
    {
        RenderWindow window = new RenderWindow(new VideoMode(WINDOW_WIDTH, WINDOW_HEIGHT), "Strategos");
        VideoMode videoMode = new VideoMode(WINDOW_WIDTH, WINDOW_HEIGHT);
        View view = new View(new FloatRect(0, 0, WINDOW_WIDTH, WINDOW_HEIGHT));
        HexStorage hexStorage = new HexStorage();
        Unit soldier = new Unit(new Cube(0, 0, 0));

        window.KeyPressed += (sender, e) => InputHandler.KeyPressed(sender, e, view);
        window.MouseButtonPressed += (sender, e) => InputHandler.MouseButtonPressed(sender, e, hexStorage);
        window.MouseWheelScrolled += (sender, e) => InputHandler.MouseWheelScrolled(sender, e, view);
        window.MouseButtonReleased += (sender, e) => InputHandler.MouseButtonReleased(sender, e, hexStorage);
        window.Closed += (sender, args) => window.Close();

        window.SetFramerateLimit(60);
        window.SetVerticalSyncEnabled(true);

        CreateHexagonsCircle(hexStorage, 5);
        while (window.IsOpen)
        {
            window.SetView(view);
            window.Clear(Color.Black);

            DrawHexagons(window, hexStorage);
            DrawUnits(window, Unit.unitList);

            window.DispatchEvents();
            window.Display();
        }
    }
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
