using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Win32;

namespace Game_Player
{
    public static class Paths
    {
        public static String RTP;
        public static String Root;
        public static String GraphicsDir { get { return Root + "Graphics\\"; } }
        public static String AudioDir { get { return Root + "Audio\\"; } }
        public static String DataDir { get { return Root + "Data\\"; } }
        public static String Title = "";

        public static String[] AudioPaths = new String[] {
            "BGM",
            "BGS",
            "ME",
            "SE"};
        public static String[] GraphicsPaths = new String[] {
            "Animations",
            "Autotiles",
            "Battlebacks",
            "Battlers",
            "Characters",
            "Fogs",
            "Gameovers",
            "Icons",
            "Panoramas",
            "Pictures",
            "Tilesets",
            "Titles",
            "Transitions",
            "Windowskins"};

        public static void Load(string root)
        {
            Root = root;

            bool defaultRTP = false;
            try
            {
                StreamReader sr = new StreamReader(Root + "Settings.txt");
                int n = 0;
                while (sr.Peek() != -1)
                {
                    string line = sr.ReadLine();
                    bool read = true;

                    read &= line.Length > 0;
                    if (line.Length >= 2)
                        read &= !(line.Substring(0, 2).Equals("//"));

                    if (read)
                    {
                        switch (n)
                        {
                            case 0:
                                Title = line;
                                break;
                            case 1:
                                if (line.Equals("Default"))
                                    defaultRTP = true;
                                else
                                    RTP = line;
                                break;
                        }
                        n++;
                    }
                }
            }
            catch
            {
                MsgBox.Show("Could not read settings file. Using default settings...");
                defaultRTP = true;
                Title = "Game";
            }

            if (defaultRTP)
            {
                try
                {
                    RegistryKey key = Registry.LocalMachine;
                    key = key.OpenSubKey("Software");
                    key = key.OpenSubKey("ORPG", true);
                    RTP = (string)key.GetValue("DefaultDir");
                }
                catch
                {
                    MsgBox.Show("Could not load default settings.\n" +
                                "Some resources may not load.");
                    RTP = "";
                }
            }
        }

        public static string FindValidPath(string filename, string[] paths, string[] extentions)
        {
            for (int i = 0; i < extentions.Length; i++)
            {
                for (int j = 0; j < paths.Length; j++)
                {
                    string path = paths[j] + filename + extentions[i];
                    if (File.Exists(path))
                        return path;
                }
            }

            return "";
        }
    }
}
