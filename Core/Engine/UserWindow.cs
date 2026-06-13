using Core.Collider;
using Core.Drawing;
using Core.Entity;
using Core.Input;

namespace Core.Engine;

/// <summary>
/// Base class for creating game windows with built-in game loop and scene management.
/// Provides virtual methods for game lifecycle events that can be overridden by derived classes.
/// </summary>
public abstract class UserWindow
{
    /// <summary>
    /// Gets the height of the window in pixels.
    /// </summary>
    protected uint Height { get; }

    /// <summary>
    /// Gets the width of the window in pixels.
    /// </summary>
    protected uint Width { get; }

    private string _title;
    private readonly string _originalTitle;
    private float _titleTimer;

    /// <summary>
    /// Gets or sets the window title. When set, updates the actual window title.
    /// </summary>
    protected string Title
    {
        get => _title;
        set
        {
            _title = value;
            GameContext.CurrentWindow?.SetTitle(_title);
        }
    }

    /// <summary>
    /// Initializes a new instance of the UserWindow class with the specified dimensions and title.
    /// </summary>
    /// <param name="width">The width of the window in pixels.</param>
    /// <param name="height">The height of the window in pixels.</param>
    /// <param name="title">The initial title of the window.</param>
    protected UserWindow(uint width, uint height, string title)
    {
        Height = height;
        Width = width;
        Title = title;
        _originalTitle = title;
    }

    /// <summary>
    /// Starts the game loop and runs the window until it is closed.
    /// This method handles the main game loop, including fixed updates, regular updates, and rendering.
    /// </summary>
    public void Run()
    {
        GameContext.Initialize(Width, Height, Title);
        GameContext.CurrentWindow.DispatchEvents();
        OnStart();

        while (GameContext.CurrentWindow.IsOpen)
        {
            _titleTimer -= GameContext.DeltaTime;
            if (_titleTimer <= 0.0f)
            {
                Title = $"{_originalTitle} - FPS: {GameContext.Fps:0.00} | Delta : {GameContext.DeltaTime:0.00} | FixedDelta : {GameContext.FixedDeltaTime:0.00}";
                _titleTimer = 1.0f;
            }

            GameContext.CurrentWindow.DispatchEvents();

            while (GameContext.ShouldFixedUpdate())
            {
                InputManager.CheckHeldKeys();
                OnFixedUpdate();
            }

            OnUpdate();

            OnRender();
            OnDebugRender();
            GameContext.UpdateTime();
            SceneManager.SceneSwapped = false;
        }

        OnClose();
    }

    /// <summary>
    /// Called once when the window starts. Override this method to perform initialization.
    /// </summary>
    protected virtual void Start() { }

    /// <summary>
    /// Called when the window is about to close. Override this method to perform cleanup.
    /// </summary>
    protected virtual void Close() { }

    /// <summary>
    /// Called every frame. Override this method to implement game logic.
    /// </summary>
    protected virtual void Update() { }

    /// <summary>
    /// Called at fixed intervals for physics and other time-dependent updates.
    /// Override this method to implement fixed update logic.
    /// </summary>
    protected virtual void FixedUpdate() { }

    /// <summary>
    /// Called every frame after Update. Override this method to implement rendering logic.
    /// </summary>
    protected virtual void Render() { }
    
    /// <summary>
    /// Called every frame for debug rendering. Override this method to implement debug rendering logic.
    /// </summary>
    protected virtual void DebugRender() { }
    
    private void OnStart() => Start();

    private void OnUpdate()
    {
        if (SceneManager.SceneSwapped) return;
        
        Scene.Current?.OnUpdate();
        Scene.Current?.UpdateAll();
        Update();
    }

    private void OnFixedUpdate()
    {
        if (SceneManager.SceneSwapped) return;

        Scene.Current?.OnFixedUpdate();
        if (!GameContext.IsPaused)
        {
            Scene.Current?.FixedUpdateAll();
            CollisionManager.UpdateCollision();
        }
        FixedUpdate();
    }

    private void OnClose()
    {
        if (SceneManager.SceneSwapped) return;
        
        Scene.Current?.OnClose();
        Scene.Current?.ReleaseAll();
        Close();
    }

    private void OnRender()
    {
        if (SceneManager.SceneSwapped) return;

        Draw.BeginFrame();
        Render();
        Scene.Current?.OnRender();
        Scene.Current?.RenderAll();
        if(!GameContext.IsDebugMode)
            GameContext.CurrentWindow.Display();
    }
    
    private void OnDebugRender()
    {
        if (!GameContext.IsDebugMode || SceneManager.SceneSwapped) return;
        
        DebugRender();
        Scene.Current?.OnDebugRender();
        Scene.Current?.DebugRenderAll();
        
        if(GameContext.IsDebugMode)
            GameContext.CurrentWindow.Display();
    }
}
