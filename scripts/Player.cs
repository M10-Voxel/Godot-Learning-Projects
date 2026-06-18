using Godot;

namespace DPlattformer.scripts;

public partial class Player : CharacterBody2D
{
    //* VARIABLES:
    
    [ExportGroup("Stats")]
    [Export] private float _walkSpeed = 300.0f;
    [Export] private float _addedSpeedWhileSprinting = 75.0f;
    [Export] private float _powerUpSpeed = 450.0f;
    [Export] private float _jumpVelocity = -800.0f;
    [Export] private float _gravity = 1400.0f;
    [Export] private float _maxFallSpeed = 1200.0f;

    [ExportGroup("Sprites")]
    [Export] private AnimatedSprite2D _playerSprite;
    [Export] private AnimatedSprite2D _poweredUpPlayerSprite;

    [ExportGroup("AnimationPlayers")]
    [Export] private AnimationPlayer _damageAnimation;
    [Export] private AnimationPlayer _gameOverAnimation;

    [ExportGroup("Collisions")]
    [Export] private CollisionShape2D _bodyCollision;
    [Export] private CollisionShape2D _hitboxCollision;

    [ExportGroup("Sounds")]
    [Export] private AudioStreamPlayer2D _coinSound;
    [Export] private AudioStreamPlayer2D _gemSound;
    [Export] private AudioStreamPlayer2D _jumpSound;
    [Export] private AudioStreamPlayer2D _damageSound;
    [Export] private AudioStreamPlayer2D _gameOverSound;

    [ExportGroup("Misc")]
    [Export] private Camera2D _camera;
    [Export] private Timer _invincibilityTimer;

    private Global _global;

    private AnimatedSprite2D _activePlayerSprite;
    private bool _isDamaged = false;
    private bool _inEnemy = false;
    private bool _hasGameEnded = false;


    //*________________________________________________________________________________________
    //* GODOT METHODS:

    public override void _Ready()
    {
        _global = GetNode<Global>("/root/Global");
        _global.GameOverChanged += OnGameOverChanged;
        _global.GameWinChanged += OnGameWinChanged;
        _global.HealthChanged += OnHealthChanged;

        _activePlayerSprite = _playerSprite;
        _playerSprite.Visible = true;
        _poweredUpPlayerSprite.Visible = false;

        _activePlayerSprite.Play(Animations.Idle);
    }

    public override void _ExitTree()
    {
        _global.GameOverChanged -= OnGameOverChanged;
        _global.GameWinChanged -= OnGameWinChanged;
        _global.HealthChanged -= OnHealthChanged;
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;
        float speed = GetCurrentSpeed();

        // When in air apply gravity over time while keeping under its maximum
        if (!IsOnFloor()) velocity.Y = Mathf.Min((velocity.Y + _gravity * (float)delta), _maxFallSpeed);

        // When space is pressed and player on ground, apply jump velocity
        if (Input.IsActionJustPressed("ui_accept") && IsOnFloor()) velocity.Y = PlayerJump();

        // When space is release before reaching apex, stop upward momentum directly and start fall
        if (Input.IsActionJustReleased("ui_accept") && velocity.Y < 0) velocity.Y = 0;

        float direction = Input.GetAxis("ui_left", "ui_right");

        // When direction given, move to its side with assigned speed, else slow down from current velocity to 0
        velocity.X = direction != 0 ?
                        direction * speed :
                        Mathf.MoveToward(Velocity.X, 0, speed);

        Velocity = velocity;    // apply velocity variable to player Velocity

        UpdateAnimations();

        MoveAndSlide();
    }


    //*________________________________________________________________________________________
    //* SIGNAL HANDLERS:

    private void OnGameOverChanged(bool gameOver)
    {
        if (!gameOver || _hasGameEnded) return;

        _hasGameEnded = true;

        SetPhysicsProcess(false);
        SetPowerUpStatus(false);

        _damageAnimation.Play(Animations.Idle);
        _activePlayerSprite.Play(Animations.GameOver);

        if (!_gameOverSound.Playing) _gameOverSound.Play();

        CallDeferred(nameof(DisableCollisions));    // Waits until after frame, where processes with collisions are finished
        _gameOverAnimation.Play(Animations.GameOver);
    }

    private void OnGameWinChanged(bool gameWin)
    {
        SetPhysicsProcess(!gameWin);

        if (gameWin)
        {
            _activePlayerSprite.Play(Animations.Idle);
        }
    }

    private void OnHealthChanged(int health)
    {
        if (health <= 0)
        {
            _global.SetGameOver(true);
        }
    }


