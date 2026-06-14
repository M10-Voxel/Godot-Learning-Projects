using Godot;

namespace SpaceShooter.scripts;

public partial class Ui : Node2D
{
    private Global _global;

    [ExportGroup( "Screens")]
    [Export] private Node2D _startScreen;
    [Export] private Node2D _chooseScreen;
    [Export] private Node2D _inGameScreen;
    [Export] private Node2D _gameOverScreen;

    [ExportGroup( "Ship-Sprites")]
    [Export] private Sprite2D _ship1;
    [Export] private Sprite2D _ship2;
    [Export] private Sprite2D _ship3;

    [ExportGroup("")]
    [Export] private Sprite2D _soundOn;
    [Export] private Sprite2D _soundOff;

    [Export] private Label _inGameScore;
    [Export] private Label _gameOverScore;

    
    public override void _Ready()
    {
        _global = GetNode<Global>("/root/Global");

        ShowOnly(_startScreen, _chooseScreen, _inGameScreen, _gameOverScreen);
        ShowOnly(_ship1, _ship2, _ship3);

        _global.GameOverChanged += OnGameOverChanged;
        OnGameOverChanged(_global.GameOver);
    }

    public override void _ExitTree()
    {
        _global.GameOverChanged -= OnGameOverChanged;
    }

    //________________________________________________________________________________________
    
    private void OnGameOverChanged(bool gameOver)
    {
        if (gameOver)
        {
            ShowOnly(_gameOverScreen, _startScreen, _chooseScreen, _inGameScreen);
            _inGameScore.Text = Global.Score.ToString();
            _gameOverScore.Text = "Score:\n" + Global.Score;
        }
    }

    //________________________________________________________________________________________

    private void OnButtonPlayPressed()
    {
        ShowOnly(_chooseScreen, _startScreen, _inGameScreen, _gameOverScreen);
    }

    //________________________________________________________________________________________

    private void OnButtonShip1Pressed()
    {
        Global.ChosenShip = 1;
        ShowOnly(_ship1, _ship2, _ship3);
    }

    private void OnButtonShip2Pressed()
    {
        Global.ChosenShip = 2;
        ShowOnly(_ship2, _ship1, _ship3);
    }

    private void OnButtonShip3Pressed()
    {
        Global.ChosenShip = 3;
        ShowOnly(_ship3, _ship1, _ship2);
    }

    private void OnButtonSelectPressed()
    {
        _global.SetGameOn(true);
        ShowOnly(_inGameScreen, _chooseScreen, _startScreen, _gameOverScreen);
    }

    //________________________________________________________________________________________

    private void OnButtonMutePressed()
    {
        if (Global.Mute)
        {
            Global.Mute = false;
            _soundOff.Visible = false;
            _soundOn.Visible = true;
        }
        else
        {
            Global.Mute = true;
            _soundOff.Visible = true;
            _soundOn.Visible = false;
        }
    }

    //________________________________________________________________________________________

    private void OnButtonGameOverPressed()
    {
        _global.ResetValues();
        GetTree().ReloadCurrentScene();
    }



    //________________________________________________________________________________________


    private static void ShowOnly(CanvasItem visibleItem, params CanvasItem[] hiddenItems)
    {
        visibleItem.Visible = true;

        foreach (CanvasItem item in hiddenItems)
        {
            item.Visible = false;
        }
    }
}
