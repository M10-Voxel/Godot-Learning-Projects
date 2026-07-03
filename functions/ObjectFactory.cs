using Godot;

public partial class ObjectFactory : Node
{
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
	//* SIGNAL METHODS:
	
	private void OnCreateBullet(Vector2 position, Vector2 direction, float speed, PackedScene scene)
	{
		var bullet = scene.Instantiate<BulletBlueprint>();
		bullet.Setup(position, direction, speed);
		CallDeferred(MethodName.AddObject, bullet);
	}

	private void OnCreateExplosion(Vector2 position)
	{
		var explosion = _explosionScene.Instantiate<Boom>();
		explosion.GlobalPosition = position;
		CallDeferred(MethodName.AddObject, explosion);
	}

	private void OnCreateDestruction(Vector2 position)
	{
		var destruction = _destructionScene.Instantiate<Boom>();
		destruction.GlobalPosition = position;
		CallDeferred(MethodName.AddObject, destruction);
	}

	private void OnEnemyDied(Vector2 position)
	{
		OnCreateExplosion(position);
		var fruit = _fruitScene.Instantiate<FruitPickup>();
		fruit.GlobalPosition = position;
		CallDeferred(MethodName.AddObject, fruit);
	}

	private void AddObject(Node node)
	{
		AddChild(node);
	}
}
