using Godot;

public partial class ScrollingBackground : Node2D
{
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
        Scale = new Vector2(_targetScale, _targetScale);
        Position -= new Vector2(0f, _baseSize.Y * _targetScale);
    }

    
    //* ________________________________________________________________________________________________
    //* SUB METHODS:
    
    private void Setup()
    {
        float scrollGap = 1.0f / _bgImages.Images.Count;
        float currentScrollScale = 0.0f;

        for (int i = 0; i < _bgImages.Images.Count; i++)
        {
            AddLayer(currentScrollScale, _bgImages.Images[i]);
            currentScrollScale += scrollGap;
        }
    }
    private void AddLayer(float currentScrollScale, Texture2D image)
    {
        Parallax2D parallax2D = new Parallax2D();
        parallax2D.RepeatSize = new Vector2(_baseSize.X, 0.0f);
        parallax2D.ScrollScale = new Vector2(currentScrollScale, 1.0f);
        
        Sprite2D sprite = new Sprite2D();
        sprite.Texture = image;
        sprite.Centered = false;
        
        parallax2D.AddChild(sprite);
        AddChild(parallax2D);
    }
}
