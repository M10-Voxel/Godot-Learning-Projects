using Godot;

/// <summary>
/// Creates a list of images used to create a parallax scrolling background
/// </summary>
[GlobalClass]
public partial class ScrollingBgImages : Resource
{
    // EXPORTS:
    [Export] public Godot.Collections.Array<Texture2D> Images { get; set; } = [];
}
