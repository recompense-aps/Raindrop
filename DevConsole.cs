using Godot;
using RainDrop;
using System;
using System.Reflection;
using System.IO;
public class DevConsole : Node2D
{
    Type[] _types;

    public override void _Ready()
    {
        _types = new Type[]
        {
            typeof(FreeFall),
            typeof(RainPod),
            typeof(DropMover)
        };

        CreateDirectionsDoc();
    }

    public void CreateDirectionsDoc()
    {
        string file = "# lines that start with '#' are ignored\n";
        file += "# if you want to use the default value for a key, assign it to [value] or get rid of the line";

        foreach(Type t in _types)
        {
            FieldInfo[] f = t.GetFields();
            file += "\n#[" + t.Name + "]\n";
            foreach(FieldInfo fi in f)
            {
                file += t.Name + "." + fi.Name + " = [value]\n";
            }
        }

        StreamWriter w = new StreamWriter(@"C:/dev/RainDrop/Raindrop/settings-file-help.txt");
        w.WriteLine(file);
        w.Close();
    }


//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
