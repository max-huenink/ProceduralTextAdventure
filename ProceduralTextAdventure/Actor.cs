using System;
using System.Collections.Generic;
using System.Text;

namespace ProceduralTextAdventure
{
    /// <summary>
    /// The base actor class
    /// </summary>
    class Actor : Thing
    {
        public Stats MyStats { get; set; }
        public double HP { get { return MyStats.HP; } }
        public double DEF { get { return MyStats.DEF; } }
        public double SPD { get { return MyStats.SPD; } }
        public double ATT { get { return MyStats.ATT; } }

        private List<Item> _Inv;
        public List<Item> Inventory { get { return _Inv; } set { _Inv = value; InvChange_Event?.Invoke(); } }

        private Item[] _Equip;
        public Item[] Equipment { get { return _Equip; } set { _Equip = value; InvChange_Event?.Invoke(); } }

        public Room CurrentRoom { get; protected set; }

        public delegate void onInvChange();
        public event onInvChange InvChange_Event;

        public Actor(GameState gameState, Room startRoom, Stats stats = null) : base(gameState)
        {
            MyStats = new Stats(5, 5, 5, 5);
            if(stats != null)
            {
                MyStats = stats;
            }
            Inventory = new List<Item>();
            Equipment = new Item[4];
            //InvChange_Event += () => { };
            CurrentRoom = startRoom;
        }

        public bool AddItem(Item item)
        {
            if (item == null) { return false; }
            Inventory.Add(item);
            item.InInventory = true;
            return true;
        }
        public bool RemoveItem(Item item)
        {
            if (item == null) { return false; }
            return Inventory.Remove(item);
        }

        public bool EquipItem(Item item)
        {
            return false;
        }
        public bool UnequipItem(Item item)
        {
            return false;
        }

        public string ChangeRooms(Room newRoom)
        {
            CurrentRoom = newRoom;
            return CurrentRoom.Description;
        }
    }
}
