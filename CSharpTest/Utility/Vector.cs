using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    class vector<TValue>
    {
        public vector()
        {
            val_list = new TValue[0];
            Capacity = 0;
            aLength = 0;
        }
        public vector(uint size)
        {
            val_list = new TValue[size];
            Capacity = size;
            aLength = size;
        }
        public vector(uint size, TValue InitVal)
        {
            val_list = new TValue[size];

            for (uint ind = 0; ind < size; ind++)
            {
                val_list[ind] = InitVal;
            }
            Capacity = size;
            aLength = size;
        }
        public vector(TValue[] val_list0)
        {
            this.val_list = val_list0;
            Capacity = (uint)val_list0.Length;
            aLength = (uint)val_list0.Length;
        }

        public void Resize(uint new_size)
        {
            Debug.Assert(new_size != aLength);

            TValue[] new_vector;
            aLength = new_size;
            if (new_size > Capacity)
            {
                Capacity = new_size * 2;
                new_vector = new TValue[Capacity];

                for (uint ind = 0; ind < val_list.Length; ind++)
                {
                    new_vector[ind] = val_list[ind];
                }
                val_list = new_vector;
            }
            else
            {

            }
        }
        public void Resize(uint new_size, TValue val)
        {
            Debug.Assert(new_size != aLength);

            TValue[] new_vector;
            aLength = new_size;
            if (new_size > Capacity)
            {
                Capacity = new_size * 2;
                new_vector = new TValue[Capacity];

                for (uint ind = 0; ind < val_list.Length; ind++)
                {
                    new_vector[ind] = val_list[ind];
                }

                {

                    for (uint ind = (uint)val_list.Length; ind < new_size; ind++)
                    {
                        new_vector[ind] = val;
                    }
                }

                val_list = new_vector;
            }
        }
        public void Emplace(TValue val)
        {
            Resize(Length + 1);

            val_list[Length - 1] = val;
        }
        //shifting every single one of the elements by one to the left , deleting the value of the first element and shrinking the vector by 1
        public void Pop()
        {
            Debug.Assert(aLength > 0);

            for(uint ind = 0 ; ind < aLength - 1; ind++) 
            {
                val_list[ind] = val_list[ind + 1];
            }
            aLength--;
        }
        public uint Length
        {
            get
            {
                return aLength;
            }
        }
        //shrinking the vector to its lenght , so that it no longer has the reserved capacity
        public void ShrinkToFit()
        {
            Capacity = aLength;

            TValue[] new_vector = new TValue[aLength];

            for(uint ind = 0; ind < aLength; ind++)
            {
                new_vector[ind] = (TValue)val_list[ind];
            }
            val_list = new_vector;
        }
        public TValue GetValueAt(uint index)
        {
            return (TValue)val_list[index];
        }
        public ref TValue GetRefAt(uint index)
        {
            return ref val_list[index];
        }
        //this is assuming your 1D array is organized in a 2D style
        public ref TValue GetRefAt(Vec2 pos , uint width , uint height)
        {
            uint index = (uint)(pos.y * width + pos.x);
            Debug.Assert(index >= 0 && index <= aLength);
            return ref val_list[index];
        }
        //this is assuming your 1D array is organized in a 2D style
        public TValue GetValueAt(Vec2 pos, uint width, uint height)
        {
            uint index = (uint)(pos.y * width + pos.x);
            Debug.Assert(index >= 0 && index <= aLength);
            return val_list[index];
        }
        public void SetValueAt(Vec2 pos, uint width, uint height , TValue val)
        {
            uint index = (uint)(pos.y * width + pos.x);
            Debug.Assert(index >= 0 && index <= aLength);
            val_list[index] = val;
        }
        public void SetValueAt(uint index , TValue val)
        {
            val_list[index] = val;
        }
        public bool IsEmpty()
        {
            return aLength == 0;
        }
        //sort from the beginning to the end index but NOT including the end index
        public void Sort(uint begin, uint end, Func<TValue, TValue, bool> ComparisonOp)
        {
            for(uint ind = begin + 1; ind < end; ind++) 
            {
                if (ComparisonOp(val_list[ind - 1] , val_list[ind]) == true)
                {
                    TValue aux = val_list[ind];
                    val_list[ind] = val_list[ind - 1];
                    val_list[ind - 1] = aux;
                }
            }
        }

        //sort the list to a user defined operator functor (hope they are called like this C# , i come from C++)
        public void Sort(Func<TValue, TValue, bool> ComparisonOp)
        {
            Sort(1 , aLength ,ComparisonOp);
        }

        //get the first value in the vector by value
        public TValue FrontVal()
        {
            return val_list[0];
        }
        //get the first value in the vector by reference
        public ref TValue FrontRef()
        {
            return ref val_list[0];
        }

        private TValue[] val_list;
        private uint Capacity;
        //array lenght
        private uint aLength;
    }
}
