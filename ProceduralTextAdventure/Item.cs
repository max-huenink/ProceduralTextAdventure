using System;
using System.Collections.Generic;
using System.Text;

namespace ProceduralTextAdventure
{
    class Item
    {
        public Stats Stats;
        public Dictionary<string, Func<string>> Actions = new Dictionary<string, Func<string>>();
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        private string Touched;
        private bool _InInventory;
        public bool InInventory { get { return _InInventory; } set { _InInventory = value; if (value) { this.DeleteRoomEvents(); } } }

        public Item(
            string name,
            string description,
            string touch = "",
            bool takeable = false,
            bool pushable = false,
            bool pullable = false,
            bool interactable = false,
            Stats stats = null
            )
        {
            Stats = new Stats(0, 0, 0, 0);
            if (name != null) { Name = name.ToUpper(); }
            Description = description;
            if (takeable) { Take_Event += () => { return GiveItem(this); }; }
            Touched = $"You can't reach the {Name}";
            if (touch != "") { Touched = touch; }
            if (pushable) { Push_Event += () => { return PushItem(this); }; }
            if (pullable) { Pull_Event += () => { return PullItem(this); }; }
            if (stats != null) { Stats += stats; }

            this.Actions.Add("USE", this.Use);
            this.Actions.Add("LOOK", this.Look);
            this.Actions.Add("TAKE", this.Take);
            this.Actions.Add("TOUCH", this.Touch);
            this.Actions.Add("PUSH", this.Push);
            this.Actions.Add("PULL", this.Pull);
        }
        public delegate bool interact();
        public event interact Take_Event;
        public event interact Push_Event;
        public event interact Pull_Event;

        public event interact PlayerUse_Event;
        public event interact RoomUse_Event;

        // public string Use() => Do(this.PlayerUse_Event, "use");
        public string Use() => "Not implemented yet.";

        public string Look() => Description;
        public string Take() => Do(this.Take_Event, "take");
        public string Touch() => Touched;
        public string Push() => Do(this.Push_Event, "push");
        public string Pull() => Do(this.Pull_Event, "pull");
        
        private string Do(interact thing, string action)
        {
            if (thing != null)
            {
                if (!(bool)thing?.Invoke()) return $"{action}ing the {Name} failed.";
                return $"You {action} the {Name}.";
            }
            //if ((bool)thing?.Invoke()) { return "thing"; }
            return $"You can't {action} that.";
        }

        private void DeleteRoomEvents()
        {
            this.Take_Event = null;
            this.Push_Event = null;
            this.Pull_Event = null;
            this.RoomUse_Event = null;
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