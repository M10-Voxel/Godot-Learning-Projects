using Godot;

public partial class ObjectFactory : Node
{
	//* ________________________________________________________________________________________________
	//* GODOT BASE METHODS:
	
	public override void _Ready()
	{
		SignalHub.Instance.OnCreateBullet += OnCreateBullet;
	}
	    
	//* ________________________________________________________________________________________________
	//* SIGNAL METHODS:
	
	private void OnCreateBullet(Vector2 position, Vector2 direction, float speed, PackedScene scene)
	{
		BulletBlueprint bullet = scene.Instantiate<BulletBlueprint>();
		bullet.Setup(position, direction, speed);
		CallDeferred(MethodName.AddProjective, bullet);
	}

	private void AddProjective(Node node)
	{
		AddChild(node);
	}
}
