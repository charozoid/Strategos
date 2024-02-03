using SFML.Graphics;
using SFML.System;
using SFML.Window;

public class InputHandler
{
    private static List<Cube> cubes { get; set; }
    static InputHandler()
    {
        cubes = new List<Cube>();
    }
    public static void KeyPressed(object sender, KeyEventArgs e, View view, HexStorage hexStorage)
    {
        if (e.Code == Keyboard.Key.W)
        {
            Strategos.isWPressed = true;
        }
        if (e.Code == Keyboard.Key.S)
        {
            Strategos.isSPressed = true;
        }
        if (e.Code == Keyboard.Key.A)
        {
            Strategos.isAPressed = true;
        }
        if (e.Code == Keyboard.Key.D)
        {
            Strategos.isDPressed = true;
        }
        if (e.Code == Keyboard.Key.Q)
        {
            Strategos.isInputBoxActive = true;
            Strategos.ConfigValue = ConfigValue.NoiseSeed;
        }
        if (e.Code == Keyboard.Key.R)
        {
            Strategos.isInputBoxActive = true;
            Strategos.ConfigValue = ConfigValue.NoiseRepeat;
        }
        if (e.Code == Keyboard.Key.Delete)
        {
            hexStorage.Clear();
            MapGenerator.GenerateMap(hexStorage);
        }
    }
    public static void KeyReleased(object? sender, KeyEventArgs e)
    {
        if (e.Code == Keyboard.Key.W)
        {
            Strategos.isWPressed = false;
        }
        if (e.Code == Keyboard.Key.S)
        {
            Strategos.isSPressed = false;
        }
        if (e.Code == Keyboard.Key.A)
        {
            Strategos.isAPressed = false;
        }
        if (e.Code == Keyboard.Key.D)
        {
            Strategos.isDPressed = false;
        }
    }
    public static void MouseButtonPressed(object sender, MouseButtonEventArgs e, HexStorage hexStorage)
    {
        RenderWindow window = (RenderWindow)sender;
        Vector2f worldPos = window.MapPixelToCoords(Mouse.GetPosition(window), window.GetView());
        Cube cube = Hex.PixelToCube(new Vector2f(worldPos.X, worldPos.Y));

        if (e.Button == Mouse.Button.Right)
        {
            cubes = Cube.Linedraw(new Cube(0, 0, 0), cube);
            if (cubes != null)
            {
                foreach (Cube cubeToChange in cubes)
                {
                    Hex hexToChange = Hex.GetFromCube(cubeToChange, hexStorage);
                    if (hexToChange != null)
                    {
                        hexToChange.Sprite.Color = Color.Red;
                    }
                }
            }
        }
        if (e.Button == Mouse.Button.Middle)
        {
            Hex.GetFromCube(cube, hexStorage).Type = TileType.Mountain;
        }
    }
    public static void MouseButtonReleased(object sender, MouseButtonEventArgs e, HexStorage hexStorage)
    {
        if (cubes != null)
        {
            foreach (Cube cubeToChange in cubes)
            {
                Hex hexToChange = Hex.GetFromCube(cubeToChange, hexStorage);
                if (hexToChange != null)
                {
                    hexToChange.Sprite.Color = Color.White;
                }
            }
        }
    }
    public static void MouseWheelScrolled(object sender, MouseWheelScrollEventArgs e, View view)
    {
        if (e.Delta > 0)
        {
            view.Zoom(1 / Strategos.ZOOM_SPEED);
        }
        else
        {
            view.Zoom(Strategos.ZOOM_SPEED);
        }
    }
    public static char KeyToChar(Keyboard.Key key)
    { 
        if (key >= Keyboard.Key.A && key <= Keyboard.Key.Z)
        {
            return (char)('A' + (key - Keyboard.Key.A));
        }
        else if (key >= Keyboard.Key.Num0 && key <= Keyboard.Key.Num9)
        {
            return (char)('0' + (key - Keyboard.Key.Num0));
        }
        else if (key >= Keyboard.Key.Numpad0 && key <= Keyboard.Key.Numpad9)
        {
            return (char)('0' + (key - Keyboard.Key.Numpad0));
        }
        else
        {
            switch (key)
            {
                case Keyboard.Key.Space: return ' ';
                case Keyboard.Key.Enter: return '\n';


                default: return '?';
            }
        }
    }
}
