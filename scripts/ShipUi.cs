using Godot;

namespace SpaceShooter.scripts;

public partial class ShipUi : Sprite2D
{
	[Export] private PackedScene _laser;
	[Export] private Node2D _anchor;

	private void OnTimerTimeout()
	{
		Laser newLaser = _laser.Instantiate<Laser>();
		AddChild(newLaser);
		newLaser.GlobalPosition = _anchor.GlobalPosition;
	}
}
