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
        /// Минимальный/Максимальный радиус кружения
        /// </summary>
        public float? MinSwirlRadius, MaxSwirlRadius;

        /// <summary>
        /// Парусность. Чем больше - тем межденее падает
        /// </summary>
        public float? Windage;

        ///// <summary>
        ///// Координаты центра появления листьев относительно корня (по горизонтали) и верха дерева
        ///// </summary>
        //public Vector2? EnterPoint;

        ///// <summary>
        ///// Разброс относительно центра появления EnterPoint
        ///// </summary>
        //public int? EnterRadius;

        ///// <summary>
        ///// Кол-во фреймов, за лист полностью проявляется
        ///// </summary>
        //public float? EnterOpacityPeriod;

        //public int? MinEnterPeriod, MaxEnterPeriod;
        //public int? MinEnterCount, MaxEnterCount;

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
        public float MinSwirlRadius = 10f, MaxSwirlRadius = 150f;
        public float Windage = 1f;
        //public Vector2 EnterPoint;
        //public int EnterRadius = 50;
        //public float EnterOpacityPeriod = 60f;
        //public int MinEnterPeriod = 100, MaxEnterPeriod = 1000;
        //public int MinEnterCount = 1, MaxEnterCount = 5;
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
            OpacityColor = new Color(Tree.Scene.BlackColor, Opacity);

            //defaultLeafX = Tree.Left * Tree.Game.LandscapeWidth + Tree.Scale * EnterPoint.X;
            //defaultLeafY = Tree.Bottom * Tree.Game.LandscapeHeight + Tree.Scale * (Tree.Nodes[0].Texture.Height - EnterPoint.Y);

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
                r.ScreenRects = new Vector4[r.Rects.Length];
                for (var i = 0; i < r.Rects.Length; i++)
                {
                    var rect = r.Rects[i];
                    r.ScreenRects[i] = new Vector4(
                        rect.X * Tree.Scale,
                        rect.Y * Tree.Scale,
                        rect.Z * Tree.Scale,
                        rect.W * Tree.Scale
                    );
                }
            }

        }


        private void AddLeafs(TreeNodePart node, int count)
        {
            var game = Tree.Game;
            var r = node.LeafRegion;
            for (var i = 0; i < count; i++)
            {
                var tex = game.Rand(Textures);
                var scale = Tree.Game.LandscapeWidth * game.Rand(MinScale, MaxScale) / tex.Height;
                var scale0 = scale / FallenLeafs.ScaleFactor;

                if (r.ScreenRects.no()) continue;

                var rect = game.Rand(r.ScreenRects);
                var l = new Leaf
                {
                    Texture = tex,
                    Region = r,
                    Scale = scale,
                    X = game.Rand(rect.X, rect.Z),
                    Y = game.ScreenHeight - game.Rand(rect.Y, rect.W),
                    SpeedX = SpeedX * (.5f + .5f * Windage * (1 - scale0)),
                    Angle = game.RandAngle(),
                    AngleSpeed = game.RandSign() * game.Rand(MinAngleSpeed, MaxAngleSpeed),
                    Origin = new Vector2(tex.Width / 2f, game.Rand(MinSwirlRadius, MaxSwirlRadius)),
                };
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


            var acc = game.AccelerationDistance2 / 1200;

            foreach (var node in Tree.FlatNodes)
            {
                var r = node.LeafRegion;
                if (acc > .1f)
                    AddLeafs(node, (int)(r.MaxEnterCount * acc));

                if (r.EnterPeriod == 0) r.EnterPeriod = r.MinEnterPeriod;

                if (game.FrameIndex % r.EnterPeriod == 0)
                {
                    //AddLeafs(MaxEnterCount);
                    AddLeafs(node, game.Rand(r.MinEnterCount, (int)(awind * awind * r.MaxEnterCount)));
                    r.EnterPeriod = game.Rand(r.MinEnterPeriod, (int)(r.MaxEnterPeriod * (1 - awind)));
                }
            }

            var speedy = .005f * AccelerationY * (1 - Windage * awind * (1 + awind * awind * awind));

            var kr = .0005f * game.GameSpeedScale * (awind - .5f) / (MaxSwirlRadius - MinSwirlRadius);
            var ka = .0003f * game.GameSpeedScale * (awind - .5f) / (MaxAngleSpeed - MinAngleSpeed);
            var swirlRadius = (MaxSwirlRadius + MinSwirlRadius) / 2;

            var ln = Leafs.First;
            while (ln != null)
            {
                var l = ln.Value;
                l.AccelerationY += l.Scale * speedy;
                l.X += l.SpeedX * wind;
                l.Y += l.AccelerationY;
                l.SwirlRadiusSpeed += kr * (swirlRadius - l.Origin.Y);
                l.Origin.Y += l.SwirlRadiusSpeed;
                var r = l.Origin.Y * l.Scale;
                if (l.X < -r || l.X > scene.WidthPx + r || l.Y < -r || l.Y > scene.HeightPx + r)
                {
                    var ln2 = ln;
                    ln = ln.Next;
                    Leafs.Remove(ln2);
                    RemovedCount++;
                    continue;
                }

                l.AngleSpeed -= ka * l.AngleSpeed;

                l.Angle += l.AngleSpeed;// *(.5f + .5f * awind);
                unchecked { l.Ticks++; }

                ln = ln.Next;
            }

            if (RemovedCount < 2048) return;
            GC.Collect();
            RemovedCount = 0;
        }

        public void Draw()
        {
            var game = Tree.Game;
            foreach (var l in Leafs)
            {
                var a0 = Opacity / l.Region.EnterOpacityPeriod;
                game.Draw(
                    l.Texture,
                    l.X - Tree.Offset, l.Y,
                    origin: l.Origin,
                    scale: l.Scale,
                    color: l.Ticks > l.Region.EnterOpacityPeriod ? OpacityColor : new Color(Tree.Scene.BlackColor, l.Ticks * a0),
                    rotation: l.Angle
                );
            }
        }


        public class Leaf
        {
            public Texture2D Texture;
            public LeafRegion Region;
            public float Scale;
            public float X, Y, Angle;
            public float SpeedX, AccelerationY, AngleSpeed;
            public Vector2 Origin;
            public float SwirlRadiusSpeed;
            public int Ticks;
        }

    }





}