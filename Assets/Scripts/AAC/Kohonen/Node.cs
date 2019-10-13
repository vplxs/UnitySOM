using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AAC.Kohonen
{
    public class Node
    {
        public int startDimensions { get; set; }
        public int endDimensions { get; set; }

        public float[] weight;
        public int[] id;

        public Node(int startDimensions, int endDimensions)
        {
            id = new int[endDimensions];
            for (int i=0; i<id.Length; i++)
            {
                id[i] = -1;
            }
        }
    }
}
