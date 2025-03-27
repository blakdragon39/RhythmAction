using Godot;

public partial class CameraController : Camera3D {

    [Export] private Node3D target;
    [Export] private Node3D pivot;
    
    private const float ZoomSpeed = 3.0f;
    private const float PanSpeed = 0.3f;
    private const float MinFov = 24.0f;
    private const float MaxFov = 100.0f;
    private const float MinAngle = -35.0f;
    private const float MaxAngle = 5.0f;

    private float distanceToTarget;
    private bool isPanning;

    public override void _Ready() {
        distanceToTarget = Position.DistanceTo(target.Position);
        LookAt(target.Position);
    }

    public override void _Process(double delta) { }

    public override void _Input(InputEvent inputEvent) {
        if (inputEvent is InputEventMouseButton) {
            var mouseEvent = (InputEventMouseButton)inputEvent;
            HandleZoom(mouseEvent);
            HandlePanEvent(mouseEvent);
        }

        if (isPanning && inputEvent is InputEventMouseMotion) {
            var mouseEvent = (InputEventMouseMotion)inputEvent;
            HandlePanMovement(mouseEvent);
        }
    }

    private void HandleZoom(InputEventMouseButton mouseEvent) {
        if (!mouseEvent.IsPressed()) return;
        
        var fovChange = mouseEvent.Factor * ZoomSpeed;
        if (mouseEvent.ButtonIndex == MouseButton.WheelUp) {
            Fov = Mathf.Clamp(Fov - fovChange, MinFov, MaxFov);
        } else if (mouseEvent.ButtonIndex == MouseButton.WheelDown) {
            Fov = Mathf.Clamp(Fov + fovChange, MinFov, MaxFov);
        }
        
        LookAt(target.Position);
    }

    private void HandlePanEvent(InputEventMouseButton mouseEvent) {
        if (mouseEvent.ButtonIndex == MouseButton.Middle) {
            isPanning = mouseEvent.IsPressed();
        }
    }

    private void HandlePanMovement(InputEventMouseMotion mouseEvent) {
        var verticalMovement = mouseEvent.Relative.Y;
        
        if (pivot.RotationDegrees.X > MinAngle && verticalMovement > 0 || 
            pivot.RotationDegrees.X < MaxAngle && verticalMovement < 0) {
            pivot.RotateObjectLocal(Vector3.Left, Mathf.DegToRad(mouseEvent.Relative.Y) * PanSpeed);    
        }
        
        pivot.RotateObjectLocal(Vector3.Down, Mathf.DegToRad(mouseEvent.Relative.X) * PanSpeed);
        
        LookAt(target.Position);
    }
}