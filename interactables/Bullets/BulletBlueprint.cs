using Godot;

public partial class BulletBlueprint : Area2D
{
	
	[Export] private bool _isPlayerBullet = false;
	
	// PRIVATE VARIABLES:
	private Vector2 _direction = Vector2.Right;
	private const float BounceStrength = -470.0f;
	
	
	//* ________________________________________________________________________________________________
	//* GODOT BASE METHODS:
	
	public override void _Ready()
	{
		AreaEntered += OnAreaEntered;
		BodyEntered += OnBodyEntered;
	}
	
	public override void _PhysicsProcess(double delta)
	{
		Position += _direction * (float)delta;
		
	}
	
	
	//* ________________________________________________________________________________________________
	//* OWN METHODS:

	public void Setup(Vector2 position, Vector2 direction, float speed)
	{
		GlobalPosition = position;
		_direction = direction * speed;
	}


	//* ________________________________________________________________________________________________
	//* SIGNAL METHODS:

	private void OnAreaEntered(Area2D area)
	{
		if (area is Hitbox && area.GetParent() is Player player && _isPlayerBullet)
		{
			// When player falls and is above the bullet
			if (player.Velocity.Y > 0 && player.GlobalPosition.Y < GlobalPosition.Y)
			{
				player.Bounce(BounceStrength);
			}

			return;
		}
		
		QueueFree();
	}

	
	private void OnBodyEntered(Node2D body)
	{
		SignalHub.EmitOnCreateDestruction(GlobalPosition);
		QueueFree();
	}


}
