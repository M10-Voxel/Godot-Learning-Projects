using Godot;

public partial class ImageManager : Node
{
    public static ImageManager Instance { get; private set; }

    public TileImagesHolder TileImagesHolder { get; private set; }
    private Godot.Collections.Array<Texture2D> _frameImages;
    
    public override void _Ready()
    {
        Instance = this;
    }

    public override void _EnterTree()
    {
        TileImagesHolder = GD.Load<TileImagesHolder>("res://Resources/ImageTiles.tres");

        if (TileImagesHolder.TileImages.Count == 0)
        {
            GD.PrintErr("ImageManager has no tiles");
        }

        _frameImages = new Godot.Collections.Array<Texture2D>()
        {
            GD.Load<Texture2D>("res://Assets/frames/blue_frame.png"),
            GD.Load<Texture2D>("res://Assets/frames/green_frame.png"),
            GD.Load<Texture2D>("res://Assets/frames/red_frame.png"),
            GD.Load<Texture2D>("res://Assets/frames/yellow_frame.png"),
        };
    }

    public static Texture2D GetRandomFrameImage()
    {
        return Instance._frameImages.PickRandom();
    }
    
    public static Texture2D GetRandomTileImage()
    {
        return Instance.TileImagesHolder.GetRandomTileImage();
    }
    
    public static Texture2D GetTileImageAtIndex(int index)
    {
        return Instance.TileImagesHolder.GetTileImageAtIndex(index);
    }
    
    public static void ShuffleTileImages()
    {
        Instance.TileImagesHolder.ShuffleTileImages();
    }
}
