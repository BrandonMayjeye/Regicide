using Godot;
using Mayjeye.Extensions;

namespace Mayjeye
{
    public class GameSave
    {
        public string SaveName { get; set; }

    }
    public class Game
    {
        #region IO
        private string _rootSaveDirectory => ProjectSettings.GlobalizePath("user://Saves");
        public string GameSaveDir => System.IO.Path.Combine(_rootSaveDirectory, this.SaveName + "/");
        public string GameSaveFile => System.IO.Path.Combine(GameSaveDir, "game.json");
        public void CheckCreateDir(string path)
        {
            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);
        }
        public bool SaveFileExists()
        {
            return System.IO.File.Exists(GameSaveFile);
        }

        #endregion
        public string SaveName { get; set; }
        public GameNode GameNode { get; }

        public Game(string saveName, GameNode gameNode)
        {
            this.SaveName = saveName;
            GameNode = gameNode;
            CheckCreateDir(GameSaveDir);
            if (SaveFileExists())
            {
                //load Game
                var gameSave = this.GameSaveFile.ParseJson<GameSave>();
                this.SaveName = gameSave.SaveName;
            }
            else
            {
                System.IO.File.WriteAllText(GameSaveFile,
                 Newtonsoft.Json.JsonConvert.SerializeObject(this.Save(),
                  Newtonsoft.Json.Formatting.Indented));
            }
            GD.Print(this.SaveName);

        }
        public GameSave Save()
        {
            return new GameSave()
            {
                SaveName = this.SaveName
            };
        }

    }
}