using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace KamGame.Wallpapers
{

    public class TreeNode : Layer<TreeNode>
    {
        public TreeNode() { }
        public TreeNode(TreeNode pattern) : this() { Pattern = pattern; }
        public TreeNode(params TreeNode[] patterns) : this() { Patterns = patterns; }



        public readonly List<TreeNode> Nodes = new List<TreeNode>();
        public readonly LeafRegion LeafRegion = new LeafRegion();
        public string TextureName;

        /// <summary>
        /// координаты точки parent-текстуры (т.е. к которой крепится эта ветка)
        /// </summary>
        public Vector2? ParentPoint;

        /// <summary>
        /// координаты точки начала ветки (на текстуре). Считается от левого верхнего угла текстуры
        /// </summary>
        public Vector2? BeginPoint;

        /// <summary>
        /// координаты точки конца ветки (на текстуре). Считается от левого верхнего угла текстуры
        /// </summary>
        public Vector2? EndPoint;

        /// <summary>
        /// максимальный (почти) угол отклонения
        /// </summary>
        public float? maxAngle;

        /// <summary>
        /// коэф-т изменения угола наклона в зависимости от силы ветра. Не влияет на колебания
        /// </summary>
        public float? K0;

        public float? K0w;
        public int? K0p;

        /// <summary>
        /// коэф-т изменения угла наклона  в зависимости от силы ветра. Но он влияет на колебания
        /// </summary>
        public float? K1;

        /// <summary>
        /// коэф-т реакции на изменение ветра (проявляется при резких перепадах
        /// </summary>
        public float? K2;

        /// <summary>
        /// коэф-т случайных колебаний (максимальный, в меняется случайным образом через случайный период)
        /// </summary>
        public float? minK3, maxK3;

        /// <summary>
        /// максимальный период, в который проявляется случайные колебация
        /// </summary>
        public int? minK3p, maxK3p;

        /// <summary>
        /// коэф-т затухания колебаний
        /// </summary>
        public float? K4;

        /// <summary>
        /// коэф-т упругости - чем больше, тем быстрее ветка возвращается к начальному положению
        /// </summary>
        public float? K5;

        public override object NewComponent(Scene scene)
        {
            return ApplyPattern(new TreeNodePart(), this);
        }
    }


    public class TreeNodePart
    {
        public TreeNodePart()
        {
            Nodes = new ObservableList<TreeNodePart>();
            Nodes.ItemAdded += (source, args) => args.Item.Parent = this;
        }


        protected internal TreeSprite Tree;
        protected internal TreeNodePart Parent;
        public Texture2D Texture;

        public readonly ObservableList<TreeNodePart> Nodes;
        public readonly LeafRegion LeafRegion = new LeafRegion();
        public string TextureName;
        public Vector2 ParentPoint;
        public Vector2 BeginPoint;
        public Vector2 EndPoint;
        public float maxAngle;
        public float K0;
        public float K0w;
        public int K0p;
        public float K1;
        public float K2;
        public float minK3, maxK3;
        public int minK3p, maxK3p;
        public float K4;
        public float K5;


        public void SetTree(TreeSprite tree)
        {
            Tree = tree;
            tree.FlatNodes.Add(this);
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

            Texture = Tree.Scene.LoadTexture(TextureName);

            if (Tree.UseFlip)
            {
                BeginPoint.X = Texture.Width - BeginPoint.X;
                EndPoint.X = Texture.Width - EndPoint.X;
                if (Parent != null)
                    ParentPoint.X = Parent.Texture.Width - ParentPoint.X;
            }


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
        protected internal float LeftPx, TopPx;

        public void Update()
        {
            var game = Tree.Game;
            var wind0 = Tree.Scene.PriorWindStrength;
            var wind = Tree.Scene.WindStrength;
            var awind = Math.Abs(wind);

            windAngle = K0 * maxAngle * wind;
            var k2 = wind - wind0;
            if (game.PriorAcceleration != Vector3.Zero)
                k2 += MathHelper.Clamp((game.Acceleration.X - game.PriorAcceleration.X + game.Acceleration.Y - game.PriorAcceleration.Y) / 2, -.75f, .75f);

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
              + K2 * k2
              - K5 * Angle / maxAngle
              + Amplitude3 * (float)Math.Sin(2 * (float)Math.PI * ticks3 / Period3);


            angleSpeed *= (1f - K4);
            //if (Math.Abs(angleSpeed) > 0.0001f)
            Angle += angleSpeed;

            //h.Angle = MathHelper.Clamp(h.Angle, -maxAngle, maxAngle);
            ParentAngle = Parent != null ? Parent.TotalAngle : 0;
            TotalAngle = windAngle + Angle + ParentAngle;

            if (Parent == null)
            {
                LeftPx = Tree.LeftPx;
                TopPx = Tree.TopPx;
            }
            else
            {
                var pv = (Parent.BeginPoint - ParentPoint) * Tree.Scale;
                var angle0 = Math.Atan2(pv.X, pv.Y) - ParentAngle;
                var len = pv.Length();

                LeftPx = Parent.LeftPx - (float)(len * Math.Sin(angle0));
                TopPx = Parent.TopPx - (float)(len * Math.Cos(angle0));
            }


            foreach (var node in Nodes)
            {
                node.Update();
            }
        }


        public void Draw()
        {
            Tree.Game.Draw(Texture, LeftPx, TopPx,
                origin: BeginPoint, scale: Tree.Scale,
                rotation: TotalAngle,
                effect: Tree.UseFlip ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                color: Tree.OpacityColor
            );

            //Tree.Game.DrawString(TotalAngle, 0, 32 * Index);

            foreach (var node in Nodes)
            {
                node.Draw();
            }
        }

    }


    public class LeafRegion : Layer<LeafRegion>
    {
        public LeafRegion() { }
        public LeafRegion(LeafRegion pattern) : this() { Pattern = pattern; }
        public LeafRegion(params LeafRegion[] patterns) : this() { Patterns = patterns; }

        public Vector4[] Rects;
        protected internal Vector4[] ScreenRects { get; set; }

        public int MinEnterPeriod = 50, MaxEnterPeriod = 500;

        public int MinEnterCount = 1, MaxEnterCount = 10;

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
