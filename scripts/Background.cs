using Godot;

namespace SpaceShooter.scripts;

public partial class Background : Node2D
{
    private AnimationPlayer _animationPlayer;
    private Global _global;
    
    public override void _Ready()
    {
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        _global = GetNode<Global>("/root/Global");

        _global.GameOverChanged += OnGameOverChanged;
        OnGameOverChanged(_global.GameOver);
    }
    
    public override void _ExitTree()
    {
        if (_global != null)
        {
            _global.GameOverChanged -= OnGameOverChanged;
        }
    }


    private void OnGameOverChanged(bool gameOver)
    {
        if (gameOver) _animationPlayer.Pause();
        else          _animationPlayer.Play("loop");
    }
}
