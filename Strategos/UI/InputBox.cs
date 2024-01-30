using SFML.Graphics;
using SFML.System;
using SFML.Window;

public class InputBox
{
    private RectangleShape background;
    private Text text;
    private Clock Clock = new Clock();

    public string Text
    {
        get { return text.DisplayedString; }
    }

    public InputBox(Font font, uint characterSize, View view)
    {
        background = new RectangleShape(new Vector2f(200, 30));
        background.FillColor = Color.White;
        background.OutlineThickness = 2;
        background.OutlineColor = Color.Black;
        background.Position = view.Center;

        text = new Text("", font, characterSize);
        text.FillColor = Color.Black;
        text.Position = background.Position + new Vector2f(5, 5);

        Clock.Restart();
    }
    public void HandleInput(RenderWindow window, out string enteredString)
    {
        enteredString = text.DisplayedString;
        if (window.HasFocus())
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Enter))
            {
                Strategos.isInputBoxActive = false;
            }
            else if (Keyboard.IsKeyPressed(Keyboard.Key.BackSpace) && Clock.ElapsedTime.AsMilliseconds() > 50)
            {
                if (text.DisplayedString.Length > 0)
                {
                    text.DisplayedString = text.DisplayedString.Remove(text.DisplayedString.Length - 1);
                    Clock.Restart();
                }
            }
            else
            {
                foreach (Keyboard.Key key in Enum.GetValues(typeof(Keyboard.Key)))
                {
                    if (Keyboard.IsKeyPressed(key) && Clock.ElapsedTime.AsMilliseconds() > 125)
                    {
                        char character = InputHandler.KeyToChar(key);
                        text.DisplayedString += character;
                        Clock.Restart();
                    }
                }
            }
        }
        window.Draw(background);
        window.Draw(text);
    }

}


