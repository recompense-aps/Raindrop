using Godot;
using RainDrop;
using RainDrop.Enums;

namespace RainDrop
{
    class Spawner
    {
        private Node2D _target;
        private RainPod _pod;
        private ObstacleSpawner _obstacleSpawner = new ObstacleSpawner();
        private string _spawnType = Util.Globals.ContainsKey("SpawnType") ? Util.Globals["SpawnType"] as string : "city";

        public Spawner(Node2D target)
        {
            _target = target;
            _pod = Util.FindNode(_target, "RainPod") as RainPod;
        }
        public PowerUp SpawnPowerUp(Vector2 position, Vector2 velocity)
        {
            PowerUp p = Util.LoadNode("PowerUp") as PowerUp;
            p.Position = position;
            p.Velocity = velocity;
            AddToTarget(p);
            return p;
        }
        public Obstacle SpawnObstacle(Vector2 position, Vector2 velocity)
        {
            Obstacle ob = _obstacleSpawner.Spawn(_spawnType);
            ob.Position = position;
            ob.Velocity = velocity;
            AddToTarget(ob);
            return ob;           
        }
        public RainDropPickUp SpawnRainDropPickUp(Vector2 position, Vector2 velocity)
        {
            RainDropPickUp rainDrop = Util.LoadNode("Drops/RainDropPickUp") as RainDropPickUp;
            rainDrop.Position = position;
            rainDrop.Velocity = velocity;
            AddToTarget(rainDrop);
            return rainDrop;
        }
        private void AddToTarget(Node2D child)
        {
            _target.AddChild(child);
        }
    }
}
