﻿#region Using Statements
using System;
using System.Collections.Generic;
using KamGame.Wallpapers;

#endregion

namespace FallenLeaves
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        private static FallenLeavesGame game;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
#if XLARGE
            FallenLeavesPattern.TreeSizeFactor = 2;
#endif
            
            game = new FallenLeavesGame{ UseAccelerometer = false, };
            var patterns = new FallenLeavesPattern();
            //Wind.DebugMode = true;
            game.StartScene("Autumn01", patterns.NewScene(
                skyId: "sky4a",
                cloudsCount: 1f,
                windId: 0, windDirection: -1, 
                layoutId: 0, grassCount: 1f,
                fallenLeafsCount: 1f,
                fallenLeafsScale: 1f
            ));

            game.Run();
        }
    }
}
