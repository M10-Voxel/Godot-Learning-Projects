using Godot;

public partial class ScrollingBackground : Node2D
{
    // EXPORTS:
    [Export] private ScrollingBgImages _bgImages;
    [Export] private Vector2 _baseSize = new(1920f, 1080f);
    [Export] private float _targetScale = 1.0f;

    
    //* ________________________________________________________________________________________________
    //* GODOT BASE METHODS:
    
    public override void _Ready()
    {
        if (_bgImages == null)
        {
            QueueFree();
            return;
        }
        
        Setup();
        Scale = new Vector2(_targetScale, _targetScale);            // Scale to target size.
        Position -= new Vector2(0f, _baseSize.Y * _targetScale);    // Move on Y axis depending on target size.
    }

    
    //* ________________________________________________________________________________________________
    //* SUB METHODS:
    
    // Layers images with different speeds to create a depth effect
    private void Setup()
    {
        float scrollGap = 1.0f / _bgImages.Images.Count;    // Regulates speed at which images move in comparison to camera
                                                            // Speed gets higher for every layer coming (visually) closer to player
        float currentScrollScale = 0.0f;                    // Starts with no movement (background)

        for (int i = 0; i < _bgImages.Images.Count; i++)
        {
            AddLayer(currentScrollScale, _bgImages.Images[i]);
            currentScrollScale += scrollGap;
        }
    }
    // Configures and adds one layer to the background
    private void AddLayer(float currentScrollScale, Texture2D image)
    {
        Parallax2D parallax2D = new Parallax2D();
        parallax2D.RepeatSize = new Vector2(_baseSize.X, 0.0f);         // Offsets images by given size (seamless transition)
        parallax2D.ScrollScale = new Vector2(currentScrollScale, 1.0f); // Speed of the layer
        
        Sprite2D sprite = new Sprite2D();
        sprite.Texture = image;     // Adding image
        sprite.Centered = false;    // Keeps image from being centered - prevents deformation
        
        parallax2D.AddChild(sprite);    // Adds image to layer
        AddChild(parallax2D);           // Adds layer to background
    }
}
