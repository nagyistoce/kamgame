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

    public class TreeSprite : ScrollSprite
    {
        public TreeSprite(Scene scene) : base(scene) { }

        public List<TreeSpriteNode> Nodes = new List<TreeSpriteNode>();
        private bool TexturesIsLoaded;
        public int TotalNodeCount;
        public int BaseHeight;


        public static TreeSprite Load(Scene scene, XElement el)
        {
            var tree = new TreeSprite(scene)
            {
                BaseScale = el.Attr("width", 1f),
                BaseHeight = el.Attr("baseHeight", 1024),
                MarginLeft = el.Attr("left", 0f),
                MarginRight = el.Attr("right", 0f),
                MarginTop = el.Attr("top", 0f),
                MarginBottom = el.Attr("bottom", 0f),
            };

            foreach (var element in el.Elements())
            {
                tree.Nodes.Add(TreeSpriteNode.Load(scene, tree, null, element));
            }

            return tree;
        }


        protected override void LoadContent()
        {
            if (TexturesIsLoaded) return;
            foreach (var node in Nodes)
            {
                node.LoadTexture(Game);
            }
            TexturesIsLoaded = true;
            base.LoadContent();
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
        }

        public override void Draw(GameTime gameTime)
        {
            var x0 = MarginLeft * Scene.ScreenWidth - Offset;
            float y0 = (int)((1 - MarginBottom) * Game.ScreenHeight);

            foreach (var node in Nodes)
            {
                node.Draw(x0, y0);
            }

            base.Draw(gameTime);
        }



        public class TreeSpriteNode
        {
            public TreeSpriteNode(TreeSprite tree, TreeSpriteNode parent)
            {
                Tree = tree;
                Parent = parent;
            }
            public readonly TreeSprite Tree;
            public readonly TreeSpriteNode Parent;
            public int Index;
            public Texture2D Texture;
            public List<TreeSpriteNode> Nodes = new List<TreeSpriteNode>();

            [XmlAttribute("texture")]
            public string TextureName;
            [XmlAttribute("parent")]
            public Vector2 ParentPoint;
            [XmlAttribute("begin")]
            public Vector2 BeginPoint;
            [XmlAttribute("end")]
            public Vector2 EndPoint;
            public float maxAngle;

            public float K1 = 0.001f;
            public float K3 = 0.0002f;
            public int K3p = 100;
            public float K4 = .025f;
            public float K5 = .0025f;

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


            public class Pattern : Theme.Pattern
            {
                public Pattern(Theme theme) : base(theme) { }

                [XmlAttribute("texture")]
                public string TextureName;
                [XmlAttribute("parent")]
                public Vector2 ParentPoint;
                [XmlAttribute("begin")]
                public Vector2 BeginPoint;
                [XmlAttribute("end")]
                public Vector2 EndPoint;
                public float maxAngle;

                public float K1 = 0.001f;
                public float K3 = 0.0002f;
                public int K3p = 100;
                public float K4 = .025f;
                public float K5 = .0025f;
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


            public void Update()
            {

                var wind = Tree.Scene.WindStrength;
                var awind = Math.Abs(wind);

                if (--ticks3 <= 0)
                {
                    var f = Tree.Game.Rand(.3f, 1f);
                    ticks3 = Period3 = (int)(f * K3p * (2 - awind - Tree.Game.Rand()));
                    Amplitude3 = .01f * K3 * f;
                }

                angleSpeed +=
                    K1 * (float)Math.Sin(Angle) * wind
                  - K5 * Angle / maxAngle
                  + Amplitude3 * (float)Math.Sin(2 * (float)Math.PI * ticks3 / Period3);

                angleSpeed *= (1f - K4);
                //if (Math.Abs(angleSpeed) > 0.0001f)
                Angle += angleSpeed;

                //h.Angle = MathHelper.Clamp(h.Angle, -maxAngle, maxAngle);
                ParentAngle = Parent != null ? Parent.TotalAngle : 0;
                TotalAngle = Angle + ParentAngle;

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
                    rotation: TotalAngle
                );

                //Tree.Game.DrawString(TotalAngle, 0, 32 * Index);

                foreach (var node in Nodes)
                {
                    node.Draw(x0, y0);
                }
            }

        }

    }


}
