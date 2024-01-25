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

        /*for (int i = 0; i < 6; i++)
        {
            Direction direction = (Direction)i;
            Cube neighborCube = Cube.CubeNeighbor(Cube.AxialToCube(new Vector2f(hex.Q, hex.R)), direction);
            Hex neighbor = new Hex((int)neighborCube.Q, (int)neighborCube.R);
            hexStorage.AddHex(neighbor);
            for (int j= 0; j < 6; j++)
            {
                Direction direction2 = (Direction)j;
                Cube neighborCube2 = Cube.CubeNeighbor(Cube.AxialToCube(new Vector2f(neighbor.Q, neighbor.R)), direction2);
                Hex neighbor2 = new Hex((int)neighborCube2.Q, (int)neighborCube2.R);
                hexStorage.AddHex(neighbor2);
            }
        }*/
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
        for (int k = -10; k < 10; k++)
        {
            for (int i = -10; i < 10; i++)
            {
                for (int j = -10; j < 10; j++)
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
