using System.Collections.Generic;
using Godot;

public partial class LevelScores : Resource
{
    private const int DefaultScore = 9999;

    [Export] private Godot.Collections.Dictionary<int, int> _levelScores = new();

    public int GetBestScore(int levelNumber) => _levelScores.GetValueOrDefault(levelNumber, DefaultScore);
    
    public void SetBestScore(int levelNumber, int score)
    {
        if (GetBestScore(levelNumber) > score)
        {
            _levelScores[levelNumber] = score;
        }
    }

}
