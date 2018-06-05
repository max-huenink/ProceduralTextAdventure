using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ProceduralTextAdventure
{
    /// <summary>
    /// A room
    /// </summary>
    class Room : Thing
    {
        public static Dictionary<string,Room> AllRooms = new Dictionary<string,Room>();
        /*
         * Directions
         * 0: North
         * 1: East
         * 2: South
         * 3: West
        */
        public string Name { get; protected set; }
        /// <summary>
        /// Cardinal Directions (N,E,S,W) as key
        /// </summary>
        public Dictionary<string, Room> Rooms { get; protected set; }
        /// <summary>
        /// Cardinal Directions (N,E,S,W) as key
        /// </summary>
        public Dictionary<string,Door> Doors { get; protected set; }
        /// <summary>
        /// Item name as key
        /// </summary>
        public Dictionary<string,Item> Items { get; protected set; }
        private string RoomAdj;

        public new string Description
        {
            get
            {
                List<Door> doors = Doors.Where(k => k.Key != null).Select(v => v.Value).ToList();
                List<Item> items = Items.Where(k => k.Key != null).Select(v => v.Value).ToList();
                string descript = $"You are standing in the {Name} room," +
                    $"it is {RoomAdj} room with {doors.Count()} door{(doors.Count() == 1 ? "" : "s")} " +
                    $"and {items.Count()} item{(items.Count() == 1 ? "" : "s")}.";
                if (doors.Count() > 0)
                {
                    descript += $" You see {(doors.Count() == 1 ? "a " : "")}door{(doors.Count() == 1 ? "" : "s")} to the ";
                    List<string> dirs = new List<string>();
                    doors.ForEach(f => dirs.Add(f.Facing));
                    descript += Stringify(dirs, false) + ".";
                }
                if (items.Count() > 0)
                {
                    descript += " Inside the room you see ";
                    List<string> itemNames = new List<string>();
                    items.ForEach(o => itemNames.Add(o.Name));
                    descript += Stringify(itemNames) + ".";
                }
                return descript;
            }
        }

        public Room(GameState gameState, string name, string adjective = "a") : base(gameState)
        {
            if (!AllRooms.TryAdd(name, this)) { return; }
            Name = name;
            Rooms = new Dictionary<string, Room>();
            Doors = new Dictionary<string, Door>();
            Items = new Dictionary<string, Item>();
            RoomAdj = adjective;
        }
        public Room(
            GameState gameState,
            string name,
            Dictionary<string,Room> startingRooms,
            Dictionary<string,Door> startingDoors,
            Dictionary<string,Item> startingItems,
            string adjective = "a") : this(gameState, name, adjective)
        {
            Rooms = startingRooms;
            Doors = startingDoors;
            Items = startingItems;
        }

        public bool AddDoorTo(int direction, Room connection, bool oneWay = false)
        {
            // Checks if the door direction exists and outputs the string for that direction
            if (!Door.Directions.TryGetValue(direction, out string facing)) { return false; }
            facing = facing[0].ToString();
            // Trys to set the door in this room
            if (!Doors.TryAdd(facing, new Door(direction, connection, this))) { return false; }
            if (!Rooms.TryAdd(facing, connection)) { Doors.Remove(facing); return false; }

            if (oneWay) { return true; }
            
            // New opposite direction
            int oppDir = ((direction < 2) ? 2 : -2);

            // Checks if the new opposite direction exists and outputs the string for the new direction
            if (!Door.Directions.TryGetValue(oppDir, out string oppFace)) { return false; }
            // Adds the door to the connecting room
            if (!connection.ConnectsTo(oppFace[0], oppDir, this, new Door(oppDir, this, connection)))
            {
                // If the add fails, remove the door from this room as well (we don't want one way doors)
                Doors.Remove(facing);
                return false;
            }
            return true;
        }
        public bool AddDoorTo(string facing, Room connection)
        {
            facing = facing.ToUpper();
            foreach (KeyValuePair<int, string> kvp in Door.Directions)
            {
                if (kvp.Value == facing)
                {
                    if (!AddDoorTo(kvp.Key, connection)) { continue; }
                    return true;
                }
            }
            return false;
        }
        private bool ConnectsTo(char facing, int direction, Room room, Door door)
        {
            if (facing != 'N' && facing != 'E' && facing != 'S' && facing != 'W') { return false; }
            if (!Doors.TryAdd(facing.ToString(), door)){ return false; }
            if (!Rooms.TryAdd(facing.ToString(), room)) { Doors.Remove(facing.ToString()); return false; }
            return true;
        }
        public bool AddItem(Item item)
        {
            if (item == null) { return false; }
            if (!Items.TryAdd(item.Name, item)) { return false; }
            return true;
        }
        public bool RemoveItem(Item item)
        {
            return Items.Remove(item.Name);
        }
    }

}
