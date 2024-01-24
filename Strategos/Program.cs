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
            
            Text text = new Text("Q: R: S:", font, 20);
            text.Position = new Vector2f(1000, 0);
            text.FillColor = Color.White;
            Vector2f mousePosition = new Vector2f(Mouse.GetPosition(window).X, Mouse.GetPosition(window).Y);
            if (Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                
                Hex hex = Hex.CubeToHex(Hex.PixelToCube(mousePosition), hexStorage);
                if (hex != null)
                {
                    hex.Sprite.Color = Color.Red;
                }
            }

            Cube cube = Hex.PixelToCube(new Vector2f(mousePosition.X, mousePosition.Y));
            text.DisplayedString = "Q: " + cube.Q + " R: " + cube.R + " S: " + cube.S;
            PrintHexagons(window, hexStorage);
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
                Hex hex = new Hex(i, j);

              
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
                    hex.Draw(window);
                }
            }
        }
    }
}
