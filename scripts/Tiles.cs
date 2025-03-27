using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class Tiles : Node {
    
    [Signal] public delegate void MoveToDestinationEventHandler();

    [Export] private Character character;

    private List<Tile> tileChildren;
    private Tile[,] tileMap; 
    private AStar3D aStar;
    
    public override void _Ready() {
        tileChildren = GetChildren().OfType<Tile>().ToList();
        aStar = new AStar3D();
        
        var maxX = (int) tileChildren.Select(tile => tile.Position.X).Max() + 1;
        var maxZ = (int) tileChildren.Select(tile => tile.Position.Z).Max() + 1;
        tileMap = new Tile[maxX, maxZ];
        
        FillTileMap();
        BuildAStar(maxX, maxZ);
    }

    private void FillTileMap() {
        var id = 0;
        
        tileChildren.ForEach(tile => {
            tile.Id = id++;
            tile.SetDestination += GetPathTo;
            tileMap[(int)tile.Position.X, (int)tile.Position.Z] = tile;
            aStar.AddPoint(tile.Id, tile.Position);
        });
    }
    
    private void BuildAStar(int maxX, int maxZ) {
        // Connect each tile to the previous tile next to it, and the 3 above it
        for (var x = 0; x < maxX; x++) {
            for (var z = 0; z < maxZ; z++) {
                Tile tile = tileMap[x, z];
                if (tile == null) continue;
                
                TryConnect(tile, x - 1, z, maxX, maxZ);
                TryConnect(tile, x - 1, z - 1, maxX, maxZ);
                TryConnect(tile, x, z - 1, maxX, maxZ);
                TryConnect(tile, x + 1, z - 1, maxX, maxZ);
            }
        }
    }

    private void TryConnect(Tile tile, int toX, int toZ, int maxX, int maxZ) {
        if (toX >= 0 && toX < maxX && toZ >= 0 && toZ < maxZ) {
            Tile toConnect = tileMap[toX, toZ];
            if (toConnect != null) {
                aStar.ConnectPoints(tile.Id, toConnect.Id);
            }
        }
    }
    
    public override void _Process(double delta) { }

    private void GetPathTo(Tile destinationTile) {
        var path = aStar.GetIdPath(character.TileLocation.Id, destinationTile.Id).ToList();
        var tilePath = path.Select(id => tileChildren.Find(tile => tile.Id == id)).ToList();
         
        character.SetPath(tilePath);
    }
}