using AK_Cutscene_Fixer.Definition;
using System;
using System.IO;

namespace AK_Cutscene_Fixer
{
    class Program
    {
        static SceneLoader SceneLoader;
        static string inputFileDirectory = Path.Combine(Config.CURRENT_DIRECTORY, Config.INPUT_DIRECTORY_NAME);
        static string outputFileDirectory = Path.Combine(Config.CURRENT_DIRECTORY, Config.OUTPUT_DIRECTORY_NAME);

        static void Main(string[] args)
        {
            CreateDirs();
            SceneLoader = new SceneLoader(inputFileDirectory);
            Console.ReadKey();
        }

        static void CreateDirs()
        {

            if (!Directory.Exists(inputFileDirectory)) Directory.CreateDirectory(inputFileDirectory);
            if (!Directory.Exists(outputFileDirectory)) Directory.CreateDirectory(outputFileDirectory);
        }
    }
}
