// See https://aka.ms/new-console-template for more information
using System.Formats.Asn1;
using System.Linq.Expressions;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Windows.Input;

using Utility;
using Graphics;
using Obstacle;

class Sus
{
    public
    static void Sussy()
    {

        Console.Write("Hello World");
        Console.Write("Hello World");
        Console.Write("Hello World");
        Console.Write("Hello World");
        Console.Write("Hello World");
        Console.Write("Hello World");
    }
}

static class Conversion
{
    public static int? StringToInt32(string s)
    {
        int number = 0;
        for(int index = 0 ; index < s.Length; index++)
        {
            try
            {
                char ch = s[index];
                int digit = (int)ch;

                if (digit >= 48 && digit <= 57)
                {
                    number = (digit - 48) + number * 10;
                }
                else
                {
                    throw new Exception("YOU HAVEN'T PASSED A NUMBER");
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }
        return number;
    }

}
namespace Program
{
class Program
{
    static class Menu
    {
        public static int GetNumber()
        {
            string? num = null;
            do
            {
                num = Console.ReadLine();
                if (num.Length == 0)
                {
                    Console.WriteLine("That is not a number");
                }

            } while (num.Length == 0);

            int? number = Conversion.StringToInt32(num);
            if (number == null)
            {
                return GetNumber();
            }
            else
            {
                return (int)number;
            }
        }
        public static Vec2 GetPoint()
        {
            int x = 0, y = 0;

            Console.WriteLine("Specify the x of the obstacle:");


            x = GetNumber();

            Console.WriteLine("Specify the y of the obstacle:");

            y = GetNumber();

            Vec2 point = new Vec2(x, y);

            return point;
        }
    }
    static void Main(string[] args)
    {
        Screen scr = new Screen();
        ObstacleSpawner obs_spawner = new ObstacleSpawner(10);
        Console.SetWindowSize(Convert.ToInt32(Screen.width + Screen.border_thickness * 2), Convert.ToInt32(Screen.height + Screen.border_thickness * 2));

        Vec2[] list = { new Vec2(0, 1), new Vec2(0, 2) };
        Figure fig = new Figure( new Vec2(3 , 4) , list );

        Random random= new Random();
        Vec2 sPos;
        Vec2 sDest;
         do
         {
             sPos = new Vec2(random.Next(0, (int)Screen.width), random.Next(0, (int)Screen.height));
             sDest = new Vec2(random.Next(0, (int)Screen.width), random.Next(0, (int)Screen.height));
         } while (sPos.GetLength(sDest) <= 5.0f);

        Searcher searcher = new Searcher(sPos, sDest, "Ciobanu");
        obs_spawner.Draw(ref scr);
        searcher.Search(ref scr);
        while (true)
        {

            if (scr.WasUpdated)
            {
                scr.WasUpdated = false;
                Console.Clear();
                scr.Draw();
            }
            Console.WriteLine();
            Console.WriteLine("Press 'E'   to place an obstacle");
            Console.WriteLine("Press 'ESC' to generate new obstacles");

            Console.WriteLine();

            ConsoleKey key;

            do
            {
                key = Console.ReadKey().Key;

            } while (key != ConsoleKey.E && key != ConsoleKey.Escape);
            
            if(key == ConsoleKey.E )
            {
                Vec2 point = Menu.GetPoint();
                scr.ChangeChar(point, 'O');
            }
            else if(key == ConsoleKey.Escape)
            {
                try
                {
                    Console.WriteLine("HHow many RANDOM obstacles do you want?");
                    int nObs = Menu.GetNumber();
                    if(nObs < 0)
                    {
                         throw new Exception("thats a negative number dawg");
                    }
                    else
                    {
                        obs_spawner = new ObstacleSpawner((uint)nObs);
                        obs_spawner.Draw(ref scr);
                        searcher.Search(ref scr);
                    }
                }
                catch(Exception e)
                {
                        Console.WriteLine(e.Message);
                }
            }
        }
    }
}
}

