using System;
using System.Collections.Generic;
using System.Text;

namespace Open_RPG_Maker
{
    public static class Paths
    {
        public static String RTP = @"C:\Program Files\Common Files\Enterbrain\RGSS\";
        public static String Root = @"C:\Users\Thomas\Documents\ORPG\";
        public static String MainDir = "";
        public static String GraphicsDir { get { return MainDir + "Graphics\\"; } }
        public static String AudioDir { get { return MainDir + "Audio\\"; } }
        public static String DataDir { get { return MainDir + "Data\\"; } }

        public static String[] audioPaths = new String[] {
            "BGM",
            "BGS",
            "ME",
            "SE"};
        public static String[] graphicsPaths = new String[] {
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
    }
}
