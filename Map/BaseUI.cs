using Godot;
using System;

public class BaseUI : Control
{
    private RichTextLabel DayCount {
        get {
            return GetNode<RichTextLabel>("DayCount");
        }
    }
    private RichTextLabel EnergyCount {
        get {
            return GetNode<RichTextLabel>("EnergyCount");
        }
    }
    private RichTextLabel LastIncome {
        get {
            return GetNode<RichTextLabel>("LastIncome");
        }
    }
    private RichTextLabel LastSpend {
        get {
            return GetNode<RichTextLabel>("LastSpend");
        }
    }
    private Button VisionButton {
        get {
            return GetNode<Button>("VisionButton");
        }
    }
    private Button PruneButton {
        get {
            return GetNode<Button>("PruneButton");
        }
    }
    private Button LevelUpButton {
        get {
            return GetNode<Button>("LevelUpButton");
        }
    }
    public override void _Process(float delta) {
        if(GameState.Instance.GameOver)
            return;

        UpdateLabels();
    }

    public void UpdateLabels() {
        DayCount.BbcodeText = $"Day {GameState.Instance.Day}";
        EnergyCount.BbcodeText = $"Energy {GameState.Instance.Energy}";
        if(GameState.Instance.LastIncome >= 0)
            LastIncome.BbcodeText = $"Income: [color=green]{GameLogic.TotalIncome}[/color]";
        else
            LastIncome.BbcodeText = "";
        if(GameState.Instance.LastSpend >= 0)
            LastSpend.BbcodeText = $"Spend: [color=red]{GameLogic.TotalSpend}[/color]";
        else
            LastSpend.BbcodeText = "";

        VisionButton.Pressed = GameState.Instance.RevealInAction;
        PruneButton.Pressed = GameState.Instance.PruneEdgesInAction;
        LevelUpButton.Disabled = !GameState.Instance.CanLevelUp;
        VisionButton.Visible = GameState.Instance.CanReveal;
        PruneButton.Visible = GameState.Instance.CanPruneEdges;

        VisionButton.Text = $"Reveal ({GameState.Instance.CurrentRevealCost}E)";
        PruneButton.Text = $"Prune ({GameState.Instance.CurrentPruneCost}E)";
    }

    public void _on_Button_pressed() {
        GameLogic.ProcessTurn();
    }

    public void _on_GrowButton_mouse_entered() {
        GameState.Instance.BlockGridSelection = true;
    }

    public void _on_GrowButton_mouse_exited() {
        GameState.Instance.BlockGridSelection = false;
    }

    public void _on_VisionButton_pressed() {
        GameState.Instance.RevealInAction = VisionButton.Pressed;
    }

    public void _on_PruneButton_pressed() {
        GameState.Instance.PruneEdgesInAction = PruneButton.Pressed;
    }

    public void _on_LevelUpButton_pressed() {
        GameLogic.LevelUp();
        if(GameState.Instance.GameWon)
            GetParent().GetParent<Map>().ShowGameOverScreen();
    }
}
