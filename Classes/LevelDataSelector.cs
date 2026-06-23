using Godot;
using System;

public partial class LevelDataSelector
{
    public Godot.Collections.Array<Texture2D> GetImagesForLevel(LevelSetting levelSetting)
    {
        ImageManager.ShuffleTileImages();
        Godot.Collections.Array<Texture2D> selectedImages = new();

        for (int i = 0; i < levelSetting.TargetPairs; i++)
        {
            selectedImages.Add(ImageManager.GetTileImageAtIndex(i));
            selectedImages.Add(ImageManager.GetTileImageAtIndex(i));
        }
        
        selectedImages.Shuffle();
        return selectedImages;
    }
}
