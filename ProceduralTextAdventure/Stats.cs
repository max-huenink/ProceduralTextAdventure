using System;
using System.Collections.Generic;
using System.Text;

namespace ProceduralTextAdventure
{
    class Stats
    {
        public double[] Values { get; protected set; }

        public double HP { get { return Values[0]; } }
        public double DEF { get { return Values[1]; } }
        public double SPD { get { return Values[2]; } }
        public double ATT { get { return Values[3]; } }


        public Stats()
        {
            Values = new double[4];
        }
        public Stats(double hp, double def, double spd, double att)
        {
            Values = new double[] { hp, def, spd, att };
        }
        private Stats(double[] values)
        {
            Values = values;
        }

        public Stats Clone() => new Stats(HP, DEF, SPD, ATT);

        private static Stats Operation(Stats left, Stats right, Func<double, double, double> op)
        {
            double[] values = new double[4];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = op(left.Values[i], right.Values[i]);
            }
            return new Stats(values);
        }

        public static Stats operator +(Stats left, Stats right) => 
            Operation(left, right, (i, j) => i + j);
        public static Stats operator -(Stats left, Stats right) => 
            Operation(left, right, (i, j) => i - j);
        public static Stats operator *(Stats left, Stats right) => 
            Operation(left, right, (i, j) => i * j);
        public static Stats operator /(Stats left, Stats right) => 
            Operation(left, right, (i, j) => i / j);
    }
}
