using System;
using System.Collections.Generic;
using System.Text;
using KamGame.Wallpapers;


namespace FallenLeaves
{
    partial class FallenLeavesPattern
    {
        public Wind wind1;
        public Wind[] winds;
        public Wind wind1_max;
        public Wind wind1_fast;

        public void CreateWinds()
        {
            wind1 = new Wind
            {
                ChangeSpeedPeriod = 500,
                MaxSpeedFactor = 100,
                MinAmplitude = 0.1f,
                MaxAmplitude = .7f,
                AmplitureScatter = .3f,
                MinChangeAmplitudePeriod = 200,
                MaxChangeAmplitudePeriod = 700,
                AmplitudeStep = 0.005f
            };

            winds = new[]
            {
                // случайный
                wind1,
                // сильный
                new Wind(wind1) { MinAmplitude = .60f, MaxAmplitude = .80f, AmplitureScatter = .2f },
                // слабый
                new Wind(wind1) { MinAmplitude = .02f, MaxAmplitude = .30f, AmplitureScatter = .2f },
                // резкий порывистый
                new Wind(wind1) { AmplitureScatter = .1f, MinChangeAmplitudePeriod = 100, MaxChangeAmplitudePeriod = 300, AmplitudeStep = 0.02f },
            };

            wind1_max = new Wind(wind1)
            {
                ChangeSpeedPeriod = 10000,
                MaxSpeedFactor = 20,
                MinAmplitude = 1,
                MaxAmplitude = 1,
                AmplitureScatter = 0,
                MinChangeAmplitudePeriod = 10000,
                AmplitudeStep = 1000,
            };

            //Шаблон ветра, наследующийся от wind1. Более резко стабилизируется 
            wind1_fast = new Wind(wind1)
            {
                AmplitudeStep = 0.02f
            };
        }
    }
}
