#region Using Statements

using KamGame;
using KamGame.Wallpapers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

#endregion


namespace FallenLeaves
{

    public partial class FallenLeavesGame : Game2D
    {
        public FallenLeavesGame()
        {
            Graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft;
        }

        protected override void DoLoadContent()
        {
            DefaultFont = Content.Load<SpriteFont>("spriteFont1");
            base.DoLoadContent();
        }
        
        protected override void DoUpdate()
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            base.DoUpdate();
        }

        protected override void DoDraw()
        {
            GraphicsDevice.Clear(Color.Green);
            DrawString("WWWWWWWWWWWWWWWWWWW", 100, 100, color: Color.Red);
            base.DoDraw();
        }
    }


}