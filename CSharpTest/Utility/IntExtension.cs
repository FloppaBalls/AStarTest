using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    static class IntExtension
    {
        public static int PositiveInfinity = int.MaxValue;
        public static int NegativeInfinity = int.MinValue;

        public static bool IsPositiveInfinity(int x)
        {
            return PositiveInfinity == x;
        }
        public static bool IsNegativeInfinity(int x)
        {
            return NegativeInfinity == x;
        }
    }
}
