using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace KamGame
{
    public class Game2D : GameBase
    {
        public SpriteBatch SpriteBatch { get; private set; }
        public SpriteFont DefaultFont;
        public Texture2D OneTexture;

        protected override void BeforeLoadContent()
        {
            base.BeforeLoadContent();
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            OneTexture = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            OneTexture.SetData(new[] { 0xFFFFFF }, 0, 1);
        }

        protected override void BeforeDraw()
        {
            base.BeforeDraw();
            SpriteBatch.Begin();
        }
        protected override void AfterDraw()
        {
            SpriteBatch.End();
            base.AfterDraw();
        }


        #region Draw Utilites

        #region Standart Sprite Draw

        //public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effect, float depth)
        //{
        //    SpriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, effect, depth);
        //}

        //public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effect, float depth)
        //{
        //    SpriteBatch.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, effect, depth);
        //}

        public void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effect, float depth)
        {
            SpriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color, rotation, origin, effect, depth);
        }

        public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color)
        {
            SpriteBatch.Draw(texture, position, sourceRectangle, color);
        }

        public void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color)
        {
            SpriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color);
        }

        public void Draw(Texture2D texture, Vector2 position, Color color)
        {
            SpriteBatch.Draw(texture, position, color);
        }

        public void Draw(Texture2D texture, Rectangle rectangle, Color color)
        {
            SpriteBatch.Draw(texture, rectangle, color);
        }


        public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color)
        {
            SpriteBatch.DrawString(spriteFont, text, position, color);
        }

        public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float depth)
        {
            SpriteBatch.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, depth);
        }

        public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effect, float depth)
        {
            SpriteBatch.DrawString(spriteFont, text, position, color, rotation, origin, scale, effect, depth);
        }

        public void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color)
        {
            SpriteBatch.DrawString(spriteFont, text, position, color);
        }

        public void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float depth)
        {
            SpriteBatch.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, depth);
        }

        public void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effect, float depth)
        {
            SpriteBatch.DrawString(spriteFont, text, position, color, rotation, origin, scale, effect, depth);
        }


        public void DrawString(string text, Vector2 position, Color color)
        {
            if (DefaultFont != null)
                SpriteBatch.DrawString(DefaultFont, text, position, color);
        }

        public void DrawString(string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float depth)
        {
            if (DefaultFont != null)
                SpriteBatch.DrawString(DefaultFont, text, position, color, rotation, origin, scale, effects, depth);
        }

        public void DrawString(string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effect, float depth)
        {
            if (DefaultFont != null)
                SpriteBatch.DrawString(DefaultFont, text, position, color, rotation, origin, scale, effect, depth);
        }

        public void DrawString(StringBuilder text, Vector2 position, Color color)
        {
            if (DefaultFont != null)
                SpriteBatch.DrawString(DefaultFont, text, position, color);
        }

        public void DrawString(StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float depth)
        {
            if (DefaultFont != null)
                SpriteBatch.DrawString(DefaultFont, text, position, color, rotation, origin, scale, effects, depth);
        }

        public void DrawString(StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effect, float depth)
        {
            if (DefaultFont != null)
                SpriteBatch.DrawString(DefaultFont, text, position, color, rotation, origin, scale, effect, depth);
        }


        #endregion


        #region Draw

        public void Draw(
            Texture2D texture,
            float x, float y,
            Rectangle? sourceRectangle = new Rectangle?(),
            Color color = default(Color),
            float rotation = 0,
            Vector2 origin = default(Vector2),
            Vector2 vscale = default(Vector2),
            float scale = 1,
            SpriteEffects effect = SpriteEffects.None,
            float depth = 0
            )
        {
            if (vscale == default(Vector2))
                vscale = new Vector2(scale, scale);
            SpriteBatch.Draw(
                texture, new Vector2(x, y), sourceRectangle,
                color == default(Color) ? Color.White : color,
                rotation, origin, vscale, effect, depth
                );
        }

        public void Draw(
            Texture2D texture,
            Vector2 position,
            Rectangle? sourceRectangle = new Rectangle?(),
            float rotation = 0,
            Vector2 origin = default(Vector2),
            Vector2 vscale = default(Vector2),
            float scale = 1,
            SpriteEffects effect = SpriteEffects.None,
            float depth = 0,
            Color color = default(Color)
            )
        {
            if (vscale == default(Vector2))
                vscale = new Vector2(scale, scale);
            SpriteBatch.Draw(
                texture, position, sourceRectangle,
                color == default(Color) ? Color.White : color,
                rotation, origin, vscale, effect, depth
                );
        }

        #endregion


        #region DrawString

        public void DrawString(SpriteFont spriteFont, string text, float x, float y, Color color)
        {
            DrawString(spriteFont, text, new Vector2(x, y), color);
        }

        public void DrawString(SpriteFont spriteFont, string text, float x, float y)
        {
            DrawString(spriteFont, text, new Vector2(x, y), Color.Gray);
        }

        public void DrawString(SpriteFont spriteFont, string text)
        {
            DrawString(spriteFont, text, Vector2.Zero, Color.Gray);
        }

        public void DrawString(string text, float x, float y, Color color)
        {
            DrawString(text, new Vector2(x, y), color);
        }

        public void DrawString(string text, float x, float y)
        {
            DrawString(text, x, y, Color.Gray);
        }

        public void DrawString(string text)
        {
            DrawString(text, 0, 0);
        }

        public void DrawString(float value, float x, float y, Color color)
        {
            DrawString(value.ToStringInvariant(), new Vector2(x, y), color);
        }

        public void DrawString(float value, float x, float y)
        {
            DrawString(value, x, y, Color.Gray);
        }

        public void DrawString(float value)
        {
            DrawString(value, 0, 0);
        }

        public void DrawString(object value, float x, float y, Color color)
        {
            if (value != null)
                DrawString(value.ToString(), new Vector2(x, y), color);
        }

        public void DrawString(object value, float x, float y)
        {
            DrawString(value, x, y, Color.Gray);
        }

        public void DrawString(object value)
        {
            DrawString(value, 0, 0);
        }

        #endregion


        public void DrawFrame(int x0, int y0, int x1, int y1, Color color = default(Color), int thickness = 1)
        {
            var w = x1 - x0;
            var h = y1 - y0;
            Draw(OneTexture, new Rectangle(x0, y0, w, thickness), color);
            Draw(OneTexture, new Rectangle(x0, y0, thickness, h), color);
            Draw(OneTexture, new Rectangle(x0, y1 - thickness, w, thickness), color);
            Draw(OneTexture, new Rectangle(x1 - thickness, y0, thickness, h), color);
        }

        public void DrawFrame(Rectangle r, Color color = default(Color), int thickness = 1)
        {
            Draw(OneTexture, new Rectangle(r.Left, r.Top, r.Width, thickness), color);
            Draw(OneTexture, new Rectangle(r.Left, r.Top, thickness, r.Height), color);
            Draw(OneTexture, new Rectangle(r.Left, r.Bottom - thickness, r.Width, thickness), color);
            Draw(OneTexture, new Rectangle(r.Right - thickness, r.Top, thickness, r.Height), color);
        }

        #endregion
    }


    public class DrawableGame2DComponent : DrawableGameComponent
    {
        public new Game2D Game { get; private set; }

        public DrawableGame2DComponent(Game2D game)
            : base(game)
        {
            Game = game;
        }
    }

}
