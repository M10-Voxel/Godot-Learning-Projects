using Godot;

public partial class BulletBlueprint : Area2D
{
	// EXPORTS:
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
		Position += _direction * (float)delta;	// Apply horizontal movement
	}
	
	
	//* ________________________________________________________________________________________________
	//* OWN METHODS:

	/// <summary>
	/// Sets bullet parameters before it is spawned
	/// </summary>
	/// <param name="position">The position the bullet spawns</param>
	/// <param name="direction">The horizontal direction the bullet travels</param>
	/// <param name="speed">The speed at which the bullet travels</param>
	public void Setup(Vector2 position, Vector2 direction, float speed)
	{
		GlobalPosition = position;
		_direction = direction * speed;
	}


	//* ________________________________________________________________________________________________
	//* SIGNAL METHODS:

	/// <summary>
	/// Checks bullet collision. When hitting the player and player falls onto the bullet, the player is bounced up.
	/// Else, bullet is destroyed - explosion is set off in EnemyBlueprint
	/// </summary>
	/// <param name="area">The colliding object with the bullet</param>
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

	/// <summary>
	/// Checks bullet collision. Triggers destruction animation (Method is called when hitting platforms)
	/// </summary>
	/// <param name="body">The colliding object with the bullet</param>
	private void OnBodyEntered(Node2D body)
	{
		SignalHub.EmitOnCreateDestruction(GlobalPosition);
		QueueFree();
	}
}
