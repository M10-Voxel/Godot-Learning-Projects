using Godot;

public partial class Game : Control
{
    [Export] private PackedScene _memoryTileScene;
    [Export] private GridContainer _parentGrid;
    [Export] private TextureButton _exitButton;
    
    //* ________________________________________________________________________________________________
    //* GODOT BASE METHODS:
    
    public override void _Ready()
    {
        SignalHub.Instance.OnLevelSelected += OnLevelSelected;
    }

    
    
    
    //* ________________________________________________________________________________________________
    //* SIGNAL METHODS:
    
    private void OnLevelSelected(LevelSetting levelSettings)
    {
        _parentGrid.Columns = levelSettings.Cols;
        
        for (int i = 0; i < levelSettings.TotalTiles; i++)
        {
            _parentGrid.AddChild(_memoryTileScene.Instantiate<MemoryTile>());
        }
    }

    private void OnExitButtonPressed()
    {
        foreach (var item in _parentGrid.GetChildren())
        {
            item.QueueFree();
        }
        SignalHub.EmitOnGameExitPressed();
    }
}
