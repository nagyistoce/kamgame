using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using KamGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace KamGame.Wallpapers
{

    public class Stone : ScrollLayer<Stone>
    {
        public string TextureName;

        public Stone() { }
        public Stone(Stone pattern) { Pattern = pattern; }
        public Stone(params Stone[] patterns) { Patterns = patterns; }

        public override object NewComponent(Scene scene)
        {
            if (Width == null) Width = scene.Width;
            return ApplyPattern(new StoneSprite(scene), this);
        }
    }

    public class StoneSprite : ScrollSprite
    {
        public string TextureName;

        public StoneSprite(Scene scene) : base(scene) { }

        protected Texture2D Texture;
        protected float LeftPx, TopPx, topPx0;

        protected override void LoadContent()
        {
            base.LoadContent();
            OpacityColor = Scene.BlackColor * Opacity;
            Texture = Scene.LoadTexture(TextureName);
            WidthPx = Texture.Width;
            HeightPx = Texture.Height;

            Scale = Width * Game.LandscapeWidth / WidthPx;
            LeftPx = Left * Game.LandscapeWidth;
            topPx0 = -Bottom * Game.LandscapeHeight - (int)(HeightPx * Scale);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            TopPx = Game.ScreenHeight + topPx0;
        }
        
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            Game.Draw(Texture, LeftPx - Offset, TopPx, scale: Scale, color: OpacityColor);
            //Game.DrawString(minX + "\n" + maxX);
        }

    }

}
