using System;
using System.Collections.Generic;
using System.Xml.Linq;
using KamGame;
using Microsoft.Xna.Framework;


namespace FallenLeaves
{


    public class Scene : GameComponent
    {
        public Theme Theme;
        public readonly List<GameComponent> Components = new List<GameComponent>();

        public string ID;
        public float ScreenWidth;
        public float ScreenHeight;
        public float ScaleWidth;
        public float Width;
        public float Height;

        public float DragSlowing;

        public float PriorWindStrength;
        public float WindStrength;

        public Scene(Game game) : base(game) { }

        public static Scene Load(Theme theme, XElement el)
        {
            var game = theme.Game;
            var scene = new Scene(game)
            {
                Theme = theme,
                ID = el.Attr("id"),
                ScaleWidth = el.Attr("width", 1f),
                DragSlowing = el.Attr("dragSlowing", .85f),
                ScreenWidth = Math.Max(game.ScreenWidth, game.ScreenHeight),
                ScreenHeight = Math.Min(game.ScreenWidth, game.ScreenHeight),
            };

            scene.Width = scene.ScaleWidth * scene.ScreenWidth;
            scene.Height = scene.ScreenHeight;


            foreach (var element in el.Elements())
            {
                var se = Theme.LoadElement(scene, element);
                if (se != null)
                    scene.Components.Add(se);
            }

            return scene;
        }

        private string Path { get { return "Themes/" + Theme.ID + "/"; } }
        public T Load<T>(string assetName)
        {
            return Theme.Game.Content.Load<T>(Path + assetName);
        }

        public override void Update(GameTime gameTime)
        {
            //ScreenWidth = Math.Max(Theme.Game.ScreenWidth, Theme.Game.ScreenHeight);
            //ScreenHeight = Math.Min(Theme.Game.ScreenWidth, Theme.Game.ScreenHeight);
            base.Update(gameTime);
        }

        public bool IsAddedToGame { get; private set; }

        public void Start()
        {
            if (!IsAddedToGame)
            {
                var cmps = Theme.Game.Components;
                cmps.Add(this);
                foreach (var cmp in Components)
                {
                    cmps.Add(cmp);
                }
                IsAddedToGame = true;
            }
            else
            {
                foreach (var cmp in Components)
                {
                    cmp.Enabled = true;
                }
            }
        }

        public void Stop()
        {
            if (!IsAddedToGame) return;
            foreach (var cmp in Components)
            {
                cmp.Enabled = false;
            }
        }

    }

}
