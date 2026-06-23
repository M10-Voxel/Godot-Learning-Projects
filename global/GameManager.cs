using Godot;

public partial class GameManager : Node
{
    public GameManager Instance { get; private set; }


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

    public void LoadMain()
    {
        GetTree().ChangeSceneToPacked(_mainScene);
    }
    
    public void LoadLevel()
    {
        GetTree().ChangeSceneToPacked(_levelBlueprint);
    }
}
