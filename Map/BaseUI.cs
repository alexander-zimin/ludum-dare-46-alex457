using Godot;
using System;

public class BaseUI : Control
{
    private Label DayCount {
        get {
            return GetNode<Label>("UIPanel/HSplit/DayCenter/DayCount");
        }
    }
    private Label EnergyCount {
        get {
            return GetNode<Label>("UIPanel/HSplit/EnergyCenter/EnergyCount");
        }
    }
    private Label LastIncome {
        get {
            return GetNode<Label>("UIPanel/HSplit/IncomeCenter/LastIncome");
        }
    }
    private Label LastSpend {
        get {
            return GetNode<Label>("UIPanel/HSplit/SpendCenter/LastSpend");
        }
    }
    private Label Message {
        get {
            return GetNode<Label>("UIPanel/HSplit/MessageCenter/Message");
        }
    }
    private Button VisionButton {
        get {
            return GetNode<Button>("ButtonPanel/VisionButton");
        }
    }
    private Button PruneButton {
        get {
            return GetNode<Button>("ButtonPanel/PruneButton");
        }
    }
    private Button LevelUpButton {
        get {
            return GetNode<Button>("ButtonPanel/LevelUpButton");
        }
    }
    private Button MusicButton {
        get {
            return GetNode<Button>("UIPanel/HSplit/MusicButton");
        }
    }
    private RichTextLabel Instructions {
        get {
            return GetNode<RichTextLabel>("Instruction");
        }
    }
    public override void _Process(float delta) {
        if(GameState.Instance.GameOver)
            return;

        UpdateLabels();
    }

    public override void _Ready() {
        Instructions.Text = GameConstants.FirstInstructions;
    }

    public void UpdateLabels() {
        DayCount.Text = $"Day {GameState.Instance.Day}";
        EnergyCount.Text = $"Energy: {GameState.Instance.Energy}E";
        if(GameState.Instance.LastIncome >= 0)
            LastIncome.Text = $"Income: {GameLogic.TotalIncome}E";
        else
            LastIncome.Text = "";
        if(GameState.Instance.LastSpend >= 0)
            LastSpend.Text = $"Spend: {GameLogic.TotalSpend}E";
        else
            LastSpend.Text = "";

        Message.Text = GameState.Instance.CurrentMessage;

        VisionButton.Pressed = GameState.Instance.RevealInAction;
        PruneButton.Pressed = GameState.Instance.PruneEdgesInAction;

        if(LevelUpButton.Disabled && GameState.Instance.CanLevelUp) {
            Instructions.Text = GameConstants.LevelUpInstructions;
            Instructions.Visible = true;
        }
        LevelUpButton.Disabled = !GameState.Instance.CanLevelUp;

        if(!VisionButton.Visible && GameState.Instance.CanReveal) {
            Instructions.Text = GameConstants.RevealInstructions;
            Instructions.Visible = true;
        }
        VisionButton.Visible = GameState.Instance.CanReveal;
        VisionButton.Disabled = GameState.Instance.Energy < GameState.Instance.CurrentRevealCost;

        if(!PruneButton.Visible && GameState.Instance.CanPruneEdges) {
            Instructions.Text = GameConstants.PruneInstructions;
            Instructions.Visible = true;
        }
        PruneButton.Visible = GameState.Instance.CanPruneEdges;
        PruneButton.Disabled = GameState.Instance.Energy < GameState.Instance.CurrentPruneCost;

        VisionButton.Text = $"Reveal ({GameState.Instance.CurrentRevealCost}E)";
        PruneButton.Text = $"Prune ({GameState.Instance.CurrentPruneCost}E)";
        LevelUpButton.Text = $"Grow! ({GameLogic.GetNextLevelCost()}E)";

        if(GameState.Instance.ShowInstructions) {
            Instructions.Text = GameState.Instance.CurentInstructions;
            Instructions.Visible = true;
        }
    }

    public void _on_Button_pressed() {
        Sounds.Instance.PlaySound(Sounds.SkipSound);
        GameLogic.ProcessTurn();
        GetParent().GetParent<Map>().RestartUI();
        if(GameState.Instance.GameOver) {
            GetParent().GetParent<Map>().ShowGameOverScreen();
            return;
        }
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
        GetParent().GetParent<Map>().RestartUI();
        if(GameState.Instance.GameWon)
            GetParent().GetParent<Map>().ShowGameWonScreen();
    }

    public void _on_Instruction_gui_input(InputEvent @event) {
        if(Instructions.Visible && @event is InputEventMouseButton mouseButtonEvent) {
            if(mouseButtonEvent.Pressed)
                Instructions.Visible = false;
        }
    }

    public void _on_MusicButton_pressed() {
        Sounds.Instance.Disabled = !MusicButton.Pressed;
        GetParent().GetParent<Map>().SwitchMusic(MusicButton.Pressed);
    }
}
