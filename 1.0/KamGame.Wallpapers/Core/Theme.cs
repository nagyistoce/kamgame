using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public Theme(Game2D game, string id, params Scene[] scenes)
        {
            Game = game;
            ID = id;
            Scenes = new ObservableCollection<Scene>();
            Scenes.CollectionChanged += (sender, args) =>
            {
                var i = Scenes.Count;
                foreach (Scene newItem in args.NewItems)
                {
                    newItem.Theme = this;
                    newItem.ThemeIndex = i++;
                }
            };

            Scenes.AddRange(scenes);
        }

        public readonly Game2D Game;
        public readonly string ID;

        public string Title;
        public readonly ObservableCollection<Scene> Scenes;
    }




}
