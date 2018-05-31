using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ProceduralTextAdventure
{
    class Room
    {
        /*
         * Directions
         * 0: North
         * 1: East
         * 2: South
         * 3: West
        */
        public string Name { get; protected set; }
        public Door[] Doors { get; protected set; }
        public List<Item> Items { get; protected set; }
        private string roomAdj;

        public string Description
        {
            get
            {
                List<Door> doors = this.Doors.Where(d => d != null).ToList();
                List<Item> items = this.Items.Where(i => i != null).ToList();
                string descript = $"You are standing in a {this.roomAdj} room with {doors.Count() } doors and {items.Count()} items.";
                if (doors.Count() > 0)
                {
                    descript += " You see doors to the ";
                    List<string> dirs = new List<string>();
                    doors.ForEach(f => dirs.Add(f.Direction));
                    descript += string.Join(", ", dirs.Take(dirs.Count - 1)) + (dirs.Count <= 1 ? "" : ", and ") + dirs.LastOrDefault() + ".";
                }
                if (items.Count() > 0)
                {
                    descript += " Inside the room you see a ";
                    List<string> itemNames = new List<string>();
                    items.ForEach(o => itemNames.Add(o.Name));
                    descript += string.Join(", ", itemNames.Take(itemNames.Count - 1)) + (itemNames.Count <= 1 ? "" : ", and") + itemNames.LastOrDefault() + ".";
                }
                return descript;
            }
        }

        public Room(string name, string adjective = "")
        {
            this.Name = name;
            this.Doors = new Door[4];
            this.Items = new List<Item>();
            this.roomAdj = adjective;
        }
        public Room(string name, Door[] startingDoors, List<Item> startingItems, string adjective = "") : this(name, adjective)
        {
            this.Items = startingItems;
            this.Doors = startingDoors;
        }

        public void AddDoor(int direction, Room connection)
        {
            // Set the door in this room
            Doors[direction] = new Door(direction, connection);

            direction += ((direction < 2) ? 2 : -2);
            // Set the door in the new room
            connection.AddDoor(direction, new Door(direction, this));
        }
        public void AddDoor(int direction, Door door)
        {
            Doors[direction] = door;
        }
        public void AddItem(Item item)
        {
            Items.Add(item);
        }
        public bool RemoveItem(Item item)
        {
            return Items.Remove(item);
        }
    }

}
