using Godot;

namespace SpaceShooter.scripts;

public partial class Global : Node
{

    [Signal] public delegate void GameOverChangedEventHandler(bool gameOver);
    public bool GameOver { get; private set; } = false;

    [Signal] public delegate void GameOnChangedEventHandler(bool gameOn);
    public bool GameOn { get; private set; } = false;

    [Signal] public delegate void ScoreChangedEventHandler(int score);
    public static int Score { get; private set; } = 0;
    public static int ChosenShip { get; set; } = 1;
    public static bool Mute { get; set; } = false;

    public void SetGameOver(bool value)
    {
        if (GameOver == value) return;

        GameOver = value;
        EmitSignal(SignalName.GameOverChanged, GameOver);
    }

    public void SetGameOn(bool value)
    {
        if (GameOn == value) return;

        GameOn = value;
        EmitSignal(SignalName.GameOnChanged, GameOn);
        GD.Print("Emitted GameOnChanged signal: " + GameOn);
    }

    public void AddGameScore(int score)
    {
        Score += score;
        EmitSignal(SignalName.ScoreChanged, Score);
    }

    public void ResetValues()
    {
        SetGameOver(false);
        SetGameOn(false);
        Score = 0;
        ChosenShip = 1;
        Mute = false;
    }

    public override void _Process(double delta)
    {
        if (Mute)
        {
            AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Master"), -80);
        }
        else
        {
            AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Master"), 0);
        }
    }
}
