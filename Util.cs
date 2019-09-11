using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using System.Diagnostics;

namespace RainDrop
{
    public static class Util
    {
        public static Node2D LoadNode(string path)
        {
            PackedScene o = GD.Load<PackedScene>("res://" + path + ".tscn");
            return o.Instance() as Node2D;
        }
        public static void CreateLevel(Node2D mainScene)
        {
            int lanes = 4;
            int rows = 3;
            int obstacles = 6;
            int spaceX = 260;
            int spaceY = 64;
            int start = 100;

            for (int i = 0; i < obstacles; i++)
            {
                int lane = (int)GD.RandRange(1, lanes);
                int row = (int)GD.RandRange(1, rows);
                GD.Randomize();

                Debug.WriteLine(lane + ","+ row);

                Node2D obs = LoadNode("Obstacle");
                mainScene.AddChild(obs);
                obs.Position = new Vector2(lane * spaceX + start, row * spaceY + start);
            }
            
        }
    }
}
