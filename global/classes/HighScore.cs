using Godot;

/// <summary>
/// Creates one high score consisting of a score and a date
/// </summary>
public partial class HighScore : Resource
{
    // EXPORTS:
    [Export] public int Score { get; set; } = 0;
    [Export] public string DateScored { get; set; } = "";

    
    //* ________________________________________________________________________________________________
    //* OWN METHODS:

    /// <summary>
    /// Creates a new HighScore with the current date.
    /// </summary>
    /// <param name="score">The integer value of the score</param>
    /// <returns>A HighScore containing the score and date it was created</returns>
    public static HighScore Create(int score)
    {
        return new HighScore
        {
            Score = score,
            DateScored = GetFormattedDate()
        };
    }

    /// <summary>
    /// Uses Godot Date Dictionary to get current date and formates it
    /// </summary>
    /// <returns>Formatted Date as a string with format "dd.mm.yyyy"</returns>
    private static string GetFormattedDate()
    {
        var date = Time.GetDateDictFromSystem();
        
        int day = (int)date["day"];
        int month = (int)date["month"];
        int year = (int)date["year"];
        return $"{day:D2}.{month:D2}.{year:D4}";
    }

}
