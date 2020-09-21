using Godot;
using Godot.Collections;

namespace RainDrop
{
    class RainDropSave
    {
        private const string fileName = "user://rain.save";
        public int Score { get; set; }
        public bool PlaySounds { get; set; }
        public void Save()
        {
            Dictionary<string, object> data = new Dictionary<string, object>()
            {
                {"Score",Score },
                {"PlaySounds",PlaySounds }
            };

            File saveGame = new File();
            saveGame.Open(fileName, File.ModeFlags.Write);

            saveGame.StoreLine(JSON.Print(data));

            saveGame.Close();
        }

        public void Load()
        {
            File saveGame = new File();
            Dictionary<string, object> data = new Dictionary<string, object>();

            if (!saveGame.FileExists(fileName))
            {
                Score = 0;
                PlaySounds = true;
                return;
            }

            saveGame.Open(fileName, File.ModeFlags.Read);

            while (saveGame.GetPosition() < saveGame.GetLen())
            {
                //JSONParseResult line = JSON.Parse(saveGame.GetLine());
                data = new Dictionary<string, object>((Godot.Collections.Dictionary)JSON.Parse(saveGame.GetLine()).Result);
            }

            saveGame.Close();

            Score = int.Parse(data["Score"].ToString());
            PlaySounds = bool.Parse(data["PlaySounds"].ToString());
        }
    }
}
