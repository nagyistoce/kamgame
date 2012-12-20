using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using KamGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace FallenLeaves
{
    public class GroundSprite : ScrollBackground
    {
        public GroundSprite(Scene scene) : base(scene) { }

        public List<Grass> Grasses = new List<Grass>();

        public int[] heights;

        public new class Pattern : ScrollBackground.Pattern
        {
            public int[] heights;
        }

        public static GroundSprite Load(Scene scene, XElement el)
        {
            var ground = new GroundSprite(scene);
            scene.Theme.Deserialize<Pattern>(el, ground);

            foreach (var element in el.Elements("grass"))
            {
                ground.Grasses.Add(Grass.Load(scene, ground, element));
            }

            return ground;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            foreach (var grass in Grasses)
            {
                grass.LoadContent();
            }
        }

        public override void Update(GameTime gameTime)
        {
            Scale = BaseScale * Scene.ScreenWidth / Width;
            base.Update(gameTime);

            var minX = (int)(.95f * Offset / Scale);
            var maxX = (int)(1.05f * minX + Game.ScreenWidth / Scale);

            foreach (var grass in Grasses)
            {
                grass.Update(minX, maxX);
            }

        }

        protected override void BeforeDraw()
        {
            y0 = Game.ScreenHeight - (float)Math.Truncate(Height * Scale) + 1;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            var minX = (int)(.95f * Offset / Scale);
            var maxX = (int)(1.11f * minX + Game.ScreenWidth / Scale);

            foreach (var grass in Grasses)
            {
                grass.Draw(minX, maxX);
            }

            //Game.DrawString(minX + "\n" + maxX);
        }



        public class Grass
        {
            public Grass(Scene scene, GroundSprite ground)
            {
                Scene = scene;
                Ground = ground;
            }

            public readonly Scene Scene;
            public readonly GroundSprite Ground;

            private Texture2D[] Textures;

            [XmlAttribute("textures")]
            public string TextureNames;
            [XmlAttribute("begin")]
            public Vector2 BeginPoint;
            public int density;
            public float minScale;
            public float maxScale;
            public float maxAngle;
            public float minRotation;
            public float maxRotation;

            public float K0 = .5f;
            public float K0w = .8f;
            public int K0p = 20;
            public float minK1 = .00015f;
            public float maxK1 = .00025f;
            public float minK2 = .15f;
            public float maxK2 = .25f;
            public float minK3 = .00125f;
            public float maxK3 = .00135f;
            public int minK3p = 50;
            public int maxK3p = 100;
            public float minK4 = .012f;
            public float maxK4 = .016f;
            public float minK5 = .025f;
            public float maxK5 = .035f;

            List<Herb> Herbs;

            public static Grass Load(Scene scene, GroundSprite ground, XElement el)
            {
                return (Grass)scene.Theme.Deserialize<Pattern>(el, new Grass(scene, ground));
            }


            public class Pattern : Theme.Pattern
            {
                [XmlAttribute("textures")]
                public string TextureNames;
                [XmlAttribute("begin")]
                public Vector2 BeginPoint;
                public int density;
                public float minScale;
                public float maxScale;
                public float maxAngle;
                public float minRotation;
                public float maxRotation;

                public float K0 = .5f;
                public float K0w = .8f;
                public int K0p = 20;
                public float minK1 = .00015f;
                public float maxK1 = .00025f;
                public float minK2 = .15f;
                public float maxK2 = .25f;
                public float minK3 = .00125f;
                public float maxK3 = .00135f;
                public int minK3p = 50;
                public int maxK3p = 100;
                public float minK4 = .012f;
                public float maxK4 = .016f;
                public float minK5 = .025f;
                public float maxK5 = .035f;
            }


            public void LoadContent()
            {
                var textureNames = (TextureNames ?? "")
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(a => a.Trim()).ToArray();
                Textures = new Texture2D[textureNames.Length];

                var i1 = 0;
                foreach (var textureName in textureNames)
                {
                    Textures[i1++] = Scene.Load<Texture2D>(textureName);
                }

                var game = Scene.Theme.Game;

                var count = (int)(density * Scene.ScaleWidth);
                Herbs = new List<Herb>(count);

                var heights = Ground.heights;
                var step = heights!=null && heights.Length > 0 ? Ground.Width / Ground.RepeatX / heights.Length : 0;

                for (var i = 0; i < count; i++)
                {
                    var h = new Herb
                    {
                        Texture = Textures[game.Rand(Textures.Length)],
                        X = game.Rand(Ground.Width),
                        Scale = minScale + (maxScale - minScale) * game.Rand(),
                        Angle0 = minRotation + (maxRotation - minRotation) * game.Rand(),
                        K1 = game.Rand(minK1, maxK1),
                        K2 = game.Rand(minK2, maxK2),
                        K3 = game.Rand(minK3, maxK3),
                        K3p = game.Rand(minK3p, maxK3p),
                        K4 = game.Rand(minK4, maxK4),
                        K5 = game.Rand(minK5, maxK5),
                    };

                    if (step > 0)
                    {
                        var hi0 = (h.X / step) % heights.Length;
                        var hi1 = hi0 < heights.Length - 1 ? hi0 + 1 : 0;
                        var x0 = (h.X / step) * step;
                        var x1 = x0 + step;
                        //
                        h.Y = Ground.Height - (heights[hi0] + (heights[hi1] - heights[hi0]) * (h.X - x0) / (x1 - x0));
                    }
                    Herbs.Add(h);
                }

            }

            //private float windAngle;
            private int ticks;

            public void Update(int minX, int maxX)
            {
                unchecked { ticks++; }

                var wind = Scene.WindStrength;
                var wind0 = Scene.PriorWindStrength;

                var windAngle = K0 * maxAngle * wind;
                var windAngleW = K0w * maxAngle * wind * wind;
                var k0 = (float)ticks / (K0p * (maxX - minX)) * Math.Sign(wind);

                foreach (var h in Herbs)
                {
                    if (h.X < minX || h.X > maxX) continue;

                    h.windAngle = windAngle;

                    var k3p = (int)(h.K3p * (1.1f - Math.Abs(wind)));
                    var t3 = (float)(ticks % k3p) / k3p * (1 - 2 * ((ticks / k3p) & 1)) * wind * wind;

                    h.angleSpeed += 0
                        + windAngleW * (float)Math.Sin(k0 * h.X)
                        + h.K1 * wind
                        + h.K2 * (wind - wind0)
                        + h.K3 * t3
                        - h.K5 * h.Scale * (h.Angle + .3f * windAngle) / maxAngle;
                    h.angleSpeed *= 1 - h.K4;

                    //if (h.Angle < -maxAngle && h.angleSpeed < 0 || h.Angle > maxAngle && h.angleSpeed > 0)
                    //    h.angleSpeed = 0;
                    h.Angle += h.angleSpeed;
                    //h.Angle = MathHelper.Clamp(h.Angle, -maxAngle, maxAngle);
                }

            }

            public void Draw(int minX, int maxX)
            {
                var game = Ground.Game;
                var gscale = Ground.Scale;
                foreach (var h in Herbs)
                {
                    if (h.X < minX || h.X > maxX) continue;

                    game.Draw(h.Texture,
                        h.X * gscale - Ground.Offset,
                        Scene.ScreenHeight - h.Y * gscale,
                        origin: BeginPoint,
                        scale: h.Scale * Scene.ScreenHeight / h.Texture.Height,
                        rotation: h.Angle0 + h.Angle + h.windAngle
                        //effect: h.Effect
                    );
                }
            }

        }

        public class Herb
        {
            public Texture2D Texture;
            public float Scale;
            public int X, Y;
            public float Angle0, Angle;
            public float K1, K2, K3, K4, K5;
            public int K3p;
            public float angleSpeed;
            public float windAngle;
        }

    }
}
