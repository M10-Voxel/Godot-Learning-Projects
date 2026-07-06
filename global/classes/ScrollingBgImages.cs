using Godot;

[GlobalClass]
public partial class ScrollingBgImages : Resource
{
    [Export] public Godot.Collections.Array<Texture2D> Images { get; set; } = [];
    
    public ScrollingBgImages() { }
}
