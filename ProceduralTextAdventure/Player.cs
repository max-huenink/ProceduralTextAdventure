using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ProceduralTextAdventure
{
    /// <summary>
    /// The player
    /// </summary>
    class Player : Actor
    {
        public Dictionary<string, Func<string[], string>> Commands = new Dictionary<string, Func<string[], string>>();
        public Dictionary<string, Func<string>> SingleCommands = new Dictionary<string, Func<string>>();
        public Player(GameState gameState, Room startRoom, Stats stats = null) : base(gameState, startRoom, stats)
        {
            this.GameState.Player = this;
            Commands.Add("GO", i =>
            {
                string x = i[1][0].ToString();
                if (!CurrentRoom.Doors.TryGetValue(x, out Door door)) { return "There is no door in that direction, is it spelled right?"; }
                return ChangeRooms(door.To);
            });
            Commands.Add("CHECK", i =>
            {
                char c = i[1][0];
                List<string> things = new List<string>();
                string slot = "";
                if (c == 'I')
                {
                    if (Inventory.Count() == 0) { return "Your inventory has no items."; }
                    Inventory.ForEach(j => things.Add(j.Name.ToUpper()));
                    slot = "in your inventory";
                }
                if (c == 'E')
                {
                    if (Equipment.Where(k => k != null).Count() == 0) { return "You have no items equipped."; }
                    Equipment.ToList().ForEach(j => things.Add(j.Name.ToUpper()));
                    slot = "equipped";
                }
                if (slot != "")
                {
                    return $"You have {Stringify(things)} {slot}.";
                }
                return "That is not something you can check";
            });
            Commands.Add("TAKE", i => GetItem(i[1], "TAKE"));
            Commands.Add("TOUCH", i => GetItem(i[1], "TOUCH"));
            Commands.Add("INSPECT", i => GetItem(i[1], "LOOK"));
            Commands.Add("LOOK", i => GetItem(i[2], "LOOK"));
            Commands.Add("PUSH", i => GetItem(i[1], "PUSH"));
            Commands.Add("PULL", i => GetItem(i[1], "PULL"));
            Commands.Add("USE", i => GetItem(i[1], "USE"));
            SingleCommands.Add("LOOK", () => CurrentRoom.Look(this));
        }
        string GetItem(string name, string actionIdentifier)
        {
            if (!CurrentRoom.Items.TryGetValue(name, out Item item)) { return "That item doesn't exist, is it spelled right?"; }
            if (!item.Actions.TryGetValue(actionIdentifier, out Func<Actor, string> action)) { return "That action doesn't exist."; }
            return action?.Invoke(this);
        }
        public void Act(string[] full, string command)
        {
            if (full.Count() == 1)
            {
                if (!SingleCommands.TryGetValue(command, out Func<string> a))
                {
                    Print("Not valid");
                    return;
                }
                Print(a?.Invoke());
            }
            if (!Commands.TryGetValue(command, out Func<string[], string> y))
            {
                Print("Not a valid command!");
                return;
            }
            Print(y?.Invoke(full));
        }
    }
}
