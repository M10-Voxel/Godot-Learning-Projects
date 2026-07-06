using Godot;

public partial class Shooter : Node2D
{
	// EXPORTS:
	[Export] private float _bulletSpeed = 50.0f;
	[Export] private float _cooldownTime = 0.8f;
	
	[Export] private PackedScene _bulletScene;
	[Export] private Timer _cooldown;
	[Export] private AudioStreamPlayer2D _sound;
	
	// PRIVATE VARIABLES:
	private bool _canShoot = true;


	//* ________________________________________________________________________________________________
	//* GODOT BASE METHODS:
	
	public override void _Ready()
	{
		_cooldown.Timeout += OnTimerTimeout;
		_cooldown.WaitTime = _cooldownTime;
	}

	
	//* ________________________________________________________________________________________________
	//* OWN METHODS:
	
	/// <summary>
	/// Shoots a bullet in direction, when cooldown is over
	/// </summary>
	/// <param name="direction">The (horizontal) direction the object travels</param>
	public void Shoot(Vector2 direction)
	{
		if (!_canShoot) return;
		_canShoot = false;
		
		SignalHub.EmitOnCreateBullet(GlobalPosition, direction, _bulletSpeed, _bulletScene);
		_sound.Play();
		_cooldown.Start();
	}
	

	//* ________________________________________________________________________________________________
	//* SIGNAL METHODS:
	
	private void OnTimerTimeout()
	{
		_canShoot = true;
	}
}
