using Godot;

/// <summary>
/// Node that deletes the parent after a certain amount of time
/// </summary>
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

	/// <summary>
	/// Deletes the parent object, after timer ran out.
	/// </summary>
	private void OnTimeout()
	{
		GetParent().QueueFree();
	}
}
