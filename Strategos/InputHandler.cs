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
}
