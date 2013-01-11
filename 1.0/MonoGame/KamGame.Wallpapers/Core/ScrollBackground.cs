using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace KamGame.Wallpapers
{

    public abstract class ScrollBackgroundLayer<TLayer> : ScrollLayer<TLayer>
        where TLayer : Layer
    {
        public string TextureNames;
        public int? BaseHeight;
        public int? RowCount;
        public int? RepeatX;
        public bool? Stretch;
        public float? BaseVScale;
    }


    public enum SpriteAlign { Top, Bottom }

    public abstract class ScrollBackground : ScrollSprite
    {
        protected ScrollBackground(Scene scene) : base(scene) { }

        public string TextureNames;
        public int BaseHeight;
        public int RowCount = 1;
        public int RepeatX = 1;
        public bool Stretch;
        public float BaseVScale = 1;

        public SpriteAlign Align;
        protected Texture2D[] Textures;
        protected Vector2 VScale;
        protected int ColCount = 1;
        protected float x0, y0;

        protected override void LoadContent()
        {
            if (Textures == null)
                LoadTextures();
            base.LoadContent();

            // ReSharper disable PossibleNullReferenceException
            Stretch = Stretch && Textures.Length == 1;
            if (Stretch)
                VScale = new Vector2(Scene.WidthPx / Textures[0].Width, Game.ScreenHeight / Textures[0].Height);
            // ReSharper restore PossibleNullReferenceException

            TotalWidthPx -= ColCount + 1;
        }

        private void LoadTextures()
        {
            if (RepeatX < 1) RepeatX = 1;
            if (RowCount < 1) RowCount = 1;

            var textureNames = (TextureNames ?? "")
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(a => a.Trim()).ToArray();

            Textures = new Texture2D[RepeatX * textureNames.Length];
            ColCount = textureNames.Length / RowCount;
            var i = 0;
            WidthPx = 0;
            HeightPx = 0;
            var height = 0;
            foreach (var textureName in textureNames)
            {
                var t = Textures[i] = Scene.LoadTexture(textureName);
                for (var j = 1; j < RepeatX; j++)
                {
                    Textures[i + j * ColCount] = t;
                }
                i++;

                WidthPx += RepeatX * t.Width;
                height = Math.Max(height, (int)(BaseVScale * t.Height));
                if (i % ColCount != 0) continue;
                HeightPx += height;
                height = 0;
            }
            WidthPx /= RowCount;
            ColCount *= RepeatX;

            if (BaseHeight == 0) BaseHeight = HeightPx;
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            x0 = Left * Game.LandscapeWidth;

            if (Stretch)
            {
                y0 = (int)(Top * Game.LandscapeHeight);
                VScale.Y = Game.ScreenHeight / Textures[0].Height;
            }
            else if (Align == SpriteAlign.Bottom)
            {
                y0 = Game.ScreenHeight - Bottom * Game.LandscapeHeight - (int)(HeightPx * Scale);
                VScale = new Vector2(Scale, Scale * BaseVScale);
            }
            else
            {
                y0 = (int)(Top * Game.LandscapeHeight);
                VScale = new Vector2(Scale, Scale * BaseVScale);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (Stretch && Textures.Length == 1)
                Game.Draw(Textures[0], x0 - Offset, y0, vscale: VScale, color: OpacityColor);
            else
            {
                var i = 0;
                foreach (var texture in Textures)
                {
                    Game.Draw(texture, x0 - Offset - i % ColCount, y0, vscale: VScale, color: OpacityColor);
                    x0 += (int)(texture.Width * Scale);
                    if (++i % ColCount != 0) continue;
                    y0 += (int)(texture.Height * Scale);
                    x0 = Left * Game.LandscapeWidth;
                }
            }
            base.Draw(gameTime);
        }

    }

}
