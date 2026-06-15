using Godot;

namespace SpaceShooter.scripts;

public partial class Player : CharacterBody2D
{
	[Export] private float _speed = 1000.0f;
	[Export] private float _stoppingDistance = 0.0f;
	[Export] private int _health = 3;

	[Export] private Node2D _anchor;
	
	[ExportGroup( "Timers" )]
	[Export] private Timer _shootTimer;
	[Export] private Timer _powerUpTimer;
	[Export] private Timer _invincibilityTimer;

	
	[ExportGroup("Laser-Scenes")]
	[Export] private PackedScene _laser1;
	[Export] private PackedScene _laser2;
	[Export] private PackedScene _laser3;
	
	[ExportGroup("Ships")]
	[Export] private Sprite2D _ship1;
	[Export] private Sprite2D _ship2;
	[Export] private Sprite2D _ship3;
	
	[ExportGroup("Animations")]
	[Export] private AnimationPlayer _explosionAnimation;
	[Export] private AnimationPlayer _powerUpAnimation;
	[Export] private AnimationPlayer _damageAnimation;

	[ExportGroup("Sounds")]
	[Export] private AudioStreamPlayer2D _laserSound;
	[Export] private AudioStreamPlayer2D _damageSound;
	[Export] private AudioStreamPlayer2D _gameOverSound;
	
	private Global _global;
	
	private bool _isDestroyed = false;
	private bool _canShoot = true;
	private float _powerUpBoost = 0.0f;
	private bool _isInvincible = false;
	
	
	private void OnGameOnChanged(bool gameOn)
	{
		if (!gameOn) return;

		_isDestroyed = false;
		_canShoot = true;
		_powerUpBoost = 0.0f;
		_isInvincible = false;

		_explosionAnimation.Play(Animations.Idle);
		_powerUpAnimation.Play(Animations.Idle);
		_damageAnimation.Play(Animations.Idle);

		ShowPlayer();
	}

	
	//________________________________________________________________________________________
	
	public override void _Ready()
	{
		_global = GetNode<Global>("/root/Global");
		_global.GameOnChanged += OnGameOnChanged;
		
		_explosionAnimation.Play(Animations.Idle);
		_powerUpAnimation.Play(Animations.Idle);
		_damageAnimation.Play(Animations.Idle);
		
		_ship1.Visible = false;
		_ship2.Visible = false;
		_ship3.Visible = false;
	}

	

	public override void _PhysicsProcess(double delta)
	{
		if (_global.GameOver || !_global.GameOn) return;

		HandleMovement();
		
		if (Input.IsActionPressed("left_click") && _canShoot)
		{
			HandleShooting();
		}

		if (_health <= 0 || _isDestroyed)
		{
			GameOver();
			return;
		}
		
	}
	
	public override void _ExitTree()
	{
		if (_global != null)
		{
			_global.GameOnChanged -= OnGameOnChanged;
		}
	}

	
	//________________________________________________________________________________________
	// SUB METHODS OF _PhysicsProcess:
	
	private void ShowPlayer()
	{
		Visible = true;
		
		_ship1.Visible = false;
		_ship2.Visible = false;
		_ship3.Visible = false;

		switch (Global.ChosenShip)
		{
			case 1:
				_ship1.Visible = true;
				break;
			case 2:
				_ship2.Visible = true;
				break;
			case 3:
				_ship3.Visible = true;
				break;
		}
	}

	
	private void HandleMovement()
	{
		Vector2 mousePosition = GetGlobalMousePosition();
		float distanceToMouse = GlobalPosition.DistanceTo(mousePosition);

		if (distanceToMouse > _stoppingDistance)
		{
			Vector2 direction = (mousePosition - GlobalPosition).Normalized();
			Velocity = direction * _speed;
		}
		else
		{
			// Stop the ship if mouse is too close to it
			Velocity = Vector2.Zero;
		}

		MoveAndSlide();
	}

	private void HandleShooting()
	{
		switch (Global.ChosenShip)
		{
			case 1:
				ShootLaser(_laser1, 0.4f);
				break;
			case 2:
				ShootLaser(_laser2, 0.8f);
				break;
			case 3:
				ShootLaser(_laser3, 2.0f, true);
				break;
			default:
				GD.PushWarning($"Unknown chosen ship: {Global.ChosenShip}");
				return;
		}
		
		_canShoot = false;
		_laserSound.Play();
		_shootTimer.Start();
	}

	private void ShootLaser(PackedScene laserType, float cooldown, bool followPlayer = false)
	{
		Laser newLaser = laserType.Instantiate<Laser>();
		
		if (followPlayer) AddChild(newLaser);
		else			  AddSibling(newLaser);
		
		newLaser.GlobalPosition = _anchor.GlobalPosition;
		
		_shootTimer.WaitTime = Mathf.Max(0.01f, cooldown - _powerUpBoost);
	}
	
	//________________________________________________________________________________________
	// ON COLLISION:
	
	private void OnArea2DAreaEntered(Area2D area)
	{
		if (area.IsInGroup("enemy"))
		{
			TakeDamage();
		}
		else if (area.IsInGroup("meteor"))
		{
			GameOver();
		}
		else if (area.IsInGroup("power_up"))
		{
			area.GetParent().QueueFree();
			_powerUpBoost = 0.4f;
			_powerUpAnimation.Play(Animations.PowerUp);
			_powerUpTimer.Start();
		}
	}
	
	private void TakeDamage(int amount = 1)
	{
		if (_isInvincible || _isDestroyed) return;
		
		_isInvincible = true;

		_damageAnimation.Play(Animations.Damage);
		_health = Mathf.Max(0, _health - amount);

		if (_health <= 0) GameOver();
		if (_health >= 1) _damageSound.Play();

		OnPowerUpTimerTimeout();
		_invincibilityTimer.Start();
	}


	private void GameOver()
	{
		if (_isDestroyed) return;

		_isDestroyed = true;

		_global.SetGameOver(true);
		_global.SetGameOn(false);

		_ship1.Visible = false;
		_ship2.Visible = false;
		_ship3.Visible = false;

		_gameOverSound.Play();
		_explosionAnimation.Play(Animations.Explosion);
	}


	//________________________________________________________________________________________
	// LINKED TIMER METHODS:
	
	// Stops the PowerUp
	private void OnPowerUpTimerTimeout()
	{
		_powerUpBoost = 0.0f;
		_powerUpAnimation.Play(Animations.Idle);
	}

	private void OnShootTimerTimeout()
	{
		_canShoot = true;
	}

	private void OnInvincibilityTimerTimeout()
	{
		_isInvincible = false;
	}
	
	//________________________________________________________________________________________
	// HELPER CLASS:
	
	private static class Animations
	{
		public static readonly StringName Idle = "idle";
		public static readonly StringName Explosion = "explode";
		public static readonly StringName PowerUp = "power_up";
		public static readonly StringName Damage = "damage";
	}
}
