using Godot;
using System;

public static class GameConstants
{
    public const int InitialEnergy = 20;
    public const int FilledCellBaseSpend = 1;
    public const int FoodNodeIncome = 10;
    public const int WaterNodeIncome = 10;
    public const int RevealRadius = 1;
    public const int StepVisionRadius = 1;
    public const int RevealCost = 20;
    public const int PruneCost = 5;
    public const int Level2Cost = 200;
    public const int Level3Cost = 1000;
    public const int Level4Cost = 3000;
    public const int Level5Cost = 5000;
    public const int Level6Cost = 20000;
    public const string IntroMessage = "Explore and collect energy to grow your plant!";
    public const string Level2Message = "Woohoo! You now reveal more when you expand your roots.";
    public const string Level3Message = "Woohoo! You can now prune your roots.";
    public const string Level4Message = "Woohoo! You can now reveal any area.";
    public const string Level5Message = "Woohoo! You can now expand through rocks!";
    public const string FirstInstructions = "You can move the screen using WASD keys. Press on an area next to existing root to grow. Each root part have an up-keep costs. You can skip a day if you want, but be sure that your income is greater than spend!";
    public const string LevelUpInstructions = "You can now grow your plant!";
    public const string PruneInstructions = "To prune your roots, first click the 'Prune' button and then choose a part to prune. Your roots will need to remain connected aftewards for the prune to work! The prune cost will increase with each use.";
    public const string RevealInstructions = "To reveal a area on the map, click the 'Reveal' button and then on a hidden area. The reveal cost will increase with each use.";
    public const string PruneRootMessage = "Your roots need to be connected to the plant!";
    public const string PruneNotConnectedMessage = "Your roots need to remain connected after removing the root part!";
}