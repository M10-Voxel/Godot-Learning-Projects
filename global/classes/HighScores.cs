using System.Linq;
using Godot;

public partial class HighScores : Resource
{
    [Export] public Godot.Collections.Array<HighScore> Scores { get; set; } = new();

    private const int MaxScores = 10;




    public void AddNewScore(int score)
    {
        HighScore highScore = new HighScore {Score = score};
        Scores.Add(highScore);
        
        SortScores();
        
        if(Scores.Count > MaxScores) Scores.Resize(MaxScores);
    }

    private void SortScores()
    {
        var list = Scores.ToList();
        list.Sort((a, b) => b.Score.CompareTo(a.Score));
        Scores = new(list);
    }
}
