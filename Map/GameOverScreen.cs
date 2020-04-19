using Godot;
using System;

public class GameOverScreen : Control
{
    public void _on_Restart_pressed() {
        GameState.Restart();

        var map = GetParent().GetParent<Map>();

        Visible = false;
        GetParent().RemoveChild(this);
        QueueFree();

        map.RestartUI();
    }
}
