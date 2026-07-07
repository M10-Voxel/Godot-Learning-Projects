using Godot;

public partial class Snail : EnemyBlueprint
{
	// EXPORTS:
	[Export] private RayCast2D _floorDetect;
	
	// PRIVATE VARIABLES:
	private bool IsInMovingFrame => AnimatedSprite.Frame is >= 2 and <= 5;
	
	//* ________________________________________________________________________________________________
	//* GODOT BASE METHODS:

	public override void _Ready()
	{
		base._Ready();
		DelayInitialAnimation();
	}
	
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = ApplyGravity(delta);

		velocity = MoveSnail(velocity);
		
		Velocity = velocity;
		MoveAndSlide();
		
		if (!_floorDetect.IsColliding() || IsOnWall()) FlipSnail();
	}

	
	//* ________________________________________________________________________________________________
	//* SUB METHODS:

	/// <summary>
	/// Moves the snail in the direction it is facing only during movement animation frames
	/// </summary>
	/// <param name="velocity">The speed/velocity the snail currently has</param>
	/// <returns></returns>
	private Vector2 MoveSnail(Vector2 velocity)
	{
		if (!IsOnFloor()) return velocity;
		

		velocity.X = IsInMovingFrame ?
			AnimatedSprite.FlipH ? Speed : -Speed // If sprite is flipped (looking right), move right, else move left
			: 0.0f;

		return velocity;
	}

	/// <summary>
	/// Flips snail sprite and Raycast
	/// </summary>
	private void FlipSnail()
	{
		AnimatedSprite.FlipH = !AnimatedSprite.FlipH;	// Flips sprite in opposite direction from where it was facing before
		_floorDetect.Position = new Vector2(
			_floorDetect.Position.X * -1,				// Moves RayCast to opposite side via changing its operator (+/-)
			_floorDetect.Position.Y
		);
	}
}
