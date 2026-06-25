using Godot;

public partial class BulletBlueprint : Area2D
{
	
	// PRIVATE VARIABLES:
	private Vector2 _direction = Vector2.Right;
	
	
	//* ________________________________________________________________________________________________
	//* GODOT BASE METHODS:
	
	public override void _Ready()
	{
	}
	
	public override void _PhysicsProcess(double delta)
	{
		Position += _direction * (float)delta;
		
	}
	
	
	//* ________________________________________________________________________________________________
	//* OWN METHODS:

	public void Setup(Vector2 position, Vector2 direction, float speed)
	{
		GlobalPosition = position;
		_direction = direction * speed;
	}
}
