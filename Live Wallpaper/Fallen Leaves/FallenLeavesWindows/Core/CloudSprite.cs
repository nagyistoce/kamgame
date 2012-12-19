using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using KamGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace FallenLeaves
{
    public class CloudSprite : ScrollSprite
    {
        public CloudSprite(Scene scene) : base(scene) { }

        public Texture2D[] Textures;
        public List<Cloud> Clouds = new List<Cloud>();

        [XmlAttribute("textures")]
        public string TextureNames;
        [XmlAttribute("baseHeight")]
        public int BaseHeight = 256;
        public int densty = 3;
        public float minScale = .5f;
        public float maxScale = 1.5f;
        public int minGroupCount = 1;
        public int maxGroupCount = 3;


        public new class Pattern : ScrollSprite.Pattern
        {
            [XmlAttribute("textures")]
            public string TextureNames;
            [XmlAttribute("baseHeight")]
            public int BaseHeight = 256;
            public int densty = 3;
            public float minScale = .5f;
            public float maxScale = 1.5f;
            public int minGroupCount = 1;
            public int maxGroupCount = 3;
        }


        public static CloudSprite Load(Scene scene, XElement el)
        {
            return (CloudSprite)scene.Theme.Deserialize<Pattern>(el, new CloudSprite(scene));
        }

        protected override void LoadContent()
        {
            if (Textures == null)
                LoadTextures();

            Scale = BaseScale * Game.ScreenWidth / BaseHeight;

            for (int i = 0, len = (int)(BaseScale * densty); i < len; i++)
            {

            }

            base.LoadContent();
        }

        private void LoadTextures()
        {
            var textureNames = (TextureNames ?? "")
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(a => a.Trim()).ToArray();

            Textures = new Texture2D[textureNames.Length];
            var i = 0;
            foreach (var textureName in textureNames)
            {
                Textures[i++] = Scene.Load<Texture2D>(textureName);
            }
        }

        public override void Update(GameTime gameTime)
        {
            ScaleWidth = BaseScale;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            var i = 0;
            foreach (var c in Clouds)
            {
                Game.Draw(c.Texture, c.X - Offset, c.Y, scale: Scale, effect: c.Effects);
            }

            base.Draw(gameTime);
        }


        public class Cloud
        {
            public Texture2D Texture;
            public float X, Y;
            public SpriteEffects Effects;
        }


    }
}
