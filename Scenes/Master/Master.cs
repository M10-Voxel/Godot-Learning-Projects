using Godot;
public partial class Master : Control
{
    [Export] private Control _mainMenuScene;
    [Export] private Game _gameScene;

    //* ________________________________________________________________________________________________
    //* GODOT BASE METHODS:

    public override void _Ready()
    {
        ShowGame(false);
        SignalHub.Instance.OnLevelSelected += OnLevelSelected;
        SignalHub.Instance.OnGameExitPressed += OnGameExitPressed;
    }
    
    
    //* ________________________________________________________________________________________________
    //* OWN METHODS:
    
    private void ShowGame(bool show)
    {
        _gameScene.Visible = show;
        _mainMenuScene.Visible = !show;
    }


    //* ________________________________________________________________________________________________
    //* SIGNAL METHODS:
    
    private void OnLevelSelected(LevelSetting levelSettings)
    {
        GD.Print($"OnLevelSelected: {levelSettings}");
        ShowGame(true);
    }
    
    private void OnGameExitPressed()
    {
        ShowGame(false);
    }
    
}
