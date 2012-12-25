using System;
using System.Xml.Linq;
using Microsoft.Xna.Framework;


namespace KamGame.Wallpaper
{

    public class Sky : ScrollBackgroundLayer<Sky>
    {
        public override GameComponent NewComponent(Scene scene)
        {
            return new SkySprite(scene, this);
        }
    }

    public class SkySprite : ScrollBackground<Sky>
    {
        public SkySprite(Scene scene, Sky layer) : base(scene, layer) {}

        public override void Update(GameTime gameTime)
        {
            Scale = Math.Max(BaseScale * Game.LandscapeWidth / Width, Game.ScreenHeight / BaseHeight);
            base.Update(gameTime);
        }
    }
}
