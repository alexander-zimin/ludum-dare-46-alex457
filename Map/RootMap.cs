using Godot;
using System.Collections.Generic;

[Tool]
public class RootMap : Node2D
{
    private TileMap InitialGrid {
        get {
            return GetNode<TileMap>("InitialGrid");
        }
    }

    private TileMap ActualGrid {
        get {
            return GetNode<TileMap>("ActualGrid");
        }
    }

    private DGrid StartingGrid;

    public override void _Ready() {
        Regenerate();
    }

    [Export]
    public bool Redraw {
        set {
            if(!Engine.IsEditorHint())
                return;
            Regenerate();
        }
        get {
            return false;
        }
    }

    public void Regenerate() {
        GameState.Instance.Initialize(InitialGrid);

        if(GameState.Instance.GameOver) {
            GetParent<Map>().ShowGameOverScreen();
            return;
        }

        ActualGrid.Clear();
        foreach(var pair in GameState.Instance.ActualGrid.GetNonEmptyCells())
            ActualGrid.SetCell(pair.Key.X, pair.Key.Y, pair.Value);

        InitialGrid.Visible = false;
        ActualGrid.Visible = true;
    }

    public override void _Input(InputEvent @event) {
        if(GameState.Instance.BlockGridSelection)
            return;
        if(@event is InputEventMouseButton mouseButtonEvent) {
            if(mouseButtonEvent.ButtonIndex == (int)ButtonList.Left && mouseButtonEvent.Pressed) {
                var pos = ActualGrid.WorldToMap(ActualGrid.GetLocalMousePosition());

                if(GameLogic.ProcessCellClick(DVector.FromVector2(pos)))
                    Regenerate();
            }
        }
    }
}
