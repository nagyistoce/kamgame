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

            game = new FallenLeavesGame();
            var patterns = new FallenLeavesPattern();
            game.StartScene("Autumn01", patterns.NewScene(
                skyId: 4,
                cloudsCount: 1,
                windId: 0, windDirection: 0,
                layoutId: 3,
                fallenLeafsCount: 2,
                fallenLeafsScale: 1.75f
            ));
            game.Run();
        }
    }
}
