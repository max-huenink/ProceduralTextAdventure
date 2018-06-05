using System;
using System.Collections.Generic;
using System.Text;

namespace ProceduralTextAdventure
{
    /// <summary>
    /// A door
    /// </summary>
    class Door
    {
        public Room To;
        public Room From;
        public int Direction;
        public static readonly Dictionary<int,string> Directions = new Dictionary<int, string>() { { 0, "NORTH" }, { 1, "EAST" }, { 2, "SOUTH" }, { 3, "WEST" } };
        public string Facing { get { return Directions[Direction]; } }
        public Door(int dir, Room to, Room from)
        {
            Direction = dir;
            To = to;
            From = from;
        }
    }
}
