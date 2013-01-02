using System;
using Android.Accounts;
using Android.App;
using Android.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Microsoft.Xna.Framework.GamerServices
{
    internal class MonoGameGamerServicesHelper
    {
        private static MonoLiveGuide guide;


        public static void ShowSigninSheet()
        {
            guide.Enabled = true;
            guide.Visible = true;
            Guide.IsVisible = true;
        }

        internal static void Initialise(Game game)
        {
            if (guide == null)
            {
                guide = new MonoLiveGuide(game);
                game.Components.Add(guide);
            }
        }
    }

    internal class MonoLiveGuide : DrawableGameComponent
    {
        private Color alphaColor = new Color(128, 128, 128, 0);
        private int delay = 2;
        private TimeSpan gt = TimeSpan.Zero;
        private TimeSpan last = TimeSpan.Zero;
        private Texture2D signInProgress;
        private SpriteBatch spriteBatch;
        private byte startalpha;

        public MonoLiveGuide(Game game)
            : base(game)
        {
            Enabled = false;
            Visible = false;
            //Guide.IsVisible = false;
            DrawOrder = Int32.MaxValue;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        private Texture2D Circle(GraphicsDevice graphics, int radius)
        {
            int aDiameter = radius*2;
            var aCenter = new Vector2(radius, radius);

            var aCircle = new Texture2D(graphics, aDiameter, aDiameter, false, SurfaceFormat.Color);
            var aColors = new Color[aDiameter*aDiameter];

            for (int i = 0; i < aColors.Length; i++)
            {
                int x = (i + 1)%aDiameter;
                int y = (i + 1)/aDiameter;

                var aDistance = new Vector2(Math.Abs(aCenter.X - x), Math.Abs(aCenter.Y - y));


                if (Math.Sqrt((aDistance.X*aDistance.X) + (aDistance.Y*aDistance.Y)) > radius)
                {
                    aColors[i] = Color.Transparent;
                }
                else
                {
                    aColors[i] = Color.White;
                }
            }

            aCircle.SetData(aColors);

            return aCircle;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            signInProgress = Circle(Game.GraphicsDevice, 10);

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(); //SpriteSortMode.Immediate, BlendState.AlphaBlend);

            var center = new Vector2(Game.GraphicsDevice.Viewport.Width/2, Game.GraphicsDevice.Viewport.Height - 100);
            Vector2 loc = Vector2.Zero;
            alphaColor.A = startalpha;
            for (int i = 0; i < 12; i++)
            {
                var angle = (float) (i/12.0*Math.PI*2);
                loc = new Vector2(center.X + (float) Math.Cos(angle)*50, center.Y + (float) Math.Sin(angle)*50);
                spriteBatch.Draw(signInProgress, loc, alphaColor);
                alphaColor.A += 255/12;
                if (alphaColor.A > 255) alphaColor.A = 0;
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            if (gt == TimeSpan.Zero) gt = last = gameTime.TotalGameTime;

            if ((gameTime.TotalGameTime - last).Milliseconds > 100)
            {
                last = gameTime.TotalGameTime;
                startalpha += 255/12;
            }

            if ((gameTime.TotalGameTime - gt).TotalSeconds > delay) // close after 10 seconds
            {
                string name = "androiduser";
                try
                {
                    var mgr = (AccountManager) Application.Context.GetSystemService(Context.AccountService);
                    if (mgr != null)
                    {
                        Account[] accounts = mgr.GetAccounts();
                        if (accounts != null && accounts.Length > 0)
                        {
                            name = accounts[0].Name;
                            if (name.Contains("@"))
                            {
                                // its an email 
                                name = name.Substring(0, name.IndexOf("@"));
                            }
                        }
                    }
                }
                catch
                {
                }

                var sig = new SignedInGamer();
                sig.DisplayName = name;
                sig.Gamertag = name;
                sig.IsSignedInToLive = false;

                Gamer.SignedInGamers.Add(sig);

                Visible = false;
                Enabled = false;
                //Guide.IsVisible = false;
                gt = TimeSpan.Zero;
            }
            base.Update(gameTime);
        }
    }
}