using Godot;

namespace TappyPlane.scripts.components;

public partial class Tappy : CharacterBody2D
{
	[Export] private AnimatedSprite2D _playerSprite;
	
	private readonly float _gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	private const float JumpPower = -350.0f;

	public override void _Ready()
	{
	}
	
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		velocity.Y += _gravity * (float)delta;
		if (Input.IsActionJustPressed("jump")) velocity.Y = JumpPower;

		Velocity = velocity;
		MoveAndSlide();

		if (IsOnFloor()) Die();
	}


	private void Die()
	{
		SetPhysicsProcess(false);
		_playerSprite.Stop();
	}
}
