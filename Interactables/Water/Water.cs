using Godot;

public partial class Water : Area2D
{
	[Export] private AudioStreamPlayer2D _splashSound;

	private void OnBodyWaterEntered(Node2D body)
	{
		_splashSound.GlobalPosition = body.GlobalPosition;
		_splashSound.Play();
		SignalHub.EmitOnAttemptMade();

		if (body is Animal animal) animal.Die();
	}
}