using Godot;

public partial class EnemyBlueprint : CharacterBody2D
{
	// EXPORTS:
	[Export] private VisibleOnScreenNotifier2D _screenNotifier;
	[Export] private EnemyHitbox _hitbox;
	[Export] protected AnimatedSprite2D AnimatedSprite;

	[Export] protected float Speed = 30.0f;
	
	
	// CONSTS:
	protected float Gravity = 800.0f;
	protected float FallenOff = 200.0f;
	
	

	//* ________________________________________________________________________________________________
	//* GODOT BASE METHODS:

	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
		EnemyFallenOff();
	}

	
	//* ________________________________________________________________________________________________
	//* SUB METHODS:

	private void EnemyFallenOff()
	{
		if (GlobalPosition.Y > FallenOff)
		{
			CallDeferred(Node.MethodName.QueueFree);
		}
	}

	
	//* ________________________________________________________________________________________________
	//* OWN METHODS:
	
}
