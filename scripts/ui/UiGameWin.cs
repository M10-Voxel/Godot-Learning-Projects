using Godot;

namespace DPlattformer.scripts.ui;

public partial class UiGameWin : CanvasLayer
{
	[Export] private Label _finalScore;
	
	private Global _global;
	
	public override void _Ready()
	{
		_global = GetNode<Global>("/root/Global");

		_global.GameOverChanged += OnGameOver;
	}

	public override void _ExitTree()
	{
		_global.GameOverChanged -= OnGameOver;
	}

	private void OnGameOver(bool gameOver)
	{
		if (gameOver)
		{
			_finalScore.Text = "Coins: " + Global.Coins;
		}
	}
}
