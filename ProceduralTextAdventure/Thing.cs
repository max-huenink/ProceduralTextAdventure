using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ProceduralTextAdventure
{
    class Thing
    {
        public GameState GameState;
        public Dictionary<string, Func<Actor,string>> Actions { get; protected set; }
        public string Description;
        public static Action<string> Print = Console.WriteLine;
        public Thing(GameState gameState)
        {
            GameState = gameState;
            Actions = new Dictionary<string, Func<Actor,string>>()
            {
                { "LOOK", Look }
            };
        }
        public string Look(Actor a) => Description;

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
