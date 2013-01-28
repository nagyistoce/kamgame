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
            var sky = Scene.Layers.OfType<Sky>().FirstOrDefault();
            OpacityColor = (sky != null && sky.CloudColor != default(Color) ? sky.CloudColor : Color.White) * Opacity;
            Speed *= Game.GameSpeedScale;

            var textureNames = (TextureNames ?? "")
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(a => a.Trim()).ToArray();

            var texCount = textureNames.Length;
            var textureIndexes = Game.RandSequence(texCount);

            texCount = Math.Min(texCount, (int)Math.Round(texCount * Wallpapers.Clouds.DensityFactor));

            var textures = new Texture2D[texCount];

            var count = Density == 0 ? texCount : (int)(Width * Density * Wallpapers.Clouds.DensityFactor);
            WidthPx = (int)(Width * Game.LandscapeWidth);
            Scale = Game.LandscapeHeight / BaseHeight;
            minY = (int)(Game.LandscapeHeight * Top);
            maxY = (int)(Game.LandscapeHeight * (1 - Bottom));
            stepX = (WidthPx + 2 * BaseHeight) / Math.Max(2, count - 1);

            for (var i = 0; i < count; i++)
            {
                var j = i % texCount;
                var c = new Cloud
                {
                    Index = i,
                    Texture = textures[j] ?? (textures[j] = Scene.LoadTexture_Large(textureNames[textureIndexes[j]])),
                };

                c.Reset(this, Clouds.LastOrDefault());
                //c.Zt = Game.Rand(2000);

                Clouds.Add(c);
            }

        }


        public override void Update(GameTime gameTime)
        {
            TotalWidth = Width;
            var speed = Speed * (.5f + .5f * Math.Abs(Scene.WindStrength));

            //const float z0 = 200f;
            //const float zt0 = 2000f;
            //var y0 = Game.LandscapeHeight / 3f;

            //foreach (var c in Clouds)
            //{
            //    c.Zt += 8 * speed;
            //    if (c.Zt >= zt0)
            //    {
            //        c.Reset(this, null);
            //        c.Zt = 0;
            //    }
            //    c.Scale_t = z0 / (zt0 - c.Zt);
            //    c.Y = y0 * (1 - 1.2f * z0 / (zt0 - c.Zt));
            //    //c.OpacityColor = new Color(Color.White, c.Zt / zt0);
            //    c.OpacityColor = 2 * c.Zt < zt0 ? new Color(Color.White, 2 * c.Zt / zt0) : Color.White;
            //}


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

            //foreach (var c in Clouds)
            //{
            //    var x = c.X - Offset;
            //    Game.Draw(c.Texture, x, c.Y,
            //        scale: c.Scale0 * c.Scale_t,
            //        origin: c.Origin,
            //        effect: c.Effects,
            //        color: c.OpacityColor
            //    );
            //    //Game.DrawString("Y: " + c.Y, x, c.Y, Color.Red);
            //}

            var sw = (Game.ScreenHeight - Game.LandscapeHeight * (Top + Bottom)) / (maxY - minY);
            foreach (var c in Clouds)
            {
                var x = c.X - Offset + c.Offset;
                Game.Draw(c.Texture, x, minY + c.Y * sw,
                    scale: c.Scale,
                    origin: new Vector2(c.Texture.Width / 2f, c.Texture.Height / 2f),
                    effect: c.Effects,
                    color: OpacityColor
                );
            }

            //Game.DrawString(minY + " .. " + maxY + " * " + sw, 100, 100, color: Color.Red);
            //for (int i = 0; i < Clouds.Count; i++)
            //{
            //    var c = Clouds[i];
            //    Game.DrawString(c.Y, 100, 140 + 32 * i, color: Color.Red);
            //}


            base.Draw(gameTime);
        }


        public class Cloud
        {
            public int Index;
            public Texture2D Texture;
            public float X, Y, Offset;
            public SpriteEffects Effects;
            public float Scale, Scale_t, Zt;
            public int Width;
            public Vector2 Origin;
            public Color OpacityColor;

            public void Reset(CloudsSprite sprite, Cloud prior)
            {
                var game = sprite.Game;
                var minY = 0;
                var maxY = sprite.maxY - sprite.minY;

                Scale = sprite.Scale * game.Rand(sprite.MinScale, sprite.MaxScale);
                var ef = game.Rand(2);
                Effects = SpriteEffects.None;
                if ((ef & 1) == 1) Effects |= SpriteEffects.FlipHorizontally;
                //if ((ef & 2) == 2) Effects |= SpriteEffects.FlipVertically;
                Width = (int)(Texture.Width * Scale);
                X = -sprite.BaseHeight + Index * sprite.stepX + game.Rand(-sprite.stepX / 4, sprite.stepX / 4);

                float y1 = maxY, y2 = minY;
                if (prior != null)
                {
                    var pheight = prior.Texture.Height * prior.Scale;
                    y1 = MathHelper.Clamp(prior.Y + pheight * .25f, minY, maxY);
                    y2 = MathHelper.Clamp(prior.Y + pheight * .75f, minY, maxY);
                }

                if (y1 <= minY && y2 >= maxY)
                    Y = game.Rand(minY, maxY);
                else if (y2 >= maxY)
                    Y = game.Rand(minY, y1);
                else if (y1 <= minY)
                    Y = game.Rand(y2, maxY);
                else if (game.Rand() > .5f)
                    Y = game.Rand(y2, maxY);
                else
                    Y = game.Rand(minY, y1);

                Origin = new Vector2(Texture.Width / 2f, Texture.Height / 2f);

            }

        }


    }
}
