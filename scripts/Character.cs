using Godot;
using RhythmAction.scripts;
using System.Collections.Generic;
using Vector3 = Godot.Vector3;

public partial class Character : StaticBody3D, IBeatable {

    [Export] private Tile startTile;

    public Tile TileLocation {
        get {
            if (currentPath.Count > 0) {
                return currentPath[0];
            }
            return startTile;
        }
    }

    private Node3D body;
    private AnimationPlayer animationPlayer;
    
    private List<Tile> currentPath = new ();
    private float tickProgress;
    
    public override void _Ready() {
        GetNode<BeatTimer>("/root/BeatTimer").Beatables.Add(this);
        Position = new Vector3(startTile.Position.X, Position.Y, startTile.Position.Z); // todo y position will eventually come from Tile too?

        body = GetNode<Node3D>("Body");
        animationPlayer = GetNode<AnimationPlayer>("Body/AnimationPlayer");
    }

    public override void _Process(double delta) {
        ProcessMovement(delta);
        RotateBody();
    }

    public void SetPath(List<Tile> path) {
        var newPath = new List<Tile>();
        
        if (currentPath.Count > 0) {
            newPath.Add(currentPath[0]);
        }
        newPath.AddRange(path);
        
        currentPath = newPath;
    }

    private void ProcessMovement(double delta) {
        if (currentPath.Count == 0) {
            animationPlayer.Play("Idle");
            return;
        }
        
        animationPlayer.Play("Walk");
        
        var newCharPosition = new Vector3(TileLocation.Position.X, Position.Y, TileLocation.Position.Z);
        Position = Position.MoveToward(newCharPosition, (float) delta * Constants.Velocity);
        
        if (Mathf.IsEqualApprox(Position.X, TileLocation.Position.X) && 
            Mathf.IsEqualApprox(Position.Z, TileLocation.Position.Z)) {
            startTile = currentPath[0];
            currentPath.RemoveAt(0);
        }
    }

    private void RotateBody() {
        if (currentPath.Count > 0) {
            var direction = (TileLocation.Position - startTile.Position).Normalized();
            
            var newRotation = Mathf.Atan2(direction.X, direction.Z);
            newRotation = Mathf.LerpAngle(body.Rotation.Y, newRotation, 0.3f);
            
            body.Rotation = body.Rotation.Slerp(new Vector3(body.RotationDegrees.X, newRotation, body.RotationDegrees.Z), 0.3f);
        }
    }

    public void OnBeat() {
        GD.Print("do actions!");
    }
}
