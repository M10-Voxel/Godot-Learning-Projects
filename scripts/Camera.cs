using Godot;

namespace DPlattformer.scripts;

public partial class Camera : Camera2D
{
	[Export] private Node2D _target;
	[Export] private float _followSpeed = 8.0f;
	[Export] private Vector2 _offset = new(0, -40);

	public override void _PhysicsProcess(double delta)
	{
		if (_target == null) return;

		Vector2 wanted = _target.GlobalPosition + _offset;
		GlobalPosition = GlobalPosition.Lerp(wanted, 1.0f - Mathf.Exp(-_followSpeed * (float)delta));
		GlobalPosition = GlobalPosition.Round();
	}
}
