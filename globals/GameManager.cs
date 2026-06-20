using Godot;

namespace TappyPlane.globals;

public partial class GameManager : Node
{
    
    public static GameManager Instance { get; private set; }

    private PackedScene _mainScreenScene = GD.Load<PackedScene>("res://scenes/MainScreen.tscn");
    private PackedScene _gameScene = GD.Load<PackedScene>("res://scenes/Game.tscn");

    
    public override void _Ready()
    {
        Instance = this;
    }

    public static void LoadMainScreen()
    {
        Instance.GetTree().ChangeSceneToPacked(Instance._mainScreenScene);
    }

    public static void LoadGame()
    {
        Instance.GetTree().ChangeSceneToPacked(Instance._gameScene);
    }
}
