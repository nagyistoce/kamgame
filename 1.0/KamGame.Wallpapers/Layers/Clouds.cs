using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace KamGame.Wallpapers
{

    public class Clouds : ScrollLayer<Clouds>
    {
        public static float DensityFactor = 1;

        public Clouds() { }
        public Clouds(Clouds pattern) { Pattern = pattern; }
        public Clouds(params Clouds[] patterns) { Patterns = patterns; }

        public string TextureNames;
        public int? BaseHeight;
        public int? Density;
        public float? MinScale;
        public float? MaxScale;
        public float? Speed;

        public override object NewComponent(Scene scene)
        {
            return ApplyPattern(new CloudsSprite(scene), this);
        }
    }


    public class CloudsSprite : ScrollSprite
    {
        public CloudsSprite(Scene scene) : base(scene) { }

        public List<Cloud> Clouds = new List<Cloud>();

        public string TextureNames;
        public int BaseHeight = 256;
        public int Density = 3;
        public float MinScale = .5f;
        public float MaxScale = 1.5f;
        public float Speed = .5f;

        protected int stepX, minY, maxY;


        protected override void LoadContent()
        {
            base.LoadContent();

            Speed *= Game.GameSpeedScale;

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

            texCount = Math.Min(texCount, (int)Math.Round(texCount * Wallpapers.Clouds.DensityFactor));

            var textures = new Texture2D[texCount];

            var count = Density == 0 ? texCount : (int)(Width * Density * Wallpapers.Clouds.DensityFactor);
            WidthPx = (int)(Width * Game.LandscapeWidth);
            Scale = Game.LandscapeHeight / BaseHeight;
            minY = (int)(Game.LandscapeHeight * Top);
            maxY = (int)(Game.LandscapeHeight * (1 - Bottom));
            stepX = WidthPx / count;

            for (var i = 0; i < count; i++)
            {
                var j = i % texCount;
                var c = new Cloud
                {
                    Index = i,
                    Texture = textures[j] ?? (textures[j] = Scene.LoadTexture(textureNames[textureIndexes[j]])),
                };

                c.Reset(this, Clouds.LastOrDefault());
                Clouds.Add(c);
            }

        }


        public override void Update(GameTime gameTime)
        {
            TotalWidth = Width;
            var speed = Speed * (.5f + .5f * Math.Abs(Scene.WindStrength));
            for (var i = 0; i < Clouds.Count; i++)
            {
                var c = Clouds[i];
                if (c.X + c.Offset < -c.Width)
                {
                    c.Reset(this, i > 0 ? Clouds[i - 1] : null);
                    c.Offset = WidthPx - c.X;
                }
                else if (c.X + c.Offset - c.Width / 2f > WidthPx)
                {
                    c.Reset(this, i < Clouds.Count - 1 ? Clouds[i + 1] : null);
                    c.Offset = -c.Width - c.X;
                }
                c.Offset += speed;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (var c in Clouds)
            {
                var x = c.X - Offset + c.Offset;
                Game.Draw(c.Texture, x, c.Y,
                    scale: c.Scale,
                    // ReSharper disable PossibleLossOfFraction
                    origin: new Vector2(c.Texture.Width / 2, c.Texture.Height / 2),
                    // ReSharper restore PossibleLossOfFraction
                    effect: c.Effects,
                    color: OpacityColor
                );
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

            public void Reset(CloudsSprite sprite, Cloud prior)
            {
                var game = sprite.Game;
                var minY = (int)(sprite.Game.LandscapeHeight * sprite.Top);
                var maxY = (int)(sprite.Game.LandscapeHeight * (1 - sprite.Bottom));

                Scale = sprite.Scale * game.Rand(sprite.MinScale, sprite.MaxScale);
                var ef = game.Rand(2);
                Effects = SpriteEffects.None;
                if ((ef & 1) == 1) Effects |= SpriteEffects.FlipHorizontally;
                //if ((ef & 2) == 2) Effects |= SpriteEffects.FlipVertically;
                Width = (int)(Texture.Width * Scale);
                X = Index * sprite.stepX + game.Rand(-sprite.stepX / 4, sprite.stepX / 4);

                float y1 = maxY, y2 = minY;
                if (prior != null)
                {
                    var pheight = prior.Texture.Height * prior.Scale;
                    y1 = prior.Y + pheight / 4;
                    y2 = prior.Y + pheight * 3 / 4;
                }

                if (y1 < minY && y2 > maxY)
                    Y = game.Rand(minY, maxY);
                else if (y2 > maxY)
                    Y = game.Rand(minY, y1);
                else if (y1 < minY)
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
