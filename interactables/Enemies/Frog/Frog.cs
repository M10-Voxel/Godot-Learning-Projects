using Godot;

public partial class Frog : EnemyBlueprint
{
	// EXPORTS:
	[Export] private RayCast2D _smallWallDetect;
	[Export] private RayCast2D _highWallDetect;
	
	// PRIVATE VARIABLES:
	private const float MinXDistance = 80.0f;
	private const float MaxXDistance = 130.0f;
	private const float JumpHeight = -240.0f;
	
	private bool _inJump = false;
	
	//* ________________________________________________________________________________________________
	//* GODOT BASE METHODS:
	
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = ApplyGravity(delta);
		Velocity = velocity;
		
		ApplyJump();
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

	// FOR _PhysicsProcess:
	private void ApplyJump()
	{
		if (IsOnFloor() && _inJump)
		{
			AnimatedSprite.Play("frog_jump");
			Velocity = GetJumpDirection();
			_inJump = false;
			Timer.Start(GD.RandRange(2.0f, 4.0f));
		}
	}
	private Vector2 GetJumpDirection()
	{
		if (_smallWallDetect.IsColliding() || _highWallDetect.IsColliding())
		{
			AnimatedSprite.FlipH = !AnimatedSprite.FlipH;	// Flips sprite in opposite direction from where it was facing before
		}

		var randomRange = (float)GD.RandRange(MinXDistance, MaxXDistance);
		var finalRange = AnimatedSprite.FlipH ? randomRange : -randomRange;
		
		return new Vector2(finalRange, JumpHeight);
	}

	private void FlipFrog()
	{
		FlipSprite();
		_smallWallDetect.RotationDegrees = AnimatedSprite.FlipH ? 180.0f : 0.0f;
		_highWallDetect.RotationDegrees = AnimatedSprite.FlipH ? 180.0f : 0.0f;
	}

	//* ________________________________________________________________________________________________
	//* OWN METHODS:
	
	
	
	//* ________________________________________________________________________________________________
	//* SIGNAL METHODS:

	protected override void OnScreenEntered()
	{
		Timer.Start(GD.RandRange(2.0f, 4.0f));
	}
	
	protected override void OnTimerTimeout()
	{
		_inJump = true;
	}
}
