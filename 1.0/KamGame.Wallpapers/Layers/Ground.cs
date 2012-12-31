using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using KamGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace KamGame.Wallpapers
{

    public class Ground : ScrollBackgroundLayer<Ground>
    {
        public Ground() { }
        public Ground(Ground pattern) { Pattern = pattern; }
        public Ground(params Ground[] patterns) { Patterns = patterns; }

        public int[] Heights;
        public readonly List<Grass> Grasses = new List<Grass>();

        public override object NewComponent(Scene scene)
        {
            if (Width == null) Width = scene.Width;
            return ApplyPattern(new GroundSprite(scene), this);
        }
    }

    public class GroundSprite : ScrollBackground
    {

        public GroundSprite(Scene scene): base(scene)
        {
            Align = SpriteAlign.Bottom;
            Grasses = new ObservableList<GrassPart>();
            Grasses.ItemAdded+= (source, args) => 
            {
                args.Item.Scene = scene;
                args.Item.Ground = this;
            };
        }

        public int[] Heights;
        public readonly ObservableList<GrassPart> Grasses;

        protected override void LoadContent()
        {
            base.LoadContent();

            //OpacityColor = new Color(Color.Green, Opacity);
            OpacityColor = new Color(Scene.BlackColor, Opacity);

            foreach (var grass in Grasses)
            {
                grass.LoadContent();
            }
        }

        public override void Update(GameTime gameTime)
        {
            Scale = Width * Game.LandscapeWidth / WidthPx;
            base.Update(gameTime);

            var minX = (int)(.95f * Offset / Scale);
            var maxX = (int)(1.05f * minX + Game.ScreenWidth / Scale);

            foreach (var grass in Grasses)
            {
                grass.Update(minX, maxX);
            }
        }


        public override void Draw(GameTime gameTime)
        {
            var minX = (int)(.95f * Offset / Scale);
            var maxX = (int)(1.11f * minX + Game.ScreenWidth / Scale);

            base.Draw(gameTime);

            foreach (var grass in Grasses)
            {
                grass.Draw(minX, maxX);
            }

            //Game.DrawString(minX + "\n" + maxX);
        }

    }

}
