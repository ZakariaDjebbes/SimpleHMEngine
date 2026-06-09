using SFML.Graphics;

namespace Demo;

/// <summary>
/// Demo-app color palette. Lives in the app (not the engine) — games own their own colors.
/// </summary>
public static class Palette
{
    public static Color Beige      { get; } = new(201, 193, 176);
    public static Color Brown      { get; } = new(124, 102, 100);
    public static Color DarkGray   { get; } = new(37,  37,  40);
    public static Color LightAqua  { get; } = new(179, 202, 201);
    public static Color AquaGray   { get; } = new(121, 150, 148);
    public static Color DarkAqua   { get; } = new(85,  105, 103);
    public static Color LightRed   { get; } = new(141, 70,  64);
    public static Color DarkRed    { get; } = new(100, 47,  47);
    public static Color DeepRed    { get; } = new(73,  38,  34);
}
