using Godot;
using System.Collections.Generic;

public class Sounds
{
    private static Sounds _instance = null;
    public static Sounds Instance {
        get {
            if(_instance == null)
                _instance = new Sounds();
            return _instance;
        }
    }

    private AudioStreamPlayer player;
    private Dictionary<string, AudioStream> Library = new Dictionary<string, AudioStream>();

    public const string FoodSound = "Food";
    public const string GrowSound = "Grow";
    public const string NopeSound = "Nope";
    public const string PruneSound = "Prune";
    public const string RevealSound = "Reveal";
    public const string WaterSound = "Water";
    public const string RockSound = "Rock";
    public const string SkipSound = "Skip";
    public const string LevelUpSound = "LevelUp";

    public bool Disabled {get; set;}

    public void Initialize(AudioStreamPlayer player) {
        if(this.player != null)
            return;
        this.player = player;
        LoadSounds();
    }

    public void LoadSounds() {
        Library.Add(FoodSound, ResourceLoader.Load<AudioStreamSample>("res://Sounds/food.wav"));
        Library.Add(GrowSound, ResourceLoader.Load<AudioStreamSample>("res://Sounds/grow.wav"));
        Library.Add(NopeSound, ResourceLoader.Load<AudioStreamSample>("res://Sounds/nope.wav"));
        Library.Add(PruneSound, ResourceLoader.Load<AudioStreamSample>("res://Sounds/prune.wav"));
        Library.Add(RevealSound, ResourceLoader.Load<AudioStreamSample>("res://Sounds/reveal.wav"));
        Library.Add(WaterSound, ResourceLoader.Load<AudioStreamSample>("res://Sounds/water.wav"));
        Library.Add(RockSound, ResourceLoader.Load<AudioStreamSample>("res://Sounds/rock.wav"));
        Library.Add(SkipSound, ResourceLoader.Load<AudioStreamSample>("res://Sounds/skip.wav"));
        Library.Add(LevelUpSound, ResourceLoader.Load<AudioStreamSample>("res://Sounds/level-up.wav"));
    }

    public void PlaySound(string sound) {
        if(Disabled)
            return;
        player.Stream = Library[sound];
        player.Play();
    }
}
