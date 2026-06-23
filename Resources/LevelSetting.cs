using Godot;

[GlobalClass]
public partial class LevelSetting : Resource
{
    [Export] public int Rows { get; private set; }
    [Export] public int Cols { get; private set; }
    
    public int TargetPairs => (Rows * Cols) / 2;
    public int TotalTiles => Rows * Cols;

    
    // EMPTY CONSTRUCTOR:
    public LevelSetting() {}

    public override string ToString()
    {
        return $"{Rows}x{Cols}";
    }
}
