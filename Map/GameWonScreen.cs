using Godot;
using System;

public class GameWonScreen : Control
{
    private Label TextLabel {
        get {
            return GetNode<Label>("Panel/VSplit/Center/Text");
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        TextLabel.Text = $"Congratulations! You have grown a very nice plant. It took you only {GameState.Instance.Day} days! Can you do better??";
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
