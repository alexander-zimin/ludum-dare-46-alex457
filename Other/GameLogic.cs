using Godot;
using System;
using System.Linq;

public static class GameLogic
{
   public static bool ProcessCellClick(DVector position) {
       if(GameState.Instance.RevealInAction) {
           if(GameState.Instance.Energy >= GameState.Instance.CurrentRevealCost) {
                RevealCellsAround(position, GameConstants.RevealRadius);
                GameState.Instance.AddToEnergy(-GameState.Instance.CurrentRevealCost);
                GameState.Instance.CurrentRevealCost = IncrementCost(GameState.Instance.CurrentRevealCost);
           }
           else {
               // play some sound or show message
           }
           GameState.Instance.RevealInAction = false;
           return true;
       }

       if(GameState.Instance.PruneEdgesInAction) {
            if(!TileTypesHelper.IsFilled((TileType)GameState.Instance.ActualGrid.GetCell(position)))
                return false;
            
           if(GameState.Instance.Energy >= GameState.Instance.CurrentPruneCost) {
                if(!position.Equals(new DVector(0, 0))) {
                    if(GameState.Instance.ActualGrid.IsConnectedWithout(position)) {
                        PruneCell(position);
                        GameState.Instance.AddToEnergy(-GameState.Instance.CurrentPruneCost);
                        GameState.Instance.CurrentPruneCost = IncrementCost(GameState.Instance.CurrentPruneCost);
                    }
                    else {
                        // play some sound or show message
                    }
                }
                else {
                    // play some sound or show message
                }
           }
           else {
               // play some sound or show message
           }
           GameState.Instance.PruneEdgesInAction = false;
           return true;
       }

        TileType posType = (TileType)GameState.Instance.ActualGrid.GetCell(position);
        if(!TileTypesHelper.IsActive(posType))
            return false;
        switch(posType) {
            case TileType.ActiveBaseNode:
                GameState.Instance.ActualGrid.SetCell(position, (int)TileType.FilledBaseNode);
                RevealCellsAround(position, GameState.Instance.CurrentStepVisionRadius);
                ExpandAreaAround(position);
                ProcessTurn();
                return true;

            case TileType.ActiveFoodNode:
                GameState.Instance.ActualGrid.SetCell(position, (int)TileType.FilledFoodNode);
                GameState.Instance.InitialGrid.SetCell(position, (int)TileType.ActiveBaseNode);
                GameState.Instance.AddToEnergy(GameConstants.FoodNodeIncome);
                RevealCellsAround(position, GameState.Instance.CurrentStepVisionRadius);
                ExpandAreaAround(position);
                ProcessTurn();
                return true;
            
            case TileType.ActiveWaterNode:
                GameState.Instance.ActualGrid.SetCell(position, (int)TileType.FilledWaterNode);
                GameState.Instance.FoodSources.Add(position);
                RevealCellsAround(position, GameState.Instance.CurrentStepVisionRadius);
                ExpandAreaAround(position);
                ProcessTurn();
                return true;
            
            case TileType.ActiveRockNode:
                if(!GameState.Instance.CanGrowRocks)
                    return false;
                GameState.Instance.ActualGrid.SetCell(position, (int)TileType.FilledRockNode);
                RevealCellsAround(position, GameState.Instance.CurrentStepVisionRadius);
                ExpandAreaAround(position);
                ProcessTurn();
                return true;
        }

        return false;
   }

   public static void ExpandAreaAround(DVector pos) {
        foreach(var neighbor in pos.GetNeighbours()) {
            if(GameState.Instance.InitialGrid.IsEmpty(neighbor)
                || TileTypesHelper.IsFilled((TileType)GameState.Instance.ActualGrid.GetCell(neighbor)))
                continue;
            GameState.Instance.ActualGrid.SetCell(
                neighbor,
                GameState.Instance.InitialGrid.GetCell(neighbor));
        }
    }

    public static int TotalIncome {
        get {
            return GameState.Instance.FoodSources.Count * GameConstants.WaterNodeIncome;
        }
    }

    public static int TotalSpend {
        get {
            int filledNonWaterCellsCount = 
            GameState.Instance.ActualGrid.GetFilledCells()
            .Where(p => GameState.Instance.ActualGrid.GetCell(p) != (int)TileType.FilledWaterNode)
            .Count();
            return filledNonWaterCellsCount * GameConstants.FilledCellBaseSpend;
        }
    }

    public static void ProcessTurn() {
        // Add all energy sources
        GameState.Instance.AddToEnergy(TotalIncome);

        // Spend energy
        GameState.Instance.AddToEnergy(-TotalSpend);

        // Check for the game over state
        GameState.Instance.GameOver = GameState.Instance.Energy == 0;

        // Increment day
        GameState.Instance.Day += 1;
    }

    public static void RevealCellsAround(DVector position, int radius) {
        foreach(var cell in position.GetSquare(radius)) {
            if(GameState.Instance.InitialGrid.IsEmpty(cell) 
                || GameState.Instance.ActualGrid.GetCell(cell) >= 0)
                continue;
            GameState.Instance.ActualGrid.SetCell(
                cell, 
                (int)TileTypesHelper.ToInactive((TileType)GameState.Instance.InitialGrid.GetCell(cell)));
        }
    }

    public static void PruneCell(DVector cell) {
        GameState.Instance.FoodSources.Remove(cell);

        GameState.Instance.ActualGrid.SetCell(
                cell, 
                GameState.Instance.InitialGrid.GetCell(cell));

        foreach(var neighbor in cell.GetNeighbours()) {
            if(!TileTypesHelper.IsActive((TileType)GameState.Instance.ActualGrid.GetCell(neighbor)))
                continue;
            bool hasFilledNeigbor = 
                neighbor.GetNeighbours()
                .Select(GameState.Instance.ActualGrid.GetCell)
                .Cast<TileType>()
                .Any(TileTypesHelper.IsFilled);
            if(hasFilledNeigbor)
                continue;
            GameState.Instance.ActualGrid.SetCell(
                neighbor,
                (int)TileTypesHelper.ToInactive((TileType)GameState.Instance.ActualGrid.GetCell(neighbor)));
        }
    }

    public static int GetNextLevelCost() {
        switch(GameState.Instance.Level) {
            case 1:
                return GameConstants.Level2Cost;
            case 2:
                return GameConstants.Level3Cost;
            case 3:
                return GameConstants.Level4Cost;
            case 4:
                return GameConstants.Level5Cost;
            case 5:
                return GameConstants.Level6Cost;
            default:
                return Int16.MaxValue;
        }
    }

    public static void LevelUp() {
        GameState.Instance.AddToEnergy(-GetNextLevelCost());

        switch(GameState.Instance.Level) {
            case 1:
                GameState.Instance.CurrentStepVisionRadius += 1;
                break;
            case 2:
                GameState.Instance.CanPruneEdges = true;
                break;
            case 3:
                GameState.Instance.CanReveal = true;
                break;
            case 4:
                GameState.Instance.CanGrowRocks = true;
                break;
            case 5:
                GameState.Instance.GameWon = true;
                break;
        }

        GameState.Instance.Level += 1;
    }

    private static int IncrementCost(int currentCost) {
        return (int)Math.Floor(1.5 * currentCost);
    }
}
