using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using KamGame;
using Microsoft.Xna.Framework;


namespace FallenLeaves
{
    public class ScrollSprite : DrawableGame2DComponent
    {
        public Scene Scene;
        public string ID;
        public string PatternID;

        [XmlAttribute("width")]
        public float BaseScale = 1;
        [XmlAttribute("left")]
        public float MarginLeft;
        [XmlAttribute("top")]
        public float MarginTop;
        [XmlAttribute("right")]
        public float MarginRight;
        [XmlAttribute("bottom")]
        public float MarginBottom;
        public float opacity = 1;

        public float OffsetScale = 1;
        public float Offset = -1;
        public float Scale;
        public float ScaleWidth;
        public int Width;
        public int Height;

        public class Pattern : Theme.Pattern
        {
            [XmlAttribute("width")]
            public float BaseScale = 1;
            [XmlAttribute("left")]
            public float MarginLeft;
            [XmlAttribute("top")]
            public float MarginTop;
            [XmlAttribute("right")]
            public float MarginRight;
            [XmlAttribute("bottom")]
            public float MarginBottom;
            public float opacity = 1;
        }


        public ScrollSprite(Scene scene) : base(scene.Theme.Game) { Scene = scene; }

        float priorOffsetSpeed;
        protected Color OpacityColor;

        protected override void LoadContent()
        {
            base.LoadContent();
            OpacityColor = new Color(Color.White, opacity);
        }

        public override void Update(GameTime gameTime)
        {
            OffsetScale = (ScaleWidth - 1) / (Scene.ScaleWidth - 1f);
            if (Offset < 0)
                Offset = (ScaleWidth - 1) * Scene.ScreenWidth / 2;

            var offsetSpeed = 0f;
            if (Game.CursorIsDraged)
            {
                offsetSpeed = -Game.CursorOffset.X * OffsetScale;
            }
            else if (Math.Abs(priorOffsetSpeed) > 1)
            {
                offsetSpeed = priorOffsetSpeed * Scene.DragSlowing;
                if (Math.Abs(offsetSpeed) < 1)
                    offsetSpeed = 0;
            }

            Offset += offsetSpeed;
            Offset = Math.Max(Offset, 0);
            Offset = Math.Min(Offset, ScaleWidth * Scene.ScreenWidth - Game.ScreenWidth);
            priorOffsetSpeed = offsetSpeed;

            base.Update(gameTime);
        }

        //public override void Draw(GameTime gameTime)
        //{
        //    //Game.DrawString(
        //    //    "Screen = " + Game.ScreenWidth + " x " + +Game.ScreenHeight + "\n" +
        //    //    "Window = " + Game.Window.ClientBounds.ScreenWidth + " x " + Game.Window.ClientBounds.ScreenHeight + "\n" +
        //    //    "Buffer = " + Game.Graphics.PreferredBackBufferWidth + " x " + Game.Graphics.PreferredBackBufferHeight,
        //    //    Game.ScreenWidth / 2 - 120, 600
        //    //    //" Offset = " + Offset + " (" + Kind + ")", 0, 33 * (int)Kind
        //    //);
        //    //Game.DrawString("qqq", -20, 50);

        //    base.Draw(gameTime);
        //}

    }


}
