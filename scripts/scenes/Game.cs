using Godot;
using TappyPlane.globals;
using TappyPlane.scripts.components;

namespace TappyPlane.scripts.scenes;

public partial class Game : Node
{

	[Export] private Marker2D _upperSpawn;
	[Export] private Marker2D _lowerSpawn;
	[Export] private Node _pipesHolder;
	[Export] private PackedScene _pipesScene;
	
	public override void _Ready()
	{
		SpawnPipes();
	}
	
	private void SpawnPipes()
	{
		Pipes newPipes = _pipesScene.Instantiate<Pipes>();
		
		Vector2 position = new Vector2(_upperSpawn.Position.X, (float)GD.RandRange(_upperSpawn.Position.Y, _lowerSpawn.Position.Y));
		newPipes.Position = position;
		
		_pipesHolder.AddChild(newPipes);
	}
}
