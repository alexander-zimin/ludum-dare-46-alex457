using Godot;
using System.Linq;

public enum TileType {
        FilledBaseNode,
        InactiveBaseNode,
        ActiveBaseNode,
        FilledWaterNode,
        InactiveWaterNode,
        ActiveWaterNode,
        FilledFoodNode,
        InactiveFoodNode,
        ActiveFoodNode,
        FilledRockNode,
        InactiveRockNode,
        ActiveRockNode
}

public static class TileTypesHelper {
    public static TileType[] FilledTypes = {
            TileType.FilledBaseNode,
            TileType.FilledFoodNode,
            TileType.FilledWaterNode,
            TileType.FilledRockNode
        };
    public static TileType[] ActiveTypes = {
            TileType.ActiveBaseNode,
            TileType.ActiveFoodNode,
            TileType.ActiveWaterNode,
            TileType.ActiveRockNode
        };
    public static TileType[] InactiveTypes = {
            TileType.InactiveBaseNode,
            TileType.InactiveFoodNode,
            TileType.InactiveWaterNode,
            TileType.InactiveRockNode
        };
    public static bool IsFilled(TileType tile) {
        return FilledTypes.Contains(tile);
    }

    public static bool IsActive(TileType tile) {
        return ActiveTypes.Contains(tile);
    }

    public static bool IsInactive(TileType tile) {
        return InactiveTypes.Contains(tile);
    }

    public static TileType ToInactive(TileType tile) {
        if(IsInactive(tile))
            return tile;
        switch(tile) {
            case TileType.ActiveBaseNode:
            case TileType.FilledBaseNode:
                return TileType.InactiveBaseNode;
            case TileType.ActiveWaterNode:
            case TileType.FilledWaterNode:
                return TileType.InactiveWaterNode;
            case TileType.ActiveFoodNode:
            case TileType.FilledFoodNode:
                return TileType.InactiveFoodNode;
            case TileType.ActiveRockNode:
            case TileType.FilledRockNode:
                return TileType.InactiveRockNode;
            default:
                return tile;
        }
    }

    public static TileType ToActive(TileType tile) {
        if(IsActive(tile))
            return tile;
        switch(tile) {
            case TileType.InactiveBaseNode:
            case TileType.FilledBaseNode:
                return TileType.ActiveBaseNode;
            case TileType.InactiveWaterNode:
            case TileType.FilledWaterNode:
                return TileType.ActiveWaterNode;
            case TileType.InactiveFoodNode:
            case TileType.FilledFoodNode:
                return TileType.ActiveFoodNode;
            case TileType.InactiveRockNode:
            case TileType.FilledRockNode:
                return TileType.ActiveRockNode;
            default:
                return tile;
        }
    }
}