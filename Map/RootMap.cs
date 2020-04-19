using Godot;
using System.Collections.Generic;

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

    private AudioStreamPlayer AudioPlayer {
        get {
            return GetNode<AudioStreamPlayer>("EffectPlayer");
        }
    }

    private Control Animations {
        get {
            return GetNode<Control>("ActualGrid/Animations");
        }
    }

    private DGrid StartingGrid;

    public override void _Ready() {
        Regenerate();
    }

    public void Regenerate() {
        GameState.Instance.Initialize(InitialGrid, AudioPlayer);

        if(GameState.Instance.GameOver) {
            GetParent<Map>().ShowGameOverScreen();
            return;
        }

        ActualGrid.Clear();
        foreach(var pair in GameState.Instance.ActualGrid.GetNonEmptyCells())
            ActualGrid.SetCell(pair.Key.X, pair.Key.Y, pair.Value);

        InitialGrid.Visible = false;
        ActualGrid.Visible = true;

        if(GameState.Instance.ShowIncome) {
            GD.Print($"Here! {Animations.GetChildCount()}");

            List<EnergyNumber> allAnimations = new List<EnergyNumber>();
            foreach(var pos in GameState.Instance.FoodSpenders)
                allAnimations.Add(CreateIncomeAnimation(pos, -GameConstants.FilledCellBaseSpend));
            foreach(var pos in GameState.Instance.FoodSources)
                allAnimations.Add(CreateIncomeAnimation(pos, GameConstants.WaterNodeIncome));
            while(GameState.Instance.OneTimeSources.Count > 0)
                allAnimations.Add(CreateIncomeAnimation(
                    GameState.Instance.OneTimeSources.Dequeue(), 
                    GameConstants.FoodNodeIncome, 
                    30));
            
            foreach(var player in allAnimations)
                Animations.AddChild(player);

            foreach(var player in allAnimations)
                player.Play();
        }
    }

    public EnergyNumber CreateIncomeAnimation(DVector position, int energy, int shift=0) {
        var node = ResourceLoader.Load<PackedScene>("res://Map/EnergyNumber.tscn").Instance() as EnergyNumber;
        node.Energy = energy;
        var newPosition = ActualGrid.MapToWorld(new Vector2(position.X, position.Y));
        newPosition.x += shift;
        node.RectPosition = newPosition;
        return node;
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
