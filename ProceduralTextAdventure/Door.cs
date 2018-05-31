using System;
using System.Collections.Generic;
using System.Text;

namespace ProceduralTextAdventure
{
    class Door
    {
        public Room To;
        public string Direction;
        private readonly string[] directs = new string[] { "North", "East", "West", "South" };
        public Door(int dir, Room to)
        {
            Direction = directs[dir];
            To = to;
        }
    }
}
