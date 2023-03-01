using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using Utility;

namespace Geometry
{

    static class Star
    {
	    public static vector<Vec2> Make(int flares, int flare_lenght = 6, int flare_deepness = 3)
        {
           vector<Vec2> poli = new vector<Vec2>(0);
            
            float PI = 3.1415926536f;
            float flare_angle = 360.0f / (float)flares;
            float portrusion_angle = 360.0f / ((float)flares * 2.0f);

            float flare_dist = flare_lenght - flare_deepness;
            for (float angle = 0; angle < 360; angle += flare_angle)
            {
                double cos = Math.Cos(angle * PI / 180.0f);
                double sin = Math.Sin(angle * PI / 180.0f);

                Vec2 pos = new Vec2((int)(cos * (double)flare_lenght), (int)(sin * (double)flare_lenght));
                poli.Emplace(pos);

                cos = Math.Cos((angle + portrusion_angle) * PI / 180.0f);
                sin = Math.Sin((angle + portrusion_angle) * PI / 180.0f);

                pos = new Vec2((int)(cos * (double)flare_dist), (int)(sin * (double)flare_dist));

                poli.Emplace(pos);
            }

            return poli;
        }
    };

    static class Circle
    {
        public static vector<Vec2> Make(int radius)
        {
            Rect rect = new Rect(new Vec2(-radius, radius), new Vec2(radius, -radius));
            vector<Vec2> poligon = new vector<Vec2>(0);

            for (int y = rect.bottom(); y < rect.top; y++)
            {
                for (int x = rect.left ; x < rect.right(); x++)
                {
                    Vec2 point = new Vec2(x, y);
                    double length = point.GetLength();

                    if(length <= radius && length >= radius - 1)
                    {
                        poligon.Emplace(point);
                    }
                }
            }

            return poligon;
      }
    }
}
