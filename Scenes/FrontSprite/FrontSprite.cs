using Godot;
using System;

public partial class FrontSprite : TextureRect
{
	
	//* ________________________________________________________________________________________________
	//* GODOT BASE METHODS:
	
	public override void _Ready()
	{
		SetRandomImage();
		RunMe();
	}

	    
	//* ________________________________________________________________________________________________
	//* OWN METHODS:

	private void RunMe()
	{
		Tween tween = CreateTween();

		tween.TweenProperty(
			this,
			Control.PropertyName.Scale.ToString(),
			new Vector2(0.1f, 0.1f),
			1.0f);

		tween.TweenCallback(Callable.From(SetRandomImage));
		
		tween.TweenProperty(
			this,
			Control.PropertyName.Scale.ToString(),
			new Vector2(1.0f, 1.0f),
			1.0f);
		
		tween.TweenProperty(
			this,
			Control.PropertyName.Rotation.ToString(),
			GetRandomRotation(),
			GetRandomSpinTime());

		tween.TweenInterval(0.1f);
		tween.TweenCallback(Callable.From(RunMe));
	}
	
	
	//* ________________________________________________________________________________________________
	//* HELPER METHODS:
	
	private void SetRandomImage()
	{
		Texture = ImageManager.GetRandomTileImage();
	}

	private float GetRandomRotation()
	{
		return (float)Mathf.DegToRad(GD.RandRange(-360.0f, 360.0f));
	}

	private float GetRandomSpinTime()
	{
		return (float)GD.RandRange(1.0f, 2.0f);
	}
}
