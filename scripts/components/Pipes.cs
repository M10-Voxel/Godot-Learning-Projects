using Godot;
using TappyPlane.globals;

namespace TappyPlane.scripts.components;

public partial class Pipes : Node2D
{

    [Export] private float _pipeSpeed = 120.0f;
    [Export] private AudioStreamPlayer _scoreSound;
    
    private bool _isLaserActive = true;

    public override void _PhysicsProcess(double delta)
    {
        Position -= new Vector2(_pipeSpeed * (float)delta, 0);
    }

    private void OnVisibleNotifierScreenExited()
    {
        QueueFree();
    }

    private void OnPipeBodyEntered(Node2D body)
    {
        if (body is Tappy tappy) tappy.Die();
    }

    private void OnLaserBodyExited(Node2D body)
    {
        if (!_isLaserActive || body is not Tappy)
            return;

        _isLaserActive = false;

        if (_scoreSound.IsInsideTree())
            _scoreSound.Play();

        SignalHub.EmitOnPointScored();
    }

}
