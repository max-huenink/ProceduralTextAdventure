using System;
using System.Collections.Generic;
using System.Text;

namespace ProceduralTextAdventure
{
    class Door
    {
        public Room To;
        public int Direction;
        public static readonly Dictionary<int,string> Directions = new Dictionary<int, string>() { { 0, "North" }, { 1, "East" }, { 2, "West" }, { 3, "South" } };
        public string Facing { get { return Directions[Direction]; } }
        public Door(int dir, Room to)
        {
            Direction = dir;
            To = to;
        }
    }
}
