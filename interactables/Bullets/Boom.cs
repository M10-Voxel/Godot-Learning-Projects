using Godot;

/// <summary>
/// Script logic for both explosion and destruction scene
/// </summary>
public partial class Boom : AnimatedSprite2D
{
	[Export] private AudioStreamPlayer2D _sound;
	
	public override void _Ready()
	{
		_sound.Play();
		AnimationFinished += QueueFree;
	}
}
