using System.Linq;
using Godot;
using Godot.Collections;

public partial class HighScores : Resource
{
    // EXPORTS:
    [Export] public Godot.Collections.Array<HighScore> Scores { get; set; } = [];

    // PRIVATE VARIABLES:
    private const int MaxScores = 10;
    
    
    //* ________________________________________________________________________________________________
    //* OWN METHODS:

    public void AddNewScore(int score)
    {
        HighScore highScore = new HighScore {Score = score};
        Scores.Add(highScore);  // Date is added automatically
        
        SortScores();
        
        // Cuts all scores off, when over limit (deletes lowest scores)
        if(Scores.Count > MaxScores) Scores.Resize(MaxScores);
    }

    // Sorts the scores in descending order using Linq
    private void SortScores()
    {
        var list = Scores.ToList();
        list.Sort((a, b) => b.Score.CompareTo(a.Score));
        Scores = new Array<HighScore>(list);
    }
}