    //*________________________________________________________________________________________
    //* HELPER METHODS

    private float GetCurrentSpeed()
    {
        float speed = Global.ActivePowerUp ? _powerUpSpeed : _walkSpeed;

        if (Input.IsActionPressed("ui_sprint"))
        {
            speed += _addedSpeedWhileSprinting;
        }

        return speed;
    }


    private float PlayerJump()
    {
        if (!_jumpSound.Playing)
        {
            _jumpSound.Play();
        }

        return _jumpVelocity;
    }

    private void UpdateAnimations()
    {
        if (IsOnFloor())
        {
            // When Velocity isn't 0, apply animation depending on sprint state
            // Else, play Idle animation
            _activePlayerSprite.Play(Velocity.X != 0
                ? (Input.IsActionPressed("ui_sprint")
                    ? PlayerAnimations.Sprinting
                    : PlayerAnimations.Walking)
                : PlayerAnimations.Idle);
        }
        else
        {
            // When moving up do InAirUp animation, else do InAirDown Animation
            _activePlayerSprite.Play(Velocity.Y < 0 ? PlayerAnimations.InAirUp : PlayerAnimations.InAirDown);
        }

        if (Velocity.X != 0)
        {
            _playerSprite.FlipH = Velocity.X < 0;
            _poweredUpPlayerSprite.FlipH = Velocity.X < 0;
        }


    }

    private void PlayerHit()
    {
        if (!_damageSound.Playing)
        {
            _damageSound.Play();
        }
        SetPowerUpStatus(false);
        _isDamaged = true;
        _damageAnimation.Play(Animations.Damaged);

        //_camera.Shake();
        _invincibilityTimer.Start();

        _global.ChangeHealth(-1);
    }

    private void SetPowerUpStatus(bool isPoweredUp)
    {
        _global.SetActivePowerUp(isPoweredUp);

        _activePlayerSprite = isPoweredUp ? _poweredUpPlayerSprite : _playerSprite;
        _playerSprite.Visible = !isPoweredUp;
        _poweredUpPlayerSprite.Visible = isPoweredUp;
    }


    private void DisableCollisions()
    {
        _bodyCollision.Disabled = true;
        _hitboxCollision.Disabled = true;
    }


    private void OnHitboxAreaEntered(Area2D area)
    {
        if (area.IsInGroup("win"))
        {
            GD.Print("Player Won!");
            _global.SetGameWin(true);
        }
        else if (area.IsInGroup("coin"))
        {
            _coinSound.Play();
            area.GetParent().QueueFree();
            _global.AddCoins(1);
        }
        else if (area.IsInGroup("heart"))
        {
            _gemSound.Play();
            area.GetParent().QueueFree();
            _global.ChangeHealth(+1);
        }
        else if (area.IsInGroup("spikes"))
        {
            _global.SetGameOver(true);
        }
        else if (area.IsInGroup("boss_fight"))
        {
            _global.SetIsInBossBattle(true);
            area.QueueFree();
        }
        else if (area.IsInGroup("boss_leave"))
        {
            area.QueueFree();
        }
    }


    private void OnHitboxBodyEntered(Node2D body)
    {
        if (body.IsInGroup("enemy"))
        {
            _inEnemy = true;
            TryPlayerHit();
        }
        else if (body.IsInGroup("power_up"))
        {
            _gemSound.Play();
            body.QueueFree();
            _global.ChangeHealth(+3);
            SetPowerUpStatus(true);
        }
    }

    private void OnHitboxBodyExited(Node2D body)
    {
        if (body.IsInGroup("enemy"))
        {
            _inEnemy = false;
        }
    }

    private void TryPlayerHit()
    {
        if (_isDamaged || _hasGameEnded) return;

        PlayerHit();
    }

    private void OnInvincibilityTimerTimeout()
    {
        _isDamaged = false;

        if (_inEnemy && !_hasGameEnded)
        {
            TryPlayerHit();
            return;
        }

        _damageAnimation.Play(Animations.Idle);
    }


    private static class Animations
    {
        public static readonly StringName Idle = "idle";
        public static readonly StringName Damaged = "damage";
        public static readonly StringName GameOver = "game_over";
    }

    private static class PlayerAnimations
    {
        public static readonly StringName GameOver = "game_over";
        public static readonly StringName Idle = "idle";
        public static readonly StringName InAirDown = "in_air_down";
        public static readonly StringName InAirUp = "in_air_up";
        public static readonly StringName Sprinting = "sprinting";
        public static readonly StringName Walking = "walking";
    }
}
