using System;
using System.Xml.Linq;
using Microsoft.Xna.Framework;


namespace FallenLeaves
{
    public class CloudSprite : ScrollBackground
    {
        public CloudSprite(Scene scene) : base(scene) { }

        public static CloudSprite Load(Scene scene, XElement el)
        {
            return (CloudSprite)scene.Theme.Deserialize<Pattern>(el, new CloudSprite(scene));
        }

        public override void Update(GameTime gameTime)
        {
            Scale = Game.ScreenWidth > Game.ScreenHeight
                ? BaseScale * Game.ScreenWidth / Width
                : BaseScale * Game.ScreenWidth / Height;
            base.Update(gameTime);
        }
    }
}
