using Godot;
using System;

public class Camera : Camera2D
{
    [Export]
    public bool UseKeys = true;

    [Export]
    public bool UseDrag = true;

    [Export]
    public bool UseEdge = true;

    [Export]
    public bool UseWheel = true;

    [Export]
    public int CameraSpeed = 400;

    [Export]
    public int ZoomOutLimit = 2;

    [Export]
    public int CameraMargin = 50;

    [Export]
    public Vector2 ZoomSpeed = new Vector2(0.5f, 0.5f);

    private Vector2 _zoom;
    private Vector2 _previousMousePosition;
    private bool _dragMouseOn = false;
    private bool[] moveKeysOn = {false, false, false, false};
    private readonly string[] moveKeysActionNames = {"move_left", "move_up", "move_right", "move_down"};

    private readonly Vector2[] MOVE_DIRECTIONS = {Vector2.Left, Vector2.Up, Vector2.Right, Vector2.Down};

    private int _limitLeft;
    private int _limitRight;
    private int _limitTop;
    private int _limitBottom;

    public Camera() {
        _zoom = Zoom;

        SetHDragEnabled(false);
        SetVDragEnabled(false);
        SetEnableFollowSmoothing(true);
        SetFollowSmoothing(4);
    }

    public override void _Ready() {
    }

    private int CameraLimitLeft {
        get {
            return (int)(Zoom.x * GetViewportRect().Size.x / 2);
        }
    }

    private int CameraLimitRight {
        get {
            return (int)(GetParent<TextureRect>().RectSize.x - Zoom.x * GetViewportRect().Size.x / 2);
        }
    }

    private int CameraLimitTop {
        get {
            return (int)(Zoom.y * GetViewportRect().Size.y / 2);
        }
    }

    private int CameraLimitBottom {
        get {
            return (int)(GetParent<TextureRect>().RectSize.y - Zoom.y * GetViewportRect().Size.y / 2);
        }
    }

    private void AddToPosition(Vector2 change) {
        var newPosition = Position + change;
        newPosition.x = Mathf.Clamp(newPosition.x, CameraLimitLeft, CameraLimitRight);
        newPosition.y = Mathf.Clamp(newPosition.y, CameraLimitTop, CameraLimitBottom);
        Position = newPosition;
        // GD.Print(Position);
    }

    public override void _PhysicsProcess(float delta) {
        Vector2 cameraMovement = Vector2.Zero;
        bool skipMove = false;

        // SetZoom(Utils.LerpVec2(GetZoom(), _zoom, delta));
        Zoom = _zoom;

        if(UseKeys) {
            for(int i = 0; i < moveKeysOn.Length; i++) {
                if(!moveKeysOn[i])
                    continue;
                cameraMovement += MOVE_DIRECTIONS[i] * CameraSpeed * delta;
            }
        }

        if(UseEdge) {
            Rect2 rect = GetViewport().GetVisibleRect();
            Vector2 pos = GetLocalMousePosition() + rect.Size / 2;
            foreach(Vector2 direction in MOVE_DIRECTIONS) {
                Vector2 bounds = Utils.ClampVec2(direction, 0, 1) * rect.Size;
                if((pos - bounds).Abs().Min() < CameraMargin) {
                    // GD.Print(pos, bounds, (pos - bounds).Abs().Min(), direction * CameraSpeed * delta);
                    cameraMovement += direction * CameraSpeed * delta;
                }
            }
        }

        if(UseDrag && _dragMouseOn) {
            // this somehow fixes funky camera movement
            AddToPosition((_previousMousePosition - GetLocalMousePosition()) * GetZoom());
            Align();
            skipMove = true;
        }
        _previousMousePosition = GetLocalMousePosition();

        if(skipMove)
            return;
        AddToPosition(cameraMovement * Zoom);
    }

    public override void _UnhandledInput(InputEvent @event) {
        if(@event is InputEventMouseButton mouseButtonEvent) {
            // if(UseDrag && mouseButtonEvent.ButtonIndex == (int)ButtonList.Middle) {
            if(UseDrag && mouseButtonEvent.ButtonIndex == (int)ButtonList.Right) {
                _dragMouseOn = mouseButtonEvent.Pressed;
            }
            if(UseWheel && mouseButtonEvent.ButtonIndex == (int)ButtonList.WheelUp) {
                _zoom = Utils.ClampVec2(Zoom - ZoomSpeed, 1, ZoomOutLimit);
            }
            if(UseWheel && mouseButtonEvent.ButtonIndex == (int)ButtonList.WheelDown) {
                _zoom = Utils.ClampVec2(Zoom + ZoomSpeed, 1, ZoomOutLimit);
            }
        }
        for (int i = 0; i < moveKeysActionNames.Length; i++) {
            if(Input.IsActionJustPressed(moveKeysActionNames[i]))
                moveKeysOn[i] = true;
            if (Input.IsActionJustReleased(moveKeysActionNames[i]))
                moveKeysOn[i] = false;
        }
    }
}
