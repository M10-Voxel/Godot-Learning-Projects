using Godot;

public partial class LevelBase : Node
{
	// EXPORTS:
	[Export] private Marker2D _animalStartPos;
	[Export] private PackedScene _animalScene;
	
	//* ________________________________________________________________________________________________
	//* STANDARD GODOT METHODS:
	
	public override void _Ready()
	{
		SignalHub.Instance.OnAnimalDied += SpawnAnimal;
		SpawnAnimal();
	}

	public override void _ExitTree()
	{
		SignalHub.Instance.OnAnimalDied -= SpawnAnimal;
	}

	//* ________________________________________________________________________________________________
	//* HELPER METHODS:

	private void SpawnAnimal()
	{
		GD.Print("Spawning animal");
		Animal newAnimal = _animalScene.Instantiate<Animal>();
		newAnimal.GlobalPosition = _animalStartPos.GlobalPosition;
		CallDeferred(Node.MethodName.AddChild, newAnimal);
	}
}
