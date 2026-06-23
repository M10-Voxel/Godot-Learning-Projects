using Godot;

[GlobalClass]
public partial class TileImagesHolder : Resource
{
    [Export] public Godot.Collections.Array<Texture2D> TileImages { get; private set; } = new();

    // EMPTY CONSTRUCTOR:
    public TileImagesHolder() {}


    
    
    //* ________________________________________________________________________________________________
    //* OWN METHODS:

    public Texture2D GetRandomTileImage()
    {
        return TileImages.PickRandom();
    }

    public Texture2D GetTileImageAtIndex(int index)
    {
        if (TileImages.Count == 0 || index >= TileImages.Count)
        {
            GD.PrintErr("TileImages.Count == 0 || index >= TileImages.Count");
        }
        
        return TileImages[index];
    }
    
    public void ShuffleTileImages()
    {
        TileImages.Shuffle();
    }
}
