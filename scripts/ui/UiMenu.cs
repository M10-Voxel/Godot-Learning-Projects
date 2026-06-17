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
			AudioServer.SetBusMute(AudioServer.GetBusIndex("Master"), false);
			GD.Print("Sound On");
		}
		else
		{
			_muteAnimation.Play("switch");
			AudioServer.SetBusMute(AudioServer.GetBusIndex("Master"), true);
			GD.Print("Sound Off");
		}
	}


	private void OnLvl1ButtonPressed()
	{
		Node2D newLevel = _lvl1Scene.Instantiate<Node2D>();
		AddSibling(newLevel);
	}
	
	private void OnLvl2ButtonPressed()
	{
		Node2D newLevel = _lvl2Scene.Instantiate<Node2D>();
		AddSibling(newLevel);
	}

	private void OnLvl3ButtonPressed()
	{
		Node2D newLevel = _lvl3Scene.Instantiate<Node2D>();
		AddSibling(newLevel);
	}
	
	
}
