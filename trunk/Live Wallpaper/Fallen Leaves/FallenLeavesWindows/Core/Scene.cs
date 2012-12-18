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
        public float ScaleWidth;
        public float ScreenWidth;
        public float ScreenHeight;
        public float DragSlowing;

        public float PriorWindStrength;
        public float WindStrength;

        public Scene(Game game) : base(game) { }

        public static Scene Load(Theme theme, XElement el)
        {
            var scene = new Scene(theme.Game)
            {
                Theme = theme,
                ID = el.Attr("id"),
                ScaleWidth = el.Attr("width", 1f),
                DragSlowing = el.Attr("dragSlowing", .85f),
            };

            foreach (var element in el.Elements())
            {
                var se = LoadElement(scene, element);
                if (se != null)
                    scene.Components.Add(se);
            }

            return scene;
        }

        public static GameComponent LoadElement(Scene scene, XElement element)
        {
            switch (element.Name.ToString().ToLowerInvariant())
            {
                case "sky":
                    return SkySprite.Load(scene, element);
                case "cloud":
                    return CloudSprite.Load(scene, element);
                case "ground":
                    return GroundSprite.Load(scene, element);
                case "tree":
                    return TreeSprite.Load(scene, element);
                case "wind":
                    return WindController.Load(scene, element);
            }
            return null;
        }

        private string Path { get { return "Themes/" + Theme.ID + "/"; } }
        public T Load<T>(string assetName)
        {
            return Theme.Game.Content.Load<T>(Path + assetName);
        }

        public override void Update(GameTime gameTime)
        {
            ScreenWidth = Math.Max(Theme.Game.ScreenWidth, Theme.Game.ScreenHeight);
            ScreenHeight = Math.Min(Theme.Game.ScreenWidth, Theme.Game.ScreenHeight);
            base.Update(gameTime);
        }

        public void Start()
        {
            var cmps = Theme.Game.Components;
            cmps.Add(this);
            foreach (var cmp in Components)
            {
                cmps.Add(cmp);
            }
        }

    }

}
