using System;
using System.Collections.Generic;
using System.Text;
using KamGame.Wallpapers;


namespace FallenLeaves
{
    partial class FallenLeavesGame
    {
        private Wind wind1;
        private Wind wind1_max;
        private Wind wind1_fast;

        private void CreateWinds()
        {
            wind1 = new Wind
            {
                ChangeSpeedPeriod = 1500,
                MaxSpeedFactor = 200,
                MinAmplitude = 0.1f,
                MaxAmplitude = .7f,
                AmplitureScatter = .3f,
                MinChangeAmplitudePeriod = 200,
                MaxChangeAmplitudePeriod = 700,
                AmplitudeStep = 0.005f
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
