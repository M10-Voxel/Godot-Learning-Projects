using Godot;

namespace DiceCatcher.components.Dice;

public partial class Dice : Area2D
{
	[Export] private Sprite2D _sprite;
	
	private const float Speed = 80.0f;
	private const float BaseRotationSpeed = 4.0f;

	private float _rotationSpeed = BaseRotationSpeed;

	public override void _Ready()
	{
		if(GD.Randf() < 0.5f) _rotationSpeed *= -1;
	}
	
	public override void _PhysicsProcess(double delta)
	{
		Position += new Vector2(0, Speed * (float)delta);
		_sprite.Rotate(_rotationSpeed * (float)delta);
	}
}