using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

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
        public static Dictionary<string, Func<string, string>> Commands { get; protected set; }
        public static Actor Player { get; protected set; }
        public static Room CurrentRoom { get { return Player.CurrentRoom; } }

        void Start()
        {
            SetCommands();
            Room start = new Room("Starter room", adjective: "A well lit but old looking room");
            start.AddDoorTo(0, new Room("North"));
            // Tell the user what commands they can use before 
            Console.WriteLine("These are the commands you can use: ");
            foreach (KeyValuePair<string,Func<string,string>> kvp in Commands)
            {
                Console.WriteLine(kvp.Key);
            }
            Player = new Actor(start);
        }

        void SetCommands()
        {
            Commands.Add("go", i =>
            {
                if(!CurrentRoom.Doors.TryGetValue(i,out Door door)) { return "There is no door in that direction, is it spelled right?"; }
                return Player.ChangeRooms(door.To);
            });
            Commands.Add("take", i =>
            {
                Item item = GetItem(i);
                if (item.Name == null) { return item.Description; }
                return (item.Take());
            });
            Commands.Add("inspect", i =>
            {
                Item item = GetItem(i);
                if (item.Name == null) { return item.Description; }
                return (item.Look());
            });
            Commands.Add("push", i =>
            {
                Item item = GetItem(i);
                if (item.Name == null) { return item.Description; }
                return (item.Push());
            });
            Commands.Add("pull", i =>
            {
                Item item = GetItem(i);
                if (item.Name == null) { return item.Description; }
                return (item.Pull());
            });
            Commands.Add("use", i =>
            {
                Item item = GetItem(i);
                if (item.Name == null) { return item.Description; }
                return (item.Use());
            });
        }
        Item GetItem(string i)
        {
            if(!CurrentRoom.Items.TryGetValue(i,out Item item)) { return new Item(null, "That item doesn't exist, is it spelled right?"); }
            return item;
        }
    }
}
