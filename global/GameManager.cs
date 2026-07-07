using System.Collections.Generic;
using Godot;

/// <summary>
/// A singleton node that manages game scenes and player lives
/// </summary>
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
    
    /// <summary>
    /// Sets live to default amount
    /// </summary>
    private void ResetLives()
    {
        CurrentLives = MaxLives;
    }

    /// <summary>
    /// Changes to the Main Screen Scene
    /// </summary>
    private void LoadMain()
    {
        CurrentLevel = -1;
        ResetLives();
        GetTree().ChangeSceneToPacked(_mainScene);
    }
    /// <summary>
    /// Static method to change to Main Screen
    /// </summary>
    public static void ChangeToMainScreen() => Instance.LoadMain();
    
    /// <summary>
    /// Changes to the Next Level Scene and does not reset lives
    /// </summary>
    private void LoadNextLevel()
    {
        CurrentLevel++;
        if (CurrentLevel >= _levelScenes.Count) CurrentLevel = 0;
        GetTree().ChangeSceneToPacked(_levelScenes[CurrentLevel]);
    }
    /// <summary>
    /// Static method to change to the Next Level
    /// </summary>
    public static void ChangeToNextLevel() => Instance.LoadNextLevel();


    /// <summary>
    /// Reloads the Current Level Scene
    /// </summary>
    private void ReloadCurrentLevel()
    {
        if (CurrentLevel < 0) return;
        ResetLives();
        GetTree().ChangeSceneToPacked(_levelScenes[CurrentLevel]);
    }
    /// <summary>
    /// Static method to reload the Current Level
    /// </summary>
    public static void ReloadLevel() => Instance.ReloadCurrentLevel();
}
