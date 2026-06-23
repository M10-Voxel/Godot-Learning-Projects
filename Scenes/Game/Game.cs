using Godot;

public partial class Game : Control
{
    [Export] private PackedScene _memoryTileScene;
    [Export] private GridContainer _parentGrid;
    [Export] private TextureButton _exitButton;
    [Export] private Label _movesLabel;
    [Export] private Label _pairLabel;
    [Export] private Scorer _scorer;
    
    //* ________________________________________________________________________________________________
    //* GODOT BASE METHODS:
    
    public override void _Ready()
    {
        SignalHub.Instance.OnLevelSelected += OnLevelSelected;
    }

    public override void _Process(double delta)
    {
        _movesLabel.Text = _scorer.GetMovesMade();
        _pairLabel.Text = _scorer.GetPairsMade();
    }
    
    
    //* ________________________________________________________________________________________________
    //* SIGNAL METHODS:
    
    private void OnLevelSelected(LevelSetting levelSettings)
    {
        Texture2D frameImage = ImageManager.GetRandomFrameImage();
        _parentGrid.Columns = levelSettings.Cols;

        LevelDataSelector levelDataSelector = new LevelDataSelector();

        foreach (var item in levelDataSelector.GetImagesForLevel(levelSettings))
        {
            MemoryTile newTile = _memoryTileScene.Instantiate<MemoryTile>();
            _parentGrid.AddChild(newTile);
            newTile.Setup(item, frameImage);
        }

        _scorer.ClearNewGame(levelSettings);
    }

    private void OnExitButtonPressed()
    {
        foreach (var item in _parentGrid.GetChildren())
        {
            item.QueueFree();
        }
        SignalHub.EmitOnGameExitPressed();
        SignalHub.EmitOnButtonPressed();
    }
}
