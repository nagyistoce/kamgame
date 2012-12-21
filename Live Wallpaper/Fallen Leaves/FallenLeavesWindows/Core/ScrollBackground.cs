using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using KamGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace FallenLeaves
{

    public abstract class ScrollBackground : ScrollSprite
    {
        [XmlAttribute("textures")]
        public string TextureNames;
        [XmlAttribute("baseHeight")]
        public int BaseHeight;
        [XmlAttribute("rowCount")]
        public int RowCount = 1;
        [XmlAttribute("repeatX")]
        public int RepeatX = 1;

        private int ColCount = 1;

        protected Texture2D[] Textures;


        protected ScrollBackground(Scene scene) : base(scene) { }

        public new class Pattern : ScrollSprite.Pattern
        {
            [XmlAttribute("textures")]
            public string TextureNames;
            [XmlAttribute("baseHeight")]
            public int BaseHeight;
            [XmlAttribute("rowCount")]
            public int RowCount = 1;
            [XmlAttribute("repeatX")]
            public int RepeatX = 1;
        }


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
            Width = 0;
            Height = 0;
            var height = 0;
            foreach (var textureName in textureNames)
            {
                var t = Textures[i] = Scene.Load<Texture2D>(textureName);
                for (var j = 1; j < RepeatX; j++)
                {
                    Textures[i + j * ColCount] = t;
                }
                i++;

                Width += RepeatX * t.Width;
                height = Math.Max(height, t.Height);
                if (i % ColCount != 0) continue;
                Height += height;
                height = 0;
            }
            Width /= RowCount;
            ColCount *= RepeatX;

            if (BaseHeight == 0) BaseHeight = Height;
        }


        public override void Update(GameTime gameTime)
        {
            ScaleWidth = (BaseScale + MarginLeft + MarginRight);
            base.Update(gameTime);
        }

        protected virtual void BeforeDraw()
        {
        }

        protected float x0, y0;
        public override void Draw(GameTime gameTime)
        {
            x0 = MarginLeft * Scene.ScreenWidth;
            y0 = (int)(MarginTop * Game.ScreenHeight);
            BeforeDraw();

            var i = 0;
            foreach (var texture in Textures)
            {
                Game.Draw(texture, x0 - Offset - i % ColCount, y0, scale: Scale, color: OpacityColor);
                x0 += (float)Math.Truncate(texture.Width * Scale);
                if (++i % ColCount != 0) continue;
                y0 += (float)Math.Truncate(texture.Height * Scale);
                x0 = MarginLeft * Scene.ScreenWidth;
            }

            base.Draw(gameTime);
        }

    }

}
