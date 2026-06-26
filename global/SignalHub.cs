using Godot;

public partial class SignalHub : Node
{
    public static SignalHub Instance { get; private set; }

    [Signal] public delegate void OnCreateBulletEventHandler(Vector2 position, Vector2 direction, float speed, PackedScene scene);
    [Signal] public delegate void OnCreateExplosionEventHandler(Vector2 position);
    [Signal] public delegate void OnCreateDestructionEventHandler(Vector2 position);

    [Signal] public delegate void OnEnemyDiedEventHandler(Vector2 position);

    
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

    public static void EmitOnCreateExplosion(Vector2 position)
    {
        Instance.EmitSignalOnCreateExplosion(position);
    }
    
    public static void EmitOnCreateDestruction(Vector2 position)
    {
        Instance.EmitSignalOnCreateDestruction(position);
    }

    public static void EmitOnEnemyDied(Vector2 position)
    {
        Instance.EmitSignalOnEnemyDied(position);
    }
}
