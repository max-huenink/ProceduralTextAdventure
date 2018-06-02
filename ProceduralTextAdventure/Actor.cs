using System;
using System.Collections.Generic;
using System.Text;

namespace ProceduralTextAdventure
{
    class Actor
    {
        public Stats MyStats { get; set; }
        public double HP { get { return MyStats.HP; } }
        public double DEF { get { return MyStats.DEF; } }
        public double SPD { get { return MyStats.SPD; } }
        public double ATT { get { return MyStats.ATT; } }

        private List<Item> _Inv;
        public List<Item> Inv { get { return _Inv; } set { _Inv = value; InvChange_Event?.Invoke(); } }

        private Item[] _Equip;
        public Item[] Equip { get { return _Equip; } set { _Equip = value; InvChange_Event?.Invoke(); } }

        private Room _CurrentRoom;
        public Room CurrentRoom { get { return _CurrentRoom; } set { _CurrentRoom = value; RoomChange_Event?.Invoke(); } }

        public delegate void onRoomChange();
        public event onRoomChange RoomChange_Event;
        public delegate void onInvChange();
        public event onInvChange InvChange_Event;

        public Actor(Room startRoom, double hp = 5, double def = 5, double spd = 5, double att = 5)
        {
            this.MyStats = new Stats(hp, def, spd, att);
            this.Inv = new List<Item>();
            this.Equip = new Item[4];
            //RoomChange_Event += () => { Console.WriteLine(CurrentRoom.Description); };
            //InvChange_Event += () => { };
            this.CurrentRoom = startRoom;
        }
        public bool AddItem(Item item)
        {
            if (item == null) { return false; }
            this.Inv.Add(item);
            item.InInventory = true;
            return true;
        }
        public bool RemoveItem(Item item)
        {
            if (item == null) { return false; }
            return this.Inv.Remove(item);
        }
        public string ChangeRooms(Room newRoom)
        {
            this.CurrentRoom = newRoom;
            return this.CurrentRoom.Description;
        }
    }
}
