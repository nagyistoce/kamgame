using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using KamGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace FallenLeaves
{
    public class CloudSprite : ScrollSprite
    {
        public CloudSprite(Scene scene) : base(scene) { }

        public List<Cloud> Clouds = new List<Cloud>();

        [XmlAttribute("textures")]
        public string TextureNames;
        [XmlAttribute("baseHeight")]
        public int BaseHeight = 256;
        public int densty = 3;
        public float minScale = .5f;
        public float maxScale = 1.5f;
        public int minGroupCount = 1;
        public int maxGroupCount = 3;
        public float speed = .5f;
        public Color color;

        public int stepX;
        public int minY;
        public int maxY;


        public new class Pattern : ScrollSprite.Pattern
        {
            [XmlAttribute("textures")]
            public string TextureNames;
            [XmlAttribute("baseHeight")]
            public int BaseHeight = 256;
            public int densty = 3;
            public float minScale = .5f;
            public float maxScale = 1.5f;
            public int minGroupCount = 1;
            public int maxGroupCount = 3;
            public float speed = 2.5f;
            public Color color;
        }


        public static CloudSprite Load(Scene scene, XElement el)
        {
            return (CloudSprite)scene.Theme.Deserialize<Pattern>(el, new CloudSprite(scene));
        }


        protected override void LoadContent()
        {
            base.LoadContent();

            var textureNames = (TextureNames ?? "")
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(a => a.Trim()).ToArray();

            var texCount = textureNames.Length;
            var textureIndexes = new int[texCount];
            for (int i = 0, len = texCount; i < len; i++)
            {
                textureIndexes[i] = i;
            }
            for (int i = 0, len = texCount; i < len; i++)
            {
                var a = textureIndexes[i];
                var j = Game.Rand(len);
                textureIndexes[i] = textureIndexes[j];
                textureIndexes[j] = a;
            }
            var textures = new Texture2D[texCount];

            var count = densty == 0 ? texCount : Math.Min(texCount, (int)(BaseScale * densty));
            Width = (int)(BaseScale * Scene.ScreenWidth);
            Scale = Scene.ScreenHeight / BaseHeight;
            minY = (int)(Scene.ScreenHeight * MarginTop);
            maxY = (int)(Scene.ScreenHeight * (1 - MarginBottom));
            stepX = Width / (count + 1);

            for (var i = 0; i < count; i++)
            {
                var c = new Cloud
                {
                    Index = i,
                    Texture = textures[i] = Scene.Load<Texture2D>(textureNames[textureIndexes[i]]),
                };

                c.Reset(this, Clouds.LastOrDefault());
                Clouds.Add(c);
            }

        }


        public override void Update(GameTime gameTime)
        {
            ScaleWidth = BaseScale;
            var awind = speed * Math.Abs(Scene.WindStrength);
            for (var i = 0; i < Clouds.Count; i++)
            {
                var c = Clouds[i];
                if (c.X + c.Offset < -c.Width)
                {
                    c.Reset(this, i > 0 ? Clouds[i - 1] : null);
                    c.Offset = Width - c.X;
                }
                else if (c.X + c.Offset > Width)
                {
                    c.Reset(this, i < Clouds.Count - 1 ? Clouds[i + 1] : null);
                    c.Offset = -c.Width - c.X;
                }
                c.Offset += awind;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            var i = 0;
            foreach (var c in Clouds)
            {
                var x = c.X - Offset + c.Offset;
                Game.Draw(c.Texture, x, c.Y, scale: c.Scale, effect: c.Effects, color: color);
            }

            base.Draw(gameTime);
        }


        public class Cloud
        {
            public int Index;
            public Texture2D Texture;
            public float X, Y, Offset;
            public SpriteEffects Effects;
            public float Scale;
            public int Width;

            public void Reset(CloudSprite sprite, Cloud prior)
            {
                var game = sprite.Game;
                var minY = (int)(sprite.Scene.ScreenHeight * sprite.MarginTop);
                var maxY = (int)(sprite.Scene.ScreenHeight * (1 - sprite.MarginBottom));

                Scale = sprite.Scale * game.Rand(sprite.minScale, sprite.maxScale);
                var ef = game.Rand(5);
                Effects = SpriteEffects.None;
                if ((ef & 1) == 1) Effects |= SpriteEffects.FlipHorizontally;
                if ((ef & 2) == 2) Effects |= SpriteEffects.FlipVertically;
                Width = (int)(Texture.Width * Scale);
                X = Index * sprite.stepX + game.Rand(-sprite.stepX / 2, sprite.stepX / 2);

                float y1 = maxY, y2 = minY;
                if (prior != null)
                {
                    var pheight = prior.Texture.Height * prior.Scale;
                    y1 = prior.Y + pheight / 4;
                    y2 = prior.Y + pheight * 3 / 4;
                }

                if (y1 < minY)
                    Y = game.Rand(y2, maxY);
                else if (y2 > maxY)
                    Y = game.Rand(minY, y1);
                else if (game.Rand() > .5)
                    Y = game.Rand(y2, maxY);
                else
                    Y = game.Rand(minY, y1);
            }
        }


    }
}
