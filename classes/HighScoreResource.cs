using Godot;

namespace TappyPlane.classes;

public partial class HighScoreResource : Resource
{
    [Export] private int _highScore = 0;

    public int HighScore
    {
        get => _highScore;

        set => _highScore = Mathf.Max(0, value);
    }
}
