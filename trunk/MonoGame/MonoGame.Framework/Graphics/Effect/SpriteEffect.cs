#region File Description

//-----------------------------------------------------------------------------
// SpriteEffect.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

#endregion

#region Using Statements

#if ANDROID || IOS
using ActiveUniformType = OpenTK.Graphics.ES20.All;

#elif MONOMAC
using MonoMac.OpenGL;
#elif PSM
using Sce.PlayStation.Core.Graphics;
#elif !WINRT
using OpenTK.Graphics.OpenGL;
#endif

#endregion

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    ///     The default effect used by SpriteBatch.
    /// </summary>
    public class SpriteEffect : Effect
    {
        #region Effect Parameters

        private EffectParameter matrixParam;

        #endregion

        internal static readonly byte[] Bytecode = LoadEffectResource(
#if DIRECTX
            "Microsoft.Xna.Framework.Graphics.Effect.Resources.SpriteEffect.dx11.mgfxo"
#elif PSM
            "MonoGame.Framework.PSMobile.PSSuite.Graphics.Resources.SpriteEffect.cgx" //FIXME: This shader is totally incomplete
#else
            "Microsoft.Xna.Framework.Graphics.Effect.Resources.SpriteEffect.ogl.mgfxo"
#endif
            );

        #region Methods

        /// <summary>
        ///     Creates a new SpriteEffect.
        /// </summary>
        public SpriteEffect(GraphicsDevice device)
            : base(device, Bytecode)
        {
            CacheEffectParameters();
        }

        /// <summary>
        ///     Creates a new SpriteEffect by cloning parameter settings from an existing instance.
        /// </summary>
        protected SpriteEffect(SpriteEffect cloneSource)
            : base(cloneSource)
        {
            CacheEffectParameters();
        }


        /// <summary>
        ///     Creates a clone of the current SpriteEffect instance.
        /// </summary>
        public override Effect Clone()
        {
            return new SpriteEffect(this);
        }


        /// <summary>
        ///     Looks up shortcut references to our effect parameters.
        /// </summary>
        private void CacheEffectParameters()
        {
            matrixParam = Parameters["MatrixTransform"];
        }

        /// <summary>
        ///     Lazily computes derived parameter values immediately before applying the effect.
        /// </summary>
        protected internal override bool OnApply()
        {
            Viewport viewport = GraphicsDevice.Viewport;

            Matrix projection = Matrix.CreateOrthographicOffCenter(0, viewport.Width, viewport.Height, 0, 0, 1);
            Matrix halfPixelOffset = Matrix.CreateTranslation(-0.5f, -0.5f, 0);

            matrixParam.SetValue(halfPixelOffset*projection);

            return false;
        }

        #endregion
    }
}