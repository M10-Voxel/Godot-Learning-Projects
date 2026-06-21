using Godot;
using TappyPlane.scripts.components;

namespace TappyPlane.globals;

public partial class GameManager : Node
{
    
    public static GameManager Instance { get; private set; }

    private PackedScene _mainScreenScene = GD.Load<PackedScene>("res://scenes/MainScreen.tscn");
    private PackedScene _gameScene = GD.Load<PackedScene>("res://scenes/Game.tscn");
    private PackedScene _sceneChangeScene = GD.Load<PackedScene>("res://components/SceneChange.tscn");

    private PackedScene _nextScene;
    private SceneChange _sceneChange;

    
    public override void _Ready()
    {
        Instance = this;
        ProcessMode = ProcessModeEnum.Always;

        _sceneChange = _sceneChangeScene.Instantiate<SceneChange>();
        AddChild(_sceneChange);
    }

    private void StartChange(PackedScene toScene)
    {
        _nextScene = toScene;
        _sceneChange.PlayAnimation();
    }

    public static void LoadNextScene()
    {
        if (Instance._nextScene != null)
        {
            Instance.GetTree().ChangeSceneToPacked(Instance._nextScene);
        }
    }
    
    public static void LoadMainScreen()
    {
        Instance.StartChange(Instance._mainScreenScene);
    }

    public static void LoadGame()
    {
        Instance.StartChange(Instance._gameScene);
    }
}
