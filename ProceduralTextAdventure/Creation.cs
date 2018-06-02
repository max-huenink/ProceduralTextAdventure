using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ProceduralTextAdventure
{
    class Creation
    {
        // Initializer
        static void Main(string[] args) => new Creation().Start();

        public static Creation Instance;

        public Dictionary<string, Func<string, string>> Commands { get; protected set; }

        public Actor Player { get; protected set; }
        public Room CurrentRoom { get { return Player.CurrentRoom; } }

        void Start()
        {
            if(Instance == null)
            {
                Instance = this;
            }

            Commands = new Dictionary<string, Func<string, string>>();
            SetCommands();
            Room start = new Room("Starter room", adjective: "a well lit but old looking");
            start.AddDoorTo(0, new Room("North"));
            Player = new Actor(start);
            start.AddItem(new Item("Apple","A delicious looking red apple",touch:"It feels like an apple.",takeable:true));
            start.AddItem(new Item("Ball", "A red ball"));
            start.Doors["N"].To.AddItem(new Item("Sword", "A shiny metal sword", touch: "It is cool to the touch", takeable: true));

            
            Console.WriteLine("These are the commands you can use: ");
            foreach (KeyValuePair<string,Func<string,string>> kvp in Commands)
            {
                Console.WriteLine(kvp.Key);
            }
            Console.WriteLine("----------------");
            Console.WriteLine(CurrentRoom.Description);
            while (true)
            {
                string[] a = Console.ReadLine().Split(' ');
                if (a.Count() < 1) { continue; }
                string x = a[0];
                if (x == "exit") { return; }
                if (a.Count() < 2) { Console.WriteLine("You need to enter something else"); continue; }
                string z = a[1];
                if (!Commands.TryGetValue(x,out Func<string,string> y)) { Console.WriteLine("Not a valid command!"); }
                Console.WriteLine(y?.Invoke(z));
            }
        }

        void SetCommands()
        {
            Commands.Add("go", i =>
            {
                i = i.ToUpper().First<char>().ToString();
                if (!CurrentRoom.Doors.TryGetValue(i,out Door door)) { return "There is no door in that direction, is it spelled right?"; }
                return Player.ChangeRooms(door.To);
            });
            Commands.Add("check", i =>
            {
                char c = i.ToUpper().First<char>();
                List<string> x = new List<string>();
                string slot = "";
                if (c == 'I')
                {
                    if (Player.Inv.Count() == 0) { return "Your inventory has no items."; }
                    Player.Inv.ForEach(j => x.Add(j.Name.ToUpper()));
                    slot = "in your inventory";
                }
                if (c == 'E')
                {
                    if (Player.Equip.Where(k => k != null).Count() == 0) { return "You have no items equipped."; }
                    Player.Equip.ToList().ForEach(j => x.Add(j.Name.ToUpper()));
                    slot = "equipped";
                }
                if (slot != "")
                {
                    string t = Stringify(x);
                    return $"You have {t} {slot}.";
                }
                return "That is not something you can check";
            });
            Commands.Add("take", i =>
            {
                Item item = GetItem(i);
                if (item.Name == null) { return item.Description; }
                return (item.Take());
            });
            Commands.Add("touch", i =>
            {
                Item item = GetItem(i);
                if (item.Name == null) { return item.Description; }
                return (item.Touch());
            });
            Commands.Add("inspect", i =>
            {
                Item item = GetItem(i);
                if (item.Name == null) { return item.Description; }
                return (item.Look());
            });
            Commands.Add("push", i =>
            {
                Item item = GetItem(i);
                if (item.Name == null) { return item.Description; }
                return (item.Push());
            });
            Commands.Add("pull", i =>
            {
                Item item = GetItem(i);
                if (item.Name == null) { return item.Description; }
                return (item.Pull());
            });
            Commands.Add("use", i =>
            {
                Item item = GetItem(i);
                if (item.Name == null) { return item.Description; }
                return (item.Use());
            });
        }
        Item GetItem(string i)
        {
            i = i.ToUpper();
            if(!CurrentRoom.Items.TryGetValue(i,out Item item)) { return new Item(null, "That item doesn't exist, is it spelled right?"); }
            return item;
        }
        public string Stringify(List<string> list, bool grammar = true)
        {
            List<string> newList = new List<string>();
            foreach (string s in list)
            {
                if (!grammar) { newList = list; break; }
                string a = "a";
                if (s.StartsWith("A") || s.StartsWith("E") || s.StartsWith("I") || s.StartsWith("O") || s.StartsWith("U"))
                {
                    a += "n";
                }
                newList.Add($"{a} {s}");
            }
            return string.Join(", ", newList.Take(newList.Count - 1)) + (newList.Count <= 1 ? "" : ", and ") + newList.LastOrDefault();
        }
    }
}
