using System;
using System.Collections.Generic;
using System.Linq;

namespace ProceduralTextAdventure
{
    class Creation
    {
        static void Main(string[] args)
        {
            Creation a = new Creation();
            a.Start();
            new Creation().Start();
        }
        void Start()
        {
            Room start = new Room("Starter room", adjective: "A well lit but old looking room");
            start.AddDoor(0, new Room("North"));
            Actor player = new Actor(start);
        }
    }
}
