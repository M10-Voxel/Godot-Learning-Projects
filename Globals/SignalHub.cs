using Godot;

public partial class SignalHub : Node
{
	[Signal] public delegate void OnTileSelectedEventHandler(MemoryTile memoryTile);
	[Signal] public delegate void OnLevelSelectedEventHandler(LevelSetting levelSetting);
	[Signal] public delegate void OnGameExitPressedEventHandler();
	[Signal] public delegate void OnGameWonEventHandler(int moves);
	[Signal] public delegate void OnButtonPressedEventHandler();
	
	public static SignalHub Instance { get; private set; }


	public override void _Ready()
	{
		Instance = this;
	}


	public static void EmitOnTileSelected(MemoryTile memoryTile)
	{
		Instance.EmitSignalOnTileSelected(memoryTile);
	}
	
	public static void EmitOnLevelSelected(LevelSetting levelSetting)
	{
		Instance.EmitSignalOnLevelSelected(levelSetting);
	}

	public static void EmitOnGameExitPressed()
	{
		Instance.EmitSignalOnGameExitPressed();
	}

	public static void EmitOnGameWon(int moves)
	{
		Instance.EmitSignalOnGameWon(moves);
	}
	
	public static void EmitOnButtonPressed()
	{
		Instance.EmitSignalOnButtonPressed();
	}
}
