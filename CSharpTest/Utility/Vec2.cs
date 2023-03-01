using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    class Vec2
    {
        public Vec2(int x0, int y0)
        {
            x = x0;
            y = y0;
        }
        public static Vec2 operator +(Vec2 p1, Vec2 p2)
        {
            return new Vec2(p1.x + p2.x, p1.y + p2.y);
        }
        public static Vec2 operator -(Vec2 p1, Vec2 p2)
        {
            return new Vec2(p1.x - p2.x, p1.y - p2.y);
        }
        public static bool operator !=(Vec2 p1, Vec2 p2)
        {
            return (p1.x != p2.x) || (p1.y != p2.y);
        }
        public static bool operator ==(Vec2 p1, Vec2 p2)
        {
            return (p1.x == p2.x) && (p1.y == p2.y);
        }
        public double GetLengthSq(Vec2 p1)
        {
            double length = Math.Pow(p1.x - x , 2) + Math.Pow(p1.y - y , 2);

            return length;
        }
        public double GetLength(Vec2 p1 )
        {
            double length = Math.Sqrt(Math.Pow(p1.x - x, 2) + Math.Pow(p1.y - y, 2));

            return length;
        }
        public double GetLength()
        {
            double length = Math.Sqrt(Math.Pow( x, 2) + Math.Pow( y, 2));

            return length;
        }
        public double GetLengthSq()
        {
            double length = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));

            return length;
        }
        public int x;
        public int y;
    }
}
