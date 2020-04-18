using Godot;
using System;

public class Map : Node2D
{
    public void ShowGameOverScreen() {
        var UINode = GetNode<Node>("UILayer");
        var gameOverScreen = ResourceLoader.Load<PackedScene>("res://Map/GameOverScreen.tscn").Instance();
        UINode.AddChild(gameOverScreen);

        GetNode<BaseUI>("UILayer/BaseUI").UpdateLabels();
    }
}
