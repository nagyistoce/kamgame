using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace KamGame.Wallpapers
{

    public class Tree : ScrollLayer<Tree>
    {
        public Tree() { }
        public Tree(Tree pattern) { Pattern = pattern; }
        public Tree(params Tree[] patterns) { Patterns = patterns; }

        public int BaseHeight;
        public readonly List<TreeNode> Nodes = new List<TreeNode>();
        public readonly FallenLeafs Leafs = new FallenLeafs();

        public override object NewComponent(Scene scene)
        {
            return ApplyPattern(new TreeSprite(scene), this);
        }
    }


    public class TreeSprite : ScrollSprite
    {
        public TreeSprite(Scene scene): base(scene)
        {
            Leafs = new FallenLeafs { Tree = this };
            Nodes = new ObservableList<TreeNode>();
            Nodes.ItemAdded += (source, args) => args.Item.SetTree(this);
        }

        public int BaseHeight;
        public readonly ObservableList<TreeNode> Nodes;
        public readonly FallenLeafs Leafs;

        protected int TotalNodeCount;

        protected override void LoadContent()
        {
            base.LoadContent();

            foreach (var node in Nodes)
            {
                node.LoadContent(Game);
            }
            Leafs.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            Scale = Width * Game.LandscapeWidth / BaseHeight;
            TotalWidth = (Left + Right);

            base.Update(gameTime);

            foreach (var node in Nodes)
            {
                node.Update();
            }

            Leafs.Update();
        }

        public override void Draw(GameTime gameTime)
        {
            var x0 = Left * Game.LandscapeWidth - Offset;
            float y0 = (int)(Game.ScreenHeight - Bottom * Game.LandscapeHeight);

            foreach (var node in Nodes)
            {
                node.Draw(x0, y0);
            }

            Leafs.Draw();

            base.Draw(gameTime);
        }


    }


    public class TreeNode : Layer<TreeNode>
    {
        public TreeNode()
        {
            Nodes = new ObservableList<TreeNode>();
            Nodes.ItemAdded += (source, args) => args.Item.Parent = this;
        }
        public TreeNode(TreeNode pattern) : this() { Pattern = pattern; }
        public TreeNode(params TreeNode[] patterns) : this() { Patterns = patterns; }


        protected internal TreeSprite Tree;
        protected internal TreeNode Parent;
        protected Texture2D Texture;

        public readonly ObservableList<TreeNode> Nodes;
        public string TextureName;

        /// <summary>
        /// координаты точки parent-текстуры (т.е. к которой крепится эта ветка)
        /// </summary>
        public Vector2 ParentPoint;

        /// <summary>
        /// координаты точки начала ветки (на текстуре). Считается от левого верхнего угла текстуры
        /// </summary>
        public Vector2 BeginPoint;

        /// <summary>
        /// координаты точки конца ветки (на текстуре). Считается от левого верхнего угла текстуры
        /// </summary>
        public Vector2 EndPoint;

        /// <summary>
        /// максимальный (почти) угол отклонения
        /// </summary>
        public float maxAngle;

        /// <summary>
        /// коэф-т изменения угола наклона в зависимости от силы ветра. Не влияет на колебания
        /// </summary>
        public float K0;

        public float K0w;
        public int K0p;

        /// <summary>
        /// коэф-т изменения угла наклона  в зависимости от силы ветра. Но он влияет на колебания
        /// </summary>
        public float K1;

        /// <summary>
        /// коэф-т реакции на изменение ветра (проявляется при резких перепадах
        /// </summary>
        public float K2;

        /// <summary>
        /// коэф-т случайных колебаний (максимальный, в меняется случайным образом через случайный период)
        /// </summary>
        public float minK3, maxK3;

        /// <summary>
        /// максимальный период, в который проявляется случайные колебация
        /// </summary>
        public int minK3p, maxK3p;

        /// <summary>
        /// коэф-т затухания колебаний
        /// </summary>
        public float K4;

        /// <summary>
        /// коэф-т упругости - чем больше, тем быстрее ветка возвращается к начальному положению
        /// </summary>
        public float K5;


        public void SetTree(TreeSprite tree)
        {
            Tree = tree;
            foreach (var node in Nodes)
            {
                node.SetTree(tree);
            }
        }

        public void LoadContent(Game2D game)
        {
            var timeScale = Tree.Game.GameTimeScale;
            var accScale = Tree.Game.GameAccelerateScale;
            K0w *= accScale;
            K0p = (int)(K0p * timeScale);
            K1 *= accScale;
            K2 *= accScale;
            minK3 *= accScale;
            maxK3 *= accScale;
            minK3p = (int)(minK3p * timeScale);
            maxK3p = (int)(maxK3p * timeScale);
            K4 *= accScale;
            K5 *= accScale;

            Texture = Tree.Scene.Load<Texture2D>(TextureName);
            foreach (var node in Nodes)
            {
                node.LoadContent(game);
            }
        }

        public float Angle;
        private float angleSpeed;

        private int ticks3;
        private int Period3;
        private float Amplitude3;
        public float ParentAngle;

        public float TotalAngle;
        private float windAngle;

        public void Update()
        {
            var game = Tree.Game;
            var wind0 = Tree.Scene.PriorWindStrength;
            var wind = Tree.Scene.WindStrength;
            var awind = Math.Abs(wind);

            windAngle = K0 * maxAngle * wind;

            if (--ticks3 <= 0)
            {
                if (Equals(Amplitude3, 0f))
                {
                    var f = game.Rand();
                    ticks3 = Period3 = (int)((minK3p + f * (maxK3p - minK3p)) * (1.1f - awind));
                    Amplitude3 = minK3 + f * (maxK3 - minK3);
                }
                else
                {
                    ticks3 = Period3;
                    Amplitude3 = 0f;
                }
            }

            angleSpeed +=
                (K0p == 0 ? 0f : K0w * maxAngle * wind * wind * (float)Math.Sin((float)game.FrameIndex / K0p))
              + K1 * wind
              + K2 * (wind - wind0)
              - K5 * Angle / maxAngle
              + Amplitude3 * (float)Math.Sin(2 * (float)Math.PI * ticks3 / Period3);


            angleSpeed *= (1f - K4);
            //if (Math.Abs(angleSpeed) > 0.0001f)
            Angle += angleSpeed;

            //h.Angle = MathHelper.Clamp(h.Angle, -maxAngle, maxAngle);
            ParentAngle = Parent != null ? Parent.TotalAngle : 0;
            TotalAngle = windAngle + Angle + ParentAngle;

            foreach (var node in Nodes)
            {
                node.Update();
            }
        }


        public void Draw(float x0, float y0)
        {
            if (Parent == null)
            {
                x0 += (int)(ParentPoint.X * Tree.Scale);
                y0 += (int)(ParentPoint.Y * Tree.Scale);
            }
            else
            {
                var pv = (Parent.BeginPoint - ParentPoint) * Tree.Scale;
                var angle0 = Math.Atan2(pv.X, pv.Y) - ParentAngle;
                var len = pv.Length();
                x0 -= (float)(len * Math.Sin(angle0));
                y0 -= (float)(len * Math.Cos(angle0));
            }

            Tree.Game.Draw(Texture, x0, y0,
                origin: BeginPoint, scale: Tree.Scale,
                rotation: TotalAngle,
                color: Tree.OpacityColor
            );

            //Tree.Game.DrawString(TotalAngle, 0, 32 * Index);

            foreach (var node in Nodes)
            {
                node.Draw(x0, y0);
            }
        }

        public override object NewComponent(Scene scene)
        {
            throw new NotImplementedException();
        }
    }


    public class FallenLeafs : Layer<FallenLeafs>
    {
        public FallenLeafs() { }
        public FallenLeafs(FallenLeafs pattern) { Pattern = pattern; }
        public FallenLeafs(params FallenLeafs[] patterns) { Patterns = patterns; }

        public TreeSprite Tree { get; set; }
        public readonly List<Leaf> Leafs = new List<Leaf>(16);
        public Texture2D[] Textures { get; set; }

        public string TextureNames;
        public float MinScale = .5f;
        public float MaxScale = 1.5f;
        public float SpeedX = 5f;
        public float AccelerationY = 5f;
        public float MinAngleSpeed = .01f;
        public float MaxAngleSpeed = .03f;
        /// <summary>
        /// Минимальный/Максимальный радиус кружения
        /// </summary>
        public float MinSwirlRadius = 10f, MaxSwirlRadius = 150f;
        /// <summary>
        /// Парусность. Чем больше - тем межденее падает
        /// </summary>
        public float Windage = 1f;
        /// <summary>
        /// Координаты центра появления листьев относительно корня (по горизонтали) и верха дерева
        /// </summary>
        public Vector2 EnterPoint;
        /// <summary>
        /// Разброс относительно центра появления EnterPoint
        /// </summary>
        public int EnterRadius = 50;
        /// <summary>
        /// Кол-во фреймов, за лист полностью проявляется
        /// </summary>
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
            OpacityColor = new Color(Color.White, Opacity);
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
                var defaultY = (1 - Tree.Bottom) * game.LandscapeHeight - Tree.Scale * Tree.BaseHeight + EnterPoint.Y;

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
                    color: l.Ticks > EnterOpacityPeriod ? OpacityColor : new Color(Color.White, l.Ticks * a0)
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


        public override object NewComponent(Scene scene)
        {
            throw new NotImplementedException();
        }
    }


}
