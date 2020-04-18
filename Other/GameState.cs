using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public class GameState
{
    private static GameState _instance = null;
    public static GameState Instance {
        get {
            if(_instance == null)
                _instance = new GameState();
            return _instance;
        }
    }

    public DGrid InitialGrid = new DGrid();
    public DGrid ActualGrid = new DGrid();
    public int Day {get; set;} = 1;
    public int Level {get; set;} = 1;
    public int Energy {get; private set;} = GameConstants.InitialEnergy;
    public int LastIncome {get; set;} = 0;
    public int LastSpend {get; set;} = 1;
    public bool GameOver {get; set;}
    public bool GameWon {get; set;}
    public bool CanLevelUp {
        get {
            return Energy >= GameLogic.GetNextLevelCost();
        }
    }
    public bool BlockGridSelection {get; set;} = false;
    public List<DVector> FoodSources = new List<DVector>();
    public bool CanReveal {get; set;} = false;
    public bool CanGrowRocks {get; set;} = false;
    public bool CanPruneEdges {get; set;} = false;
    public bool RevealInAction {get; set;}
    public bool PruneEdgesInAction {get; set;}
    public int CurrentStepVisionRadius {get; set;}
    public int CurrentRevealCost {get; set;}
    public int CurrentPruneCost {get; set;}

    private bool initialized = false;
    public void Initialize(TileMap map) {
        if(initialized)
            return;

        InitialGrid.InitFromTileMap(map);
        InitActualMap();
        CurrentStepVisionRadius = GameConstants.StepVisionRadius;
        CurrentPruneCost = GameConstants.PruneCost;
        CurrentRevealCost = GameConstants.RevealCost;

        initialized = true;
    }

    private void InitActualMap() {
        List<DVector> filledCells = InitialGrid.GetFilledCells();
        foreach(var pos in filledCells)
            ActualGrid.SetCell(pos, InitialGrid.GetCell(pos));
        foreach(var pos in filledCells) {
            GameLogic.RevealCellsAround(pos, CurrentStepVisionRadius);
            GameLogic.ExpandAreaAround(pos);
        }
    }

    public void AddToEnergy(int amount) {
        Energy = Math.Max(0, Energy + amount);
    }
}
