using System.Linq;
using Godot;
using Godot.Collections;

/// <summary>
/// Creates a list of HighScores limiting it to a MaxScore and sorting it in descending order
/// </summary>
public partial class HighScores : Resource
{
    // EXPORTS:
    [Export] public Godot.Collections.Array<HighScore> Scores { get; set; } = [];

    // PRIVATE VARIABLES:
    private const int MaxScores = 10;
    
    
    //* ________________________________________________________________________________________________
    //* OWN METHODS:

    /// <summary>
    /// Adds a new HighScore to list, sorting and limiting it to MaxScores
    /// </summary>
    /// <param name="score">The integer value of the score to add</param>
    public void AddNewScore(int score)
    {
        HighScore highScore = HighScore.Create(score);
        Scores.Add(highScore);
        
        SortScores();
        
        // Cuts all scores off, when over limit (deletes lowest scores)
        if(Scores.Count > MaxScores) Scores.Resize(MaxScores);
    }

    /// <summary>
    /// Sorts the scores in descending order using Linq
    /// </summary>
    private void SortScores()
    {
        var list = Scores.ToList();
        list.Sort((a, b) => b.Score.CompareTo(a.Score));
        Scores = new Array<HighScore>(list);
    }
}
