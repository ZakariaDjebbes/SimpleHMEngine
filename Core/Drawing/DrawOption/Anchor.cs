namespace Core.Drawing.DrawOption;

/// <summary>
/// Defines where an element is placed within its parent area.
/// </summary>
public enum Anchor
{
    /// <summary>Top-left corner.</summary>
    TopLeft,
    /// <summary>Top edge, horizontally centered.</summary>
    TopCenter,
    /// <summary>Top-right corner.</summary>
    TopRight,
    /// <summary>Left edge, vertically centered.</summary>
    MiddleLeft,
    /// <summary>Centered on both axes.</summary>
    Center,
    /// <summary>Right edge, vertically centered.</summary>
    MiddleRight,
    /// <summary>Bottom-left corner.</summary>
    BottomLeft,
    /// <summary>Bottom edge, horizontally centered.</summary>
    BottomCenter,
    /// <summary>Bottom-right corner.</summary>
    BottomRight
}