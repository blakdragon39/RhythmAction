using Godot;
using RhythmAction.scripts;
using System.Collections.Generic;
using System.Numerics;
using Vector3 = Godot.Vector3;

public partial class Character : StaticBody3D {

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
    
    private List<Tile> currentPath = new ();
    private float tickProgress;
    
    public override void _Ready() {
        Position = new Vector3(startTile.Position.X, Position.Y, startTile.Position.Z); // todo y position will eventually come from Tile too?

        body = GetNode<Node3D>("Body");
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
        if (currentPath.Count == 0) return;
        
        var newCharPosition = new Vector3(TileLocation.Position.X, Position.Y, TileLocation.Position.Z);
        Position = Position.MoveToward(newCharPosition, (float) delta * Constants.Velocity);
        
        if (Mathf.IsEqualApprox(Position.X, TileLocation.Position.X) && 
            Mathf.IsEqualApprox(Position.Z, TileLocation.Position.Z)) {
            startTile = currentPath[0];
            currentPath.RemoveAt(0);
        }
    }

    private void RotateBody() {
        var oldX = startTile.Position.X;
        var newX = TileLocation.Position.X;
        var oldZ = startTile.Position.Z;
        var newZ = TileLocation.Position.Z;
        var newRotation = 0f;

        if (oldX < newX) {
            if (oldZ < newZ) newRotation = 45f;
            else if (oldZ > newZ) newRotation = 135;
            else newRotation = 90;
        } else if (oldX > newX) {
            if (oldZ < newZ) newRotation = 315;
            else if (oldZ > newZ) newRotation = 225;
            else newRotation = 270;
        } else {
            if (oldZ > newZ) newRotation = 180;
        }
        
        TileLocation.Position.SignedAngleTo(startTile.Position, Vector3.)
        
        if (currentPath.Count > 0) {
            GD.Print($"{newRotation}");
            body.RotationDegrees = body.RotationDegrees.Lerp(new Vector3(body.RotationDegrees.X, newRotation, body.RotationDegrees.Z), 0.3f);
            // body.RotationDegrees = ;
        }
    }
}
