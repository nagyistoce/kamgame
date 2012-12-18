using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;


namespace KamGame
{
    public class Game2D : GameBase
    {
        public SpriteBatch SpriteBatch { get; private set; }
        public SpriteFont DefaultFont;

        protected override void BeforeLoadContent()
        {
            base.BeforeLoadContent();
            SpriteBatch = new SpriteBatch(GraphicsDevice);
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
            DrawString(text, new Vector2(x, y), Color.Gray);
        }

        public void DrawString(string text)
        {
            DrawString(text, Vector2.Zero, Color.Gray);
        }

        public void DrawString(float value, float x, float y, Color color)
        {
            DrawString(value.ToStringInvariant(), new Vector2(x, y), color);
        }

        public void DrawString(float value, float x, float y)
        {
            DrawString(value.ToStringInvariant(), new Vector2(x, y), Color.Gray);
        }

        public void DrawString(float value)
        {
            DrawString(value.ToStringInvariant(), Vector2.Zero, Color.Gray);
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
