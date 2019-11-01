using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace RainDrop
{
    public static class Util
    {
        public static bool PermaLog = false;
        public static bool ConsoleLog = false;

        public static Dictionary<string, object> Globals = new Dictionary<string, object>();
        public static string LogFilePath = @"C:/dev/raindrop.log";

        public static HUD HUD;

        private static SaveFile _saveFile;

        public static SaveFile SaveFile
        {
            get
            {
                if (_saveFile == null)
                {
                    _saveFile = new SaveFile(true);
                }
                return _saveFile;
            }
        }

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
            if (!PermaLog) return;

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
        public static int Direction(float value)
        {
            if (value == 0)
            {
                return 0;
            }
            else if (value > 0)
            {
                return 1;
            }
            return -1;
        }

    }
    
    public class ClassInspector
    {
        private object _target;
        Type _targetType;

        public ClassInspector(object target)
        {
            _target = target;
            _targetType = target.GetType();
        }

        public object Get(string key)
        {
            PropertyInfo myPropInfo = _targetType.GetProperty(key);
            return myPropInfo.GetValue(_target, null);
        }

        public void Set(string key, object value)
        {

        }
    }
}
