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
	
	// Method used to be called as deferred -> creates object after frame end
	private void AddObject(Node node)
	{
		AddChild(node);
	}

	
	//* ________________________________________________________________________________________________
	//* SIGNAL METHODS:
	
	/// <summary>
	/// Creates a specified (enemy- or player-) bullet
	/// </summary>
	/// <param name="position">Position where bullet should be created</param>
	/// <param name="direction">Direction (left/right) where bullet goes to</param>
	/// <param name="speed">Horizontal Speed that bullet travels at</param>
	/// <param name="scene">Type of bullet</param>
	private void OnCreateBullet(Vector2 position, Vector2 direction, float speed, PackedScene scene)
	{
		var bullet = scene.Instantiate<BulletBlueprint>();
		bullet.Setup(position, direction, speed);
		CallDeferred(MethodName.AddObject, bullet);
	}

	// Creates an explosion at given position
	private void OnCreateExplosion(Vector2 position)
	{
		var explosion = _explosionScene.Instantiate<Boom>();
		explosion.GlobalPosition = position;
		CallDeferred(MethodName.AddObject, explosion);
	}

	// Creates a destruction effect on bullet collision (with terrain) at given position
	private void OnCreateDestruction(Vector2 position)
	{
		var destruction = _destructionScene.Instantiate<Boom>();
		destruction.GlobalPosition = position;
		CallDeferred(MethodName.AddObject, destruction);
	}

	// Creates a fruit pickup (item) at given Position, after explosion, when enemy was hit
	private void OnEnemyDied(Vector2 position)
	{
		OnCreateExplosion(position);
		var fruit = _fruitScene.Instantiate<FruitPickup>();
		fruit.GlobalPosition = position;
		CallDeferred(MethodName.AddObject, fruit);
	}

}
