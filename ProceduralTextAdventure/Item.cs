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
        public bool InInventory { get { return _InInventory; } set { _InInventory = value; if (value) { DeleteEvents(); } } }
        public Item(
            string name,
            string description,
            string touch = "",
            Action<Item> take = null,
            Action push = null,
            Action pull = null
            )
        {
            Name = name;
            Description = description;
            if (take != null) { Take_Event += () => { take(this); }; }
            if (touch != "") { Touch_Event += () => { Console.WriteLine(touch); }; }
            if (push != null) { Push_Event += () => { push(); }; }
            if (pull != null) { Pull_Event += () => { pull(); }; }
        }
        public delegate void interact();
        public event interact Take_Event;
        public event interact Touch_Event;
        public event interact Push_Event;
        public event interact Pull_Event;

        public void Take() => Do(Take_Event, "take");
        public void Touch() => Do(Touch_Event, "touch", "reach");
        public void Push() => Do(Push_Event, "push");
        public void Pull() => Do(Pull_Event, "pull");

        private void Do(interact thing, string action, string no = "do")
        {
            if (thing != null)
            {
                Console.WriteLine($"You {action} the {this.Name}");
                thing?.Invoke();
                return;
            }
            Console.WriteLine($"You can't {no} that");
        }
        private void DeleteEvents()
        {
            Take_Event = null;
            Touch_Event = null;
            Push_Event = null;
            Pull_Event = null;
        }
        ~Item()
        {
            DeleteEvents();
        }
    }
}
