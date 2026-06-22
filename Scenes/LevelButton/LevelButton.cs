using Godot;

public partial class LevelButton : TextureButton
{
    [Export] private LevelSetting _levelSetting;
    [Export] private Label _levelSize;


    public override void _Ready()
    {
        if (_levelSetting == null)
        {
            GD.PrintErr("LevelSetting is null");
            QueueFree();
            return;
        }
        
        _levelSize.Text = _levelSetting.ToString();
    }
    
    private void OnButtonPressed()
    {
        SignalHub.EmitOnLevelSelected(_levelSetting);
    }
}
