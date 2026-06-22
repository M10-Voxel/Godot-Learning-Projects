using Godot;
using System;

public partial class GameUi : Control
{
	// EXPORTS:
	[Export] private Label _levelLabel;
	[Export] private Label _attemptsLabel;
	[Export] private VBoxContainer _gameWonContainer;
	[Export] private AudioStreamPlayer _backgroundMusic;

	// LOCAL VARIABLES:
	private int _attempts = 0;
	
	//* ________________________________________________________________________________________________
	//* STANDARD GODOT METHODS:
	
	public override void _Ready()
	{
		OnAttemptMade();
		SignalHub.Instance.OnAttemptMade += OnAttemptMade;
		SignalHub.Instance.OnCupDestroyed += OnCupDestroyed;
	}

	public override void _ExitTree()
	{
		SignalHub.Instance.OnAttemptMade -= OnAttemptMade;
		SignalHub.Instance.OnCupDestroyed -= OnCupDestroyed;
	}


	//* ________________________________________________________________________________________________
	//* SIGNAL METHODS:
	
	private void OnAttemptMade()
	{
		_attempts++;
		_attemptsLabel.Text = "Attempts: " + _attempts;
	}

	private void OnCupDestroyed(int numRemaining)
	{
		if (numRemaining > 0) return;

		_gameWonContainer.Show();
		_backgroundMusic.Stop();
		ScoreManager.SetScoreForCurrentLevel(_attempts);
	}
}
