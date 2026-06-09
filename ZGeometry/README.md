# ZGeometry

A small, generic **2D geometry library** for .NET. It provides a numeric vector type and a handful of
2D primitives (circle, rectangle, triangle, line, point) with a fluent API for the common spatial
queries: *overlaps*, *contains*, and *intersects*.

Everything is generic over the numeric type (`float`, `double`, `int`, `long`, `decimal`, `byte`)
via `System.Numerics.INumber<T>`, so you can pick the precision you need.

> ⚠️ **Disclaimer:** This is a small hobby library. It's handy for games, prototypes, and learning,
> but it is not a comprehensive or heavily optimised computational-geometry package. Expect a limited
> primitive set and possible breaking changes between versions.

## Features

- **`Vector2D<T>`** — a generic 2D vector with arithmetic operators and helpers: `Magnitude`,
  `Normalize`, `DotProduct`, `CrossProduct`, `Perpendicular`, `Lerp`, `Reflect`, `Clamp`,
  polar/cartesian conversion, and more.
- **Primitives** — `Circle<T>`, `Rectangle<T>`, `Triangle<T>`, `Line<T>`, each with `Create(...)` factories.
- **Fluent spatial queries** — `Overlaps`, `Contains`, and `Intersects` extension methods across primitive pairs.
- **Tuple sugar** — `(T, T)` implicitly converts to `Vector2D<T>`, so call sites stay terse.

## Installation

```sh
dotnet add package ZGeometry
```

## Getting started

### Vectors

```csharp
using ZGeometry.Primitives.Point;

var a = Vector2D<float>.Create(3, 4);
var b = new Vector2D<float>(1, 2);

var sum       = a + b;            // (4, 6)
var scaled    = a * 2f;           // (6, 8)
var length    = a.Magnitude();    // 5
var unit      = a.Normalize();
var dot       = a.DotProduct(b);
```

### Primitives

Each primitive has a `Create` factory. The numeric type is inferred from the arguments, and `(x, y)`
tuples convert to vectors automatically.

```csharp
using ZGeometry.Primitives.Circle;
using ZGeometry.Primitives.Rectangle;
using ZGeometry.Primitives.Line;
using ZGeometry.Primitives.Triangle;

var circle    = Circle<float>.Create((100, 100), 40);
var rectangle = Rectangle<float>.Create((10, 10), (200, 120));   // position, size
var line      = Line<float>.Create((0, 0), (300, 300));
var triangle  = Triangle<float>.Create((0, 0), (100, 0), (50, 80));
```

### Spatial queries

The fluent extension methods read naturally. `Overlaps` and `Contains` return `bool`; `Intersects`
returns the boundary intersection points.

```csharp
using ZGeometry.Logic;

bool touching   = circle.Overlaps(rectangle);
bool inside     = circle.Contains(line);
var  crossings  = circle.Intersects(line);   // IEnumerable<Vector2D<float>>
```

## Namespaces at a glance

| Namespace                       | What's in it                                          |
| ------------------------------- | ----------------------------------------------------- |
| `ZGeometry.Primitives.Point`    | `Vector2D<T>`                                         |
| `ZGeometry.Primitives.Circle`   | `Circle<T>` and its `Create` factory                  |
| `ZGeometry.Primitives.Rectangle`| `Rectangle<T>` and its `Create` factory               |
| `ZGeometry.Primitives.Triangle` | `Triangle<T>` and its `Create` factory                |
| `ZGeometry.Primitives.Line`     | `Line<T>` and its `Create` factory                    |
| `ZGeometry.Logic`               | `Overlaps`, `Contains`, `Intersects` fluent queries   |

## License

[MIT](https://mit-license.org/)
