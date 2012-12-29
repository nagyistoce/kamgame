using System;
using System.Text;

namespace Microsoft.Xna.Framework.Graphics
{
    public class SpriteBatch : GraphicsResource
    {
        private readonly SpriteBatcher _batcher;
        private bool _beginCalled;

        private BlendState _blendState;
        private DepthStencilState _depthStencilState;
        private Effect _effect;

        private Matrix _matrix;
        private RasterizerState _rasterizerState;
        private SamplerState _samplerState;
        private SpriteSortMode _sortMode;
        private Effect _spriteEffect;
        private Rectangle _tempRect = new Rectangle(0, 0, 0, 0);
        private Vector2 _texCoordBR = new Vector2(0, 0);
        private Vector2 _texCoordTL = new Vector2(0, 0);

        public SpriteBatch(GraphicsDevice graphicsDevice)
        {
            if (graphicsDevice == null)
            {
                throw new ArgumentException("graphicsDevice");
            }

            GraphicsDevice = graphicsDevice;

            // Use a custom SpriteEffect so we can control the transformation matrix
            _spriteEffect = new Effect(graphicsDevice, SpriteEffect.Bytecode);

            _batcher = new SpriteBatcher(graphicsDevice);

            _beginCalled = false;
        }

        public void Begin()
        {
            Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None,
                  RasterizerState.CullCounterClockwise, null, Matrix.Identity);
        }

        public void Begin(SpriteSortMode sortMode, BlendState blendState, SamplerState samplerState,
                          DepthStencilState depthStencilState, RasterizerState rasterizerState, Effect effect,
                          Matrix transformMatrix)
        {
            if (_beginCalled)
                throw new InvalidOperationException(
                    "Begin cannot be called again until End has been successfully called.");

            // defaults
            _sortMode = sortMode;
            _blendState = blendState ?? BlendState.AlphaBlend;
            _samplerState = samplerState ?? SamplerState.LinearClamp;
            _depthStencilState = depthStencilState ?? DepthStencilState.None;
            _rasterizerState = rasterizerState ?? RasterizerState.CullCounterClockwise;

            _effect = effect;

            _matrix = transformMatrix;

            // Setup things now so a user can chage them.
            if (sortMode == SpriteSortMode.Immediate)
                Setup();

            _beginCalled = true;
        }

        public void Begin(SpriteSortMode sortMode, BlendState blendState)
        {
            Begin(sortMode, blendState, SamplerState.LinearClamp, DepthStencilState.None,
                  RasterizerState.CullCounterClockwise, null, Matrix.Identity);
        }

        public void Begin(SpriteSortMode sortMode, BlendState blendState, SamplerState samplerState,
                          DepthStencilState depthStencilState, RasterizerState rasterizerState)
        {
            Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, null, Matrix.Identity);
        }

        public void Begin(SpriteSortMode sortMode, BlendState blendState, SamplerState samplerState,
                          DepthStencilState depthStencilState, RasterizerState rasterizerState, Effect effect)
        {
            Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, Matrix.Identity);
        }

        public void End()
        {
            _beginCalled = false;

            if (_sortMode != SpriteSortMode.Immediate)
                Setup();

            _batcher.DrawBatch(_sortMode);
        }

        private void Setup()
        {
            GraphicsDevice gd = GraphicsDevice;
            gd.BlendState = _blendState;
            gd.DepthStencilState = _depthStencilState;
            gd.RasterizerState = _rasterizerState;
            gd.SamplerStates[0] = _samplerState;

            // Setup the default sprite effect.
            Viewport vp = gd.Viewport;

            // GL requires a half pixel offset where as DirectX and PSS does not.
#if PSM || DIRECTX
            var projection = Matrix.CreateOrthographicOffCenter(0, vp.Width, vp.Height, 0, -1, 0);
            var transform = _matrix * projection;
#else
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, vp.Width, vp.Height, 0, 0, 1);
            Matrix halfPixelOffset = Matrix.CreateTranslation(-0.5f, -0.5f, 0);
            Matrix transform = _matrix * (halfPixelOffset * projection);
