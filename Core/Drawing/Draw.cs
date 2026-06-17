using Core.Drawing.DrawOption;
using Core.Engine;
using Core.Resources;
using SFML.Graphics;
using SFML.System;

namespace Core.Drawing;

/// <summary>
/// Provides drawing utilities for various shapes, such as circles, rectangles, triangles, and lines, using SFML.
/// </summary>
public static class Draw
{
    private static RenderWindow _window = GameContext.CurrentWindow;

    private static readonly CircleShape CircleShape = new();
    private static readonly RectangleShape RectangleShape = new();
    private static readonly ConvexShape TriangleShape = new(3);
    private static readonly ConvexShape PolygonShape = new();
    private static readonly VertexArray LineVertices = new(PrimitiveType.Lines, 2);
    private static readonly Text TextDrawable = new() { Font = EmbeddedResources.DefaultFont };
    private static readonly Sprite SpriteDrawable = new();

    // The current drawing state (coordinate transform + anchor mode) and the saved-state stack behind
    // Push/Pop. This lives entirely inside Draw and is applied only through RenderStates on Draw's own
    // calls, so it can never alter the window View or how scenes/components render.
    private static DrawState _state = DrawState.Default;
    private static readonly Stack<DrawState> StateStack = new();

    private static RenderStates CurrentStates() => new(_state.Transform);

    /// <summary>
    /// Draws a circle at the specified center with the given radius.
    /// </summary>
    /// <param name="center">The center position of the circle.</param>
    /// <param name="radius">The radius of the circle.</param>
    /// <param name="options">Optional drawing options.</param>
    public static void Circle(Vector2f center, float radius, DrawOptions options = null)
    {
        CircleShape.Radius = radius;
        // A circle's position is always its center; DrawMode/anchor does not apply (its parameter is named center).
        CircleShape.Origin = new Vector2f(radius, radius);
        CircleShape.Position = center;
        ApplyDrawOptions(CircleShape, options);
        _window.Draw(CircleShape, CurrentStates());
    }

    /// <summary>
    /// Draws a rectangle with the specified position, width, and height.
    /// </summary>
    /// <param name="x">The X-coordinate of the rectangle's position.</param>
    /// <param name="y">The Y-coordinate of the rectangle's position.</param>
    /// <param name="width">The width of the rectangle.</param>
    /// <param name="height">The height of the rectangle.</param>
    /// <param name="options">Optional drawing options.</param>
    public static void Rectangle(float x, float y, float width, float height, DrawOptions options = null)
    {
        RectangleShape.Size = new Vector2f(width, height);
        // Anchor decides which point of the box (x, y) refers to; default TopLeft keeps the origin at (0, 0).
        RectangleShape.Origin = AnchorOrigin(width, height);
        RectangleShape.Position = new Vector2f(x, y);
        ApplyDrawOptions(RectangleShape, options);
        _window.Draw(RectangleShape, CurrentStates());
    }

    /// <summary>
    /// Draws a triangle defined by three points.
    /// </summary>
    /// <param name="point1">The first point of the triangle.</param>
    /// <param name="point2">The second point of the triangle.</param>
    /// <param name="point3">The third point of the triangle.</param>
    /// <param name="options">Optional drawing options.</param>
    public static void Triangle(Vector2f point1, Vector2f point2, Vector2f point3, DrawOptions options = null)
    {
        TriangleShape.SetPoint(0, point1);
        TriangleShape.SetPoint(1, point2);
        TriangleShape.SetPoint(2, point3);

        ApplyDrawOptions(TriangleShape, options);
        _window.Draw(TriangleShape, CurrentStates());
    }

    /// <summary>
    /// Draws a filled convex polygon through the given points, in order.
    /// The points must describe a convex shape; concave outlines will render incorrectly.
    /// </summary>
    /// <param name="points">The polygon's vertices, in winding order. At least three are required.</param>
    /// <param name="options">Optional drawing options.</param>
    public static void ConvexPolygon(IReadOnlyList<Vector2f> points, DrawOptions options = null)
    {
        if (points.Count < 3)
            throw new ArgumentException("A convex polygon needs at least three points.", nameof(points));

        PolygonShape.SetPointCount((uint)points.Count);
        for (uint i = 0; i < points.Count; i++)
            PolygonShape.SetPoint(i, points[(int)i]);

        ApplyDrawOptions(PolygonShape, options);
        _window.Draw(PolygonShape, CurrentStates());
    }


