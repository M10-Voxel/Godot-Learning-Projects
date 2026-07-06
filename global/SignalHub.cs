using Godot;

/// <summary>
/// A singleton node that acts as a central hub for emitting and receiving signals in the game.
/// </summary>
public partial class SignalHub : Node
{
    public static SignalHub Instance { get; private set; }

    [Signal] public delegate void OnCreateBulletEventHandler(
        Vector2 position,
        Vector2 direction,
        float speed,
        PackedScene scene
    );
    [Signal] public delegate void OnCreateDestructionEventHandler(Vector2 position);
    
    [Signal] public delegate void OnPointScoredEventHandler(int points);
    [Signal] public delegate void OnPlayerHitEventHandler(int lives, bool shake = true);
    [Signal] public delegate void OnEnemyDiedEventHandler(Vector2 position);
    
    [Signal] public delegate void OnLevelCompletedEventHandler(bool isCompleted);

    
    //* ________________________________________________________________________________________________
    //* GODOT BASE METHODS:
    
    public override void _Ready()
    { 
        Instance = this;
    }
    
    
    //* ________________________________________________________________________________________________
    //* SIGNAL METHODS:
    
    public static void EmitOnCreateBullet(Vector2 position, Vector2 direction, float speed, PackedScene scene)
    {
        Instance.EmitSignalOnCreateBullet(position, direction, speed, scene);
    }
    
    public static void EmitOnCreateDestruction(Vector2 position)
    {
        Instance.EmitSignalOnCreateDestruction(position);
    }

    public static void EmitOnPointScored(int points)
    {
        Instance.EmitSignalOnPointScored(points);
    }

    public static void EmitOnPlayerHit(int lives, bool shake = true)
    {
        Instance.EmitSignalOnPlayerHit(lives, shake);
    }
    
    public static void EmitOnEnemyDied(Vector2 position)
    {
        Instance.EmitSignalOnEnemyDied(position);
    }

    public static void EmitOnLevelCompleted(bool isCompleted)
    {
        Instance.EmitSignalOnLevelCompleted(isCompleted);
    }
}
