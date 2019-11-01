using Godot;

namespace RainDrop
{
    class ScoreKeeper: Godot.Object
    {
        [Signal]
        public delegate void ScoreChanged();

        int _score = 0;
        int _objectsPassed = 0;
        int _powerUpsCollected = 0;

        public int Score
        {
            get
            {
                return _score;
            }
        }

        public int PowerUpsCollected
        {
            get
            {
                return _powerUpsCollected;
            }
        }

        public void ScoreObstacle(Obstacle ob)
        {
            ob.Connect("PassedPlayer", this, nameof(OnObstaclePassedPlayer), null, (int)ConnectFlags.Oneshot);
        }

        public void ScorePowerUp(PowerUp p)
        {
            p.Connect("Collected", this, nameof(OnPowerUpCollected), null, (int)ConnectFlags.Oneshot);
        }

        private void CalculateScore()
        {
            _score = _objectsPassed + _powerUpsCollected;
            EmitSignal(nameof(ScoreChanged));
        }

        private void OnObstaclePassedPlayer()
        {
            _objectsPassed++;
            CalculateScore();
        }

        private void OnPowerUpCollected()
        {
            _powerUpsCollected++;
            CalculateScore();
        }
    }
}
