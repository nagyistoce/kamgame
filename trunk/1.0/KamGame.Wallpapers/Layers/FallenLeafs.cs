using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace KamGame.Wallpapers
{

    public class FallenLeafs : Layer<FallenLeafs>
    {
        public FallenLeafs() { }
        public FallenLeafs(FallenLeafs pattern) { Pattern = pattern; }
        public FallenLeafs(params FallenLeafs[] patterns) { Patterns = patterns; }

        public string TextureNames;
        public float? MinScale, MaxScale;
        public float? SpeedX;
        public float? AccelerationY;
        public float? MinAngleSpeed, MaxAngleSpeed;

        /// <summary>
        /// �����������/������������ ������ ��������
        /// </summary>
        public float? MinSwirlRadius, MaxSwirlRadius;

        /// <summary>
        /// ����������. ��� ������ - ��� �������� ������
        /// </summary>
        public float? Windage;

        /// <summary>
        /// ���������� ������ ��������� ������� ������������ ����� (�� �����������) � ����� ������
        /// </summary>
        public Vector2? EnterPoint;

        /// <summary>
        /// ������� ������������ ������ ��������� EnterPoint
        /// </summary>
        public int? EnterRadius;

        /// <summary>
        /// ���-�� �������, �� ���� ��������� �����������
        /// </summary>
        public float? EnterOpacityPeriod;

        public int? MinEnterPeriod, MaxEnterPeriod;
        public int? MinEnterCount, MaxEnterCount;
        public float? Opacity;

        public override object NewComponent(Scene scene)
        {
            return ApplyPattern(new FallenLeafsPart(), this);
        }
    }


    public class FallenLeafsPart
    {
        public TreeSprite Tree { get; set; }
        public readonly List<Leaf> Leafs = new List<Leaf>(16);
        public Texture2D[] Textures { get; set; }

        public string TextureNames;
        public float MinScale = .5f, MaxScale = 1.5f;
        public float SpeedX = 5f;
        public float AccelerationY = 5f;
        public float MinAngleSpeed = .01f, MaxAngleSpeed = .03f;
        public float MinSwirlRadius = 10f, MaxSwirlRadius = 150f;
        public float Windage = 1f;
        public Vector2 EnterPoint;
        public int EnterRadius = 50;
        public float EnterOpacityPeriod = 60f;
        public int MinEnterPeriod = 100, MaxEnterPeriod = 1000;
        public int MinEnterCount = 1, MaxEnterCount = 5;
        public float Opacity = 1;

        private Color OpacityColor;
        private int EnterPeriod;


        public void LoadContent()
        {
            var speedScale = Tree.Game.GameSpeedScale;
            var accScale = Tree.Game.GameAccelerateScale;
            SpeedX *= speedScale;
            AccelerationY *= accScale;
            MinAngleSpeed *= speedScale;
            MaxAngleSpeed *= speedScale;

            var texNames = (TextureNames ?? "")
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(a => a.Trim()).ToArray();

            Textures = texNames.Select(a => Tree.Load<Texture2D>(a)).ToArray();
            OpacityColor = new Color(Tree.Scene.BlackColor, Opacity);
        }


        public void Update()
        {
            if (Textures.no()) return;

            var game = Tree.Game;
            var scene = Tree.Scene;
            var wind = scene.WindStrength;
            var awind = Math.Abs(wind);

            if (EnterPeriod == 0) EnterPeriod = MinEnterPeriod;

            if (game.FrameIndex % EnterPeriod == 0)
            {
                var defaultX = Tree.Left * Tree.Game.LandscapeWidth + EnterPoint.X;
                var defaultY = (game.ScreenHeight - Tree.Bottom * game.LandscapeHeight) - Tree.Scale * Tree.BaseHeight + EnterPoint.Y;

                for (int i = 1, len = game.Rand(MinEnterCount, (int)(awind * awind * MaxEnterCount)); i <= len; i++)
                {
                    var tex = game.Rand(Textures);
                    var scale = Tree.Game.LandscapeWidth * game.Rand(MinScale, MaxScale) / tex.Height;
                    Leafs.Add(new Leaf
                    {
                        Texture = tex,
                        Scale = scale,
                        X = defaultX + game.Rand(-EnterRadius, EnterRadius),
                        Y = defaultY + game.Rand(-EnterRadius, EnterRadius),
                        SpeedX = SpeedX * (.5f + .5f * Windage * (1 - scale)),
                        Angle = game.RandAngle(),
                        AngleSpeed = game.RandSign() * game.Rand(MinAngleSpeed, MaxAngleSpeed),
                        Origin = new Vector2(tex.Width / 2f, game.Rand(MinSwirlRadius, MaxSwirlRadius)),
                    });
                }

                EnterPeriod = game.Rand(MinEnterPeriod, (int)(MaxEnterPeriod * (1 - awind)));
            }

            var speedy = .005f * AccelerationY * (1 - Windage * awind * (1 + awind * awind * awind));
            for (var i = Leafs.Count - 1; i >= 0; i--)
            {
                var l = Leafs[i];
                l.AccelerationY += l.Scale * speedy;
                l.Y += l.AccelerationY;
                l.X += l.SpeedX * wind;
                var r = l.Origin.Y * l.Scale;
                if (l.X < -r || l.X > scene.WidthPx + r || l.Y < -r || l.Y > scene.HeightPx + r)
                {
                    Leafs.RemoveAt(i);
                    continue;
                }

                l.Angle += l.AngleSpeed * (.5f + .5f * awind);
                unchecked { l.Ticks++; }
            }

        }

        public void Draw()
        {
            var game = Tree.Game;
            var a0 = Opacity / EnterOpacityPeriod;
            foreach (var l in Leafs)
            {
                game.Draw(
                    l.Texture,
                    l.X - Tree.Offset, l.Y,
                    origin: l.Origin,
                    scale: l.Scale,
                    rotation: l.Angle,
                    color: l.Ticks > EnterOpacityPeriod ? OpacityColor : new Color(Tree.Scene.BlackColor, l.Ticks * a0)
                    );
            }
        }


        public class Leaf
        {
            public Texture2D Texture;
            public float Scale;
            public float X, Y, Angle;
            public float SpeedX, AccelerationY, AngleSpeed;
            public Vector2 Origin;
            public int Ticks;
        }

    }


}