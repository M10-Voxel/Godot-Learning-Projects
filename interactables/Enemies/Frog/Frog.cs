using Godot;

public partial class Frog : EnemyBlueprint
{
	// EXPORTS:
	[Export] private RayCast2D _smallWallDetect;
	[Export] private RayCast2D _highWallDetect;
	
	// PRIVATE VARIABLES:
	private const float MinXDistance = 60.0f;
	private const float MaxXDistance = 150.0f;
	private const float JumpHeight = -240.0f;
	
	//* ________________________________________________________________________________________________
	//* GODOT BASE METHODS:
	
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = ApplyGravity(delta);
		Velocity = velocity;
		
		MoveAndSlide();
		
		if (IsOnFloor())
		{
			AnimatedSprite.Play("frog_idle");
			Velocity = Vector2.Zero;
			FlipFrog();
		}
	}

	
	//* ________________________________________________________________________________________________
	//* SUB METHODS:

	/// <summary>
	/// Applies jump velocity and starts timer for next jump, when frog is on floor
	/// </summary>
	private void ApplyJump()
	{
		if (IsOnFloor())
		{
			AnimatedSprite.Play("frog_jump");
			Velocity = GetJump();
			Timer.Start(GD.RandRange(2.0f, 4.0f));
		}
	}
	/// <summary>
	/// Checks if frog can jump in direction. Determines jump direction (sprite rotation) and randomizes jump range.
	/// </summary>
	/// <returns>Vector2 containing jump range (with direction) and jump height</returns>
	private Vector2 GetJump()
	{
		if (_smallWallDetect.IsColliding() || _highWallDetect.IsColliding())
		{
			AnimatedSprite.FlipH = !AnimatedSprite.FlipH;	// Flips sprite in opposite direction from where it was facing before
		}

		var randomRange = (float)GD.RandRange(MinXDistance, MaxXDistance);
		var finalRange = AnimatedSprite.FlipH ? randomRange : -randomRange;
		
		return new Vector2(finalRange, JumpHeight);
	}
	
	/// <summary>
	/// Flips sprite and rotates raycasts in the opposite direction
	/// </summary>
	private void FlipFrog()
	{
		FlipSprite();
		_smallWallDetect.RotationDegrees = AnimatedSprite.FlipH ? 180.0f : 0.0f;
		_highWallDetect.RotationDegrees = AnimatedSprite.FlipH ? 180.0f : 0.0f;
	}

	
	//* ________________________________________________________________________________________________
	//* SIGNAL METHODS:

	/// <summary>
	/// Starts frog movement only after first time entering screen
	/// </summary>
	protected override void OnScreenEntered()
	{
		Timer.Start(GD.RandRange(2.0f, 4.0f));
	}
	
	/// <summary>
	/// On timer timeout make the frog jump
	/// </summary>
	protected override void OnTimerTimeout()
	{
		ApplyJump();
	}
}
