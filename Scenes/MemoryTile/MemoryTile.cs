using Godot;

public partial class MemoryTile : TextureButton
{
    [Export] private Control _imageParent;
    [Export] private TextureRect _itemImage;
    [Export] private TextureRect _frameImage;
    
    public Texture2D ItemTexture => _itemImage.Texture;
    
    //* ________________________________________________________________________________________________
    //* GODOT BASE METHODS:

    public void Reveal(bool show)
    {
        _imageParent.Visible = show;
    }

    public void Setup(Texture2D itemImage, Texture2D frameImage)
    {
        _itemImage.Texture = itemImage;
        _frameImage.Texture = frameImage;
    }
    
    
    //* ________________________________________________________________________________________________
    //* OWN METHODS:

    public bool MatchesOtherTile(MemoryTile otherTile)
    {
        return (this != otherTile && ItemTexture == otherTile.ItemTexture);
    }

    public void KillOnPair()
    {
        Disabled = true;

        ZIndex = 10;
        Tween tween = CreateTween();
        tween.SetParallel(true);
        tween.TweenProperty(this, Control.PropertyName.RotationDegrees.ToString(), 720.0f, 0.5f);
        tween.TweenProperty(this, Control.PropertyName.Scale.ToString(), new Vector2(1.5f, 1.5f), 0.5f);
        tween.SetParallel(false);
        tween.TweenInterval(0.5f);
        tween.TweenProperty(this, Control.PropertyName.Scale.ToString(), Vector2.Zero, 0.2f);
    }

    //* ________________________________________________________________________________________________
    //* SIGNAL METHODS:
    
    private void OnButtonPressed()
    {
        if (Scorer.SelectionEnabled)
        {
            Reveal(true);
            SignalHub.EmitOnTileSelected(this);
        }
    }
}
