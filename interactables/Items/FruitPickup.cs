using System;
using Godot;

public partial class FruitPickup : Area2D
{
	[Export] private AnimatedSprite2D _sprite;
	[Export] private AudioStreamPlayer2D _sound;
	[Export] private int _points = 8;

	private bool _isOnGround = false;
	private const float FallSpeed = 200.0f;
	public override void _Ready()
	{
		PlayRandomAnimation();
		AreaEntered += OnPlayerEntered;
		BodyEntered += OnPlatformEntered;
		_sound.Finished += QueueFree;
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if (!_isOnGround)
		{
			Position += new Vector2(0, FallSpeed * (float)delta);
		}
	}

	
	private void PlayRandomAnimation()
	{
		var animationNames = _sprite.SpriteFrames.GetAnimationNames();

		if (animationNames.Length > 0)
		{
			string randName = animationNames[new Random().Next(animationNames.Length)];
			_sprite.Play(randName);
		}
	}
	
	private void OnPlayerEntered(Area2D area)
	{
		_sound.Play();
		Hide();
		AreaEntered -= OnPlayerEntered;
		SignalHub.EmitOnPointScored(_points);
	}
	
	private void OnPlatformEntered(Node2D body)
	{
		_isOnGround = true;
	}
}
