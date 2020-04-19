using Godot;
using System.Linq;
using System.Collections.Generic;

public class DGrid
{
    private Dictionary<DVector,int> states = new Dictionary<DVector, int>();
    public DGrid() {

    }

    public int GetCell(int x, int y) {
        return GetCell(new DVector(x, y));
    }

    public int GetCell(DVector pos) {
        if(!states.ContainsKey(pos))
            return -1;
        return states[pos];
    }

    public void SetCell(DVector position, int value) {
        states[position] = value;
    }

    public void InitFromTileMap(TileMap map) {
        foreach(Vector2 e in map.GetUsedCells()) {
            var dv = DVector.FromVector2(e);
            SetCell(dv, map.GetCell(dv.X, dv.Y));
        }
    }

    public List<DVector> GetCellsWithValue(int value) {
        return states.Where(p => p.Value == value).Select(p => p.Key).ToList();
    }

    public bool IsEmpty(DVector pos) {
        return !states.ContainsKey(pos);
    }

    public List<KeyValuePair<DVector,int>> GetNonEmptyCells() {
        return states.ToList();
    }

    public bool IsConnectedWithout(DVector position) {
        if(!states.ContainsKey(position))
            return true;
        
        HashSet<DVector> visitedCells = new HashSet<DVector>() {position};
        Queue<DVector> toVisit = new Queue<DVector>();
        toVisit.Enqueue(new DVector(0, 0));
        while(toVisit.Count > 0) {
            var next = toVisit.Dequeue();
            visitedCells.Add(next);
            foreach(var neighbor in next.Get4Neighbours()) {
                if(!states.ContainsKey(neighbor) 
                    || visitedCells.Contains(neighbor)
                    || !TileTypesHelper.IsFilled((TileType)states[neighbor]))
                    continue;
                toVisit.Enqueue(neighbor);
            }
        }

        return visitedCells.SetEquals(GetFilledCells());
    }

    public List<DVector> GetFilledCells() {
        return TileTypesHelper.FilledTypes
                .Cast<int>()
                .SelectMany(GetCellsWithValue)
                .ToList();
    }
}
