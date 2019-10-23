using Godot;
using RainDrop;

public class LevelSelect : Node2D
{
    public override void _Ready()
    {
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }

    private void OnCityButtonPressed()
    {
        SetSpawnType("city");
        LaunchFreeFall();
    }


    private void OnJungleButtonPressed()
    {
        SetSpawnType("jungle");
        LaunchFreeFall();
    }


    private void OnOceanButtonPressed()
    {
        SetSpawnType("ocean");
        LaunchFreeFall();
    }
    private void LaunchFreeFall()
    {
        GetTree().ChangeScene("res://Modes/FreeFall.tscn");
    }

    private void SetSpawnType(string type)
    {
        if (Util.Globals.ContainsKey("SpawnType"))
        {
            Util.Globals["SpawnType"] = type;
        }
        else
        {
            Util.Globals.Add("SpawnType", type);
        }
    }
}
