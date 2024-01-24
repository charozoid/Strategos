using System;

public class InputHandler
{
    public static void KeyPressed(object sender, KeyEventArgs e)
    {
        if (e.Code == Keyboard.Key.Escape)
        {
            window.Close();
        }
    }
}
