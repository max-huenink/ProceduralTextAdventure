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
        public Dictionary<string,Door> Doors { get; protected set; }
        public List<Item> Items { get; protected set; }
        private string RoomAdj;

        public string Description
        {
            get
            {
                List<Door> doors = this.Doors.Where(k => k.Key != null).Select(v => v.Value).ToList();
                List<Item> items = this.Items.Where(i => i != null).ToList();
                string descript = $"You are standing in a {this.RoomAdj} room with {doors.Count() } doors and {items.Count()} items.";
                if (doors.Count() > 0)
                {
                    descript += " You see doors to the ";
                    List<string> dirs = new List<string>();
                    doors.ForEach(f => dirs.Add(f.Facing));
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
            this.Doors = new Dictionary<string, Door>();
            this.Items = new List<Item>();
            this.RoomAdj = adjective;
        }
        public Room(string name, Dictionary<string,Door> startingDoors, List<Item> startingItems, string adjective = "") : this(name, adjective)
        {
            this.Items = startingItems;
            this.Doors = startingDoors;
        }

        public bool AddDoorTo(int direction, Room connection)
        {
            // Checks if the door direction exists and outputs the string for that direction
            if (!Door.Directions.TryGetValue(direction, out string facing)) { return false; }

            // Trys to set the door in this room
            if (!this.Doors.TryAdd(facing, new Door(direction, connection))) { return false; }

            // New opposite direction
            int oppDir = ((direction < 2) ? 2 : -2);

            // Checks if the new opposite direction exists and outputs the string for the new direction
            if (!Door.Directions.TryGetValue(oppDir, out string oppFace)) { return false; }
            
            // Adds the door to the connecting room
            if (!connection.ConnectsTo(oppFace, oppDir, new Door(oppDir, this)))
            {
                // If the add fails, remove the door from this room as well (we don't want one way doors)
                this.Doors.Remove(facing);
                return false;
            }
            return true;
        }
        public bool AddDoorTo(string facing, Room connection)
        {
            foreach (KeyValuePair<int, string> kvp in Door.Directions)
            {
                if (kvp.Value == facing)
                {
                    if (!this.AddDoorTo(kvp.Key, connection)) { continue; }
                    return true;
                }
            }
            return false;
        }
        private bool ConnectsTo(string facing, int direction, Door door)
        {
            if (!this.Doors.TryAdd(facing, door)){ return false; }
            return true;
        }
        public void AddItem(Item item)
        {
            this.Items.Add(item);
        }
        public bool RemoveItem(Item item)
        {
            return this.Items.Remove(item);
        }
    }

}
