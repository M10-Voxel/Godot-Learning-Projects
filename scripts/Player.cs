using Godot;

namespace SpaceShooter.scripts;

public partial class Player : CharacterBody2D
{
	[Export] private float _speed = 800.0f;
	[Export] private float _stoppingDistance = 0.0f;
	[Export] private int _health = 3;

	[Export] private Node2D _anchor;
	[Export] private Timer _shootTimer;
	[Export] private Timer _powerUpTimer;
	
	[ExportGroup("Laser-Scenes")]
	[Export] private PackedScene _laser1;
	[Export] private PackedScene _laser2;
	[Export] private PackedScene _laser3;
	
	[ExportGroup("Ships")]
	[Export] private Node2D _allShips;
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

	public override void _Ready()
	{
		_global = GetNode<Global>("/root/Global");
		
		_explosionAnimation.Play(Animations.Idle);
		_powerUpAnimation.Play(Animations.Idle);
		_damageAnimation.Play(Animations.Idle);
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_global.GameOver || !Global.GameOn) return;
		
		TurnOnShipBody();
		Visible = true;

		if (Input.IsActionPressed("left_click"))
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
			
			if (_canShoot) ShootLaser();
		}

		if (_health <= 0 || _isDestroyed)
		{
			_global.SetGameOver(true);
			_speed = 0.0f;
			_allShips.Visible = false;
			_gameOverSound.Play();
			_explosionAnimation.Play(Animations.Explosion);
			_isDestroyed = true;
		}
		
	}


	private void TurnOnShipBody()
	{
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

	private void ShootLaser()
	{
		Laser newLaser;

		switch (Global.ChosenShip)
		{
			case 1:
				newLaser = _laser1.Instantiate<Laser>();
				AddSibling(newLaser);	// To be independent of the player
				_shootTimer.WaitTime = 0.5f - _powerUpBoost;
				break;
			case 2:
				newLaser = _laser2.Instantiate<Laser>();
				AddSibling(newLaser);
				_shootTimer.WaitTime = 0.8f - _powerUpBoost;
				break;
			case 3:
				newLaser = _laser3.Instantiate<Laser>();
				AddChild(newLaser);	// To follow the player
				_shootTimer.WaitTime = 1.5f - _powerUpBoost;
				break;
			default:
				newLaser = _laser1.Instantiate<Laser>();
				break;
		}
		
		newLaser.GlobalPosition = _anchor.GlobalPosition;
		_canShoot = false;
		_laserSound.Play();
		_shootTimer.Start();
	}


	private void OnArea2DAreaEntered(Area2D area)
	{
		if (area.IsInGroup("enemy"))
		{
			_damageAnimation.Play(Animations.Damage);
			_health -= 1;

			if (_health >= 1) _damageSound.Play();
			OnPowerUpTimerTimeout();
		}

		if (area.IsInGroup("power_up"))
		{
			area.GetParent().QueueFree();
			_powerUpBoost = 0.4f;
			_powerUpAnimation.Play(Animations.PowerUp);
			_powerUpTimer.Start();
		}
	}

	private void OnPowerUpTimerTimeout()
	{
		_powerUpBoost = 0.0f;
		_powerUpAnimation.Play(Animations.Idle);
	}

	private void OnShootTimerTimeout()
	{
		_canShoot = true;
	}
	
	private static class Animations
	{
		public static readonly StringName Idle = "idle";
		public static readonly StringName Explosion = "explosion";
		public static readonly StringName PowerUp = "power_up";
		public static readonly StringName Damage = "damage";
	}
}
