using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ProceduralTextAdventure
{
    class Thing
    {
        public GameState GameState { get; protected set; }
        public Dictionary<string, Func<string[], bool>> Actions { get; protected set; }
        public Thing(GameState gameState)
        {
            GameState = gameState;
            Actions = new Dictionary<string, Func<string[], bool>>()
            {
                {
                    "LOOK",
                    i =>
                    {
                        return false;
                    }
                },
            };
        }
    }
}