    /// <summary>
    /// Draws a line between two points.
    /// </summary>
    /// <param name="start">The start point of the line.</param>
    /// <param name="end">The end point of the line.</param>
    /// <param name="options">Optional drawing options.</param>
    public static void Line(Vector2f start, Vector2f end, DrawOptions options = null)
    {
        var color = options?.FillColor ?? Color.White;
        LineVertices[0] = new Vertex(start, color);
        LineVertices[1] = new Vertex(end, color);
        _window.Draw(LineVertices, CurrentStates());
    }

    /// <summary>
    /// Draws a prebuilt vertex array directly. The caller owns the array, making this the right
    /// entry point for static geometry (curves, grids, meshes) that is built once and drawn each frame.
    /// </summary>
    /// <param name="vertices">The vertex array to draw.</param>
    public static void Vertices(VertexArray vertices) => _window.Draw(vertices, CurrentStates());

    /// <summary>
    /// Draws a prebuilt vertex array with caller-supplied render states (e.g. a texture and transform).
    /// Use this for textured static geometry such as tile maps that manage their own transform.
    /// </summary>
    /// <param name="vertices">The vertex array to draw.</param>
    /// <param name="states">The render states (texture, transform, blend mode, shader) to draw with.</param>
    public static void Vertices(VertexArray vertices, RenderStates states) => _window.Draw(vertices, states);

    /// <summary>
    /// Draws a connected polyline through the given points.
    /// </summary>
    /// <param name="points">The points to connect, in order.</param>
    /// <param name="options">Optional drawing options. <c>FillColor</c> sets the line color.</param>
    /// <param name="closed">When <c>true</c>, connects the last point back to the first.</param>
    public static void Polyline(IReadOnlyList<Vector2f> points, DrawOptions options = null, bool closed = false)
    {
        if (points.Count < 2)
            return;

        var color = options?.FillColor ?? Color.White;
        var count = (uint)(points.Count + (closed ? 1 : 0));
        var strip = new VertexArray(PrimitiveType.LineStrip, count);

        for (uint i = 0; i < points.Count; i++)
            strip[i] = new Vertex(points[(int)i], color);
        if (closed)
            strip[(uint)points.Count] = new Vertex(points[0], color);

        _window.Draw(strip, CurrentStates());
    }

    /// <summary>
    /// Draws a string of text at the specified position with the given character size.
    /// </summary>
    /// <param name="text">The text to draw.</param>
    /// <param name="position">The position of the text's top-left corner.</param>
    /// <param name="options">Optional drawing options. <c>FillColor</c> sets the text color.</param>
    public static void Text(string text, Vector2f position, TextDrawOptions options = null)
    {
        TextDrawable.DisplayedString = text;

        // SFML glyph bounds carry an internal offset; fold it into the origin so the visual box (not the
        // glyph cell) is what the anchor lands on. With the default TopLeft anchor the origin is just the
        // glyph offset, so position marks the visual top-left.
        var bounds = TextDrawable.GetLocalBounds();
        var anchor = AnchorOrigin(bounds.Width, bounds.Height);
        TextDrawable.Origin = new Vector2f(bounds.Left + anchor.X, bounds.Top + anchor.Y);
        TextDrawable.Position = position;

        ApplyDrawOptions(TextDrawable, options);
        _window.Draw(TextDrawable, CurrentStates());
    }

