using Godot;

namespace SpaceShooter.scripts;

public partial class Global : Node
{

    [Signal] public delegate void GameOverChangedEventHandler(bool gameOver);
    public bool GameOver { get; private set; } = false;

    public static bool GameOn { get; set; } = false;
    public static int Score { get; set; } = 0;
    public static int ChosenShip { get; set; } = 1;
    public static bool Mute { get; set; } = false;

    public void SetGameOver(bool value)
    {
        if (GameOver == value) return;

        GameOver = value;
        EmitSignal(SignalName.GameOverChanged, GameOver);
    }

    public void ResetValues()
    {
        SetGameOver(false);
        GameOn = false;
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
