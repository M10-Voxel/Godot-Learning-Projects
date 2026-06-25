using Godot;

public partial class SignalHub : Node
{
    public static SignalHub Instance { get; private set; }

    [Signal] public delegate void OnCreateBulletEventHandler(Vector2 position, Vector2 direction, float speed, PackedScene scene);

    
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
}
