using Godot;
using System;

public class EnergyNumber : Control
{
    public int Energy = 0;

    public Label TextLabel {
        get {
            return GetNode<Label>("Text");
        }
    }

    private AnimationPlayer Player {
        get {
            return GetNode<AnimationPlayer>("AnimationPlayer");
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        string sign = Energy >= 0 ? "+" : "";
        TextLabel.Text = sign + Energy.ToString();
    }

    public void Play() {
        Player.Play("EnergyNumberAnimation");
    }
}
