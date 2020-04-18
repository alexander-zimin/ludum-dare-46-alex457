using Godot;
using System;

public class RootNode : Node2D
{
    public enum NodeType {
        Empty,
        SingleReward,
        ConstantReward,
        Impassable
    }

    public bool VisibleByPlayer {get; set;}
    public bool SelectableByPlayer {get; set;}
    public bool ChosenByPlayer {get; set;}
    public NodeType ActualType {get; private set;}

    public RootNode(RootNode.NodeType type) {
        ActualType = type;
    }
}
