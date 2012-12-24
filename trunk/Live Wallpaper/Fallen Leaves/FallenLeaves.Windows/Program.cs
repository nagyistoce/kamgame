#region Using Statements
using System;
using System.Collections.Generic;

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
            game.Run();
        }
    }
}