    /// <summary>
    /// Draws a textured sprite at the given position. The texture is taken from the options
    /// (<see cref="SpriteDrawOptions.Texture"/> first, then <see cref="SpriteDrawOptions.TexturePath"/>),
    /// falling back to the engine's default texture when neither is set. <c>FillColor</c> tints the
    /// sprite (default white), <c>Opacity</c> fades it, and <c>Rotation</c> rotates it about the origin.
    /// </summary>
    /// <param name="position">The on-screen position of the sprite's origin (top-left by default).</param>
    /// <param name="options">
    /// Optional sprite options. A <see cref="Texture"/> or texture path converts implicitly, so
    /// <c>Draw.Sprite(pos, "player.png")</c> and <c>Draw.Sprite(pos, texture)</c> both work.
    /// </param>
    public static void Sprite(Vector2f position, SpriteDrawOptions options = null)
    {
        var texture = ResolveTexture(options);

        // The Sprite instance is shared and reused, so every call must restate the full state it wants
        // (texture, source rect, transform, tint) otherwise it inherits leftovers from the last draw.
        var rect = options?.SourceRect ?? new IntRect(0, 0, (int)texture.Size.X, (int)texture.Size.Y);
        SpriteDrawable.Texture = texture;
        SpriteDrawable.TextureRect = rect;
        SpriteDrawable.Position = position;
        SpriteDrawable.Origin = options?.Origin ?? AnchorOrigin(rect.Width, rect.Height);
        SpriteDrawable.Scale = options?.Scale ?? new Vector2f(1, 1);
        SpriteDrawable.Rotation = options?.Rotation ?? 0f;
        SpriteDrawable.Color = ResolveFill(options);

        _window.Draw(SpriteDrawable, CurrentStates());
    }

    private static Texture ResolveTexture(SpriteDrawOptions options)
    {
        if (options?.Texture is { } texture)
            return texture;
        
        return ResourceManager<Texture>.GetResource(options?.TexturePath);
    }

    /// <summary>
    /// Clears the drawing window to a solid color, erasing the previous frame. Call this once at the
    /// start of rendering. Unlike clearing through <see cref="GameContext"/>, this targets the same
    /// window <see cref="Draw"/> renders to the drawing window has been
    /// redirected via <see cref="SetDrawingWindow"/>.
    /// </summary>
    /// <param name="color">The color to fill the window with. Defaults to <see cref="Color.Black"/>.</param>
    public static void Clear(Color? color = null) => _window.Clear(color ?? Color.Black);

    /// <summary>
    /// Translates the current drawing frame by the given offset. Like all transform calls, this affects
    /// only subsequent <see cref="Draw"/> calls (through their <see cref="RenderStates"/>); it never
    /// touches the window <see cref="View"/>, so it cannot move scenes, components, or anything not drawn
    /// through <see cref="Draw"/>.
    /// </summary>
    /// <param name="x">The horizontal offset.</param>
    /// <param name="y">The vertical offset.</param>
    public static void Translate(float x, float y) => _state.Transform.Translate(x, y);

    /// <summary>Translates the current drawing frame by the given offset.</summary>
    /// <param name="offset">The offset to translate by.</param>
    public static void Translate(Vector2f offset) => _state.Transform.Translate(offset.X, offset.Y);

    /// <summary>Scales the current drawing frame uniformly on both axes.</summary>
    /// <param name="factor">The uniform scale factor.</param>
    public static void Scale(float factor) => _state.Transform.Scale(factor, factor);

    /// <summary>Scales the current drawing frame independently on each axis.</summary>
    /// <param name="x">The horizontal scale factor.</param>
    /// <param name="y">The vertical scale factor.</param>
    public static void Scale(float x, float y) => _state.Transform.Scale(x, y);

    /// <summary>Rotates the current drawing frame about its origin.</summary>
    /// <param name="degrees">The rotation angle, in degrees.</param>
    public static void Rotate(float degrees) => _state.Transform.Rotate(degrees);

    /// <summary>
    /// Sets the anchor that box primitives (<see cref="Rectangle"/>, <see cref="Text"/>,
    /// <see cref="Sprite"/>) interpret their position against. Point-based primitives (circle, triangle,
    /// polygon, line) are unaffected. Stays in effect until changed, popped or reset each frame.
    /// </summary>
    /// <param name="anchor">The anchor mode.</param>
    public static void DrawMode(Anchor anchor) => _state.Anchor = anchor;

