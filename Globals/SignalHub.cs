using Godot;
using System;

public partial class SignalHub : Node
{
	[Signal] public delegate void OnLevelSelectedEventHandler(LevelSetting levelSetting);
	[Signal] public delegate void OnGameExitPressedEventHandler();
	
	public static SignalHub Instance { get; private set; }


	public override void _Ready()
	{
		Instance = this;
	}


	public static void EmitOnLevelSelected(LevelSetting levelSetting)
	{
		Instance.EmitSignalOnLevelSelected(levelSetting);
	}

	public static void EmitOnGameExitPressed()
	{
		Instance.EmitSignalOnGameExitPressed();
	}
}
