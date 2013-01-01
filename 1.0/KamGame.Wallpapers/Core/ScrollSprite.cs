using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using KamGame;
using Microsoft.Xna.Framework;


namespace KamGame.Wallpapers
{

    public abstract class ScrollLayer<TLayer> : Layer<TLayer>
        where TLayer : Layer
    {
        /// <summary>
        /// Единица измерения - максимальный размер экрана
        /// </summary>
        public float? Width, Left, Right;

        public float? Top, Bottom;

        /// <summary>
        /// Прозрачность
        /// </summary>
        public float? Opacity;
    }



    public abstract class ScrollSprite : LayerComponent
    {
        protected ScrollSprite(Scene scene) : base(scene) { }

        /// <summary>
        /// Единица измерения - максимальный размер экрана
        /// </summary>
        public float Width = 1;
        public float Left, Top, Right, Bottom;
        public float Opacity = 1;

        protected internal float OffsetScale = 1;
        protected internal float Offset = -1;
        protected internal float Scale;

        protected internal int WidthPx;
        protected internal int HeightPx;

        private float _totalWidth;
        protected internal float TotalWidth
        {
            get { return _totalWidth; }
            set
            {
                _totalWidth = value;
                TotalWidthPx = value * Game.LandscapeWidth;
            }
        }
        protected internal float TotalWidthPx { get; set; }


        float priorOffsetSpeed;
        protected internal Color OpacityColor;


        protected override void LoadContent()
        {
            base.LoadContent();
            OpacityColor = new Color(Color.White, Opacity);
            TotalWidth = Left + Width + Right;
        }

        public override void Update(GameTime gameTime)
        {
            if (Game.UsePageOffset)
            {

                Offset = (TotalWidthPx - Game.ScreenWidth) * Game.PageOffset;
                base.Update(gameTime);
                return;
            }

            var sw = Game.ScreenWidth / Game.LandscapeWidth;
            OffsetScale = (TotalWidth - sw) / (Scene.Width - sw);
            if (Offset < 0)
                Offset = (TotalWidthPx - Game.ScreenWidth) / 2;

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

            //if (Offset + offsetSpeed <= 0 || Offset + offsetSpeed >= TotalWidthPx - Game.ScreenWidth - 1)
            //    offsetSpeed = -offsetSpeed / 4;
            Offset += offsetSpeed;
            Offset = Math.Max(Offset, 0);
            Offset = Math.Min(Offset, TotalWidthPx - Game.ScreenWidth - 1);
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
