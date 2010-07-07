using System;

namespace TryOut
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (TierGame game = new TierGame())
            {
                game.Run();
            }
        }
    }
}

