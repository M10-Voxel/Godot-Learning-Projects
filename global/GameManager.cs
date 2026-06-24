using Godot;

public partial class GameManager : Node
{
    public static GameManager Instance { get; private set; }


    // NAVIGATION SCENES:
    private PackedScene _mainScene = GD.Load<PackedScene>("res://ui/main_screen.tscn");
    private PackedScene _levelBlueprint = GD.Load<PackedScene>("res://scenes/Levels/level_blueprint.tscn");
    
    //* ________________________________________________________________________________________________
    //* GODOT BASE METHODS:
    
    public override void _Ready()
    {
        Instance = this;
    }
    
    
    //* ________________________________________________________________________________________________
    //* OWN METHODS:

    // Changes to the Main Screen Scene
    private void LoadMain() => GetTree().ChangeSceneToPacked(_mainScene);
    public static void ChangeToMainScreen() => Instance.LoadMain();
    
    // Changes to the Level Blueprint Scene
    private void LoadLevel() => GetTree().ChangeSceneToPacked(_levelBlueprint);
    public static void ChangeToLevel() => Instance.LoadLevel();


}
