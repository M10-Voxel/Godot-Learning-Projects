using System.Collections.Generic;
using Godot;

public partial class GameManager : Node
{
    // PROPERTIES:
    public static GameManager Instance { get; private set; }
    public int CurrentLevel { get; private set; } = -1;
    public int CurrentLives { get; set; } = 3;
    private int MaxLives { get; set; } = 3;


    // NAVIGATION SCENES:
    private PackedScene _mainScene = GD.Load<PackedScene>("uid://d00dg5ugpbytd");
    private readonly List<PackedScene> _levelScenes =
    [
        GD.Load<PackedScene>("uid://yvxhb8rdsdm0"), // LVL 1
        GD.Load<PackedScene>("uid://cvrmybxlxkvlf") // LVL 2
    ];
    
    
    //* ________________________________________________________________________________________________
    //* GODOT BASE METHODS:
    
    public override void _Ready()
    {
        Instance = this;
    }
    
    
    //* ________________________________________________________________________________________________
    //* OWN METHODS:
    
    // Sets live to default amount
    private void ResetLives()
    {
        CurrentLives = MaxLives;
    }

    // Changes to the Main Screen Scene
    private void LoadMain()
    {
        CurrentLevel = -1;
        ResetLives();
        GetTree().ChangeSceneToPacked(_mainScene);
    }
    public static void ChangeToMainScreen() => Instance.LoadMain();
    
    // Changes to the Next Level Scene
    private void LoadNextLevel()
    {
        CurrentLevel++;
        if (CurrentLevel >= _levelScenes.Count) CurrentLevel = 0;
        GetTree().ChangeSceneToPacked(_levelScenes[CurrentLevel]);
    }
    public static void ChangeToNextLevel() => Instance.LoadNextLevel();


    // Reloads the Current Level Scene
    private void ReloadCurrentLevel()
    {
        if (CurrentLevel < 0) return;
        ResetLives();
        GetTree().ChangeSceneToPacked(_levelScenes[CurrentLevel]);
    }
    public static void ReloadLevel() => Instance.ReloadCurrentLevel();
}
