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

    private int level = 1;
    public int Level {
        get {
            return level;
        } 
        set {
            level = value;
            LevelChanged = true;
        }
    }
    private bool levelChanged;
    public bool LevelChanged {
        get {
            bool previousValue = levelChanged;
            levelChanged = false;
            return previousValue;
        }
        set {
            levelChanged = value;
        }
    }
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
    public List<DVector> FoodSpenders = new List<DVector>() {new DVector(0, 0)};
    public Queue<DVector> OneTimeSources = new Queue<DVector>();
    public bool CanReveal {get; set;} = false;
    public bool CanGrowRocks {get; set;} = false;
    public bool CanPruneEdges {get; set;} = false;
    public bool RevealInAction {get; set;}
    public bool PruneEdgesInAction {get; set;}
    public int CurrentStepVisionRadius {get; set;}
    public int CurrentRevealCost {get; set;}
    public int CurrentPruneCost {get; set;}
    public string CurrentMessage {get; set;}
    private string currentInstructions = "";
    public string CurentInstructions 
    {
        get {
            return currentInstructions;
        } 
        set {
            currentInstructions = value;
            ShowInstructions = true;
        }
    }
    private bool showInstructions = false;
    public bool ShowInstructions {
        get {
            bool previousValue = showInstructions;
            showInstructions = false;
            return previousValue;
        }
        set {
            showInstructions = value;
        }
    }
    private bool showIncome = false;
    public bool ShowIncome {
        get {
            bool previousValue = showIncome;
            showIncome = false;
            return previousValue;
        }
        set {
            showIncome = value;
        }
    }

    private bool initialized = false;
    public void Initialize(TileMap map, AudioStreamPlayer player) {
        if(initialized)
            return;

        CurrentStepVisionRadius = GameConstants.StepVisionRadius;
        CurrentPruneCost = GameConstants.PruneCost;
        CurrentRevealCost = GameConstants.RevealCost;
        CurrentMessage = GameConstants.IntroMessage;

        InitialGrid.InitFromTileMap(map);
        InitActualMap();

        Sounds.Instance.Initialize(player);

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

    public static void Restart() {
        _instance = new GameState();
    }
}
