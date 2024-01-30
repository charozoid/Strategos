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
            view.Center += new Vector2f(0, - Strategos.CAMERA_SPEED);
        }
        if (e.Code == Keyboard.Key.S)
        {
            view.Center += new Vector2f(0, Strategos.CAMERA_SPEED);
        }
        if (e.Code == Keyboard.Key.A)
        {
            view.Center += new Vector2f(-Strategos.CAMERA_SPEED, 0);
        }
        if (e.Code == Keyboard.Key.D)
        {
            view.Center += new Vector2f(Strategos.CAMERA_SPEED, 0);
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
            MapGenerator.GenerateHexes(hexStorage, 30);
        }
        if (e.Code == Keyboard.Key.Insert)
        {
            MapGenerator.GenerateHexes(hexStorage, 10);
        }
    }

    public static void MouseButtonPressed(object sender, MouseButtonEventArgs e, HexStorage hexStorage)
    {
        RenderWindow window = (RenderWindow)sender;
        Vector2f worldPos = window.MapPixelToCoords(Mouse.GetPosition(window), window.GetView());
        Cube cube = Hex.PixelToCube(new Vector2f(worldPos.X, worldPos.Y));
        /*if (e.Button == Mouse.Button.Left)
        {
            List<Cube> cubes = Cube.Linedraw(cube, new Cube(0, 0, 0));
            foreach (Cube cubeToChange in cubes)
            {
                Hex hexToChange = Hex.GetFromCube(cubeToChange, hexStorage);
                if (hexToChange != null)
                {
                    hexToChange.Sprite.Color = Color.Red;
                }
            }
        }*/
        if (e.Button == Mouse.Button.Right)
        {
            cubes = Cube.FindPath(new Cube(0, 0, 0), cube, hexStorage);
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
