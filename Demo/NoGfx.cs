using Core.Drawing.UserInterface;
using Core.Engine;
using Core.Entity;
using Demo.Scenes;

namespace Demo;

public class NoGfx(uint height, uint width, string title) : UserWindow(height, width, title)
{
    protected override void Start()
    {
        UiTheme.FontPath = @"Resources/font.ttf";
        SceneManager.SwitchScene<MenuScene>();
    }

    protected override void Render() => GameContext.CurrentWindow.Clear(Palette.DarkGray);
}