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
                skyId: 2,
                layoutId: 0,
                windId: 5,
                fallenLeafsCount: 1
            ));
            game.Run();
        }
    }
}
