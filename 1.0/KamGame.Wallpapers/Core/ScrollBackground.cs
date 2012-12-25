﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace KamGame.Wallpaper
{

    public abstract class ScrollBackgroundLayer<TLayer> : ScrollLayer<TLayer>
        where TLayer : Layer
    {
        public string TextureNames;
        public int? BaseHeight;
        public int? RowCount;
        public int? RepeatX;
        public bool? Stretch;
    }



    public abstract class ScrollBackground<TLayer> : ScrollSprite<TLayer>
        where TLayer : Layer
    {
        protected ScrollBackground(Scene scene, TLayer layer) : base(scene, layer) { }
        
        public string TextureNames;
        public int BaseHeight;
        public int RowCount = 1;
        public int RepeatX = 1;
        public bool Stretch;


        protected Texture2D[] Textures;
        protected Vector2 VScale;
        protected int ColCount = 1;

        protected override void LoadContent()
        {
            base.LoadContent();
            if (Textures == null)
                LoadTextures();
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
                var t = Textures[i] = Scene.Load<Texture2D>(textureName);
                for (var j = 1; j < RepeatX; j++)
                {
                    Textures[i + j * ColCount] = t;
                }
                i++;

                WidthPx += RepeatX * t.Width;
                height = Math.Max(height, t.Height);
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
            ScaleWidth = (Width + Left + Right);
            if (Stretch && Textures.Length == 1)
                VScale = new Vector2(Width * Game.ScreenWidth / Textures[0].Width, Game.ScreenHeight / Textures[0].Height);
            base.Update(gameTime);
        }

        protected virtual void BeforeDraw()
        {
        }

        protected float x0, y0;
        public override void Draw(GameTime gameTime)
        {
            x0 = Left * Game.LandscapeWidth;
            y0 = (int)(Top * Game.ScreenHeight);
            BeforeDraw();

            if (Stretch && Textures.Length == 1)
                Game.Draw(Textures[0], x0 - Offset, y0, vscale: VScale, color: OpacityColor);
            else
            {
                var i = 0;
                foreach (var texture in Textures)
                {
                    Game.Draw(texture, x0 - Offset - i % ColCount, y0, scale: Scale, color: OpacityColor);
                    x0 += (float)Math.Truncate(texture.Width * Scale);
                    if (++i % ColCount != 0) continue;
                    y0 += (float)Math.Truncate(texture.Height * Scale);
                    x0 = Left * Game.LandscapeWidth;
                }
            }
            base.Draw(gameTime);
        }

    }

}