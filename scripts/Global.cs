using Godot;

namespace DPlattformer.scripts;

public partial class Global : Node
{

    [Signal] public delegate void GameOverChangedEventHandler(bool gameOver);
    public bool GameOver { get; private set; } = false;

    [Signal] public delegate void GameWinChangedEventHandler(bool gameOn);
    public bool GameWin { get; private set; } = false;

    [Signal] public delegate void PowerUpStateChangedEventHandler(bool activePowerUp);
    public static bool ActivePowerUp { get; private set; } = false;
    
    [Signal] public delegate void HealthChangedEventHandler(int health);
    public static int Health { get; private set; } = 3;
    
    [Signal] public delegate void CoinsChangedEventHandler(int coins);
    public static int Coins { get; private set; } = 0;
    
    [Signal] public delegate void InBossBattleChangedEventHandler(bool inBossBattle);
    public static bool IsInBossBattle { get; private set; } = false;
    
    [Signal] public delegate void BossDefeatedChangedEventHandler(bool bossDefeated);
    public static bool IsBossDefeated { get; private set; } = false;

    public void SetGameOver(bool value)
    {
        if (GameOver == value) return;

        GameOver = value;
        EmitSignal(SignalName.GameOverChanged, GameOver);
    }
    public void SetGameWin(bool value)
    {
        if (GameWin == value) return;

        GameWin = value;
        EmitSignal(SignalName.GameWinChanged, GameWin);
    }
    public void SetActivePowerUp(bool value)
    {
        if (ActivePowerUp == value) return;
        
        ActivePowerUp = value;
        EmitSignal(SignalName.PowerUpStateChanged, ActivePowerUp);
    }
    public void SetHealth(int value)
    {
        if (Health == value) return;
        
        Health = value;
        EmitSignal(SignalName.HealthChanged, Health);
    }
    public void SetCoins(int value)
    {
        if (Coins == value) return;
        
        Coins = value;
        EmitSignal(SignalName.CoinsChanged, Coins);
    }
    public void AddCoins(int value)
    {
        Coins += value;
        EmitSignal(SignalName.CoinsChanged, Coins);
    }
    public void SetIsInBossBattle(bool value)
    {
        if (IsInBossBattle == value) return;
        
        IsInBossBattle = value;
        EmitSignal(SignalName.InBossBattleChanged, IsInBossBattle);
    }
    public void SetIsBossDefeated(bool value)
    {
        if (IsBossDefeated == value) return;
        
        IsBossDefeated = value;
        EmitSignal(SignalName.BossDefeatedChanged, IsBossDefeated);   
    }
    
    public void ResetValues()
    {
        AudioServer.SetBusMute(AudioServer.GetBusIndex("Master"), false);
        SetGameOver(false);
        SetGameWin(false);
        SetActivePowerUp(false);
        SetHealth(3);
        SetCoins(0);
        SetIsInBossBattle(false);
        SetIsBossDefeated(false);
    }
}