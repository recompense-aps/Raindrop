using Godot;
using RainDrop;

public class LevelSelect : Node2D
{
    public override void _Ready()
    {
        Util.GetButton(this, "CityButton").Connect("Click", this, nameof(OnCityButtonClick));
        Util.GetButton(this, "JungleButton").Connect("Click", this, nameof(OnJungleButtonClick));
        Util.GetButton(this, "OceanButton").Connect("Click", this, nameof(OnOceanButtonClick));
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    private void OnCityButtonClick(Button button)
    {
        SetSpawnType("city");
        LaunchFreeFall();
    }

    private void OnJungleButtonClick(Button button)
    {
        SetSpawnType("jungle");
        LaunchFreeFall();
    }

    private void OnOceanButtonClick(Button button)
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
