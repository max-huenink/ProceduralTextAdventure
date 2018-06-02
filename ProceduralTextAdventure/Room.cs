using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ProceduralTextAdventure
{
    class Room
    {
        public static Dictionary<string,Room> Rooms = new Dictionary<string,Room>();
        /*
         * Directions
         * 0: North
         * 1: East
         * 2: South
         * 3: West
        */
        public string Name { get; protected set; }
        public Dictionary<string,Door> Doors { get; protected set; }
        public Dictionary<string,Item> Items { get; protected set; }
        private string RoomAdj;

        public string Description
        {
            get
            {
                List<Door> doors = this.Doors.Where(k => k.Key != null).Select(v => v.Value).ToList();
                List<Item> items = this.Items.Where(k => k.Key != null).Select(v => v.Value).ToList();
                string descript = $"You are standing in the {this.Name} room," +
                    $"it is {this.RoomAdj} room with {doors.Count()} door{(doors.Count() == 1 ? "" : "s")} " +
                    $"and {items.Count()} item{(items.Count() == 1 ? "" : "s")}.";
                if (doors.Count() > 0)
                {
                    descript += $" You see {(doors.Count() == 1 ? "a " : "")}door{(doors.Count() == 1 ? "" : "s")} to the ";
                    List<string> dirs = new List<string>();
                    doors.ForEach(f => dirs.Add(f.Facing));
                    descript += Creation.Instance.Stringify(dirs, false) + ".";
                }
                if (items.Count() > 0)
                {
                    descript += " Inside the room you see ";
                    List<string> itemNames = new List<string>();
                    items.ForEach(o => itemNames.Add(o.Name));
                    descript += Creation.Instance.Stringify(itemNames) + ".";
                }
                return descript;
            }
        }

        public Room(string name, string adjective = "a")
        {
            if (!Rooms.TryAdd(name, this)) { return; }
            this.Name = name;
            this.Doors = new Dictionary<string, Door>();
            this.Items = new Dictionary<string, Item>();
            this.RoomAdj = adjective;
        }
        public Room(string name, Dictionary<string,Door> startingDoors, Dictionary<string,Item> startingItems, string adjective = "a") : this(name, adjective)
        {
            this.Items = startingItems;
            this.Doors = startingDoors;
        }

        public bool AddDoorTo(int direction, Room connection)
        {
            // Checks if the door direction exists and outputs the string for that direction
            if (!Door.Directions.TryGetValue(direction, out string facing)) { return false; }

            // Trys to set the door in this room
            if (!this.Doors.TryAdd(facing.ToUpper().First<char>().ToString(), new Door(direction, connection))) { return false; }

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
            if (!this.Doors.TryAdd(facing.ToUpper().First<char>().ToString(), door)){ return false; }
            return true;
        }
        public bool AddItem(Item item)
        {
            if (!this.Items.TryAdd(item.Name, item)) { return false; }
            return true;
        }
        public bool RemoveItem(Item item)
        {
            return this.Items.Remove(item.Name);
        }
    }

}
