﻿using SFML.Graphics;
using SFML.Window;
using SFML.System;

public class Strategos
{
    public const int WINDOW_WIDTH = 1366;
    public const int WINDOW_HEIGHT = 768;

    public static float ROOTOFTHREE = 1.732050807f;
    public static float cameraSpeed = 500.0f;
    public const float ZOOM_SPEED = 1.1f;

    public static Texture tileTexture = new Texture("../../Assets/hex.png");
    public static Texture characterTexture = new Texture("../../Assets/characters.png");
    public static Texture bridgesTexture = new Texture("../../Assets/bridges.png");
    public static Texture iconsTexture = new Texture("../../Assets/icons.png");
    public static Font font = new Font("../../Assets/arial.ttf");

    public static int noiseSeed = 11111;
    public static int noiseRepeat = 80;
    public static double grassRatio = 0.45;
    public static double waterRatio = 1.00;

    public static bool isInputBoxActive = false;
    public static ConfigValue ConfigValue = ConfigValue.NoiseSeed;

    public static bool isWPressed = false;
    public static bool isSPressed = false;
    public static bool isAPressed = false;
    public static bool isDPressed = false;

    public static Unit SelectedUnit { get; set; }

    public static List<Region> regions = new List<Region>();
    public static void Main(string[] args)
    {
        RenderWindow window = new RenderWindow(new VideoMode(WINDOW_WIDTH, WINDOW_HEIGHT), "Strategos", Styles.Default);
        VideoMode videoMode = new VideoMode(WINDOW_WIDTH, WINDOW_HEIGHT);
        View view = new View(new FloatRect(0, 0, WINDOW_WIDTH, WINDOW_HEIGHT));
        View ui = new View(new FloatRect(0, 0, WINDOW_WIDTH, WINDOW_HEIGHT));
        HexStorage hexStorage = new HexStorage();
        Unit soldier = new Unit(0, 0, 3, 2);
        Unit soldier2 = new Unit(0, 0, 3, 2);
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
        window.KeyReleased += InputHandler.KeyReleased;
        window.MouseButtonPressed += (sender, e) => InputHandler.MouseButtonPressed(sender, e, hexStorage);
        window.MouseWheelScrolled += (sender, e) => InputHandler.MouseWheelScrolled(sender, e, view);
        window.MouseButtonReleased += (sender, e) => InputHandler.MouseButtonReleased(sender, e, hexStorage);
        window.Closed += (sender, args) => window.Close();

        window.SetFramerateLimit(60);
        window.SetVerticalSyncEnabled(true);

        MapGenerator.GenerateMap(hexStorage);
        regions = Region.GenerateRegions(hexStorage);
        foreach (Hex hex in hexStorage.Hexes.Values)
        {
            if (hex.Type == TileType.Grass)
            {
                soldier.SetPosition(hexStorage, hex.Q, hex.R);
                soldier2.SetPosition(hexStorage, hex.Q + 1, hex.R);
                break;
            }
        }
        view.Center = soldier.Sprite.Position;
        while (window.IsOpen)
        {
            window.SetView(view);
            window.Clear(Color.Black);

            DrawHexagons(window, hexStorage);
            DrawUnits(window, Unit.unitList);

            window.SetView(ui);

            /*if (isInputBoxActive)
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
            }*/

            window.SetView(view);
            UpdateCamera(view);

            Region.DrawRegionsText(window, regions);
            window.DispatchEvents();
            window.Display();
        }
    }

    public static void UpdateCamera(View view)
    {
        float smoothing = 0.1f;
        cameraSpeed = view.Size.X / 6;
        Vector2f targetCenter = view.Center;
        Vector2f currentCenter = view.Center;
        targetCenter += new Vector2f((isDPressed ? cameraSpeed : 0) - (isAPressed ? cameraSpeed : 0), (isSPressed ? cameraSpeed : 0) - (isWPressed ? cameraSpeed : 0));
        
        currentCenter = currentCenter + smoothing * (targetCenter - currentCenter);
        view.Center = currentCenter;
    }

    public static void DrawUnits(RenderWindow window, List<Unit> units)
    {
        foreach (Unit unit in units)
        {
            unit.Draw(window);
        }
        foreach (Sprite sprite in Unit.MovementRangeSprite)
        {
            window.Draw(sprite);
        }
    }
    public static void DrawHexagons(RenderWindow window, HexStorage hexStorage)
    {
        foreach (Hex hex in hexStorage.Hexes.Values)
        {
            hex.Draw(window);
        }
    }
    public static void UpdateFogOfWar(HexStorage hexStorage)
    {
        ResetTilesColorWhenOutOfView(hexStorage);
        foreach (Unit unit in Unit.unitList)
        {
            Cube unitPos = new Cube(unit.Q, unit.R, -unit.Q - unit.R);
            List<Cube> cubes = Cube.CubesInRange(unitPos, unit.ViewRange);
            foreach (Cube cube in cubes)
            {
                int distance = Cube.Distance(cube, unitPos);
                if (Cube.Distance(unitPos, cube) == 1)
                {
                    Hex hex = Hex.GetFromCube(cube, hexStorage);
                    if (hex != null)
                    {
                        hex.Sprite.Color = new Color(255, 255, 255);
                    }
                    continue;
                }
                List<Cube> line = Cube.DrawLine(unitPos, cube);
                foreach (Cube c in line)
                {
                    Hex hex = Hex.GetFromCube(c, hexStorage);
                    if (hex == null || hex.Type == TileType.Mountain)
                    {
                        break;
                    }
                    else
                    {
                        hex.Sprite.Color = new Color(255, 255, 255);
                    }
                }
            }
        }
    }
    public static void ResetTilesColorWhenOutOfView(HexStorage hexStorage)
    {
        foreach (Hex hex in hexStorage.Hexes.Values)
        {
            hex.Sprite.Color = new Color(150, 150, 150);
        }
    }
}
