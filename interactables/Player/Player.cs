using System.Collections.Generic;
using Godot;

public partial class Player : CharacterBody2D
{

	#region EXPORTS
	[Export] private PlayerCamera _camera;
	[Export] private AudioStreamPlayer2D _jumpSound;
	[Export] private AudioStreamPlayer2D _hurtSound;
	[Export] private Sprite2D _playerSprite;
	[Export] private Timer _hurtTimer;
	[Export] private AnimationPlayer _invincibilityAnimation;
	[Export] private Shooter _shooter;
	[Export] private Hitbox _hitbox;
	[Export (PropertyHint.Range, "1, 10,1")] private int _lives = 5;
	[Export] private int _camLimitLeft = -100000;
	[Export] private int _camLimitRight = 100000;
	[Export] private int _camLimitBottom = -100000;
	[Export] private int _camLimitTop = 100000;
	#endregion
	
	#region PLAYER STATES
	private bool IsStill	=> Mathf.IsZeroApprox(Velocity.X);
	private bool IsFalling	=> Velocity.Y > 0;
	private bool OnFloor	=> IsOnFloor();
	private bool IsHurt => _isHurt;
	#endregion
	
	#region CONSTS/READONLY
	private const float Gravity = 690.0f;
	private const float RunSpeed = 120.0f;
	private const float JumpSpeed = -270.0f;
	private const float MaxFallSpeed = 300.0f;
	private const float FallenOff = 200.0f;
	
	private readonly Vector2 _hurtJumpVelocity = new(0.0f, JumpSpeed/2);
	private readonly List<Area2D> _currentDamageAreas = [];
	#endregion

	#region PRIVATE BOOlS
	private bool _hasJumped = false;
	private bool _isHurt = false;
	private bool _isInvincible = false;
	private bool _hasFallenOff = false;
	#endregion
	

	//* ________________________________________________________________________________________________
	//* GODOT BASE METHODS:

	public override void _EnterTree()
	{
		AddToGroup(GameConstants.GroupPlayer);
	}

	public override void _Ready()
	{
		_hitbox.AreaEntered += OnHitboxEntered;
		_hitbox.AreaExited += OnHitboxExited;
		_invincibilityAnimation.AnimationFinished += TurnOffInvincibility;
		_hurtTimer.Timeout += () => _isHurt = false;	// Enable player again after hurt timer
		SetCameraLimits();
		CallDeferred(MethodName.LateInit);
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		velocity.Y += Gravity * (float)delta;

		velocity = GetInput(velocity);
		velocity.Y = Mathf.Clamp(velocity.Y, JumpSpeed, MaxFallSpeed);	// Cap falling velocity

		Velocity = velocity;

		MoveAndSlide();
		PlayerFallenOff();
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("jump") && !_hasJumped) _hasJumped = true;
		if (@event.IsActionPressed("shoot"))
		{
			Vector2 direction = _playerSprite.FlipH ? Vector2.Left : Vector2.Right;
			_shooter.Shoot(direction);
		}
	}

	
	//* ________________________________________________________________________________________________
	//* SUB METHODS:
	
	/// <summary>
	/// Sets camera boundaries based on exported values (adjusted in respective scene)
	/// </summary>
	private void SetCameraLimits()
    {
    	_camera.LimitLeft = _camLimitLeft;
    	_camera.LimitRight = _camLimitRight;
    	_camera.LimitBottom = _camLimitBottom;
    	_camera.LimitTop = _camLimitTop;
    }
	
	/// <summary>
	/// Method to delay the call for current lives to be sure, they are already set in the GameManager
	/// </summary>
	private void LateInit()
	{
		_lives = GameManager.Instance.CurrentLives;
		SignalHub.EmitOnPlayerHit(_lives, false);
	}
	
	/// <summary>
	/// Calculates velocity based on input (resulting direction) and if player has jumped (upwards velocity).
	/// Flips sprite depending on movement direction.
	/// </summary>
	/// <param name="velocity">The current velocity of the player</param>
	/// <returns>Altered velocity depending on player states and inputs</returns>
	private Vector2 GetInput(Vector2 velocity)
	{
		if (_isHurt) return velocity;
		
		// Determine move direction and speed
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

	/// <summary>
	/// When enemy reaches bottom limit, it is removed from the scene
	/// </summary>
	private void PlayerFallenOff()
	{
		if (GlobalPosition.Y > FallenOff && !_hasFallenOff)
		{
			ReduceLives(_lives);
			_hasFallenOff = true;
		}
	}

	//* ________________________________________________________________________________________________
	//* OWN METHODS:
	
	/// <summary>
	/// Adds vertical velocity to player when called
	/// </summary>
	/// <param name="bounceSpeed">The amount of velocity</param>
	public void Bounce(float bounceSpeed)
	{
		Vector2 velocity = Velocity;
		velocity.Y = bounceSpeed;
		Velocity = velocity;
	}

	/// <summary>
	/// Applies hit logic: life reduction, invincibility, and hurt jump.
	/// </summary>
	private void ApplyHit()
	{
		if (_isInvincible) return;
		ReduceLives(1);
		TurnOnInvincibility();
		ApplyHurtJump();
	}
	/// <summary>
	/// Reduces Player lives and sends signal
	/// </summary>
	/// <param name="reduction">The amount of hearts that should be reduced</param>
	private void ReduceLives(int reduction)
	{
		_lives -= reduction;
		GameManager.Instance.CurrentLives = _lives;
		SignalHub.EmitOnPlayerHit(_lives);
	}
	/// <summary>
	/// Turn _isInvincible to true and player animation
	/// </summary>
	private void TurnOnInvincibility()
	{
		_isInvincible = true;
		_invincibilityAnimation.Play("invincible");
	}
	/// <summary>
	/// Turn _isHurt to true, start immovability cooldown, play sound and overwrite velocity with hurtJump
	/// </summary>
	private void ApplyHurtJump()
	{
		_isHurt = true;
		_hurtTimer.Start();
		_hurtSound.Play();
		
		Velocity = _hurtJumpVelocity;
	}


	//* ________________________________________________________________________________________________
	//* SIGNAL METHODS:

	/// <summary>
	/// Applies a hit and adds the area to the list in order to check later if player is still in area
	/// </summary>
	/// <param name="area">The Hitbox (enemy or enemy bullet in this case) the payer entered</param>
	private void OnHitboxEntered(Area2D area)
	{
		CallDeferred(MethodName.ApplyHit);
		
		if (!_currentDamageAreas.Contains(area)) _currentDamageAreas.Add(area);
	}

	/// <summary>
	/// Removes the current area from the list
	/// </summary>
	/// <param name="area">The Hitbox (enemy or enemy bullet in this case) the payer exited</param>
	private void OnHitboxExited(Area2D area)
	{
		_currentDamageAreas.Remove(area);
    }

	/// <summary>
	/// Turns off invincibility and applies hit if player is still in an area
	/// </summary>
	/// <param name="animationName">Irrelevant</param>
	private void TurnOffInvincibility(StringName animationName)
	{
		_isInvincible = false;
		_invincibilityAnimation.Play("RESET");
		if (_currentDamageAreas.Count > 0)
		{
			CallDeferred(MethodName.ApplyHit);
		}
	}
}
