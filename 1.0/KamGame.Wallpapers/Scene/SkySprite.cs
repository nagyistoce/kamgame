using System;
using System.Xml.Linq;
using Microsoft.Xna.Framework;


namespace KamGame
{
    public class SkySprite : ScrollBackground
    {
        public SkySprite(Scene scene) : base(scene) { }

        public static SkySprite Load(Scene scene, XElement el)
        {
            return (SkySprite)scene.Theme.Deserialize<Pattern>(el, new SkySprite(scene));
        }

        public override void Update(GameTime gameTime)
        {
            Scale = Math.Max(BaseScale * Scene.ScreenWidth / Width, Game.ScreenHeight / BaseHeight);
            base.Update(gameTime);
        }
    }
}
