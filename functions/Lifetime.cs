using Godot;

public partial class Lifetime : Node
{
	// EXPORTS:
	[Export] private Timer _timer;
	[Export] private float _waitTime = 30.0f;
	
	
	//* ________________________________________________________________________________________________
	//* GODOT BASE METHODS:
	
	public override void _Ready()
	{
		_timer.Start(_waitTime);
		_timer.Timeout += OnTimeout;
	}
	
		
	//* ________________________________________________________________________________________________
	//* SIGNAL METHODS:

	// Deletes the parent object, after timer ran out
	private void OnTimeout()
	{
		GetParent().QueueFree();
	}
}