#endif

            _spriteEffect.Parameters["MatrixTransform"].SetValue(transform);
            _spriteEffect.CurrentTechnique.Passes[0].Apply();

            // If the user supplied a custom effect then apply
            // it now to override the sprite effect.
            if (_effect != null)
                _effect.CurrentTechnique.Passes[0].Apply();
        }

        private void CheckValid(Texture2D texture)
        {
            if (texture == null)
                throw new ArgumentNullException("texture");
            if (!_beginCalled)
                throw new InvalidOperationException(
                    "Draw was called, but Begin has not yet been called. Begin must be called successfully before you can call Draw.");
        }

        private void CheckValid(SpriteFont spriteFont, string text)
        {
            if (spriteFont == null)
                throw new ArgumentNullException("spriteFont");
            if (text == null)
                throw new ArgumentNullException("text");
            if (!_beginCalled)
                throw new InvalidOperationException(
                    "DrawString was called, but Begin has not yet been called. Begin must be called successfully before you can call DrawString.");
        }

        private void CheckValid(SpriteFont spriteFont, StringBuilder text)
        {
            if (spriteFont == null)
                throw new ArgumentNullException("spriteFont");
            if (text == null)
                throw new ArgumentNullException("text");
            if (!_beginCalled)
                throw new InvalidOperationException(
                    "DrawString was called, but Begin has not yet been called. Begin must be called successfully before you can call DrawString.");
        }

        public void Draw(Texture2D texture,
                         Vector2 position,
                         Rectangle? sourceRectangle,
                         Color color,
                         float rotation,
                         Vector2 origin,
                         Vector2 scale,
                         SpriteEffects effect,
                         float depth)
        {
            CheckValid(texture);

            float w = texture.Width * scale.X;
            float h = texture.Height * scale.Y;
            if (sourceRectangle.HasValue)
            {
                w = sourceRectangle.Value.Width * scale.X;
                h = sourceRectangle.Value.Height * scale.Y;
            }

            DrawInternal(texture,
                         new Vector4(position.X, position.Y, w, h),
                         sourceRectangle,
                         color,
                         rotation,
                         origin * scale,
                         effect,
                         depth);
        }

        public void Draw(Texture2D texture,
                         Vector2 position,
                         Rectangle? sourceRectangle,
                         Color color,
                         float rotation,
                         Vector2 origin,
                         float scale,
                         SpriteEffects effect,
                         float depth)
        {
            CheckValid(texture);

            float w = texture.Width * scale;
            float h = texture.Height * scale;
            if (sourceRectangle.HasValue)
            {
                w = sourceRectangle.Value.Width * scale;
                h = sourceRectangle.Value.Height * scale;
            }

            DrawInternal(texture,
                         new Vector4(position.X, position.Y, w, h),
                         sourceRectangle,
                         color,
                         rotation,
                         origin * scale,
                         effect,
                         depth);
        }

        public void Draw(Texture2D texture,
                         Rectangle destinationRectangle,
                         Rectangle? sourceRectangle,
                         Color color,
                         float rotation,
                         Vector2 origin,
                         SpriteEffects effect,
                         float depth)
        {
            CheckValid(texture);

            DrawInternal(texture,
                         new Vector4(destinationRectangle.X,
                                     destinationRectangle.Y,
                                     destinationRectangle.Width,
                                     destinationRectangle.Height),
                         sourceRectangle,
                         color,
                         rotation,
                         new Vector2(origin.X * (destinationRectangle.Width / (float)texture.Width),
                                     origin.Y * (destinationRectangle.Height / (float)texture.Height)),
                         effect,
                         depth);
        }

        internal void DrawInternal(Texture2D texture,
                                   Vector4 destinationRectangle,
                                   Rectangle? sourceRectangle,
                                   Color color,
                                   float rotation,
                                   Vector2 origin,
                                   SpriteEffects effect,
                                   float depth)
        {
            SpriteBatchItem item = _batcher.CreateBatchItem();

            item.Depth = depth;
            item.Texture = texture;

            if (sourceRectangle.HasValue)
            {
                _tempRect = sourceRectangle.Value;
            }
            else
            {
                _tempRect.X = 0;
                _tempRect.Y = 0;
                _tempRect.Width = texture.Width;
                _tempRect.Height = texture.Height;
            }

            _texCoordTL.X = _tempRect.X / (float)texture.Width;
            _texCoordTL.Y = _tempRect.Y / (float)texture.Height;
            _texCoordBR.X = (_tempRect.X + _tempRect.Width) / (float)texture.Width;
            _texCoordBR.Y = (_tempRect.Y + _tempRect.Height) / (float)texture.Height;

            if ((effect & SpriteEffects.FlipVertically) != 0)
            {
                float temp = _texCoordBR.Y;
                _texCoordBR.Y = _texCoordTL.Y;
                _texCoordTL.Y = temp;
            }
            if ((effect & SpriteEffects.FlipHorizontally) != 0)
            {
                float temp = _texCoordBR.X;
                _texCoordBR.X = _texCoordTL.X;
                _texCoordTL.X = temp;
            }

            item.Set(destinationRectangle.X,
                     destinationRectangle.Y,
                     -origin.X,
                     -origin.Y,
                     destinationRectangle.Z,
                     destinationRectangle.W,
                     (float)Math.Sin(rotation),
                     (float)Math.Cos(rotation),
                     color,
                     _texCoordTL,
                     _texCoordBR);

            if (_sortMode == SpriteSortMode.Immediate)
                _batcher.DrawBatch(_sortMode);
        }

        public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color)
        {
            Draw(texture, position, sourceRectangle, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

        public void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color)
        {
            Draw(texture, destinationRectangle, sourceRectangle, color, 0, Vector2.Zero, SpriteEffects.None, 0f);
        }

        public void Draw(Texture2D texture, Vector2 position, Color color)
        {
            Draw(texture, position, null, color);
        }

        public void Draw(Texture2D texture, Rectangle rectangle, Color color)
        {
            Draw(texture, rectangle, null, color);
        }

        public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color)
        {
            CheckValid(spriteFont, text);

            var source = new SpriteFont.CharacterSource(text);
            spriteFont.DrawInto(
                this, ref source, position, color, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }

        public void DrawString(
            SpriteFont spriteFont, string text, Vector2 position, Color color,
            float rotation, Vector2 origin, float scale, SpriteEffects effects, float depth)
        {
            CheckValid(spriteFont, text);

            var scaleVec = new Vector2(scale, scale);
            var source = new SpriteFont.CharacterSource(text);
            spriteFont.DrawInto(this, ref source, position, color, rotation, origin, scaleVec, effects, depth);
        }

        public void DrawString(
            SpriteFont spriteFont, string text, Vector2 position, Color color,
            float rotation, Vector2 origin, Vector2 scale, SpriteEffects effect, float depth)
        {
            CheckValid(spriteFont, text);

            var source = new SpriteFont.CharacterSource(text);
            spriteFont.DrawInto(this, ref source, position, color, rotation, origin, scale, effect, depth);
        }

        public void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color)
        {
            CheckValid(spriteFont, text);

            var source = new SpriteFont.CharacterSource(text);
            spriteFont.DrawInto(this, ref source, position, color, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
        }

        public void DrawString(
            SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color,
            float rotation, Vector2 origin, float scale, SpriteEffects effects, float depth)
        {
            CheckValid(spriteFont, text);

            var scaleVec = new Vector2(scale, scale);
            var source = new SpriteFont.CharacterSource(text);
            spriteFont.DrawInto(this, ref source, position, color, rotation, origin, scaleVec, effects, depth);
        }

        public void DrawString(
            SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color,
            float rotation, Vector2 origin, Vector2 scale, SpriteEffects effect, float depth)
        {
            CheckValid(spriteFont, text);

            var source = new SpriteFont.CharacterSource(text);
            spriteFont.DrawInto(this, ref source, position, color, rotation, origin, scale, effect, depth);
        }

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    if (_spriteEffect != null)
                    {
                        _spriteEffect.Dispose();
                        _spriteEffect = null;
                    }
                }
            }
            base.Dispose(disposing);
        }
    }
}