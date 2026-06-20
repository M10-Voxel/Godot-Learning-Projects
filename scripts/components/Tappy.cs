using Godot;
using TappyPlane.globals;

namespace TappyPlane.scripts.components;

public partial class Tappy : CharacterBody2D
{
	[Export] private AnimatedSprite2D _playerSprite;
	[Export] private AnimationPlayer _playerAnimation;
	[Export] private AudioStreamPlayer _engineSound;
	
	private readonly float _gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	private const float JumpPower = -350.0f;
	private const string GroupName = "tappy";

	public override void _EnterTree()
	{
		AddToGroup(GroupName);
	}
	
	public override void _PhysicsProcess(double delta)
	{
		Fly(delta);
		
		MoveAndSlide();

		if (IsOnFloor()) Die();
	}

	private void Fly(double delta)
	{
		Vector2 velocity = Velocity;

		velocity.Y += _gravity * (float)delta;
		if (Input.IsActionJustPressed("jump"))
		{
			velocity.Y = JumpPower;
			
			_playerAnimation.Stop();
			_playerAnimation.Play("jump");
		}

		Velocity = velocity;
	}


	public void Die()
	{
		_engineSound.Stop();
		SignalHub.EmitOnTappyDied();
		GetTree().Paused = true;
	}
}
