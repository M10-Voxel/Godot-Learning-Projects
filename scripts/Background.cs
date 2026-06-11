using Godot;

namespace SpaceShooter.scripts;

public partial class Background : Node2D
{
    private AnimationPlayer _animationPlayer;

    public override void _Ready()
    {
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        Global global = GetNode<Global>("/root/Global");

        global.GameOverChanged += OnGameOverChanged;
        OnGameOverChanged(global.GameOver);
    }

    private void OnGameOverChanged(bool gameOver)
    {
        if (gameOver) _animationPlayer.Pause();
        else          _animationPlayer.Play("loop");
    }
}
