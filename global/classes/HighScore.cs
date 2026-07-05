using Godot;

public partial class HighScore : Resource
{

    [Export] public int Score { get; set; } = 0;
    [Export] public string DateScored { get; set; } = GetFormattedDate();


    static string GetFormattedDate()
    {
        var date = Time.GetDateDictFromSystem();
        
        int day = (int)date["day"];
        int month = (int)date["month"];
        int year = (int)date["year"];
        return $"{day:D2}.{month:D2}.{year:D2}";
    }

}
