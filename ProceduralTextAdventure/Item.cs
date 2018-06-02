using System;
using System.Collections.Generic;
using System.Text;

namespace ProceduralTextAdventure
{
    class Item
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        private string Touched;
        private bool _InInventory;
        public bool InInventory { get { return _InInventory; } set { _InInventory = value; if (value) { DeleteRoomEvents(); } } }

        public Item(
            string name,
            string description,
            string touch = "",
            bool takeable = false,
            bool pushable = false,
            bool pullable = false,
            bool interactable = false
            )
        {
            if (name != null) { this.Name = name.ToUpper(); }
            this.Description = description;
            if (takeable) { this.Take_Event += () => { return GiveItem(this); }; }
            this.Touched = $"You can't reach the {this.Name}";
            if (touch != "") { this.Touched = touch; }
            if (pushable) { this.Push_Event += () => { return PushItem(this); }; }
            if (pullable) { this.Pull_Event += () => { return PullItem(this); }; }
        }
        public delegate bool interact();
        public event interact Take_Event;
        public event interact Push_Event;
        public event interact Pull_Event;

        public event interact PlayerUse_Event;
        public event interact RoomUse_Event;

        // public string Use() => Do(this.PlayerUse_Event, "use");
        public string Use() => "Not implemented yet.";

        public string Look() => this.Description;
        public string Take() => Do(this.Take_Event, "take");
        public string Touch() => this.Touched;
        public string Push() => Do(this.Push_Event, "push");
        public string Pull() => Do(this.Pull_Event, "pull");

        private string Do(interact thing, string action)
        {
            if (thing != null)
            {
                if (!(bool)thing?.Invoke()) { return $"{action}ing the {this.Name} failed."; }
                return $"You {action} the {this.Name}.";
            }
            //if ((bool)thing?.Invoke()) { return "thing"; }
            return $"You can't {action} that.";
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
        }
        ~Item()
        {
            DeleteRoomEvents();
        }
        public bool GiveItem(Item item)
        {
            if (Creation.Instance.CurrentRoom.RemoveItem(item))
            {
                return Creation.Instance.Player.AddItem(item);
            }
            return false;
        }
        public bool PushItem(Item item)
        {
            throw new NotImplementedException();
        }
        public bool PullItem(Item item)
        {
            throw new NotImplementedException();
        }
    }
}
