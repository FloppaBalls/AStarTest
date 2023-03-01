using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    class Rect
    {
        public Rect(int x0, int y0, uint width0, uint height0)
        {
            left = x0;
            top = y0;
            width = width0;

            height = height0;
        }

        public Rect(Vec2 TopLeft, Vec2 BottomRight)
        {
            Debug.Assert(TopLeft.y >= BottomRight.y);
            Debug.Assert(TopLeft.x <= BottomRight.x);
            left = TopLeft.x;
            top = TopLeft.y;

            width = (uint)(TopLeft.x - BottomRight.x);
            height = (uint)(-TopLeft.y + BottomRight.y);
        }
        public int right()
        {
            return left + (int)width;
        }
        public int bottom()
        {
            return top - (int)height;
        }
        public static bool Collision(Rect rect1, Rect rect2)
        {
            // vertical collision
            bool vCollision = rect1.bottom() < rect2.top && rect2.bottom() < rect1.top;
            //horizontal collision
            bool hCollision = rect1.left < rect2.right() && rect2.left < rect1.right();


            return vCollision && hCollision;
        }
        uint Area()
        {
            return width * height;
        }

        public static Rect GetRect(vector<Vec2> poligon)
        {
            Debug.Assert(poligon.Length > 0);

            Vec2 TopLeft, BottomRight;
            TopLeft = BottomRight = poligon.GetValueAt(0);

            for (uint index = 0; index < poligon.Length; index++)
            {
                Vec2 point = poligon.GetValueAt(index);

                TopLeft.x = Math.Min(point.x, TopLeft.x);
                TopLeft.y = Math.Max(point.y, TopLeft.y);

                BottomRight.x = Math.Max(point.x, BottomRight.x);
                BottomRight.y = Math.Min(point.y, BottomRight.y);
            }
            Rect rect = new Rect(TopLeft, BottomRight);

            return rect;
        }
        public void MoveBy(Vec2 offset)
        {
            left += offset.x;
            top  += offset.y;
        }
        public int left;
        public int top;
        public uint width;
        public uint height;
    }
}
