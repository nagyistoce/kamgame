﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using KamGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace KamGame.Wallpaper
{

    public class Ground : ScrollBackgroundLayer<Ground>
    {
        public Ground() { }
        public Ground(Ground pattern) { Pattern = pattern; }
        public Ground(params Ground[] patterns) { Patterns = patterns; }

        public int[] Heights;
        public readonly List<Grass> Grasses = new List<Grass>();

        public override GameComponent NewComponent(Scene scene)
        {
            if (Width == null) Width = scene.Width;
            return ApplyPattern(new GroundSprite(scene), this);
        }
    }

    public class GroundSprite : ScrollBackground
    {

        public GroundSprite(Scene scene): base(scene)
        {
            Grasses = new ObservableCollection<Grass>().OnAdd(a =>
            {
                a.Scene = scene;
                a.Ground = this;
            });
        }

        public int[] Heights;

        public readonly ObservableCollection<Grass> Grasses;

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
            Scale = Width * Game.LandscapeWidth / WidthPx;
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
            y0 = Game.ScreenHeight - (float)Math.Truncate(HeightPx * Scale) + 1;
        }

        public override void Draw(GameTime gameTime)
        {
            var minX = (int)(.95f * Offset / Scale);
            var maxX = (int)(1.11f * minX + Game.ScreenWidth / Scale);

            foreach (var grass in Grasses)
            {
                grass.Draw(minX, maxX);
            }

            base.Draw(gameTime);
            //Game.DrawString(minX + "\n" + maxX);
        }


    }


    public class Grass : Layer<Grass>
    {
        public Scene Scene { get; set; }
        public GroundSprite Ground { get; set; }

        private List<Herb> Herbs;
        private Texture2D[] Textures;

        public string TextureNames;
        public Vector2 BeginPoint;
        public int Density;
        public float MinScale;
        public float MaxScale;
        public float maxAngle;
        public float minRotation;
        public float maxRotation;
        public float opacity = .7f;

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


        private Color opacityColor;

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

            opacityColor = new Color(Color.White, opacity);

            var count = (int)(Density * Scene.Width);
            Herbs = new List<Herb>(count);

            var heights = Ground.Heights ?? new int[0];
            var step = heights != null && heights.Length > 0 ? Ground.WidthPx / Ground.RepeatX / heights.Length : 0;

            for (var i = 0; i < count; i++)
            {
                var h = new Herb
                {
                    Texture = Textures[game.Rand(Textures.Length)],
                    X = game.Rand(Ground.WidthPx),
                    Scale = MinScale + (MaxScale - MinScale) * game.Rand(),
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
                    h.Y = Ground.HeightPx - (heights[hi0] + (heights[hi1] - heights[hi0]) * (h.X - x0) / (x1 - x0));
                }
                Herbs.Add(h);
            }

        }


        public void Update(int minX, int maxX)
        {
            var game = Ground.Game;
            var wind = Scene.WindStrength;
            var awind = Math.Abs(wind);
            var wind0 = Scene.PriorWindStrength;
            float ticks = game.FrameIndex;

            var windAngle = K0 * maxAngle * wind;
            var windAngleW = K0w * maxAngle * wind;
            var k01 = (2 + awind) * Math.PI / (maxX - minX);
            var k0 = -(float)Math.Sign(wind) * ticks / K0p - k01 * minX;
            foreach (var h in Herbs)
            {
                if (h.X < minX || h.X > maxX) continue;

                //h.windAngle = windAngle;

                h.angleSpeed += 0
                    + windAngleW * (float)Math.Sin(k01 * h.X + k0)
                    + h.K1 * wind
                    + h.K2 * (wind - wind0)
                    //+ h.K3 * awind * awind * (float)Math.Sin(ticks / h.K3p)
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
                    game.ScreenHeight - h.Y * gscale,
                    origin: BeginPoint,
                    scale: h.Scale * game.LandscapeHeight / h.Texture.Height,
                    rotation: h.Angle0 + h.Angle + h.windAngle,
                    //effect: h.Effect
                    color: opacityColor
                );
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


        public override GameComponent NewComponent(Scene scene)
        {
            throw new NotImplementedException();
        }
    }
}
