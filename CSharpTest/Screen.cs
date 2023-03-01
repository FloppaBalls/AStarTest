using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Graphics
{

    class Screen
    {
        public Screen()
        {
            buffer = new char[height, width];
            for (uint y = 0; y < height; y++)
            {
                for (uint x = 0; x < width; x++)
                {
                    buffer[y, x] = ' ';
                }
            }
            WasUpdated = true;
        }
        public void Draw()
        {
            for (uint x = 0; x < width + border_thickness * 2; x++)
            {
                Console.Write("-");
            }
            Console.WriteLine();
            for (uint y = 0; y < height; y++)
            {
                Console.Write("|");
                for (uint x = 0; x < width; x++)
                {
                    Console.Write(buffer[y, x]);
                }
                Console.Write("|");
                Console.WriteLine();
            }
            for (uint x = 0; x < width + border_thickness * 2; x++)
            {
                Console.Write("-");
            }
        }
        public void ChangeChar(int x, int y, char ch)
        {
            WasUpdated = (x >= 0 && x < width && y >= 0 && y < height);
            if (WasUpdated)
            {
                buffer[y, x] = ch;
            }
        }
        public void ChangeChar(Vec2 pos, char ch)
        {
            ChangeChar(pos.x, pos.y, ch);
        }
        public char GetChar(Vec2 pos)
        {
            return buffer[pos.y, pos.x];
        }
        static public bool InBounds(Vec2 pos)
        {
            //is in bounds horizontally
            bool hBounds = pos.x >= 0 && pos.x < width;
            //is in bounds vertically
            bool vBounds = pos.y >= 0 && pos.y < height;

            return hBounds && vBounds;  
        }
        public static uint width = 90;
        public static uint height = 30;
        public static uint border_thickness = 1;
        private char[,] buffer;
        public bool WasUpdated;
    }
}
