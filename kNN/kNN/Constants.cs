using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kNN
{
    public static class Constants
    {
        public static Type attrType = typeof(float);

        public static int RandomInt<T>(List<T> list)
        {
            Random rnd = new Random();
            return rnd.Next(list.Count);
        }
    }
}
