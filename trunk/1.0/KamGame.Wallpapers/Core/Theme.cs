using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using KamGame;
using KamGame.Converts;
using Microsoft.Xna.Framework;


namespace KamGame.Wallpaper
{

    public class Theme
    {
        public Theme(Game2D game, string id)
        {
            Game = game;
            ID = id;
        }

        public readonly Game2D Game;
        public readonly string ID;

        public string Title;
        public readonly List<Scene> Scenes = new List<Scene>();
    }




}
