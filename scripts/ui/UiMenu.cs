using Godot;

namespace DPlattformer.scripts.ui;

public partial class UiMenu : CanvasLayer
{
	[Export] private AnimationPlayer _muteAnimation;
	
	[ExportGroup("lvl-Scenes")]
	[Export] private PackedScene _lvl1Scene;
	[Export] private PackedScene _lvl2Scene;
	[Export] private PackedScene _lvl3Scene;



	public override void _Ready()
	{
		_muteAnimation.Play("idle");
	}
	
	private void OnMuteButtonToggled(bool isOn)
	{
		if (isOn)
		{
			_muteAnimation.PlayBackwards("switch");
			GD.Print("Sound On");
		}
		else
		{
			_muteAnimation.Play("switch");
			GD.Print("Sound Off");
		}
	}
	
	
}
