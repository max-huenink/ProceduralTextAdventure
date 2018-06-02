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
        public static Random RND = new Random();

        public Dictionary<string, Func<string, string>> Commands { get; protected set; }
        public Item[] Items = new Item[]
        {
            new Item("Ball", "A red ball"),
            new Item("Sword", "A shiny metal sword", touch: "It is cool to the touch", takeable: true),
            new Item("Apple","A delicious looking red apple",touch:"It feels like an apple.",takeable:true),
        };

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
            if (!start.AddDoorTo(0, new Room("Other"))) { return; }
            Player = new Actor(start);
            Items = PickRandomItems(Items, 3, start).ToArray();
            Items = PickRandomItems(Items, 3, start.ConnectedRooms["N"]).ToArray();
            //start.AddItem(new Item("Apple","A delicious looking red apple",touch:"It feels like an apple.",takeable:true));
            //start.AddItem(new Item("Ball", "A red ball"));
            //start.Doors["N"].To.AddItem(new Item("Sword", "A shiny metal sword", touch: "It is cool to the touch", takeable: true));

            
            Console.WriteLine("These are the commands you can use: ");
            foreach (KeyValuePair<string,Func<string,string>> kvp in Commands)
            {
                Console.WriteLine(kvp.Key);
            }
            Console.WriteLine("----------------");
            Console.WriteLine(CurrentRoom.Description);
            while (true)
            {
                string[] a = Console.ReadLine().ToUpper().Split(' ');
                if (a.Count() < 1) { continue; }
                string x = a[0];
                if (x == "EXIT") { return; }
                if (a.Count() < 2) { Console.WriteLine("You need to enter something else"); continue; }
                string z = a[1];
                if (!Commands.TryGetValue(x,out Func<string,string> y)) { Console.WriteLine("Not a valid command!"); }
                Console.WriteLine(y?.Invoke(z));
            }
        }

        void SetCommands()
        {
            Commands.Add("GO", i =>
            {
                i = i[0].ToString();
                if (!CurrentRoom.Doors.TryGetValue(i,out Door door)) { return "There is no door in that direction, is it spelled right?"; }
                return Player.ChangeRooms(door.To);
            });
            Commands.Add("CHECK", i =>
            {
                char c = i[0];
                List<string> things = new List<string>();
                string slot = "";
                if (c == 'I')
                {
                    if (Player.Inv.Count() == 0) { return "Your inventory has no items."; }
                    Player.Inv.ForEach(j => things.Add(j.Name.ToUpper()));
                    slot = "in your inventory";
                }
                if (c == 'E')
                {
                    if (Player.Equip.Where(k => k != null).Count() == 0) { return "You have no items equipped."; }
                    Player.Equip.ToList().ForEach(j => things.Add(j.Name.ToUpper()));
                    slot = "equipped";
                }
                if (slot != "")
                {
                    return $"You have {Stringify(things)} {slot}.";
                }
                return "That is not something you can check";
            });
            Commands.Add("TAKE", i =>
            {
                return GetItem(i, "TAKE");

            });
            Commands.Add("TOUCH", i =>
            {
                return GetItem(i, "TOUCH");
            });
            Commands.Add("INSPECT", i =>
            {
                return GetItem(i, "LOOK");
            });
            Commands.Add("PUSH", i =>
            {
                return GetItem(i, "PUSH");
            });
            Commands.Add("PULL", i =>
            {
                return GetItem(i, "PULL");
            });
            Commands.Add("USE", i =>
            {
                return GetItem(i, "USE");
            });
        }
        string GetItem(string name, string actionIdentifier)
        {
            if(!CurrentRoom.Items.TryGetValue(name,out Item item)) { return "That item doesn't exist, is it spelled right?"; }
            if(!item.Actions.TryGetValue(actionIdentifier,out Func<string> action)) { return "That action doesn't exist."; }
            return action?.Invoke();
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
        public IEnumerable<Item> PickRandomItems(IEnumerable<Item> x, int number, Room room)
        {
            number++;
            if (number > x.Count()) { number = x.Count(); }
            for (int i = 0; i < number - 1; i++)
            {
                Item[] y = x.ToArray();
                int index = RND.Next(0, y.Where(j => j != null).Count());
                Item item = y[index];
                room.AddItem(item);
                y[index] = null;
                x = y.OrderBy(k => k == null);
            }
            return x;
        }
        public Room PickRandomRoom(IEnumerable<Room> x)
        {
            for (int i = 0; i < x.Count(); i++)
            {

                int index = RND.Next(0, 1);
            }
            return new Room("empty");
        }
    }
}
