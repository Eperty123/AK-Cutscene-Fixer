using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace AK_Cutscene_Fixer.Definition
{
    public class Scene
    {
        #region Public Variables
        public string InputFile { get; set; }
        public string FileName { get; protected set; }
        public string SceneData { get; protected set; }
        #endregion

        #region Constructors
        public Scene()
        {
            Initialize();
        }

        public Scene(string file)
        {
            Initialize();
            Load(file);
        }
        #endregion

        #region Private, Protected Methods
        protected void Initialize()
        {
        }
        #endregion

        #region Public Methods
        public void Load(string file)
        {
            if (File.Exists(file))
            {
                InputFile = file;
                FileName = Path.GetFileName(file);
                SceneData = File.ReadAllText(file, Encoding.GetEncoding("gbk"));
            }
        }

        public void RemoveCutscene()
        {
            if (!string.IsNullOrEmpty(SceneData) && !string.IsNullOrEmpty(InputFile))
            {
                // Now let's remove our cutscene killer!
                var regexPattern = @"(Event_PlayCutScene ([0-9a-zA-Z_]+) ([0-9]+) ([0-9]+):)";
                var matches = Regex.Matches(SceneData, regexPattern, RegexOptions.IgnoreCase);
                if (matches.Count > 0)
                {
                    for (int i = 0; i < matches.Count; i++)
                    {
                        var match = matches[i];

                        SceneData = Regex.Replace(SceneData, match.Value, "");
                        Console.WriteLine($"[Cutscene]: Cutscene event: {match.Value} removed.");
                    }

                    var correctWord = matches.Count < 2 ? "cutscene trigger" : "cutscene triggers";
                    Console.WriteLine($"[Cutscene]: Removed {matches.Count} {correctWord} from: { FileName}.");
                }
            }
        }

        public void RemoveClientTriggerEvent()
        {
            if (!string.IsNullOrEmpty(SceneData) && !string.IsNullOrEmpty(InputFile))
            {
                // Now let's remove our cutscene killer!
                var regexPattern = @"(Event_AddClientTriggerEvent ([0-9]+) ([0-9]+) ([0-9]+):)";
                var matches = Regex.Matches(SceneData, regexPattern, RegexOptions.IgnoreCase);
                if (matches.Count > 0)
                {
                    for (int i = 0; i < matches.Count; i++)
                    {
                        var match = matches[i];

                        // We need to rename the AddClientTriggerEvent to Event_TriggerEvent.
                        var fix = $"Event_TriggerEvent {match.Groups[4].Value} {match.Groups[2].Value} {match.Groups[3].Value}:";
                        SceneData = Regex.Replace(SceneData, match.Value, fix);
                        Console.WriteLine($"[Event_AddClientTriggerEvent]: Replaced: {match.Value} with: {fix}.");
                    }

                    var correctWord = matches.Count < 2 ? "Event_AddClientTriggerEvent" : "Event_AddClientTriggerEvents";
                    Console.WriteLine($"[Event_AddClientTriggerEvent]: Removed {matches.Count} {correctWord} from: { FileName}.");
                }
            }
        }

        public void Save(string outputFile)
        {
            File.WriteAllText(outputFile, SceneData, Encoding.GetEncoding("gbk"));
        }
        #endregion
    }
}
