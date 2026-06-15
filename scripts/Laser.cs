using Godot;

namespace SpaceShooter.scripts;

public partial class Laser : CharacterBody2D
{
	[Export] private float _speed = 900.0f;
	[Export] public int LasterType { get; private set; } = 1;
	[Export] private PackedScene _laser;

	[ExportGroup("Two Extra Lasers")]
	[Export] private Sprite2D _laserSprite;
	[Export] private CollisionShape2D _laserCollision;
	[Export] private Node2D _rAnchor;
	[Export] private Node2D _lAnchor;

	private Global _global;
	
	private int _yDirection = -1;
	private int _xDirection = 0;
	private bool _shot = false;


	public override void _Ready()
	{
		_global = GetNode<Global>("/root/Global");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_global.GameOver) return;

		switch (LasterType)
		{
			case 1:
				Velocity = new Vector2(_xDirection, _yDirection) * _speed;
				MoveAndSlide();
				break;
			
			case 2:
				Velocity = new Vector2(_xDirection, _yDirection) * _speed;
				MoveAndSlide();
				if (!_shot) SpawnTwoExtraLaser();
				break;
			
			case 3:
				break;
		}
	}

	private void SpawnTwoExtraLaser()
	{
		_laserSprite.Visible = false;
		_laserCollision.Disabled = true;
		CreateLaser(600.0f, 1, 0, _rAnchor);
		CreateLaser(600.0f, -1, 0, _lAnchor);
		CreateLaser(0.0f, 1, 0, this, false);

		_shot = true;
	}

	private void CreateLaser(float speed, int xDirection, int yDirection, Node2D anchor, bool useAnchorRotation = true)
	{
		Laser newLaser = _laser.Instantiate<Laser>();
		AddChild(newLaser);
		newLaser._speed = speed;
		newLaser._xDirection = xDirection;
		newLaser._yDirection = yDirection;
		newLaser.GlobalPosition = anchor.GlobalPosition;
		if (useAnchorRotation) newLaser.GlobalRotation = anchor.GlobalRotation;
	}


	private void OnTimerTimeout()
	{
		if (_global.GameOver) return;
		QueueFree();
	}
}
