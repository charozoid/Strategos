using SFML.Graphics;
using SFML.Window;
using SFML.System;

partial class Strategos
{
    public const int WINDOW_WIDTH = 1600;
    public const int WINDOW_HEIGHT = 900;

    public const int TILE_WIDTH = 95;
    public const int TILE_HEIGHT = 111;

    public const int CHARACTER_WIDTH = 22;
    public const int CHARACTER_HEIGHT = 52;

    public const float ROOTOFTHREE = 1.732050807f;
    public const float CAMERA_SPEED = 200.0f;
    public const float ZOOM_SPEED = 1.1f;

    public static Texture tileTexture = new Texture("../../Assets/hex.png");
    public static Texture characterTexture = new Texture("../../Assets/characters.png");
    public static Font font = new Font("../../Assets/arial.ttf");

    public static int noiseSeed = 11111;
    public static int noiseRepeat = 20;
    public static double grassRatio = 0.45;
    public static double waterRatio = 1.00;

    public static bool isInputBoxActive = false;
    public static ConfigValue ConfigValue = ConfigValue.NoiseSeed;
    public static void Main(string[] args)
    {
        RenderWindow window = new RenderWindow(new VideoMode(WINDOW_WIDTH, WINDOW_HEIGHT), "Strategos", Styles.Default);
        VideoMode videoMode = new VideoMode(WINDOW_WIDTH, WINDOW_HEIGHT);
        View view = new View(new FloatRect(0, 0, WINDOW_WIDTH, WINDOW_HEIGHT));
        View ui = new View(new FloatRect(0, 0, WINDOW_WIDTH, WINDOW_HEIGHT));
        HexStorage hexStorage = new HexStorage();
        Unit soldier = new Unit(new Cube(0, 0, 0));

        Text seedText = new Text($"Seed: {noiseSeed}", font, 20);
        seedText.Position = new Vector2f(10, 10);
        seedText.FillColor = Color.Red;

        Text repeatText = new Text($"Repeat: {noiseRepeat}", font, 20);
        repeatText.Position = new Vector2f(10, 40);
        repeatText.FillColor = Color.Red;

        InputBox inputBox = new InputBox(font, 20, ui);
        string enteredString = "";
        

        int result = 0;
        window.KeyPressed += (sender, e) =>
        {
            if (!isInputBoxActive)
            {
                InputHandler.KeyPressed(sender, e, view, hexStorage);
            }

        };

        window.MouseButtonPressed += (sender, e) => InputHandler.MouseButtonPressed(sender, e, hexStorage);
        window.MouseWheelScrolled += (sender, e) => InputHandler.MouseWheelScrolled(sender, e, view);
        window.MouseButtonReleased += (sender, e) => InputHandler.MouseButtonReleased(sender, e, hexStorage);
        window.Closed += (sender, args) => window.Close();

        window.SetFramerateLimit(60);
        window.SetVerticalSyncEnabled(true);

        MapGenerator.GenerateHexes(hexStorage, 10);
        while (window.IsOpen)
        {
            window.SetView(view);
            window.Clear(Color.Black);

            DrawHexagons(window, hexStorage);
            DrawUnits(window, Unit.unitList);

            window.SetView(ui);
            window.Draw(seedText);
            window.Draw(repeatText);
            if (isInputBoxActive)
            {
                switch (ConfigValue)
                {
                    case ConfigValue.NoiseSeed:
                        inputBox.HandleInput(window, out enteredString);
                        int.TryParse(enteredString, out result);
                        noiseSeed = result;
                        seedText.DisplayedString = $"Seed: {noiseSeed}";
                        break;
                    case ConfigValue.NoiseRepeat:
                        inputBox.HandleInput(window, out enteredString);
                        int.TryParse(enteredString, out result);
                        noiseRepeat = result;
                        repeatText.DisplayedString = $"Repeat: {noiseRepeat}";
                        break;
                }
            }

            window.DispatchEvents();
            window.Display();
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
