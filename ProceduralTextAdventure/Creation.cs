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

        public Item[] Items = new Item[3];

        GameState GameState;
        Actor Player { get { return GameState.Player; } }
        Room CurrentRoom { get { return GameState.CurrentRoom; } }
        bool End = false;

        void Start()
        {
            if (GameState == null)
            {
                GameState = new GameState();
            }
            GameState.RND = new Random();
            Items[0] = new Item(GameState, "Ball", "A red ball");
            Items[1] = new Item(GameState, "Sword", "A shiny metal sword", touch: "It is cool to the touch", takeable: true);
            Items[2] = new Item(GameState, "Apple", "A delicious looking red apple", touch: "It feels like an apple.", takeable: true);
            Room start = new Room(GameState,"Starter room", adjective: "a well lit but old looking");
            if (!start.AddDoorTo(0, new Room(GameState,"Other"))) { return; }
            GameState.Player = new Player(GameState, start);

            Items = PickRandomItems(Items, 3, start).ToArray();
            Items = PickRandomItems(Items, 3, start.Rooms["N"]).ToArray();

            
            Console.WriteLine("These are the commands you can use: ");
            foreach (KeyValuePair<string,Func<string[],string>> kvp in GameState.Player.Commands)
            {
                Console.WriteLine(kvp.Key);
            }
            Console.WriteLine("----------------");
            Console.WriteLine(CurrentRoom.Description);
            CommandLoop();
        }
        string[] Prompt(string text="Command")
        {
            Console.Write($"{text}: ");
            return Console.ReadLine().ToUpper().Split(' ');
        }
        void CommandLoop()
        {
            while (!End)
            {
                string[] input = Prompt();
                if (input.Count() < 1) { continue; }
                if (input[0] == "EXIT" || input[0] == "END")
                {
                    if (Prompt("Are you sure you want to exit? (Y/N)\n")[0] == "Y")
                    {
                        End = true;
                        continue;
                    }
                }
                GameState.Player.Act(input, input[0]);
            }
        }
        public IEnumerable<Item> PickRandomItems(IEnumerable<Item> x, int number, Room room)
        {
            number++;
            if (number > x.Count()) { number = x.Count(); }
            for (int i = 0; i < number - 1; i++)
            {
                Item[] y = x.ToArray();
                int index = GameState.RND.Next(0, y.Where(j => j != null).Count());
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

                int index = GameState.RND.Next(0, 1);
            }
            return new Room(GameState,"empty");
        }
    }
}
