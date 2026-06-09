# SimpleHMEngine

A small, hand-made 2D game engine built on top of [SFML.Net](https://www.sfml-dev.org/download/sfml.net/).
It gives you a game loop, a scene/component tree, input binding, a camera, simple collision, an
immediate-mode drawing helper, and a lightweight UI toolkit — so you can focus on your game instead
of the plumbing.

> ⚠️ **Disclaimer:** This is a hobby engine. It is small, opinionated, and not battle-tested.
> It's great for prototypes, game jams, learning, and small 2D games — but it is **not** a
> production-grade engine like Unity, Godot, or MonoGame. Expect rough edges, breaking changes
> between versions, and missing features. Use it because it's simple and fun, not because it's complete.

## Features

- **Game loop** with a fixed-timestep accumulator (decoupled update / fixed-update / render).
- **Scene & component tree** — everything is a `Component` with its own lifecycle and children.
- **Auto-registration** — declare a component as a field on your `Scene` and it's wired up for you.
- **Input binding** — bind keyboard/mouse to `Pressed` / `Released` / `Held` actions.
- **Camera** — follows a target with offset, zoom, and frame-rate-independent smoothing.
- **Drawing helper** — cached immediate-mode `Draw.Circle/Rectangle/Triangle/Line/Text`.
- **UI toolkit** — `UiCanvas`, `UiButton`, `UiText`, `UiSlider`, `UiProgressBar`, layout panels, etc.
- **Embedded default font** — text rendering works out of the box, no asset files required.

## Installation

```sh
dotnet add package SimpleHMEngine
```

This pulls in `SFML.Net`. Note that SFML requires its native runtime libraries (`csfml-*`) to be
present alongside your executable; these ship with the `SFML.Net` package.

## Getting started

### 1. A window that draws something

```csharp
using Core.Drawing;
using Core.Engine;
using SFML.Graphics;
using SFML.System;

public class MyGame(uint width, uint height, string title) : UserWindow(width, height, title)
{
    protected override void Render()
    {
        GameContext.CurrentWindow.Clear(Color.Black);
        Draw.Circle(new Vector2f(width / 2f, height / 2f), 50,
            new DrawOptions { FillColor = Color.Cyan });
    }
}

public static class Program
{
    public static void Main() => new MyGame(800, 600, "My Game").Run();
}
```

Override the lifecycle hooks you need: `Start`, `Update`, `FixedUpdate`, `Render`, `DebugRender`, `Close`.

### 2. A component with behaviour

Components carry a transform, children, and the same lifecycle hooks. Move things in `FixedUpdate`
(time-stepped) and draw them in `Render`.

```csharp
using Core.Drawing;
using Core.Engine;
using Core.Entity;
using SFML.Graphics;
using SFML.System;

public class Ball : Component
{
    private Vector2f _velocity = new(120, 90);

    protected override void FixedUpdate()
        => Position += _velocity * GameContext.FixedDeltaTime;

    protected override void Render()
        => Draw.Circle(Position, 20, new DrawOptions { FillColor = Color.Red });
}
```

### 3. A scene

Any `Component` field on a scene is registered automatically — no manual wiring needed.

```csharp
using Core.Entity;

public class MainScene : Scene
{
    // Auto-registered because it's a Component field on the scene.
    private readonly Ball _ball = new() { Position = new(100, 100) };

    public override void OnStart()
    {
        // scene setup goes here (input bindings, spawning, etc.)
    }
}
```

Switch to it from your window's `Start`:

```csharp
protected override void Start() => SceneManager.SwitchScene<MainScene>();
```

### 4. Input

```csharp
using Core.Input;
using SFML.Window;

InputManager.BindAction(Keyboard.Key.Space, ActionType.Pressed, () => /* jump */);
InputManager.BindAction(Keyboard.Key.D,     ActionType.Held,    () => /* move right */);
InputManager.BindAction(Mouse.Button.Left,  ActionType.Pressed, () => /* shoot */);
```

### 5. Text & fonts

Text uses an embedded default font, so it works with zero setup. To use your own font, point the UI
theme (global) or a specific draw call at a `.ttf` file:

```csharp
using Core.Drawing.UserInterface;

UiTheme.FontPath = "Resources/myfont.ttf"; // leave null to use the embedded default

// or per draw call:
Draw.Text("Hello", new SFML.System.Vector2f(20, 20),
    new TextDrawOptions { FontPath = "Resources/myfont.ttf", CharacterSize = 24 });
```

## Namespaces at a glance

| Namespace                     | What's in it                                             |
| ----------------------------- | -------------------------------------------------------- |
| `Core.Engine`                 | `UserWindow`, `GameContext`, `Camera`                    |
| `Core.Entity`                 | `Scene`, `SceneManager`, `Component`                     |
| `Core.Input`                  | `InputManager`, `ActionType`                             |
| `Core.Collider`               | `ColliderBase`, `BoxCollider`, `CircleCollider`, manager |
| `Core.Drawing`                | `Draw`, `DrawOptions`, sprites, tilemaps                 |
| `Core.Drawing.UserInterface`  | `UiCanvas` and the UI widgets/layout                     |
| `Core.Resources`              | `ResourceManager<T>`, embedded resources                 |

## License

[MIT](https://mit-license.org/)
