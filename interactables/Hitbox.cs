using Godot;

/// <summary>
/// This Tool allows to easily set layer and mask collision for the imported scene
/// </summary>
[Tool]	// Tool Scripts run in the editor
public partial class Hitbox : Area2D
{
	// EXPORTS:
	[Export] private CollisionShape2D _collisionShape;
	[Export] private Shape2D Shape
	{
		get => _shape;
		set
		{
			_shape = value;

			// If in editor, update collision shape and draw/show it
			if (Engine.IsEditorHint() && _collisionShape != null)
			{
				_collisionShape.Shape = _shape;
				_collisionShape.QueueRedraw();
			}
		}
	}

	// PRIVATE VARIABLES:
	private Shape2D _shape;
	

	//* ________________________________________________________________________________________________
	//* GODOT BASE METHODS:
	
	public override void _Ready()
	{
		_collisionShape.Shape = Shape;
	}
}
