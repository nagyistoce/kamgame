using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using KamGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace FallenLeaves
{
    public class WindController : DrawableGame2DComponent
    {
        public WindController(Scene scene) : base(scene.Theme.Game) { Scene = scene; }

        public readonly Scene Scene;
        private Texture2D windBg;

        public static WindController Load(Scene scene, XElement el)
        {
            var wind = new WindController(scene)
            {
                debugMode = el.Attr("debug", false),
                maxSpeedFactor = el.Attr("maxSpeedFactor", 100),
                changeSpeedPeriod = el.Attr("changeSpeedPeriod", 500),
                minAmplitude = el.Attr("minAmplitude", 0f),
                maxAmplitude = el.Attr("maxAmplitude", 1f),
                minChangeAmplitudePeriod = el.Attr("minChangeAmplitudePeriod", 500),
                maxChangeAmplitudePeriod = el.Attr("maxChangeAmplitudePeriod", 1000),
                amplitureScatter = el.Attr("amplitureScatter", .5f),
                amplitudeStep = el.Attr("amplitudeStep", 0.005f),
            };
            return wind;
        }

        private float[] winds;

        private bool debugMode;
        private float maxSpeedFactor;
        private int changeSpeedPeriod;
        private float minAmplitude;
        private float maxAmplitude;
        private int minChangeAmplitudePeriod;
        private int maxChangeAmplitudePeriod;
        private float amplitureScatter;
        private float amplitudeStep;

        private int speedTick;
        private int amplitudeTick;
        private float minCurrentAmplitude;
        private float maxCurrentAmplitude;

        protected override void LoadContent()
        {
            base.LoadContent();
            windBg = Scene.Load<Texture2D>("windbg1");

            winds = new float[3];
            var h = 1f / maxSpeedFactor;
            for (var i = 1; i < winds.Length; i++)
            {
                winds[i] = h * (2 * Game.Rand() - 1);
                h /= maxSpeedFactor;
            }
            speedTick = 0;
            amplitudeTick = 0;
        }

        public override void Update(GameTime gameTime)
        {

            var h = 1f;
            for (int i = 0, len = winds.Length - 1; i < len; i++)
            {
                winds[i] += winds[i + 1];
                if (winds[i] < -h)
                {
                    winds[i] = -h;
                    if (winds[i + 1] < 0)
                        winds[i + 1] = -winds[i + 1] / 5;
                }
                else if (winds[i] > h)
                {
                    winds[i] = h;
                    if (winds[i + 1] > 0)
                        winds[i + 1] = -winds[i + 1] / 5;
                }
                h /= maxSpeedFactor;
            }

            var w = winds[0];
            var av = 5f;// (minCurrentAmplitude + maxCurrentAmplitude) / 2;

            if (w <= -10 * amplitudeStep || w >= 10 * amplitudeStep)
            {
                if (w > 0)
                {
                    if (w < minCurrentAmplitude)
                        ;//w += amplitudeStep * (w - minCurrentAmplitude) * av;
                    else if (w > maxCurrentAmplitude)
                        w -= amplitudeStep * (w - maxCurrentAmplitude) * av;
                }
                else
                {
                    if (w > -minCurrentAmplitude)
                        ;//w += amplitudeStep * (w + minCurrentAmplitude) * av;
                    else if (w < -maxCurrentAmplitude)
                        w -= amplitudeStep * (w + maxCurrentAmplitude) * av;
                }
            }
            //if (w > 0)
            //{
            //    if (w < minCurrentAmplitude)
            //        w = minCurrentAmplitude - w < amplitudeStep ? minCurrentAmplitude : w + amplitudeStep;
            //    else if (w > maxCurrentAmplitude)
            //        w = w - maxCurrentAmplitude < amplitudeStep ? maxCurrentAmplitude : w - amplitudeStep;
            //}
            //else
            //{
            //    if (w > -minCurrentAmplitude)
            //        w = w + minCurrentAmplitude < amplitudeStep ? -minCurrentAmplitude : w - amplitudeStep;
            //    else if (w < -maxCurrentAmplitude)
            //        w = -w - maxCurrentAmplitude < amplitudeStep ? -maxCurrentAmplitude : w + amplitudeStep;
            //}
            winds[0] = w;

            if (--speedTick <= 0)
            {
                speedTick = Game.RandInt(changeSpeedPeriod);
                winds[winds.Length - 1] = h * (2 * Game.Rand() - 1);
            }
            if (--amplitudeTick <= 0)
            {
                amplitudeTick = Game.RandInt(minChangeAmplitudePeriod, maxChangeAmplitudePeriod);
                minCurrentAmplitude = minAmplitude + (maxAmplitude - minAmplitude) * Game.Rand();
                maxCurrentAmplitude = minCurrentAmplitude + +amplitureScatter * Game.Rand();
            }

            Scene.PriorWindStrength = Scene.WindStrength;
            Scene.WindStrength = winds[0];
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            var x = Game.ScreenWidth * .5f;
            var originP = Vector2.Zero;
            var originN = new Vector2(32, 0);

            Game.Draw(windBg, x, 0,
                origin: Scene.WindStrength > 0 ? originP : originN,
                vscale: new Vector2(Game.ScreenWidth / 32 / 2 * Math.Abs(Scene.WindStrength), .5f),
                color: new Color(Color.White, .8f)
            );

            if (debugMode)
            {
                var h = maxSpeedFactor;
                Game.DrawString(winds[0].ToString(), x, 0);
                for (var i = 1; i < winds.Length; i++)
                {
                    Game.Draw(windBg, x, 16 * i,
                        origin: winds[i] > 0 ? originP : originN,
                        vscale: new Vector2(Game.ScreenWidth / 32 / 2 * h * Math.Abs(winds[i]), .5f),
                        color: new Color(Color.White, .8f)
                    );
                    //Game.DrawString(winds[i].ToString(), x, 16 * i);
                    h *= maxSpeedFactor;
                }

                Game.DrawString("Amplitude = " + minCurrentAmplitude + " .. " + maxCurrentAmplitude, x, 64);
            }

            base.Draw(gameTime);
        }
    }
}
