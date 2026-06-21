using Godot;
using TappyPlane.globals;

namespace TappyPlane.scripts.components;

public partial class SceneChange : CanvasLayer
{
    [Export] private AnimationPlayer _changeAnimation;

    public void PlayAnimation()
    {
        _changeAnimation.Play("flash");
    }

    public void SwitchScene()
    {
        GameManager.LoadNextScene();
    }

}
