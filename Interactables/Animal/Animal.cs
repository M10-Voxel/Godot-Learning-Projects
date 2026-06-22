using Godot;

public partial class Animal : RigidBody2D
{
	// EXPORTS:
	[Export] private Label _debugLabel;
	[Export] private Sprite2D _directionIndicator;

	[ExportGroup("Audio")]
	[Export] private AudioStreamPlayer2D _stretchSound;
	[Export] private AudioStreamPlayer2D _launchSound;
	[Export] private AudioStreamPlayer2D _kickSound;

	// LOCAL VARIABLES:
	private bool _isDragging = false;
	private bool _isDead = false;

	private float _arrowScaleX = 0.0f;
	
	private Vector2 _dragStartPos = Vector2.Zero;
	private Vector2 _draggedVector = Vector2.Zero;
	private Vector2 _startPos;
	
	// CONSTANTS/READONLY:
	private readonly Vector2 _dragLimitMin = new(-60, 0);
	private readonly Vector2 _dragLimitMax = new(0, 60);
	private const float ImpulseMult = 20.0f;
	private const float ImpulseMax = 2000.0f;
	
	
	//* ________________________________________________________________________________________________
	//* STANDARD GODOT METHODS:
	
	public override void _Ready()
	{
		InputEvent += OnInputEvent;
		_startPos = Position;
		_arrowScaleX = _directionIndicator.Scale.X;
		_directionIndicator.Hide();
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_isDragging) HandleDragging();
		UpdateDebugLabel();
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionReleased("drag") && _isDragging)
		{
			CallDeferred(nameof(HandleRelease));
		}
	}

	//* ________________________________________________________________________________________________
	//* HELPER METHODS:
	
	// FOR PhysicsProcess:
	private void HandleDragging()
	{
		Vector2 newDraggedVector = GetGlobalMousePosition() - _dragStartPos;
		newDraggedVector = newDraggedVector.Clamp(_dragLimitMin, _dragLimitMax);
		Position = _startPos + _draggedVector;
		
		if ((_draggedVector - newDraggedVector).Length() > 0 && !_stretchSound.Playing) _stretchSound.Play();
		
		_draggedVector = newDraggedVector;
		
		ScaleArrow();
	}
	private void ScaleArrow()
	{
		float fraction = CalculateImpulse().Length() / ImpulseMax;
		fraction = Mathf.Clamp(fraction, 0.0f, 1.0f);
		_directionIndicator.Scale = new Vector2(
			Mathf.Lerp(_arrowScaleX, _arrowScaleX * 3, fraction),
			_directionIndicator.Scale.Y
		);
		
		_directionIndicator.Rotation = (_startPos - Position).Angle();
	}

	
	private void UpdateDebugLabel()
	{
		string ds = $"SL: {Sleeping}| FR: {Freeze}";
		ds += $"\nDrag: {_isDragging} | DragStartPos: {_dragStartPos} | DraggedVector: {_draggedVector}";
		ds += $"\n StartPos: {_startPos}";
		_debugLabel.Text = ds;
	}

	// FOR UnhandledInput:
	private void HandleRelease()
	{
		_isDragging = false;
		_launchSound.Play();
		Freeze = false;
		_directionIndicator.Hide();
		ApplyCentralImpulse(CalculateImpulse());
	}
	private Vector2 CalculateImpulse()
	{
		return _draggedVector * -ImpulseMult;
	}
	
	//* ________________________________________________________________________________________________
	//* OWN METHODS:

	public void Die()
	{
		_isDead = true;
		SignalHub.EmitOnAnimalDied();
		QueueFree();
	}
	
	private void StartDragging()
	{
		_isDragging = true;
		_dragStartPos = GetGlobalMousePosition();
		
		_directionIndicator.Show();
	}

	
	//* ________________________________________________________________________________________________
	//* SIGNAL METHODS:
	
	private void OnInputEvent(Node viewport, InputEvent @event, long shapeIdx)
	{
		if (@event.IsActionPressed("drag"))
		{
			InputEvent -= OnInputEvent;
			StartDragging();
		}
	}

	private void OnBodyEntered(Node body)
	{
		if (body is Cup && !_kickSound.Playing) _kickSound.Play();
	}

	private void OnSleepStateChanged()
	{
		if (!Sleeping) return;

		foreach (var body in GetCollidingBodies())
		{
			if (body is Cup cup) cup.Die();
		}

		Die();
	}
}
