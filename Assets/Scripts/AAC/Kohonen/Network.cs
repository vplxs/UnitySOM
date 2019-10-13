using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AAC.Kohonen
{
    public class Network
    {
        public int startDimensions;
        public int endDimensions;
        public float learningRate = 0.01f;
        public float window = 0.2f;

        public float[] itemPerDimension;

    }
}
