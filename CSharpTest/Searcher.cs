using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;
using Algorithm;
using Graphics;

namespace Program
{
    class Searcher
    {
        public Searcher(Vec2 aPos , Vec2 aDest , string aName )
        {
            last_path = new vector<Vec2>();
            pos = aPos;
            destination = aDest;
            name = aName;
        }

        public string Name
        {
            get
            {
                return name;
            }
        }
        public Vec2 Position
        {
            get { return pos; }
        }
        public Vec2 Destination
        {
            get { return destination; }
        }
        public void Search(ref Screen scr)
        {
            if (last_path.Length > 0)
            { 
                for (uint ind = 1u; ind < last_path.Length - 1; ind++)
                {
                    char ch = scr.GetChar(last_path.GetValueAt(ind));
                    if (ch == '|' || ch == '-')
                    {
                        scr.ChangeChar(last_path.GetValueAt(ind), ' ');
                    }
                }
            }

           vector<Vec2> path = Algorithm.AStarAlgorithm.Apply(pos, destination, ref scr);
           Vec2 previous_pos = path.GetRefAt(0);
           scr.ChangeChar(previous_pos, '*');

           for (uint ind =  1u; ind < path.Length; ind++)
           {
               Vec2 actual_pos = path.GetValueAt(ind);
               Vec2 dif = actual_pos - previous_pos;

               scr.ChangeChar(actual_pos, GetDesiredChar(previous_pos, actual_pos));
               previous_pos = actual_pos;
           }
           scr.ChangeChar(pos, 'P');
           scr.ChangeChar(destination, 'D');

           last_path = path;
        }
        private char GetDesiredChar(Vec2 prev , Vec2 actual)
        {
            Vec2 dif = actual - prev;

            if (dif.x != 0)
            {   
                 return '-';   
            }
            else if (dif.y != 0)
            {
                return '|';
            }
            else
            {
                return ' ';
            }
        }
        private string name;
        private vector<Vec2> last_path;
        private Vec2 pos;
        private Vec2 destination;
    }
}
