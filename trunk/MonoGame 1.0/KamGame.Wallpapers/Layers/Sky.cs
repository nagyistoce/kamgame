using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;


namespace KamGame.Wallpapers
{

    public class Sky : ScrollBackgroundLayer<Sky>
    {
        public Sky() { }
        public Sky(Sky pattern) { Pattern = pattern; }
        public Sky(params Sky[] patterns) { Patterns = patterns; }

        public Color BlackColor, CloudColor;
        public List<Clouds> Clouds = new List<Clouds>();

        public override object NewComponent(Scene scene)
        {
            return ApplyPattern(new SkySprite(scene), this);
        }
    }

    public class SkySprite : ScrollBackground
    {

        public Color BlackColor, CloudColor;

        public SkySprite(Scene scene) : base(scene)
        {
            Align = SpriteAlign.Bottom;
        }

        public override void Update(GameTime gameTime)
        {
            Scale = Width * Game.LandscapeWidth / WidthPx;
            //Scale = Math.Max(TotalWidth * Game.LandscapeWidth / WidthPx, Game.ScreenHeight / BaseHeight);
            base.Update(gameTime);
        }

    }

}
