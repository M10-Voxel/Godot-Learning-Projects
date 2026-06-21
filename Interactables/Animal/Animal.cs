using Godot;

public partial class Animal : RigidBody2D
{
	// EXPORTS:
	[Export] private Label _debugLabel;

	[ExportGroup("Audio")]
	[Export] private AudioStreamPlayer2D _stretchSound;
	[Export] private AudioStreamPlayer2D _launchSound;
	[Export] private AudioStreamPlayer2D _kickSound;

	// LOCAL VARIABLES:
	private bool _isDragging = false;
	private bool _isDead = false;
	
	private Vector2 _dragStartPos = Vector2.Zero;
	private Vector2 _draggedVector = Vector2.Zero;
	private Vector2 _startPos;
	
	// CONSTANTS/READONLY:
	private readonly Vector2 _dragLimitMin = new(-60, 0);
	private readonly Vector2 _dragLimitMax = new(0, 60);
	private readonly float _impulseMult = 20.0f;
	
	
	//* ________________________________________________________________________________________________
	//* STANDARD GODOT METHODS:
	
	public override void _Ready()
	{
		InputEvent += OnInputEvent;
		_startPos = Position;
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
		_draggedVector = GetGlobalMousePosition() - _dragStartPos;
		_draggedVector = _draggedVector.Clamp(_dragLimitMin, _dragLimitMax);
		Position = _startPos + _draggedVector;
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
		ApplyCentralImpulse(CalculateImpulse());
		SignalHub.EmitOnAttemptMade();
	}

	private Vector2 CalculateImpulse()
	{
		return _draggedVector * -_impulseMult;
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
