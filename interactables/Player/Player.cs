using Godot;

public partial class Player : CharacterBody2D
{
	// EXPORTS:
	[Export] private Label _debugLabel;
	[Export] private AudioStreamPlayer2D _jumpSound;
	[Export] private Sprite2D _playerSprite;

	// CONSTS:
	private const float Gravity = 690.0f;
	private const float RunSpeed = 120.0f;
	private const float JumpSpeed = -270.0f;
	private const float MaxFallSpeed = 300.0f;
	
	// PLAYER STATES:
	private bool IsStill	=> Mathf.IsZeroApprox(Velocity.X);
	private bool IsFalling	=> Velocity.Y > 0;
	private bool OnFloor	=> IsOnFloor();

	// PRIVATE VARIABLES:
	private bool _hasJumped = false;

	//* ________________________________________________________________________________________________
	//* GODOT BASE METHODS:

	public override void _Ready()
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		velocity.Y += Gravity * (float)delta;

		velocity = GetInput(velocity);
		velocity.Y = Mathf.Clamp(velocity.Y, JumpSpeed, MaxFallSpeed);

		Velocity = velocity;

		MoveAndSlide();

		_debugLabel.Text = $"{Velocity.Y:000}";
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("jump") && !_hasJumped) _hasJumped = true;
	}

	
	//* ________________________________________________________________________________________________
	//* SUB METHODS:

	private Vector2 GetInput(Vector2 velocity)
	{
		velocity.X = Input.GetAxis("left", "right") * RunSpeed;

		if (IsOnFloor() && _hasJumped)
		{
			velocity.Y = JumpSpeed;
			_hasJumped = false;
			_jumpSound.Play();
		}

		// If player is moving left (negative x) flip sprite
		if (!Mathf.IsZeroApprox(velocity.X)) _playerSprite.FlipH = velocity.X < 0;

		return velocity;
	}


	//* ________________________________________________________________________________________________
	//* OWN METHODS:
}
