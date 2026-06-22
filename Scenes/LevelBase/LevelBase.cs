using Godot;

public partial class LevelBase : Node
{
	// EXPORTS:
	[Export] private Marker2D _animalStartPos;
	[Export] private PackedScene _animalScene;
	[Export] private PackedScene _mainScene;
	
	//* ________________________________________________________________________________________________
	//* STANDARD GODOT METHODS:
	
	public override void _Ready()
	{
		SignalHub.Instance.OnAnimalDied += SpawnAnimal;
		SpawnAnimal();
	}

	public override void _EnterTree()
	{
		Cup.NumCoups = 0;
	}

	public override void _ExitTree()
	{
		SignalHub.Instance.OnAnimalDied -= SpawnAnimal;
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_cancel"))
		{
			GetTree().ChangeSceneToPacked(_mainScene);
		}
	}

	//* ________________________________________________________________________________________________
	//* SIGNAL METHODS:

	private void SpawnAnimal()
	{
		GD.Print("Spawning animal");
		Animal newAnimal = _animalScene.Instantiate<Animal>();
		newAnimal.GlobalPosition = _animalStartPos.GlobalPosition;
		CallDeferred(Node.MethodName.AddChild, newAnimal);
	}
}
