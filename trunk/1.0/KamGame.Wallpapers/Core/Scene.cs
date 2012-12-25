﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;


namespace KamGame.Wallpaper
{



    public class Scene
    {
        public Scene(float scaleWidth = 1f)
        {
            Width = scaleWidth;
        }

        public readonly List<Layer> Layers = new List<Layer>();
        public readonly List<GameComponent> Components = new List<GameComponent>();

        public Theme Theme { get; set; }
        public int ThemeIndex { get; set; }

        public float Width = 1f;
        public float DragSlowing = .85f;

        public float WidthPx { get; set; }
        public float HeightPx { get; set; }

        public float PriorWindStrength { get; set; }
        public float WindStrength { get; set; }



        public T Load<T>(string assetName)
        {
            return Theme.Game.Content.Load<T>("Themes/" + Theme.ID + "/" + assetName);
        }

        public void Start()
        {
            if (Components.Count == 0)
            {
                Components.AddRange(Layers.Select(a => a.NewComponent(this)));
            }
            Components.ForEach(Theme.Game.Components.Add);
        }

        public void Stop()
        {
            var cmps = Theme.Game.Components;
            Components.ForEach(a => cmps.Remove(a));
        }

        public Scene Next()
        {
            return ThemeIndex < Theme.Scenes.Count - 1 ? Theme.Scenes[ThemeIndex + 1] : Theme.Scenes[0];
        }

    }

}
