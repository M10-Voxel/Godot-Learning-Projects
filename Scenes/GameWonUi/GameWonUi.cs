using Godot;
using System;

public partial class GameWonUi : PanelContainer
{
	[Export] private Label _finalMovesLabel;
	
	public override void _Ready()
	{
		Hide();
		SignalHub.Instance.OnGameExitPressed += Hide;
		SignalHub.Instance.OnGameWon += OnGameWon;
	}
	
	
	private void OnGameWon(int moves)
	{
		_finalMovesLabel.Text = $"You took {moves} moves to find all the pairs!";
		Show();
	}
}
