using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using KamGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace KamGame
{

    public class TreeSprite : ScrollSprite
    {
        public TreeSprite(Scene scene) : base(scene) { Leafs = new FallenLeafs(this); }

        public List<TreeSpriteNode> Nodes = new List<TreeSpriteNode>();
        public int TotalNodeCount;
        public readonly FallenLeafs Leafs;

        [XmlAttribute("baseHeight")]
        public int BaseHeight;

        public new class Pattern : ScrollSprite.Pattern
        {
            [XmlAttribute("baseHeight")]
            public int BaseHeight;
        }


        public static TreeSprite Load(Scene scene, XElement el)
        {
            var tree = (TreeSprite)scene.Theme.Deserialize<Pattern>(el, new TreeSprite(scene));

            foreach (var element in el.Elements("treeNode"))
            {
                tree.Nodes.Add(TreeSpriteNode.Load(scene, tree, null, element));
            }

            scene.Theme.Deserialize<FallenLeafs.Pattern>(el.Element("fallenLeafs"), tree.Leafs);

            return tree;
        }


        protected override void LoadContent()
        {
            base.LoadContent();

            foreach (var node in Nodes)
            {
                node.LoadTexture(Game);
            }
            Leafs.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            Scale = BaseScale * Scene.ScreenWidth / BaseHeight;
            ScaleWidth = (MarginLeft + MarginRight);

            base.Update(gameTime);

            foreach (var node in Nodes)
            {
                node.Update();
            }

            Leafs.Update();
        }

        public override void Draw(GameTime gameTime)
        {
            var x0 = MarginLeft * Scene.ScreenWidth - Offset;
            float y0 = (int)((1 - MarginBottom) * Game.ScreenHeight);

            foreach (var node in Nodes)
            {
                node.Draw(x0, y0);
            }

            Leafs.Draw();

            base.Draw(gameTime);
        }



        public class TreeSpriteNode
        {
            protected TreeSpriteNode(TreeSprite tree, TreeSpriteNode parent)
            {
                Tree = tree;
                Parent = parent;
            }
            protected readonly TreeSprite Tree;
            protected readonly TreeSpriteNode Parent;
            protected int Index;
            protected Texture2D Texture;
            protected List<TreeSpriteNode> Nodes = new List<TreeSpriteNode>();

            [XmlAttribute("texture")]
            public string TextureName;
            [XmlAttribute("parent")]
            public Vector2 ParentPoint;
            [XmlAttribute("begin")]
            public Vector2 BeginPoint;
            [XmlAttribute("end")]
            public Vector2 EndPoint;
            public float maxAngle;

            public float K0;
            public float K0w;
            public int K0p;
            public float K1;
            public float K2;
            public float minK3;
            public float maxK3;
            public int minK3p;
            public int maxK3p;
            public float K4;
            public float K5;

            public class Pattern : Theme.Pattern
            {
                [XmlAttribute("texture")]
                public string TextureName;
                [XmlAttribute("parent")]
                public Vector2 ParentPoint;
                [XmlAttribute("begin")]
                public Vector2 BeginPoint;
                [XmlAttribute("end")]
                public Vector2 EndPoint;
                public float maxAngle;

                public float K0;
                public float K0w;
                public int K0p;
                public float K1;
                public float K2;
                public float minK3;
                public float maxK3;
                public int minK3p;
                public int maxK3p;
                public float K4;
                public float K5;
            }



            public static TreeSpriteNode Load(Scene scene, TreeSprite tree, TreeSpriteNode parent, XElement el)
            {
                var n = new TreeSpriteNode(tree, parent) { Index = tree.TotalNodeCount++ };
                scene.Theme.Deserialize<Pattern>(el, n);

                foreach (var element in el.Elements("treeNode"))
                {
                    n.Nodes.Add(Load(scene, tree, n, element));
                }

                return n;
            }


            public void LoadTexture(Game2D game)
            {
                Texture = Tree.Scene.Load<Texture2D>(TextureName);
                foreach (var node in Nodes)
                {
                    node.LoadTexture(game);
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

        }


        public class FallenLeafs
        {
            public FallenLeafs(TreeSprite tree) { Tree = tree; }

            public TreeSprite Tree;
            public List<Leaf> Leafs = new List<Leaf>(16);
            public Texture2D[] Textures;

            [XmlAttribute("textures")]
            public string TextureNames;
            public float minScale = .5f;
            public float maxScale = 1.5f;
            public float speedX = 5f;
            public float speedY = 5f;
            public float minAngleSpeed = .01f;
            public float maxAngleSpeed = .03f;
            /// <summary>
            /// Минимальный радиус кружения
            /// </summary>
            public float minSwirlRadius = 10f;
            /// <summary>
            /// Максимальный радиус кружения
            /// </summary>
            public float maxSwirlRadius = 150f;
            /// <summary>
            /// Парусность. Чем больше - тем межденее падает
            /// </summary>
            public float windage = 1f;
            /// <summary>
            /// Координаты центра появления листьев относительно корня (по горизонтали) и верха дерева
            /// </summary>
            public Vector2 enterPoint;
            /// <summary>
            /// Разброс относительно центра появления enterPoint
            /// </summary>
            public int enterRadius = 50;
            /// <summary>
            /// Кол-во фреймов, за лист полностью проявляется
            /// </summary>
            public float enterOpacityPeriod = 60f;
            public int minEnterPeriod = 100, maxEnterPeriod = 1000;
            public int minEnterCount = 1, maxEnterCount = 5;
            public float opacity = 1;

            public class Pattern : Theme.Pattern
            {
                [XmlAttribute("textures")]
                public string TextureNames;
                public float minScale = .5f;
                public float maxScale = 1.5f;
                public float speedX = 5f;
                public float speedY = 5f;
                public float minAngleSpeed = .01f;
                public float maxAngleSpeed = .1f;
                public float minSwirlRadius = 10f;
                public float maxSwirlRadius = 150f;
                public float windage = 1f;
                public Vector2 enterPoint;
                public float enterOpacityPeriod = 30f;
                public int minEnterPeriod = 100, maxEnterPeriod = 1000;
                public int minEnterCount = 1, maxEnterCount = 5;
            }

            private Color opacityColor;

            public void LoadContent()
            {
                var texNames = (TextureNames ?? "")
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(a => a.Trim()).ToArray();
                Textures = new Texture2D[texNames.Length];

                var i1 = 0;
                foreach (var texName in texNames)
                {
                    Textures[i1++] = Tree.Scene.Load<Texture2D>(texName);
                }

                opacityColor = new Color(Color.White, opacity);
            }


            private int enterPeriod;

            public void Update()
            {
                if (Textures.no()) return;

                var game = Tree.Game;
                var scene = Tree.Scene;
                var wind = scene.WindStrength;
                var awind = Math.Abs(wind);

                if (enterPeriod == 0) enterPeriod = minEnterPeriod;

                if (game.FrameIndex % enterPeriod == 0)
                {
                    var defaultX = Tree.MarginLeft * Tree.Scene.ScreenWidth + enterPoint.X;
                    var defaultY = (1 - Tree.MarginBottom) * game.ScreenHeight - Tree.Scale * Tree.BaseHeight + enterPoint.Y;

                    for (int i = 1, len = game.Rand(minEnterCount, (int)(awind * awind * maxEnterCount)); i <= len; i++)
                    {
                        var tex = game.Rand(Textures);
                        var scale = Tree.Scene.ScreenWidth * game.Rand(minScale, maxScale) / tex.Height;
                        Leafs.Add(new Leaf
                        {
                            Texture = tex,
                            Scale = scale,
                            X = defaultX + game.Rand(-enterRadius, enterRadius),
                            Y = defaultY + game.Rand(-enterRadius, enterRadius),
                            SpeedX = speedX * (.5f + .5f * windage * (1 - scale)),
                            Angle = game.RandAngle(),
                            AngleSpeed = game.RandSign() * game.Rand(minAngleSpeed, maxAngleSpeed),
                            Origin = new Vector2(tex.Width / 2f, game.Rand(minSwirlRadius, maxSwirlRadius)),
                        });
                    }

                    enterPeriod = game.Rand(minEnterPeriod, (int)(maxEnterPeriod * (1 - awind)));
                }

                var speedy = .005f * speedY * (1 - windage * awind * (1 + awind * awind * awind));
                for (var i = Leafs.Count - 1; i >= 0; i--)
                {
                    var l = Leafs[i];
                    l.SpeedY += l.Scale * speedy;
                    l.Y += l.SpeedY;
                    l.X += l.SpeedX * wind;
                    var r = l.Origin.Y * l.Scale;
                    if (l.X < -r || l.X > scene.Width + r || l.Y < -r || l.Y > scene.Height + r)
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
                var a0 = opacity / enterOpacityPeriod;
                foreach (var l in Leafs)
                {
                    game.Draw(
                        l.Texture,
                        l.X - Tree.Offset, l.Y,
                        origin: l.Origin,
                        scale: l.Scale,
                        rotation: l.Angle,
                        color: l.Ticks > enterOpacityPeriod ? opacityColor : new Color(Color.White, l.Ticks * a0)
                    );
                }
            }


            public class Leaf
            {
                public Texture2D Texture;
                public float Scale;
                public float X, Y, Angle;
                public float SpeedX, SpeedY, AngleSpeed;
                public Vector2 Origin;
                public int Ticks;
            }

        }

    }


}
