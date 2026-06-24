using Godot;

public partial class Snail : EnemyBlueprint
{
	// EXPORTS:
	[Export] private RayCast2D _floorDetect;
	[Export] private float _maxAnimationStartDelay = 0.8f;
	
	
	// PRIVATE VARIABLES:
	private bool IsInMovingFrame => AnimatedSprite.Frame is >= 2 and <= 5;
	
	//* ________________________________________________________________________________________________
	//* GODOT BASE METHODS:

	public override void _Ready()
	{
		DelayInitialAnimation();
	}
	
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		
		velocity.Y += Gravity * (float)delta;

		velocity = MoveSnake(velocity);
		
		Velocity = velocity;
		MoveAndSlide();
		
		FlipSnake();
	}

	
	//* ________________________________________________________________________________________________
	//* SUB METHODS:

	// FOR _Ready:
	private async void DelayInitialAnimation()
	{
		AnimatedSprite.Stop();

		float delay = GD.Randf() * _maxAnimationStartDelay;
		await ToSignal(GetTree().CreateTimer(delay), SceneTreeTimer.SignalName.Timeout);
		
		AnimatedSprite.Play();

	}

	// FOR _PhysicsProcess:
	private Vector2 MoveSnake(Vector2 velocity)
	{
		if (!IsOnFloor()) return velocity;
		

		velocity.X = IsInMovingFrame ?
			AnimatedSprite.FlipH ? Speed : -Speed // If sprite is flipped (looking right), move right, else move left
			: 0.0f;

		return velocity;
	}
	
	private void FlipSnake()
	{
		if (!_floorDetect.IsColliding() || IsOnWall())
		{
			AnimatedSprite.FlipH = !AnimatedSprite.FlipH;	// Flips sprite in opposite direction from where it was facing before
			_floorDetect.Position = new Vector2(
				_floorDetect.Position.X * -1,				// Moves RayCast to opposite side via changing its operator (+/-)
				_floorDetect.Position.Y
			);
		}
	}
	

	//* ________________________________________________________________________________________________
	//* OWN METHODS:

}