    /// <summary>Resets the transform and anchor to their defaults (identity transform, top-left anchor).</summary>
    public static void ResetState() => _state = DrawState.Default;

    /// <summary>
    /// Saves the current transform and anchor onto the stack. Pair with <see cref="Pop"/>. Prefer
    /// <see cref="Pushed"/> with a <c>using</c> block, which restores automatically even on exception.
    /// </summary>
    public static void Push() => StateStack.Push(_state);

    /// <summary>Restores the most recently pushed transform and anchor, or the defaults if the stack is empty.</summary>
    public static void Pop() => _state = StateStack.Count > 0 ? StateStack.Pop() : DrawState.Default;

    /// <summary>
    /// Saves the current state and returns a scope that restores it when disposed.
    /// </summary>
    /// <returns>A disposable scope that pops on dispose.</returns>
    public static DrawScope Pushed() => new();

    // Resets all drawing state at the start of a frame so a forgotten Pop is bounded to a single frame
    // and can never bleed forward. Called by the engine's render loop before any drawing.
    internal static void BeginFrame()
    {
        StateStack.Clear();
        _state = DrawState.Default;
    }

    // Maps the current anchor to an origin within a box of the given size: the point that a primitive's
    // position refers to. TopLeft -> (0, 0), Center -> (w/2, h/2), BottomRight -> (w, h), etc.
    private static Vector2f AnchorOrigin(float width, float height)
    {
        var x = _state.Anchor switch
        {
            Anchor.TopCenter or Anchor.Center or Anchor.BottomCenter => width / 2f,
            Anchor.TopRight or Anchor.MiddleRight or Anchor.BottomRight => width,
            _ => 0f
        };
        var y = _state.Anchor switch
        {
            Anchor.MiddleLeft or Anchor.Center or Anchor.MiddleRight => height / 2f,
            Anchor.BottomLeft or Anchor.BottomCenter or Anchor.BottomRight => height,
            _ => 0f
        };
        return new Vector2f(x, y);
    }

    /// <summary>
    /// Sets the current rendering window.
    /// </summary>
    /// <param name="window">The rendering window to use.</param>
    public static void SetDrawingWindow(RenderWindow window) => _window = window;

    // The shape/text instances are shared and reused across every draw, so each call must fully
    // restate the styling it wants. Properties are assigned unconditionally with explicit fallbacks
    // (never left untouched), otherwise a draw would silently inherit rotation/outline/fill left
    // behind by a previous draw on the same instance. This is also why null options still resets.
    private static void ApplyDrawOptions(Drawable drawable, DrawOptions options)
    {
        switch (drawable)
        {
            case Text text:
                var textOptions = options as TextDrawOptions;
                text.Font = ResourceManager<Font>.GetResource(textOptions?.FontPath);
                text.CharacterSize = textOptions?.CharacterSize ?? 16;
                text.OutlineColor = options?.OutlineColor ?? Color.Transparent;
                text.OutlineThickness = options?.OutlineThickness ?? 0f;
                text.Rotation = options?.Rotation ?? 0f;
                text.FillColor = ResolveFill(options);
                break;

            case Shape shape:
                shape.OutlineColor = options?.OutlineColor ?? Color.Transparent;
                shape.OutlineThickness = options?.OutlineThickness ?? 0f;
                shape.Rotation = options?.Rotation ?? 0f;
                shape.FillColor = ResolveFill(options);
                break;
        }
    }

    /// <summary>
    /// Resolves the fill color from the options, defaulting to white when none is given, then
    /// applies opacity to that resolved color. Opacity therefore modifies the requested fill rather
    /// than whatever color a previous draw left on a shared instance.
    /// </summary>
    private static Color ResolveFill(DrawOptions options)
    {
        var color = options?.FillColor ?? Color.White;
        if (options?.Opacity is { } opacity)
            color.A = (byte)(opacity * 255);
        return color;
    }

    // A snapshot of the mutable drawing state saved and restored by Push/Pop.
    private struct DrawState
    {
        public Transform Transform;
        public Anchor Anchor;

        public static DrawState Default => new() { Transform = Transform.Identity, Anchor = Anchor.TopLeft };
    }
}