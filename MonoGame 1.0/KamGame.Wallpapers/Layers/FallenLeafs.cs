using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace KamGame.Wallpapers
{

    public class FallenLeafs : Layer<FallenLeafs>
    {
        public static float EnterCountFactor = 1;
        public static float ScaleFactor = 1;

        public FallenLeafs() { }
        public FallenLeafs(FallenLeafs pattern) { Pattern = pattern; }
        public FallenLeafs(params FallenLeafs[] patterns) { Patterns = patterns; }

        public string TextureNames;
        public float? MinScale, MaxScale;
        public float? SpeedX;
        public float? AccelerationY;
        public float? MinAngleSpeed, MaxAngleSpeed;

        /// <summary>
        /// Периодичность изменения ускорения угла вращения
        /// </summary>
        public float? MinAnglePeriod, MaxAnglePeriod;

        /// <summary>
        /// Минимальный/Максимальный радиус кружения
        /// </summary>
        public float? MinSwirlRadius, MaxSwirlRadius;

        /// <summary>
        /// Периодичность изменения ускорения радиуса вращения
        /// </summary>
        public float? MinSwirlRadiusPeriod, MaxSwirlRadiusPeriod;

        /// <summary>
        /// Парусность. Чем больше - тем межденее падает
        /// </summary>
        public float? Windage;

        public float? Opacity;

        public override object NewComponent(Scene scene)
        {
            return ApplyPattern(new FallenLeafsPart(), this);
        }
    }


    public class FallenLeafsPart
    {
        public TreeSprite Tree { get; set; }
        public readonly LinkedList<Leaf> Leafs = new LinkedList<Leaf>();
        public Texture2D[] Textures { get; set; }

        public string TextureNames;
        public float MinScale = .5f, MaxScale = 1.5f;
        public float SpeedX = 5f;
        public float AccelerationY = 5f;
        public float MinAngleSpeed = .01f, MaxAngleSpeed = .03f;
        public float MinAnglePeriod = 20, MaxAnglePeriod = 40;
        public float MinSwirlRadius = 10f, MaxSwirlRadius = 150f;
        public float MinSwirlRadiusPeriod = 20f, MaxSwirlRadiusPeriod = 40f;
        public float Windage = 1f;
        public float Opacity = 1;

        private Color OpacityColor;

        //private float defaultLeafX, defaultLeafY;

        public void LoadContent()
        {
            var speedScale = Tree.Game.GameSpeedScale;
            var accScale = Tree.Game.GameAccelerateScale;
            SpeedX *= speedScale;
            AccelerationY *= accScale;
            MinAngleSpeed *= speedScale;
            MaxAngleSpeed *= speedScale;
            MinScale = MinScale * FallenLeafs.ScaleFactor;
            MaxScale = MaxScale * FallenLeafs.ScaleFactor;

            var texNames = (TextureNames ?? "")
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(a => a.Trim()).ToArray();

            Textures = texNames.Select(a => Tree.LoadTexture(a)).ToArray();
            OpacityColor = Tree.Scene.BlackColor * Opacity;

            foreach (var r in Tree.FlatNodes.Select(a => a.LeafRegion))
            {
                r.EnterOpacityPeriod *= speedScale;
                r.MinEnterCount = (int)(r.MinEnterCount * FallenLeafs.EnterCountFactor);
                r.MaxEnterCount = (int)(r.MaxEnterCount * FallenLeafs.EnterCountFactor);
            }

            foreach (var node in Tree.FlatNodes)
            {
                var r = node.LeafRegion;
                if (r.Rects.no()) continue;
                r.ScreenRects = new Rectangle[r.Rects.Length];
                r.Angle0s = new float[r.Rects.Length];
                r.Length0s = new float[r.Rects.Length];
                r.EnterPeriod = Tree.Game.Rand(r.MinEnterPeriod, r.MaxEnterPeriod);

                for (var i = 0; i < r.Rects.Length; i++)
                {
                    var rect = r.Rects[i];
                    var pv = (new Vector2(rect.X, rect.Y) - node.BeginPoint) * Tree.Scale;
                    r.Angle0s[i] = (float)Math.Atan2(pv.X, pv.Y);
                    r.Length0s[i] = pv.Length();
                    r.ScreenRects[i] = new Rectangle(0, 0, (int)(rect.Width * Tree.Scale), (int)(rect.Height * Tree.Scale));
                }
            }
        }


        private void AddLeaves(TreeNodePart node, int count)
        {
            var game = Tree.Game;
            var r = node.LeafRegion;
            for (var i = 0; i < count; i++)
            {
                var tex = game.Rand(Textures);
                var scale = game.LandscapeWidth * game.Rand(MinScale, MaxScale) / tex.Width;
                var windage = Windage * game.Rand(.8f, 1.2f);


                if (r.ScreenRects.no()) continue;

                var rect = game.Rand(r.ScreenRects);
                var l = new Leaf
                {
                    Texture = tex,
                    Region = r,
                    Scale = scale,
                    //VScale = new Vector2(scale * game.Rand(.5f, .95f), scale * game.Rand(.5f, .95f)),
                    //VScaleK = new Vector2(game.Rand(.2f, 1f), game.Rand(.2f, 1f)),
                    Windage = windage,
                    X = game.Rand(rect.Left, rect.Right),
                    Y = game.Rand(rect.Top, rect.Bottom),
                    SpeedX = SpeedX * (.5f + .5f * windage),
                    Origin = new Vector2(tex.Width / 2f, 0),
                    Angle = game.RandAngle(),
                    AngleSpeed0 = game.RandSign() * game.Rand(MinAngleSpeed, MaxAngleSpeed),
                    AnglePeriod = game.Rand(MinAnglePeriod, MaxAnglePeriod),
                    AngleAmplitude = game.Rand(),
                    SwirlRadius0 = game.Rand(MinSwirlRadius, MaxSwirlRadius),
                    SwirlRadiusPeriod = game.Rand(MinSwirlRadiusPeriod, MaxSwirlRadiusPeriod),
                };

                l.SwirlRadiusSpeed0 = .01f * l.SwirlRadius0 * game.Rand(.5f, 1.5f);

                Leafs.AddLast(l);
            }
        }

        private int RemovedCount;

        public void Update()
        {
            if (Textures.no()) return;

            var game = Tree.Game;
            var scene = Tree.Scene;
            var wind = scene.WindStrength;
            var awind = Math.Abs(wind);


            #region LeafRegion

            foreach (var node in Tree.FlatNodes)
            {
                var r = node.LeafRegion;
                if (r.ScreenRects.no()) continue;
                for (var i = 0; i < r.Rects.Length; i++)
                {
                    r.ScreenRects[i].X = (int)(node.LeftPx + r.Length0s[i] * (float)Math.Sin(r.Angle0s[i] - node.TotalAngle));
                    r.ScreenRects[i].Y = (int)(node.TopPx + r.Length0s[i] * (float)Math.Cos(r.Angle0s[i] - node.TotalAngle));
                }
            }

            #endregion


            #region Generate Leaves

            var acc = game.AccelerationDistance2 / 600;

            foreach (var node in Tree.FlatNodes)
            {
                var r = node.LeafRegion;
                if (r.ScreenRects.no()) continue;

                if (acc > .1f)
                    AddLeaves(node, (int)(r.MaxEnterCount * acc));

                if (game.FrameIndex % r.EnterPeriod != 0) continue;
                AddLeaves(node, game.Rand(r.MinEnterCount, (int)(awind * awind * r.MaxEnterCount)));
                r.EnterPeriod = game.Rand(r.MinEnterPeriod, (int)(r.MaxEnterPeriod * (1 - awind)));
            }

            #endregion


            #region Move and Swirl

            //var speedy = .005f * AccelerationY * (1 - Windage * awind * (1 + awind * awind * awind));
            var awind1_3 = awind * (1 + awind * awind * awind);

            var ln = Leafs.First;
            while (ln != null)
            {
                var l = ln.Value;
                if (l.Ticks > l.Region.EnterOpacityPeriod)
                {
                    var speedy = .005f * AccelerationY * (1 - l.Windage * awind1_3);
                    l.AccelerationY += l.Scale * speedy;
                    l.X += l.SpeedX * wind;
                    l.Y += l.AccelerationY;

                    l.Origin.Y += l.Origin.Y < l.SwirlRadius0
                        ? l.SwirlRadiusSpeed0
                        : awind1_3 * l.SwirlRadiusSpeed0 * game.Sin360(game.FrameIndex / l.SwirlRadiusPeriod);

                    l.Angle += l.AngleSpeed0 * (1 + l.AngleAmplitude * game.Sin360(game.FrameIndex / l.AnglePeriod));
                    var r = l.Origin.Y * l.Scale;
                    if (l.X < -r || l.X > scene.WidthPx + r || l.Y < -r || l.Y > scene.HeightPx + r)
                    {
                        var ln2 = ln;
                        ln = ln.Next;
                        Leafs.Remove(ln2);
                        RemovedCount++;
                        continue;
                    }

                    //l.VScaleSpeed -= .002f * awind * l.VScaleK * (l.VScale - new Vector2(l.Scale, l.Scale) * .75f);
                    //l.VScale += l.VScaleSpeed;
                }

                unchecked { l.Ticks++; }
                ln = ln.Next;
            }

            #endregion


            if (RemovedCount < 2048) return;
            GC.Collect();
            RemovedCount = 0;
        }

        public void Draw()
        {
            var game = Tree.Game;

            //game.DrawFrame(20, 50, 100, 100, Color.Blue, 5);
            //game.DrawFrame(220, 250, 500, 500, Color.Blue, 5);


            foreach (var l in Leafs)
            {
                var a0 = Opacity / l.Region.EnterOpacityPeriod;
                game.Draw(
                    l.Texture,
                    l.X - Tree.Offset, l.Y,
                    origin: l.Origin,
                    scale: l.Scale,
                    //vscale: l.VScale,
                    color: l.Ticks > l.Region.EnterOpacityPeriod ? OpacityColor : new Color(Tree.Scene.BlackColor, l.Ticks * a0),
                    rotation: l.Angle
                );
            }

            //foreach (var node in Tree.FlatNodes)
            //{
            //    if (node.LeafRegion.ScreenRects == null) continue;
            //    foreach (var r in node.LeafRegion.ScreenRects)
            //    {
            //        var r2 = r;
            //        r2.X -= (int)Tree.Offset;
            //        game.DrawFrame(r2, Color.Blue, 5);
            //    }
            //}
        }


        public class Leaf
        {
            public Texture2D Texture;
            public LeafRegion Region;
            public float Scale;
            public Vector2 VScale, VScaleK, VScaleSpeed;
            public float X, Y;
            public float Angle, AngleSpeed0;
            public float SpeedX, AccelerationY;
            public Vector2 Origin;
            public float SwirlRadiusSpeed0;
            public int Ticks;
            public float SwirlRadius0;
            public float Windage;
            public bool SwirlStarted;
            public float SwirlRadiusPeriod;
            public float AnglePeriod;
            public float AngleAmplitude;
        }

    }


    public class LeafRegion : Layer<LeafRegion>
    {
        public LeafRegion() { }
        public LeafRegion(LeafRegion pattern) : this() { Pattern = pattern; }
        public LeafRegion(params LeafRegion[] patterns) : this() { Patterns = patterns; }

        public Rectangle[] Rects;
        protected internal Rectangle[] ScreenRects { get; set; }
        protected internal float[] Angle0s { get; set; }
        protected internal float[] Length0s { get; set; }

        public int MinEnterPeriod = 50, MaxEnterPeriod = 1000;
        public int MinEnterCount = 1, MaxEnterCount = 6;

        /// <summary>
        /// Кол-во фреймов, за лист полностью проявляется
        /// </summary>
        public float EnterOpacityPeriod = 10;


        public int EnterPeriod { get; set; }

        public override object NewComponent(Scene scene)
        {
            throw new NotImplementedException();
        }
    }




}