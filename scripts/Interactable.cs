using Godot;

namespace SpaceShooter.scripts;

public partial class Interactable : CharacterBody2D
{
    [Export] private int _health = 1;
    [Export] private float _speed = 100.0f;
    [Export] private Type _interactableType = Type.Ship;
    [Export] private Sprite2D _spaceshipSprite;
    [Export] private AudioStreamPlayer2D _explosionSound;

    [ExportGroup("Animations")]
    [Export] private AnimationPlayer _explosionAnimation;
    [Export] private AnimationPlayer _damageAnimation;
    
    private Global _global;
    
    private bool _destroyed = false;
    
    //________________________________________________________________________________________
    
    public override void _Ready()
    {
        _global = GetNode<Global>("/root/Global");
        
        _explosionAnimation.Play(Animations.Idle);
        _damageAnimation.Play(Animations.Idle);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!_global.GameOver && _global.GameOn)
        {
            Velocity = new Vector2(0, 1) * _speed;
            MoveAndSlide();
        }

        if (_health <= 0 && !_destroyed)
        {
            _global.AddGameScore(10);
            Destroy();
        }
    }
    
    //________________________________________________________________________________________

    private async void Destroy()
    {
        _destroyed = true;
        _speed = 0;
        Velocity = Vector2.Zero;
        
        if (_interactableType == Type.Ship)
        {
            _spaceshipSprite.Visible = false;
            _explosionAnimation.Play(Animations.Explosion);
            _explosionSound.Play();
            await ToSignal(_explosionAnimation, "animation_finished");
        }
        
        QueueFree();
    }

    
    //________________________________________________________________________________________
    // ON COLLISION:

    private void OnArea2DAreaEntered(Area2D area)
    {
        if (area.IsInGroup("player"))
        {
            _health = 0;    // kill Enemy
        }
        else if (area.IsInGroup("laser"))
        {
            if (_interactableType == Type.Powerup) return; // Powerups shouldn't be able to be shot
            
            _health -= 1;   // Damage Enemy
            if (_interactableType == Type.Ship && _health > 0)  // Only Play Animation if it is a ship
            {
                _damageAnimation.Play(Animations.Damage);
            }
            
            if (area.GetParent() is Laser laser && laser.LasterType != 3)
            {
                laser.QueueFree();
            }
            
            
        }
    }

    //________________________________________________________________________________________
    // LINKED TIMER METHODS:

    private void OnLifeTimeTimerTimeout()
    {
        if (_global.GameOver) return;
        QueueFree();
    }
    
    //________________________________________________________________________________________
    // HELPER CLASS & ENUM:
    
    private enum Type
    {
        Ship,
        Meteor,
        Powerup
    }
    private static class Animations
    {
        public static readonly StringName Idle = "idle";
        public static readonly StringName Explosion = "explode";
        public static readonly StringName Damage = "damage";
    }
}
