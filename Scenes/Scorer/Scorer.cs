using Godot;
using System;
using System.Collections.Generic;

public partial class Scorer : Node
{
	[Export] private Timer _revealTimer;
	
	[Export] private AudioStream _success;
	[Export] private AudioStream _gameWon;
	[Export] private AudioStreamPlayer _effects;
	
	public static bool SelectionEnabled { get; private set; } = true;

	private readonly List<MemoryTile> _selectedTiles = [];
	private int _movesMade = 0;
	private int _pairsMade = 0;
	private int _targetPairs = 0;
	
	public override void _Ready()
	{
		SignalHub.Instance.OnTileSelected += OnTileSelected;
		SignalHub.Instance.OnGameExitPressed += OnGameExitPressed;
	}

	public string GetMovesMade() => _movesMade.ToString();

	public string GetPairsMade() => $"{_pairsMade} / {_targetPairs}";


	public void ClearNewGame(LevelSetting levelSetting)
	{
		_selectedTiles.Clear();
		SelectionEnabled = true;
		
		_movesMade = 0;
		_pairsMade = 0;
		_targetPairs = levelSetting.TargetPairs;
		_effects.Stream = _success;
	}

	private void CheckForPair()
	{
		_movesMade++;
		if (_selectedTiles[0].MatchesOtherTile(_selectedTiles[1]))
		{
			_selectedTiles[0].KillOnPair();
			_selectedTiles[1].KillOnPair();
			_pairsMade++;
			_effects.Play();
		}
	}

	private void ProcessPair()
	{
		if (_selectedTiles.Count != 2) return;
		SelectionEnabled = false;
		_revealTimer.Start();
		CheckForPair();
	}


	private void OnTileSelected(MemoryTile memoryTile)
	{
		if (!SelectionEnabled) return;
		if (_selectedTiles.Contains(memoryTile)) return;
		
		_selectedTiles.Add(memoryTile);
		
		ProcessPair();
	}
	
	private void OnRevealTimerTimeout()
	{
		foreach (var item in _selectedTiles)
		{
			item.Reveal(false);
		}
		_selectedTiles.Clear();
		CheckGameWin();
	}

	private void CheckGameWin()
	{
		if (_pairsMade == _targetPairs)
		{
			SelectionEnabled = false;
			_effects.Stream = _gameWon;
			_effects.Play();
			SignalHub.EmitOnGameWon(_movesMade);
		}
		else
		{
			SelectionEnabled = true;
		}
	}

	private void OnGameExitPressed()
	{
		_revealTimer.Stop();
		_selectedTiles.Clear();
	}
}
