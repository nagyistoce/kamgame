using System;
using System.Xml.Linq;
using Microsoft.Xna.Framework;


namespace KamGame.Wallpaper
{

    public class Sky : ScrollBackgroundLayer<Sky>
    {
        public Sky() { }
        public Sky(Sky pattern) { Pattern = pattern; }
        public Sky(params Sky[] patterns) { Patterns = patterns; }

        public override GameComponent NewComponent(Scene scene)
        {
            return ApplyPattern(new SkySprite(scene), this);
        }
    }

    public class SkySprite : ScrollBackground
    {
        public SkySprite(Scene scene) : base(scene) {}

        public override void Update(GameTime gameTime)
        {
            Scale = Math.Max(Width * Game.LandscapeWidth / WidthPx, Game.ScreenHeight / BaseHeight);
            base.Update(gameTime);
        }
    }

}
