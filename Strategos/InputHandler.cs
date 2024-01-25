using SFML.Graphics;
using SFML.System;
using SFML.Window;

public class InputHandler
{
    public static void KeyPressed(object sender, KeyEventArgs e, View view)
    {
        if (e.Code == Keyboard.Key.W)
        {
            view.Center += new Vector2f(0, - Strategos.cameraSpeed);
        }
        if (e.Code == Keyboard.Key.S)
        {
            view.Center += new Vector2f(0, Strategos.cameraSpeed);
        }
        if (e.Code == Keyboard.Key.A)
        {
            view.Center += new Vector2f(-Strategos.cameraSpeed, 0);
        }
        if (e.Code == Keyboard.Key.D)
        {
            view.Center += new Vector2f(Strategos.cameraSpeed, 0);
        }
    }

    public static void MouseButtonPressed(object sender, MouseButtonEventArgs e, HexStorage hexStorage)
    {
        RenderWindow window = (RenderWindow)sender;
        Vector2f worldPos = window.MapPixelToCoords(Mouse.GetPosition(window), window.GetView());
        Cube cube = Hex.PixelToCube(new Vector2f(worldPos.X, worldPos.Y));
        if (e.Button == Mouse.Button.Left)
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
        }
        if (e.Button == Mouse.Button.Right)
        {
            List<Cube> cubes = Cube.ObstacleSearch(3, cube, hexStorage);
            foreach (Cube cubeToChange in cubes)
            {
                Hex hexToChange = Hex.GetFromCube(cubeToChange, hexStorage);
                if (hexToChange != null)
                {
                    hexToChange.Sprite.Color = Color.Red;
                }
            }
        }
        if (e.Button == Mouse.Button.Middle)
        {
            Hex.GetFromCube(cube, hexStorage).SetType(TileType.Grass);
        }
    }

    public static void MouseWheelScrolled(object sender, MouseWheelScrollEventArgs e, View view)
    {
        if (e.Delta > 0)
        {
            view.Zoom(1 / Strategos.zoomSpeed);
        }
        else
        {
            view.Zoom(Strategos.zoomSpeed);
        }
    }
}
