using Godot;

public partial class Tile : StaticBody3D {

    [Signal] public delegate void SetDestinationEventHandler(Tile tile);

    public int Id { get; set; }
    
    private MeshInstance3D mesh;

    public override void _Ready() {
        mesh = GetNode<MeshInstance3D>("MeshInstance3D");
    }
    
    private void OnMouseEntered() {
        mesh.SetInstanceShaderParameter("isHighlighted", true);
    }

    private void OnMouseExited() {
        mesh.SetInstanceShaderParameter("isHighlighted", false);
    }
    
    private void OnTileSelected(Node camera, InputEvent input, Vector3 eventPosition, Vector3 normal, int shapedIdx) {
        var mouseClick = input as InputEventMouseButton;
        if (input is InputEventMouseButton && mouseClick.ButtonIndex == MouseButton.Left && input.IsPressed()) {
            EmitSignal(SignalName.SetDestination, this);
        }
    }
}