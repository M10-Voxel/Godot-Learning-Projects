using Godot;

namespace DiceCatcher.components.Fox;

public partial class Fox : Area2D
{
	[Signal] public delegate void PointScoredEventHandler();
	
	[Export] private Sprite2D _sprite;
	[Export] private AudioStreamPlayer2D _eatEffect;
	[Export(PropertyHint.Range,"150.0f, 300.0f")] private float _speed = 200.0f;
	
	private float _direction = 0.0f;

	

	public override void _Process(double delta)
	{
		_direction = Input.GetAxis("ui_left", "ui_right");
		
		if (!Mathf.IsZeroApprox(_direction)) _sprite.FlipH = _direction > 0;
	}

	public override void _PhysicsProcess(double delta)
	{
		Position += new Vector2(_direction * _speed * (float)delta, 0);
	}

	private void OnAreaEntered(Area2D area)
	{
		if (area is Dice.Dice)
		{
			_eatEffect.Play();
			area.QueueFree();
			EmitSignal(SignalName.PointScored);
		}
	}
}
