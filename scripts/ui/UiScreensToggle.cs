using Godot;

namespace DPlattformer.scripts.ui;

public partial class UiScreensToggle : Node2D
{

	[ExportGroup("Ui-Screens")]
	[Export] private CanvasLayer _uiMenu;
	[Export] private CanvasLayer _uiInGame;
	[Export] private CanvasLayer _uiGameOver;
	[Export] private CanvasLayer _uiGameWin;

	private Global _global;
	
	public override void _Ready()
	{
		_global = GetNode<Global>("/root/Global");

		_global.GameWinChanged += OnGameWinChanged;
		_global.GameOverChanged += OnGameOverChanged;
	}
	
	public override void _ExitTree()
	{
		_global.GameWinChanged -= OnGameWinChanged;
		_global.GameOverChanged -= OnGameOverChanged;
	}
	
	private void OnGameWinChanged(bool gameWin)
	{
		if (gameWin)
		{
			ShowOnly(_uiGameWin, _uiMenu, _uiInGame, _uiGameOver);
		}

	}
	private void OnGameOverChanged(bool gameOver)
	{
		if (gameOver)
		{
			ShowOnly(_uiGameOver, _uiMenu, _uiInGame, _uiGameWin);
		}
	}

	
	private void OnLvlButtonPressed()
	{
		ShowOnly(_uiInGame, _uiMenu, _uiGameWin, _uiGameOver );
	}

	private void OnMenuButtonPressed()
	{
		_global.ResetValues();
		GetTree().ReloadCurrentScene();
	}
	
	
	private static void ShowOnly(CanvasLayer visibleItem, params CanvasLayer[] hiddenItems)
	{
		visibleItem.Visible = true;

		foreach (CanvasLayer layer in hiddenItems)
		{
			layer.Visible = false;
		}
	}
}
