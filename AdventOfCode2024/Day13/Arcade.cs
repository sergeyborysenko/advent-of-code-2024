using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day13
{
    internal class Arcade
    {
        public const long A_Tokens = 3;
        public const long B_Tokens = 1;

        public long SpentTokens { get; private set; } = 0;
        public (long X, long Y) CurrentPosition { get; private set; } = (0, 0);

        public (long X, long Y) A { get; set; }
        public (long X, long Y) B { get; set; }
        public (long X, long Y) Prize { get; set; }

        public long CalculateNumberOfTokens(int maxPressButton = 0)
        {
            double upArg = Prize.X * B.Y - Prize.Y * B.X;
            double downArg = A.X * B.Y - A.Y * B.X;
            var a = Math.Round(upArg / downArg);
            var b = Math.Round((Prize.X - a * A.X) / B.X);

            if (maxPressButton > 0 && (a > maxPressButton || b > maxPressButton))
                return 0;
            if (a * A.X + b * B.X != Prize.X || a * A.Y + b * B.Y != Prize.Y)
                return 0;
            
            return A_Tokens * (long)a + B_Tokens * (long)b;
        }
    }
}
