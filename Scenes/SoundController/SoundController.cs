using Godot;
using System;

public partial class SoundController : Node
{
	[Export] private AudioStreamPlayer _music;
	[Export] private AudioStreamPlayer _effects;

	[ExportGroup("Audio Streams")]
	[Export] private AudioStream _mainMenuMusic;
	[Export] private AudioStream _gameMusic;
	[Export] private AudioStream _click;
	[Export] private AudioStream _tileSelected;

public override void _Ready()
	{
		SignalHub.Instance.OnTileSelected += OnTileSelected;
		SignalHub.Instance.OnLevelSelected += OnLevelSelected;
		SignalHub.Instance.OnGameExitPressed += OnGameExitPressed;
		SignalHub.Instance.OnButtonPressed += OnButtonPressed;
		
		OnGameExitPressed();
	}

	private void OnTileSelected(MemoryTile memoryTile)
	{
		_effects.Stream = _tileSelected;
		_effects.Play();
	}
	
	private void OnLevelSelected(LevelSetting levelSetting)
	{ 
		_music.Stream = _gameMusic;
		_music.Play();
	}
	
	private void OnGameExitPressed()
	{
		_music.Stream = _mainMenuMusic;
		_music.Play();
	}

	private void OnButtonPressed()
	{
		_effects.Stream = _click;
		_effects.Play();
	}
}
