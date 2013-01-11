using System.Collections.Generic;
using Microsoft.Xna.Framework;



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
        public bool UseFlip;

        public override object NewComponent(Scene scene)
        {
            return ApplyPattern(new TreeSprite(scene), this);
        }
    }


    public class TreeSprite : ScrollSprite
    {
        public TreeSprite(Scene scene)
            : base(scene)
        {
            Leafs = new FallenLeafsPart { Tree = this };
            Nodes = new ObservableList<TreeNodePart>();
            Nodes.ItemAdded += (source, args) => args.Item.SetTree(this);
        }

        public int BaseHeight;
        public readonly ObservableList<TreeNodePart> Nodes;
        public readonly List<TreeNodePart> FlatNodes = new List<TreeNodePart>(8);
        public readonly FallenLeafsPart Leafs;
        public bool UseFlip;

        protected internal float LeftPx, TopPx;
        protected int TotalNodeCount;

        protected override void LoadContent()
        {
            base.LoadContent();

            OpacityColor = Scene.BlackColor * Opacity;
            TotalWidth = Left + Right;
            Scale = Width * Game.LandscapeWidth / BaseHeight;
            //if (UseFlip)
            //    Leafs.EnterPoint.X = -Leafs.EnterPoint.X;

            foreach (var node in Nodes)
            {
                node.LoadContent(Game);
            }
            Leafs.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            LeftPx = Left * Game.LandscapeWidth;
            TopPx = Game.ScreenHeight - Bottom * Game.LandscapeHeight;

            Leafs.Update();

            foreach (var node in Nodes)
            {
                node.Update();
            }
        }

        public override void Draw(GameTime gameTime)
        {

            foreach (var node in Nodes)
            {
                node.Draw();
            }

            Leafs.Draw();

            base.Draw(gameTime);
        }


    }
}
