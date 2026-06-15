using Godot;

namespace SpaceShooter.scripts;

public partial class ShipUi : Sprite2D
{
	[Export] private PackedScene _laser;
	[Export] private Node2D _anchor;

	private Global _global;

	public override void _Ready()
	{
		_global = GetNode<Global>("/root/Global");
	}

	private void OnTimerTimeout()
	{
		if (!Visible || _global.GameOn)
		{
			return;
		}

		Laser newLaser = _laser.Instantiate<Laser>();
		AddChild(newLaser);
		newLaser.GlobalPosition = _anchor.GlobalPosition;
	}
}
