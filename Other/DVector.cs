using Godot;
using System;
using System.Collections.Generic;

public struct DVector : IEquatable<DVector>
{
    public int X {get; private set;}
    public int Y {get; private set;}

    public DVector(int x, int y) {
        X = x;
        Y = y;
    }

    public bool Equals(DVector other)
    {
        return this.X == other.X && this.Y == other.Y;
    }

    public static DVector FromVector2(Vector2 v) {
        return new DVector((int)v.x, (int)v.y);
    }

    public List<DVector> GetNeighbours() {
        return new List<DVector>() {
            new DVector(X-1, Y),
            new DVector(X+1, Y),
            new DVector(X-1, Y+1),
            new DVector(X-1, Y-1),
            new DVector(X+1, Y+1),
            new DVector(X+1, Y-1),
            new DVector(X, Y-1),
            new DVector(X, Y+1)
        };
    }

    public List<DVector> GetSquare(int radius) {
        var result = new List<DVector>();
        for(int i = -radius; i < radius+1; i++)
            for(int j = -radius; j < radius+1; j++)
                result.Add(new DVector(X+i, Y+j));
        return result;
    }
}
