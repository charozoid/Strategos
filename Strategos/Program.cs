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
    public static Font font = new Font("../../Assets/arial.ttf");
    public const float ROOTOFTHREE = 1.732f;


    public static float cameraSpeed = 200.0f;

    public static void Main(string[] args)
    {
        RenderWindow window = new RenderWindow(new VideoMode(WINDOW_WIDTH, WINDOW_HEIGHT), "Strategos");
        VideoMode videoMode = new VideoMode(WINDOW_WIDTH, WINDOW_HEIGHT);
        window.SetFramerateLimit(60);
        window.Closed += (sender, args) => window.Close();
        window.SetVerticalSyncEnabled(true);


        View view = new View(new FloatRect(0, 0, WINDOW_WIDTH, WINDOW_HEIGHT));
        window.KeyPressed += (sender, e) => InputHandler.KeyPressed(sender, e, view);


        HexStorage hexStorage = new HexStorage();
        CreateHexagonsCircle(hexStorage, 5);
        while (window.IsOpen)
        {
            window.SetView(view);
            window.Clear(Color.Black);

            Text text = new Text("Q: R: S:", font, 20);
            text.Position = new Vector2f(1000, 0);
            text.FillColor = Color.White;
            //Vector2f mousePosition = window.MapPixelToCoords(Mouse.GetPosition(), view);
            Vector2f worldPos = window.MapPixelToCoords(Mouse.GetPosition(window), view);
            if (Mouse.IsButtonPressed(Mouse.Button.Left))
            {

                Hex hex = Hex.GetFromCube(Hex.PixelToCube(worldPos), hexStorage);
                if (hex != null)
                {
                    hex.Sprite.Color = Color.Red;
                }
            }

            Cube cube = Hex.PixelToCube(new Vector2f(worldPos.X, worldPos.Y));
            text.DisplayedString = "Q: " + cube.Q + " R: " + cube.R + " S: " + cube.S;
            PrintHexagons(window, hexStorage);

            window.DispatchEvents();
            window.Draw(text);
            window.Display();
        }
    }
    public static void CreateHexagonsCircle(HexStorage hexStorage, int radius)
    {

        Hex hex = new Hex(0, 0);
        hexStorage.AddHex(hex);

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
                        Hex hex2 = new Hex(i, j);
                        hexStorage.AddHex(hex2);
                    }
                }
            }
        }
        
    }
    public static void PrintHexagons(RenderWindow window, HexStorage hexStorage)
    {
        for (int k = -100; k < 100; k++)
        {
            for (int i = -100; i < 100; i++)
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
