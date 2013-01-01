﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KamGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace KamGame.Wallpapers
{

    public class Wind : Layer<Wind>
    {
        public Wind() { }
        public Wind(Wind pattern) { Pattern = pattern; }
        public Wind(params Wind[] patterns) { Patterns = patterns; }


        public bool DebugMode;

        /// <summary>
        /// соотношение между максимальной силой ветра, его максимальной скоростью и максимальным ускорением. Т.е. чем больше, тем медленее меняется скорость
        /// </summary>
        public float MaxSpeedFactor;

        /// <summary>
        /// для ветра случайным образом меняется ускорение в течении периода этого периода. Чем больше, тем реже будет менятся ускорение ветра.
        /// </summary>
        public int ChangeSpeedPeriod;

        /// <summary>
        /// для ветра устанавливаются предельные стабильные значения его амплитуды (от 0 до 1)
        /// </summary>
        public float MinAmplitude, MaxAmplitude;

        /// <summary>
        /// предел случайного разброса стабильной амплитуды ветра при каждом его изменении
        /// </summary>
        public float AmplitureScatter;

        /// <summary>
        /// пределы того, как часто меняются пределы стабильной амплитуды ветра.
        /// </summary>
        public int MinChangeAmplitudePeriod, MaxChangeAmplitudePeriod;

        /// <summary>
        /// как быстро скорость ветра будет стремиться к стабильному значению
        /// </summary>
        public float AmplitudeStep;


        public override object NewComponent(Scene scene)
        {
            return ApplyPattern(new WindComponent(scene), this);
        }
    }


    public class WindComponent : LayerComponent
    {
        public WindComponent(Scene scene) : base(scene) { }

        public bool DebugMode;
        public float MaxSpeedFactor;
        public int ChangeSpeedPeriod;
        public float MinAmplitude;
        public float MaxAmplitude;
        public int MinChangeAmplitudePeriod;
        public int MaxChangeAmplitudePeriod;
        public float AmplitureScatter;
        public float AmplitudeStep;

        private Texture2D windBg;
        private float[] winds { get; set; }
        private int speedTick { get; set; }
        private int amplitudeTick { get; set; }
        private float minCurrentAmplitude { get; set; }
        private float maxCurrentAmplitude { get; set; }

        protected override void LoadContent()
        {
            base.LoadContent();
            windBg = Load<Texture2D>("windbg1");

            MaxSpeedFactor *= Game.GameSpeedScale;
            ChangeSpeedPeriod = (int)(Game.GameTimeScale * ChangeSpeedPeriod);
            MinChangeAmplitudePeriod = (int)(Game.GameTimeScale * MinChangeAmplitudePeriod);
            MaxChangeAmplitudePeriod = (int)(Game.GameTimeScale * MaxChangeAmplitudePeriod);
            AmplitudeStep *= Game.GameSpeedScale;

            winds = new float[3];
            var h = 1f / MaxSpeedFactor;
            for (var i = 1; i < winds.Length; i++)
            {
                winds[i] = h * (2 * Game.Rand() - 1);
                h /= MaxSpeedFactor;
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
                h /= MaxSpeedFactor;
            }

            var w = winds[0];
            var av = 5f;// (minCurrentAmplitude + maxCurrentAmplitude) / 2;

            if (w <= -10 * AmplitudeStep || w >= 10 * AmplitudeStep)
            {
                if (w > 0)
                {
                    if (w < minCurrentAmplitude)
                        w -= AmplitudeStep * (w - minCurrentAmplitude) * av;
                    else if (w > maxCurrentAmplitude)
                        w -= AmplitudeStep * (w - maxCurrentAmplitude) * av;
                }
                else
                {
                    if (w > -minCurrentAmplitude)
                        w -= AmplitudeStep * (w + minCurrentAmplitude) * av;
                    else if (w < -maxCurrentAmplitude)
                        w -= AmplitudeStep * (w + maxCurrentAmplitude) * av;
                }
            }
            //if (w > 0)
            //{
            //    if (w < minCurrentAmplitude)
            //        w = minCurrentAmplitude - w < AmplitudeStep ? minCurrentAmplitude : w + AmplitudeStep;
            //    else if (w > maxCurrentAmplitude)
            //        w = w - maxCurrentAmplitude < AmplitudeStep ? maxCurrentAmplitude : w - AmplitudeStep;
            //}
            //else
            //{
            //    if (w > -minCurrentAmplitude)
            //        w = w + minCurrentAmplitude < AmplitudeStep ? -minCurrentAmplitude : w - AmplitudeStep;
            //    else if (w < -maxCurrentAmplitude)
            //        w = -w - maxCurrentAmplitude < AmplitudeStep ? -maxCurrentAmplitude : w + AmplitudeStep;
            //}
            winds[0] = w;

            if (--speedTick <= 0)
            {
                speedTick = Game.Rand(ChangeSpeedPeriod);
                winds[winds.Length - 1] = h * (2 * Game.Rand() - 1);
            }
            if (--amplitudeTick <= 0)
            {
                amplitudeTick = Game.Rand(MinChangeAmplitudePeriod, MaxChangeAmplitudePeriod);
                minCurrentAmplitude = MinAmplitude + (MaxAmplitude - MinAmplitude) * Game.Rand();
                maxCurrentAmplitude = minCurrentAmplitude + +AmplitureScatter * Game.Rand();
            }

            Scene.PriorWindStrength = Scene.WindStrength;
            Scene.WindStrength = winds[0];
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (DebugMode)
            {
                var x = Game.ScreenWidth * .5f;
                var originP = Vector2.Zero;
                var originN = new Vector2(32, 0);

                Game.Draw(windBg, x, 32,
                    origin: Scene.WindStrength > 0 ? originP : originN,
                    vscale: new Vector2(Game.ScreenWidth / 32 / 2 * Math.Abs(Scene.WindStrength), .5f),
                    color: new Color(Color.White, .8f)
                );

                var h = MaxSpeedFactor;
                Game.DrawString(winds[0], x, 0);
                for (var i = 1; i < winds.Length; i++)
                {
                    Game.Draw(windBg, x, 16 * i,
                        origin: winds[i] > 0 ? originP : originN,
                        vscale: new Vector2(Game.ScreenWidth / 32 / 2 * h * Math.Abs(winds[i]), .5f),
                        color: new Color(Color.White, .8f)
                    );
                    // Game.DrawString(winds[i], x, 16 * i);
                    h *= MaxSpeedFactor;
                }

                Game.DrawString("Amplitude = " + minCurrentAmplitude + " .. " + maxCurrentAmplitude, x, 64);
            }

            base.Draw(gameTime);
        }

    }

}