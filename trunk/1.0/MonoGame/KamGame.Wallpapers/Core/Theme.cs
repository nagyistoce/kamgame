using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Microsoft.Xna.Framework;


namespace KamGame.Wallpapers
{

    public class Theme
    {
        public Theme(Game2D game, string id, params Scene[] scenes)
        {
            Game = game;
            ID = id;
            Scenes = new ObservableList<Scene>();
            Scenes.ItemAdded += (source, args) =>
            {
                args.Item.Theme = this;
                args.Item.ThemeIndex = args.Index;
            };

            Scenes.AddRange(scenes.Where(a => a != null));
        }

        public readonly Game2D Game;
        public readonly string ID;

        public string Title;
        public readonly ObservableList<Scene> Scenes;
    }




}
