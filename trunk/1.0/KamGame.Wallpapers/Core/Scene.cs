using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace KamGame.Wallpapers
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

        /// <summary>
        /// общая ширина сцены. Единица изменения - наибольший размер экрана (т.е. ширина в Lanscape-режиме)
        /// </summary>
        public float Width = 1f;

        /// <summary>
        /// уровень инерции при прокрутке - чем он больше, тем медленней экран будет останавливаться при отпускании пальца
        /// </summary>
        public float DragSlowing = .75f;

        /// <summary>
        /// Чёрный цвет - для искуственного зачернения силуетов
        /// </summary>
        public Color BlackColor = Color.Black;

        /// <summary>
        /// Вычисляемая ширина сцены в пикселях
        /// </summary>
        public float WidthPx { get; set; }

        /// <summary>
        /// Вычисляемая высота сцены в пикселях
        /// </summary>
        public float HeightPx { get; set; }

        public float PriorWindStrength { get; set; }
        public float WindStrength { get; set; }


        public void Start()
        {
            WidthPx = Width * Theme.Game.LandscapeWidth;
            HeightPx = Width * Theme.Game.LandscapeHeight;

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



        #region LoadTexture

        private static readonly SortedDictionary<string, int> LoadedTextureСounters = new SortedDictionary<string, int>();
        private readonly SortedSet<string> LoadedTextures = new SortedSet<string>();

        public Texture2D LoadTexture(string name)
        {
            name = "Themes/" + Theme.ID + "/" + name;
            LoadedTextureСounters[name] = LoadedTextureСounters.Try(name) + 1;
            LoadedTextures.Add(name);
            return Theme.Game.Content.Load<Texture2D>(name);
        }

        public void UnloadTextures()
        {
            foreach (var name in LoadedTextures)
            {
                var count = Math.Max(0, LoadedTextureСounters.Try(name) - 1);
                LoadedTextureСounters[name] = count;
            }
        }

        public static void UnloadTextures(Game game)
        {
#if ANDROID
            foreach (var cnt in LoadedTextureСounters)
            {
                if (cnt.Value == 0) game.Content.Unload(cnt.Key);
            }
#endif
            GC.Collect();
        }

        #endregion


    }

}
