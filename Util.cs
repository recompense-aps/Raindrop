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
        public static Dictionary<string, object> Globals = new Dictionary<string, object>();
        public static Node2D LoadNode(string path)
        {
            PackedScene o = GD.Load<PackedScene>("res://" + path + ".tscn");
            return o.Instance() as Node2D;
        }
        public static void ChangeScene(Node context, string scenePath)
        {
            context.GetTree().ChangeScene("res://" + scenePath + ".tscn");
        }
        public static Node FindNode(Node root, string path)
        {
            NodePath nodePath = new NodePath(path);
            return root.GetNode(nodePath);
        }
        public static Button GetButton(Node context, string path)
        {
            return (context.GetNode(new NodePath(path)) as Button);
        }
    }
}
