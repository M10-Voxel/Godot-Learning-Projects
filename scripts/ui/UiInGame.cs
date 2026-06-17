using Godot;

namespace DPlattformer.scripts.ui;

public partial class UiInGame : CanvasLayer
{

	[Export] private Label _coinAmount;
	[Export] private Node2D _powerHearts;
	
	[ExportGroup("Hearts")]
	[Export] private Sprite2D _heart1;
	[Export] private Sprite2D _heart2;
	[Export] private Sprite2D _heart3;
	
	private Global _global;
	
	public override void _Ready()
	{
		_global = GetNode<Global>("/root/Global");

		_global.CoinsChanged += OnCoinChanged;
		_global.HealthChanged += OnHealthChanged;
		_global.PowerUpStateChanged += OnPowerUpChanged;
	}
	
	public override void _ExitTree()
	{
		_global.CoinsChanged -= OnCoinChanged;
	}

	private void OnCoinChanged(int coins)
	{
		_coinAmount.Text = coins.ToString();
	}
	
	private void OnHealthChanged(int health)
	{
		switch (health)
		{
			case 0:
				ShowHearts([], [_heart1, _heart2, _heart3]);
				break;
			case 1:
				ShowHearts([_heart1], [_heart2, _heart3]);
				break;
			case 2:
				ShowHearts([_heart1, _heart2], [_heart3]);
				break;
			case 3:
				ShowHearts([_heart1, _heart2, _heart3], []);
				break;
		}
	}
	
	private void OnPowerUpChanged(bool hasPowerUp)
	{
		_powerHearts.Visible = hasPowerUp;
	}
	
	
	
	
	
	
	private static void ShowHearts(Sprite2D[] visibleHears, Sprite2D[] hiddenHears)
	{
		foreach (Sprite2D heart in visibleHears) heart.Visible = true;
		foreach (Sprite2D heart in hiddenHears) heart.Visible = false;
	}
}
