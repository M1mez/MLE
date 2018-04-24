using System;
using System.Collections.Generic;
using System.Text;

namespace NN_staticFunctions
{
    public static class Functions
    {
        private static readonly Random Rnd = new Random();
        public static double RandomBias() => Rnd.NextDouble() * 2 - 1;
        
    }
}
