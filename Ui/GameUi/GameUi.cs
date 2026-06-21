using Godot;
using System;

public partial class GameUi : Control
{
	[Export] private Label _levelLabel;
	[Export] private Label _attemptsLabel;

	private int _attempts = -1;
	
	public override void _Ready()
	{
		OnAttemptMade();
		SignalHub.Instance.OnAttemptMade += OnAttemptMade;
	}

	private void OnAttemptMade()
	{
		_attempts++;
		_attemptsLabel.Text = "Attempts: " + _attempts;
	}
}
