using Godot;
using System;

public class Map : Node2D
{
    private const string gameOverScreenName = "GameOverScreen";
    public void ShowGameOverScreen() {
        var UINode = GetNode<Node>("UILayer");
        if(UINode.HasNode(gameOverScreenName))
            return;
        var gameOverScreen = ResourceLoader.Load<PackedScene>("res://Map/GameOverScreen.tscn").Instance();
        gameOverScreen.Name = gameOverScreenName;
        UINode.AddChild(gameOverScreen);

        GetNode<BaseUI>("UILayer/BaseUI").UpdateLabels();
    }

    public void ShowGameWonScreen() {
        var UINode = GetNode<Node>("UILayer");
        var gameWonScreen = ResourceLoader.Load<PackedScene>("res://Map/GameWonScreen.tscn").Instance();
        UINode.AddChild(gameWonScreen);

        GetNode<BaseUI>("UILayer/BaseUI").UpdateLabels();
    }

    public void RestartUI() {
        var map = GetNode<RootMap>("RootMap");
        map.Regenerate();
    }


    public override void _Process(float delta) {
        if(GameState.Instance.LevelChanged) {
            var Texture = ResourceLoader.Load<Texture>($"res://Images/Background{GameState.Instance.Level}.png");
            GetNode<TextureRect>("Background").Texture = Texture;
        }
    }

    public void SwitchMusic(bool on) {
        var player = GetNode<AudioStreamPlayer>("BackgroundPlayer");
        if(on)
            player.Play();
        else
            player.Stop();
    }
}
