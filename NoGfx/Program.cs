namespace NoGfx;

public static class Program
{
    private const string GameTitle = "NoGfx";
    private const uint Width = 1280u;
    private const uint Height = Width / 12 * 9;
    
    public static void Main() 
        => new NoGfx(Width, Height, GameTitle).Run();
} 