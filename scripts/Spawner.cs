using Godot;


namespace SpaceShooter.scripts;

public partial class Spawner : Node2D
{
	[Export] private Timer _spawnIntervalTimer;
	[Export] private float  _powerUpChance = 0.05f;
	
	[ExportGroup( "Scenes" )]
	[Export] private PackedScene _enemy1;
	[Export] private PackedScene _enemy2;
	[Export] private PackedScene _meteor;
	[Export] private PackedScene _powerup;

	[ExportGroup("Spawn Borders")]
	[Export] private Node2D _lAnchor;
	[Export] private Node2D _rAnchor;

	private Global _global;
	private float _spawnIntervalRate = 0.01f;
	private float _minSpawnTime = 0.5f;
	
	public override void _Ready()
	{
		_global = GetNode<Global>("/root/Global");
	}

	public override void _Process(double delta)
	{
		if (_global.GameOn && !_global.GameOver)
		{
			if (_spawnIntervalTimer.WaitTime > _minSpawnTime)
			{
				_spawnIntervalTimer.WaitTime -= _spawnIntervalRate * delta;
			}
		}
	}


	private void OnSpawnIntervalTimerTimeout()
	{
		if (!_global.GameOn || _global.GameOver) return;
		
		PackedScene randomNodeScene;
		

		if (GD.Randf() < _powerUpChance)
		{
			randomNodeScene = _powerup;
		}
		else
		{
			PackedScene[] nodeToSpawn = [_enemy1, _enemy2, _enemy1, _enemy2, _meteor];
			randomNodeScene = nodeToSpawn[GD.Randi() % nodeToSpawn.Length];
		}
		
		Interactable newNode = randomNodeScene.Instantiate<Interactable>();
		float randomX = (float)GD.RandRange(_lAnchor.GlobalPosition.X, _rAnchor.GlobalPosition.X);
		newNode.GlobalPosition = new Vector2(randomX, _rAnchor.GlobalPosition.Y);
		AddChild(newNode);
		
	}
}
