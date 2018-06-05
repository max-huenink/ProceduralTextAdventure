using System;
using System.Collections.Generic;
using System.Text;

namespace ProceduralTextAdventure
{
    class Item : Thing
    {
        public string Name { get; protected set; }
        private string Touched;
        private bool _InInventory;
        public bool InInventory { get { return _InInventory; } set { _InInventory = value; if (value) { this.DeleteRoomEvents(); } } }

        public Item(
            GameState gameState,
            string name,
            string description,
            string touch = "",
            bool takeable = false,
            bool pushable = false,
            bool pullable = false,
            bool interactable = false
            ) : base(gameState)
        {
            Name = name.ToUpper();
            Description = description;
            if (takeable) { Takeable += (Actor a) => { return GiveItem(a); }; }
            Touched = $"You can't reach the {Name}";
            if (touch != "") { Touched = touch; }
            if (pushable) { Pushable += (Actor a) => { return PushItem(a); }; }
            if (pullable) { Pullable += (Actor a) => { return PullItem(a); }; }

            Actions.Add("USE", Use);
            Actions.Add("TAKE", Take);
            Actions.Add("TOUCH", Touch);
            Actions.Add("PUSH", Push);
            Actions.Add("PULL", Pull);
        }
        public delegate bool interact(Actor a);
        public interact Takeable;
        public interact Pushable;
        public interact Pullable;

        public interact PlayerUse_Event;
        public interact RoomUse_Event;
        // public string Use() => Do(this.PlayerUse_Event, "use");
        public string Use(Actor a) => "Not implemented yet.";

        public string Take(Actor a) => Do(a, Takeable, "take");
        public string Touch(Actor a) => Touched;
        public string Push(Actor a) => Do(a, Pushable, "push");
        public string Pull(Actor a) => Do(a, Pullable, "pull");
        
        private string Do(Actor a, interact thing, string action)
        {
            if (thing != null)
            {
                if (!(bool)thing?.Invoke(a)) return $"{action}ing the {Name} failed.";
                return $"You {action} the {Name}.";
            }
            return $"You can't {action} that.";
        }

        private void DeleteRoomEvents()
        {
            Takeable = null;
            Pushable = null;
            Pullable = null;
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

        public bool GiveItem(Actor a)
        {
            if (GameState.CurrentRoom.RemoveItem(this))
            {
                return a.AddItem(this);
            }
            return false;
        }

        public bool PushItem(Actor a)
        {
            throw new NotImplementedException();
        }
        public bool PullItem(Actor a)
        {
            throw new NotImplementedException();
        }
    }
}