using Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Algorithm
{
    static class AStarAlgorithm
    {
        public class Node
        {
            public Node(Vec2 pos0)
            {
                pos = pos0;
                IsObstacle = false;
                DistanceToGoal    = (double)IntExtension.PositiveInfinity;
                TraversedDistance = (double)IntExtension.PositiveInfinity;
                Verrified = false;
                neighbours = new vector<Vec2>();
                parent = new Vec2(-1, -1);
            }
            public Node(Vec2 pos0, bool IsObs)
            {
                pos = pos0;
                IsObstacle = IsObs;
                DistanceToGoal    = (double)IntExtension.PositiveInfinity;
                TraversedDistance = (double)IntExtension.PositiveInfinity;
                Verrified = false;
                neighbours = new vector<Vec2>();
                parent = new Vec2(-1, -1);
            }
            public static double heuristic(ref Node node1 , ref Node node2)
            {
                Vec2 pos1 = node1.Pos;
                Vec2 pos2 = node2.Pos;

                double dist = pos1.GetLength(pos2);

                return dist;
            }
            //this is strictly for the algorithm
            public static bool Compare(Node lhs , Node rhs)
            { 
                return lhs.DistanceToGoal < rhs.DistanceToGoal;
            }
            public static bool operator !=(Node lhs, Node rhs)
            {
                return (lhs.Pos != rhs.Pos);
            }
            public static bool operator==(Node lhs , Node rhs)
            {
                return (lhs.Pos == rhs.Pos);
            }
            public vector<Vec2> Neighbours
            {
                get
                {
                    return neighbours;
                }
                set
                {
                    neighbours = value;
                }
            }
            public bool ObstacleStatus
            {
                get
                {
                    return IsObstacle;
                }
                set
                {
                    IsObstacle = value;
                }
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
            public double DistanceToGoal;
            public double TraversedDistance;
            private vector<Vec2> neighbours;
            public Vec2 parent = null;
            private Vec2 pos;
            private bool IsObstacle;
            public bool Verrified;
        }
        //this function works assuming the node list covers the whole screen , and not a reserved area
        public static void FindNeighbours(ref Node target_node , ref Screen scr , ref vector<Node> node_list) 
        {
            //checking above
            vector<Vec2> neighbours = new vector<Vec2>(0);
            Vec2 pos;
            Vec2 obj = target_node.Pos;

            //checking above
            //for (int x = -1; x <= 1; x++)
            {
                pos = new Vec2(obj.x , obj.y - 1);

                if(PositionAvailable(pos , ref scr))
                {
                    neighbours.Emplace(pos);
                }      
            }
               
            // checking left
            {
                pos = new Vec2(obj.x - 1, obj.y);

                if (PositionAvailable(pos, ref scr))
                {
                    neighbours.Emplace(pos);
                }
            }
            //checking right
            {
                pos = new Vec2(obj.x + 1, obj.y);

                if (PositionAvailable(pos, ref scr))
                {
                    neighbours.Emplace(pos);
                }
            }
            //checking under
           // for (int x = -1; x <= 1; x++)
            {
                pos = new Vec2(obj.x , obj.y + 1);

                if (PositionAvailable(pos, ref scr))
                {
                    neighbours.Emplace(pos);
                }
            }

            target_node.Neighbours = neighbours;
        }
        private static void UpdateUntestedList(ref vector<Node> untested_nodes , ref vector<Node> node_list)
        {
            for(uint ind = 0u; ind < untested_nodes.Length; ind++)
            {
                ref Node node = ref untested_nodes.GetRefAt(ind);

                Vec2 pos = node.Pos;
                //array index
                uint aIndex = (uint)(pos.y * Screen.width + pos.x);

                node = ref node_list.GetRefAt(aIndex);
            }
        }
        public static bool PositionAvailable(Vec2 obj , ref Screen scr)
        {
            if (Screen.InBounds(obj))
            {
                return scr.GetChar(obj) == ' ';
            }
            return false;
        }
        static public vector<Vec2> Apply(Vec2 start , Vec2 end , ref Screen scr)
        {
            Debug.Assert(Screen.InBounds(start));
            Debug.Assert(Screen.InBounds(end));

            //im declaring every single character in the "Screen" class as a node , a tile which our algorithm could move by and determine a path
            vector<Node> node_list = new vector<Node>(Screen.width * Screen.height);
            
            //initializing the node_list
            for(int y = 0; y < Screen.height; y++)
            {
                for(int x = 0; x < Screen.width; x++)
                {
                    int index = y * (int)Screen.width + x;
                    Debug.Assert(index >= 0 && index < Screen.width * Screen.height);

                    Vec2 pos = new Vec2(x, y);
                    bool IsObs = !(scr.GetChar(pos) == ' ');

                    node_list.SetValueAt((uint)index , new Node(pos , IsObs));
                }
            }

            //setting the neighbours for every node
            /*the big fat piece of garbage that makes my head burst open is the fact tha C#
             * doesn't allow pointers or even REFERENCE ARRAYS which will make this algorithm waaaaaaay slower
             than it should be because I would always have to pass by value ,but also make it tedious for me to code because I would need to update some 
            future lists like a mad man*/
            for (int y = 0; y < Screen.height; y++)
            {
                for (int x = 0; x < Screen.width; x++)
                {
                    int index = y * (int)Screen.width + x;
                    Debug.Assert(index >= 0 && index < Screen.width * Screen.height);

                    ref Node node = ref node_list.GetRefAt((uint)index);
                    FindNeighbours(ref node , ref scr , ref node_list);
                }
            }

            //start point index in the array
            uint sIndex = (uint)(start.y * Screen.width + start.x);
            ref Node StartNode = ref node_list.GetRefAt(sIndex);
            StartNode.TraversedDistance = 0;

            //end point index in the array
            uint eIndex = (uint)(end.y * Screen.width + end.x);
            ref Node EndNode = ref node_list.GetRefAt(eIndex);
            StartNode.DistanceToGoal = (int)Node.heuristic(ref StartNode, ref EndNode);


            vector <Vec2> untested_nodes = new vector<Vec2>();
            untested_nodes.Emplace(StartNode.Pos);

            //since i can't use references I will use the base array as a sort data base , my nodes have their screen pos which is also their pos
            // in the node array , and the node array will ALWAYS be the one updated , from the node array we would get information 
            while(untested_nodes.IsEmpty() == false)
            {
                //mental check
                for(uint ind = 0u; ind < untested_nodes.Length; ind++)
                {
                    Node check = node_list.GetValueAt(untested_nodes.GetValueAt(ind), Screen.width, Screen.height);
                }
                //sorting by the shortest distance to the goal
               for(uint ind = 1; ind < untested_nodes.Length; ind++)
               {
                   ref Node node1 = ref node_list.GetRefAt(untested_nodes.GetValueAt(ind - 1), Screen.width, Screen.height);
                   ref Node node2 = ref node_list.GetRefAt(untested_nodes.GetValueAt(ind)    , Screen.width, Screen.height);
               
                   if(node1.DistanceToGoal > node2.DistanceToGoal)
                   {
                       //swapping the values
                       Vec2 aux = untested_nodes.GetValueAt(ind);
                       untested_nodes.SetValueAt(ind    , untested_nodes.GetValueAt(ind - 1));
                       untested_nodes.SetValueAt(ind - 1, aux);
                   }
               }
                //in case there is a SEQUENCE of verrified untested_nodes , we remove them all
                bool remove = true;
                while(remove)
                {
                    ref Node aux = ref node_list.GetRefAt(untested_nodes.FrontVal(), Screen.width, Screen.height);
                    remove = aux.Verrified;
                    if (remove == true)
                    {
                        untested_nodes.Pop();
                    }
                    else
                    {
                        break;
                    }
                    if (untested_nodes.IsEmpty() == true)
                    {
                        break;
                    }
                }

                //we've just removed elements from the list , and if it was the only one left , we should stop the algorithm
                if(untested_nodes.IsEmpty() == true)
                {
                    break;
                }

                //the node we shall test
                ref Node node = ref node_list.GetRefAt(untested_nodes.FrontRef() , Screen.width , Screen.height);

                node.Verrified = true;

                for(uint ind = 0u; ind < node.Neighbours.Length ; ind++) 
                {
                    ref Node neighbour = ref node_list.GetRefAt(node.Neighbours.GetValueAt(ind) , Screen.width , Screen.height);
                    if(neighbour.Verrified || neighbour.ObstacleStatus == true)
                    {
                        continue;
                    }
                    else
                    {
                        untested_nodes.Emplace(neighbour.Pos);
                        double PossibleShorterDist = node.TraversedDistance + Node.heuristic(ref neighbour, ref node);

                        if(PossibleShorterDist < neighbour.TraversedDistance)
                        {
                            neighbour.parent = node.Pos;
                            neighbour.TraversedDistance = Math.Round(PossibleShorterDist);
                            neighbour.DistanceToGoal = neighbour.TraversedDistance + Math.Round(Node.heuristic(ref neighbour, ref EndNode));
                        }
                    }
                }
            }

            vector<Vec2> path = new vector<Vec2>();
            while(EndNode.parent != new Vec2(-1 , -1))
            {
                path.Emplace(EndNode.parent);
                EndNode = node_list.GetValueAt(EndNode.parent, Screen.width, Screen.height);
            }

            return path;
        }
    }
}
