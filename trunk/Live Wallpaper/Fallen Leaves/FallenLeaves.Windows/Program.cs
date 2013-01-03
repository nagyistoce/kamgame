#region Using Statements
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

            game = new FallenLeavesGame{ UseAccelerometer = false, };
            var patterns = new FallenLeavesPattern();
            game.StartScene("Autumn01", patterns.NewScene(
                skyId: 4,
                cloudsCount: 1,
                windId: 1, windDirection: -1, windShow: true,
                layoutId: 0, grassCount: 1f,
                fallenLeafsCount: 1f,
                fallenLeafsScale: 1f
            ));
            game.Run();
        }
    }
}
