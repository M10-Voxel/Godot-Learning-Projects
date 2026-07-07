using System;
using Godot;

public partial class FruitPickup : Area2D
{
	// EXPORTS:
	[Export] private AnimatedSprite2D _sprite;
	[Export] private AudioStreamPlayer2D _sound;
	[Export] private int _points = 8;

	// CONSTS:
	private const float FallSpeed = 200.0f;
	
	// PRIVATE VARIABLES:
	private bool _isOnGround = false;
	
	
	//* ________________________________________________________________________________________________
	//* GODOT BASE METHODS:
	
	public override void _Ready()
	{
		PickRandomSpriteViaAnimation();
		AreaEntered += OnPlayerEntered;
		BodyEntered += OnPlatformEntered;
		_sound.Finished += QueueFree;
	}
	
	public override void _PhysicsProcess(double delta)
	{
		// Falls down until it hits the ground
		if (!_isOnGround)
		{
			Position += new Vector2(0, FallSpeed * (float)delta);
		}
	}

	
	//* ________________________________________________________________________________________________
	//* SUB METHODS:
	
	/// <summary>
	/// Picks a random animation - in this case a different sprite
	/// </summary>
	private void PickRandomSpriteViaAnimation()
	{
		var animationNames = _sprite.SpriteFrames.GetAnimationNames();

		if (animationNames.Length > 0)
		{
			string randName = animationNames[new Random().Next(animationNames.Length)];
			_sprite.Play(randName);
		}
	}
	
	
	//* ________________________________________________________________________________________________
	//* SIGNAL METHODS:
	
	/// <summary>
	/// Plays sound, hides the fruit, and emits PointScored signal when player enters/collects fruit
	/// </summary>
	/// <param name="area">In this case only the player itself</param>
	private void OnPlayerEntered(Area2D area)
	{
		_sound.Play();
		Hide();
		AreaEntered -= OnPlayerEntered;
		SignalHub.EmitOnPointScored(_points);
	}
	
	/// <summary>
	/// Sets _isOnGround to true when fruit hits the ground
	/// </summary>
	/// <param name="body">In this case only platforms</param>
	private void OnPlatformEntered(Node2D body)
	{
		_isOnGround = true;
	}
}
