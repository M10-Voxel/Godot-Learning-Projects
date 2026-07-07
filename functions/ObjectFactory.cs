using Godot;

/// <summary>
/// Creates objects in the game world at specified parameters
/// </summary>
public partial class ObjectFactory : Node
{
    // EXPORTS:
    [Export] private PackedScene _explosionScene;
    [Export] private PackedScene _destructionScene;
    [Export] private PackedScene _fruitScene;


    //* ________________________________________________________________________________________________
    //* GODOT BASE METHODS:

    public override void _Ready()
    {
        SignalHub.Instance.OnCreateBullet += OnCreateBullet;
        SignalHub.Instance.OnCreateDestruction += OnCreateDestruction;
        SignalHub.Instance.OnEnemyDied += OnEnemyDied;
    }

    public override void _ExitTree()
    {
        SignalHub.Instance.OnCreateBullet -= OnCreateBullet;
        SignalHub.Instance.OnCreateDestruction -= OnCreateDestruction;
        SignalHub.Instance.OnEnemyDied -= OnEnemyDied;
    }


    //* ________________________________________________________________________________________________
    //* OWN METHODS:
    
    /// <summary>
    /// Method used to be called as deferred -> creates object after frame end.
    /// </summary>
    /// <param name="node">The object to add as a child</param>
    private void AddObject(Node node)
    {
        AddChild(node);
    }


    //* ________________________________________________________________________________________________
    //* SIGNAL METHODS:

    /// <summary>
    /// Creates a specified bullet-projectile (scene)
    /// </summary>
    /// <param name="position">The position the bullet spawns</param>
    /// <param name="direction">The horizontal direction the bullet travels</param>
    /// <param name="speed">The speed at which the bullet travels</param>
    /// <param name="scene">The bullet type (currently either player or hawk)</param>
    private void OnCreateBullet(Vector2 position, Vector2 direction, float speed, PackedScene scene)
    {
        var bullet = scene.Instantiate<BulletBlueprint>();
        bullet.Setup(position, direction, speed);
        CallDeferred(MethodName.AddObject, bullet);
    }

    /// <summary>
    /// Creates an explosion (scene) that is used to show an animation after an enemy was hit
    /// </summary>
    /// <param name="position">The position the explosion spawns</param>
    private void OnCreateExplosion(Vector2 position)
    {
        var explosion = _explosionScene.Instantiate<Boom>();
        explosion.GlobalPosition = position;
        CallDeferred(MethodName.AddObject, explosion);
    }

    /// <summary>
    /// // Creates a destruction (scene) on bullet collision with terrain
    /// </summary>
    /// <param name="position">The position the destruction spawns</param>
    private void OnCreateDestruction(Vector2 position)
    {
        var destruction = _destructionScene.Instantiate<Boom>();
        destruction.GlobalPosition = position;
        CallDeferred(MethodName.AddObject, destruction);
    }

    /// <summary>
    /// Creates a fruit pickup (item) at given after explosion, when enemy was hit
    /// </summary>
    /// <param name="position">The position the item spawns</param>
    private void OnEnemyDied(Vector2 position)
    {
        OnCreateExplosion(position);
        var fruit = _fruitScene.Instantiate<FruitPickup>();
        fruit.GlobalPosition = position;
        CallDeferred(MethodName.AddObject, fruit);
    }

}
