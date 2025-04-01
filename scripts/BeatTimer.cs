using Godot;
using RhythmAction.scripts;
using System.Collections.Generic;

public partial class BeatTimer : Node {

    public double BPM { get; private set; } = 0.6f;

    private Timer timer;

    public List<IBeatable> Beatables = new ();

    public override void _Ready() {
        timer = new Timer();
        AddChild(timer);
        timer.Start(BPM);
        timer.Timeout += DispatchBeat;
    }

    private void DispatchBeat() {
        Beatables.ForEach(actionable => actionable.OnBeat());
        timer.Start(BPM);
    }
}
