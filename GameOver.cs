using Godot;
using RainDrop;

public class GameOver : Node2D
{
    private string[] _facts = new string[]
    {
        "Rain falls from clouds in the sky in the form of water droplets, this is called precipitation.",
        "Water can also fall from the sky in the form of hail, sleet or snow.",
        "Rain is an important part of the water cycle.",
        "Rain occurs on other planets in our Solar System but it is different to the rain we experience here on Earth. For example, rain on Venus is made of sulfuric acid and due to the intense heat it evaporates before it even reaches the surface!",
        "Weather radar is used to detect and monitor rain."
    };

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        RandomNumberGenerator g = new RandomNumberGenerator();
        g.Randomize();
        string fact = _facts[g.RandiRange(0, _facts.Length - 1)];
        (FindNode("FactText") as Label).Text = fact;
        (FindNode("FinalScoreText") as Label).Text = Global.FinalScore.ToString();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
