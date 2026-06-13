# SimpleHMEngine

A small, hand-made 2D game engine built on [SFML.Net](https://www.sfml-dev.org/download/sfml.net/),
together with a generic 2D geometry library and a demo gallery that shows the engine in action.

> ⚠️ **Disclaimer:** This is a hobby project: small, opinionated, and not battle-tested. It's good
> for prototypes, game jams, and learning, but it is **not** a production engine like Unity, Godot,
> or MonoGame. Expect rough edges and breaking changes between versions.

## What's in here

| Project        | Folder       | Package          | Description                                                                 |
| -------------- | ------------ | ---------------- | --------------------------------------------------------------------------- |
| **Core**       | `Core/`      | `SimpleHMEngine` | The engine: game loop, scene/component tree, input, camera, collision, drawing, and a UI toolkit. |
| **ZGeometry**  | `ZGeometry/` | `ZGeometry`      | A generic 2D geometry library: `Vector2D<T>` and primitives with fluent `Overlaps`/`Contains`/`Intersects` queries. |
| **Demo**       | `NoGfx/`     | none             | A gallery app showcasing each area of the engine. (`Demo.csproj`)           |

`SimpleHMEngine` depends on `ZGeometry`. Each library has its own detailed README:
[Core](Core/README.md) and [ZGeometry](ZGeometry/README.md).

## Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/download) or newer
- SFML's native runtime libraries, which ship with the `SFML.Net` NuGet dependency

## Building & running

```sh
# build the whole solution
dotnet build SimpleHMEngine.sln

# run the demo gallery
dotnet run --project NoGfx/Demo.csproj
```

## The demo gallery

The demo opens a menu hub; each entry is a focused scene demonstrating one part of the engine:

- **Geometry & ZGeometry**: a mouse probe tested against shapes via overlaps / contains / intersects, with live intersection points.
- **Physics & Collisions**: bouncing balls driven by fixed-update physics and `CollisionEnter`/`Exit` events.
- **Drawing Primitives**: circles, rectangles, triangles, lines, gradients, text and sprites via the immediate-mode `Draw` helper, including a transform stack with anchor modes.
- **UI Toolkit**: cards, stack panels, a slider, progress bar, checkboxes and buttons wired to live state.
- **TileMap & Camera**: a tiled map rendered in one draw call, panned with a follow camera (WASD / arrows).

Press **Esc** in any scene to return to the menu.

## Using the engine in your own project

```sh
dotnet add package SimpleHMEngine
```

A minimal window:

```csharp
using Core.Drawing;
using Core.Engine;
using SFML.Graphics;
using SFML.System;

public class MyGame(uint width, uint height, string title) : UserWindow(width, height, title)
{
    protected override void Render()
    {
        Draw.Clear(Color.Black);
        Draw.Circle(new Vector2f(width / 2f, height / 2f), 50, new DrawOptions { FillColor = Color.Cyan });
    }
}

public static class Program
{
    public static void Main() => new MyGame(800, 600, "My Game").Run();
}
```

See the [Core README](Core/README.md) for scenes, components, input, sprites, transforms, fonts, and more.

## License

[MIT](https://mit-license.org/)
