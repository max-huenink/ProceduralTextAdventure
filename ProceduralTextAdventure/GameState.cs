using System;
using System.Collections.Generic;
using System.Text;

namespace ProceduralTextAdventure
{
    class GameState
    {
        public Player Player;
        public Room CurrentRoom { get { return Player.CurrentRoom; } }
        public Random RND;
    }
}
