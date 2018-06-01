using System;
using System.Collections.Generic;
using System.Text;

namespace ProceduralTextAdventure
{
    class Item
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        private bool _InInventory;
        public bool InInventory { get { return _InInventory; } set { _InInventory = value; if (value) { DeleteRoomEvents(); } } }

        public Item(
            string name,
            string description,
            string touch = "",
            Action<Item> take = null,
            Action push = null,
            Action pull = null
            )
        {
            this.Name = name;
            this.Description = description;
            if (take != null) { this.Take_Event += () => { take(this); }; }
            if (touch != "") { this.Touch_Event += () => { Console.WriteLine(touch); }; }
            if (push != null) { this.Push_Event += () => { push(); }; }
            if (pull != null) { this.Pull_Event += () => { pull(); }; }
        }
        public delegate void interact();
        public event interact Take_Event;
        public event interact Touch_Event;
        public event interact Push_Event;
        public event interact Pull_Event;

        public event interact PlayerUse_Event;
        public event interact RoomUse_Event;

        // public string Use() => Do(this.PlayerUse_Event, "use");
        public string Use() => "Not implemented yet.";

        public string Look() => this.Description;
        public string Take() => Do(this.Take_Event, "take");
        public string Touch() => Do(this.Touch_Event, "touch", "reach");
        public string Push() => Do(this.Push_Event, "push");
        public string Pull() => Do(this.Pull_Event, "pull");

        private string Do(interact thing, string action, string no = "do", bool t = false)
        {
            if (thing != null)
            {
                thing?.Invoke();
                return $"You {action} the {this.Name}";
            }
            return $"You can't {no} that";
        }

        private void DeleteRoomEvents()
        {
            Take_Event = null;
            Push_Event = null;
            Pull_Event = null;
            RoomUse_Event = null;
        }
        private void DeleteAllEvents()
        {
            DeleteRoomEvents();
            PlayerUse_Event = null;
            Touch_Event = null;
        }
        ~Item()
        {
            DeleteRoomEvents();
        }
        public bool TakeItem(Item item)
        {
            if (Creation.Player.CurrentRoom.RemoveItem(item))
            {
                return Creation.Player.AddItem(item);
            }
            return false;
        }
    }
}
