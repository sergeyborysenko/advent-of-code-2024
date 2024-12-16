using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day13
{
    internal class Arcade
    {
        public const int A_Tokens = 3;
        public const int B_Tokens = 1;

        public int SpentTokens { get; private set; } = 0;
        public (int X, int Y) CurrentPosition { get; private set; } = (0, 0);

        public (int X, int Y) A { get; set; }
        public (int X, int Y) B { get; set; }
        public (int X, int Y) Prize { get; set; }

        public int CalculateNumberOfTokens()
        {
            decimal upArg = Prize.X * B.Y - Prize.Y * B.X;
            decimal downArg = A.X * B.Y - A.Y * B.X;
            var a = Math.Round(upArg / downArg);
            var b = Math.Round(Prize.X - a * A.X) / B.X;
            
            if (a > 100 || b > 100 || a * A.X + b * B.X != Prize.X || a * A.Y + b * B.Y != Prize.Y)
                return 0;
            
            return A_Tokens * (int)a + B_Tokens * (int)b;
        }
    }
}
