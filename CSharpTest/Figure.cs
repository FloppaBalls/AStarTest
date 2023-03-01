using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;
using Geometry;
using Graphics;

namespace Obstacle
{
    class Figure
    {
        public Figure(Vec2 pos0 , Vec2[] obs_list)
        {
            point_list = new vector<Vec2> (obs_list);
            pos = pos0;
        }
        public Figure(Vec2 pos0, vector<Vec2> obs_list)
        {
            point_list = obs_list;
            pos = pos0;
        }
        public Vec2 Pos
        {
            get
            {
                return pos;
            }
            set
            {
                pos = value;
            }
        }
        public vector<Vec2> PointList
        {
            get
            {
                return point_list;
            }
        }

        public void Draw(Screen screen)
        {
           for(uint ind = 0; ind < point_list.Length; ind++) 
           {
                screen.ChangeChar(point_list.GetValueAt(ind) + pos, 'O');
           }
        }
        public static bool Collision(Figure fig1 , Figure fig2)
        {
            Rect rect1 = Rect.GetRect(fig1.PointList);
            rect1.MoveBy(fig1.pos);

            Rect rect2 = Rect.GetRect(fig2.PointList);
            rect2.MoveBy(fig2.pos);

            return Rect.Collision(rect1 , rect2);
        }
        private vector<Vec2> point_list;
        private Vec2 pos;
    }
    class FigureDictionary
    {
        public enum Type {
            None,
            Star,
            Dick,
            Square,
            Circle,
            Count
        }
        public FigureDictionary()
        {
            rng = new Random();
            FigureD = new Dictionary<Type, vector<Vec2>>();
            Vec2[] list = { };
            FigureD.Add(Type.None, new vector<Vec2>(list));
            // making a dick
            list = new Vec2[] { new Vec2(0 , 0),
                                new Vec2(0 , -1),
                                new Vec2(0 , 1),
                                new Vec2(-1 , 1),
                                new Vec2(1 , 1)};

            FigureD.Add(Type.Dick, new vector<Vec2>(list));
            //making a square
            list = new Vec2[] { new Vec2( 1 ,  1),  /* upper part of the square*/
                                new Vec2( 0 ,  1),
                                new Vec2(-1 ,  1),
                                new Vec2(-1 ,  0), /* middle part of the square*/
                                new Vec2( 1 ,  0),
                                new Vec2( 1 , -1), /* lower part of the square*/
                                new Vec2( 0 , -1),
                                new Vec2(-1 , -1) };

            FigureD.Add(Type.Square, new vector<Vec2>(list));

            FigureD.Add(Type.Circle, Geometry.Circle.Make(5));

            FigureD.Add(Type.Star, Geometry.Star.Make(5 ));

        }
        public vector<Vec2> GetRandomFigure()
        {
            Type key = (Type)rng.Next(1, (int)Type.Count - 1);
            return FigureD[key];
        }

        private Dictionary<Type, vector<Vec2>> FigureD;
        private Random rng;
    }

    class ObstacleSpawner
    { 
        public ObstacleSpawner(uint nObstacles)
        {
            dFig = new FigureDictionary();
            fig_list = new vector<Figure>(0);
            Random rng = new Random();

            for(uint n = 0u; n < nObstacles; n++)
            {
                Figure fig ;
                bool Spawned = false;
                do
                {
                    int x = rng.Next(0, (int)(Screen.width - 1));
                    int y = rng.Next(0, (int)(Screen.height - 1));

                    vector<Vec2> poli = dFig.GetRandomFigure();

                    fig = new Figure(new Vec2(x, y), poli);

                    Spawned = !CheckCollision(ref fig);
                } while (Spawned == false);

                fig_list.Emplace(fig);
            }
        }
        //check collision with the previously spawned obstacles
        public bool CheckCollision(ref Figure fig)
        {
            bool Collision = false;
            for(uint index = 0u; index < fig_list.Length; index++)
            { 
                Collision = Collision || Figure.Collision(fig , fig_list.GetValueAt(index));
            }

            return Collision;
        }

        public void Draw(ref Screen scr)
        {
            for (uint index = 0u; index < fig_list.Length; index++)
            {
                Figure fig = fig_list.GetValueAt(index);
                fig.Draw(scr);
            }
        }

        private vector<Figure> fig_list;
        FigureDictionary dFig;
    }
}
