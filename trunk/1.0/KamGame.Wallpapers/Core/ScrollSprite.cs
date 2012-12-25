using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using KamGame;
using Microsoft.Xna.Framework;


namespace KamGame.Wallpaper
{

    public abstract class ScrollLayer<TLayer> : Layer<TLayer>
        where TLayer: Layer
    {
        public float BaseScale = 1;
        public float MarginLeft;
        public float MarginTop;
        public float MarginRight;
        public float MarginBottom;
        public float Opacity = 1;
    }



    public abstract class ScrollSprite<TLayer> : LayerComponent<TLayer>
        where TLayer : Layer
    {
        protected ScrollSprite(Scene scene, TLayer layer) : base(scene, layer) { }

        public float BaseScale = 1;
        public float MarginLeft;
        public float MarginTop;
        public float MarginRight;
        public float MarginBottom;
        public float Opacity = 1;

        protected internal float OffsetScale = 1;
        protected internal float Offset = -1;
        protected internal float Scale;
        protected internal float ScaleWidth;
        protected internal int Width;
        protected internal int Height;


        float priorOffsetSpeed;
        protected internal Color OpacityColor;


        protected override void LoadContent()
        {
            base.LoadContent();
            OpacityColor = new Color(Color.White, Opacity);
        }

        public override void Update(GameTime gameTime)
        {
            OffsetScale = (ScaleWidth - 1) / (Scene.ScaleWidth - 1f);
            if (Offset < 0)
                Offset = (ScaleWidth - 1) * Game.LandscapeWidth / 2;

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
            Offset = Math.Min(Offset, ScaleWidth * Game.LandscapeWidth - Game.ScreenWidth);
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
