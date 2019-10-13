using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using System.Diagnostics;
using System.IO;

namespace RainDrop
{
    public static class Util
    {
        public static Dictionary<string, object> Globals = new Dictionary<string, object>();
        public static bool PermaLog = true;
        public static bool ConsoleLog = false;
        public static string LogFilePath = @"C:/dev/raindrop.log";

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
            Node n = root.GetNode(nodePath);

            if (n == null)
            {
                throw new Exception("Could not find node: " + path);
            }
            return n;
        }
        public static Button GetButton(Node context, string path)
        {
            return (context.GetNode(new NodePath(path)) as Button);
        }
        public static void FlushLog()
        {
            StreamWriter w = new StreamWriter(LogFilePath, false);
            w.Write("[" + DateTime.Now + "]\tLog File Reset\n-----------------------------------\n");
            w.Close();
        }
        public static void Log(object message)
        {
            if(ConsoleLog)
            {
                Debug.WriteLine(message);
            }

            if (PermaLog)
            {
                using (StreamWriter file =
                    new StreamWriter(LogFilePath, true))
                {
                    file.WriteLine("[" + DateTime.Now + "]\t" + message.ToString());
                }
            }
        }
    }
}
