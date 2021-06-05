using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AK_Cutscene_Fixer.Definition
{
    public class SceneLoader
    {
        #region Public Variables
        public Dictionary<string, Scene> Scenes { get; protected set; }
        #endregion

        #region Constructors
        public SceneLoader()
        {
            Initialize();
        }

        public SceneLoader(string folder)
        {
            Initialize();
            FindScenes(folder);
        }
        #endregion

        #region Private, Protected Methods
        protected void Initialize()
        {
            Scenes = new Dictionary<string, Scene>();
        }
        #endregion

        #region Public Methods
        public void FindScenes(string folder)
        {
            if (Directory.Exists(folder))
            {
                var sceneFiles = Directory.EnumerateFiles(folder, "*.ini", SearchOption.TopDirectoryOnly).ToList();

                if (sceneFiles.Count > 0)
                {
                    for (int i = 0; i < sceneFiles.Count; i++)
                    {
                        var sceneFile = sceneFiles[i];
                        var fileName = Path.GetFileName(sceneFile);
                        var outputFileDirectory = Path.Combine(Config.CURRENT_DIRECTORY, Config.OUTPUT_DIRECTORY_NAME);
                        var outputFile = Path.Combine(Config.CURRENT_DIRECTORY, Config.OUTPUT_DIRECTORY_NAME, fileName);
                        if (!Directory.Exists(outputFileDirectory)) Directory.CreateDirectory(outputFileDirectory);

                        if (!Scenes.ContainsKey(fileName))
                        {
                            var scene = new Scene(sceneFile);
                            scene.RemoveCutscene();
                            scene.RemoveClientTriggerEvent();
                            scene.Save(outputFile);
                            Scenes.Add(fileName, scene);
                        }
                    }

                    Console.WriteLine("Operation complete. Enjoy.");
                }
                else Console.WriteLine("No scene files found.");
            }
            else Console.WriteLine($"{folder} doesn't exist!");
        }
        #endregion
    }
}
