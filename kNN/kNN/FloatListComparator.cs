using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kNN
{
    public class FloatListComparator : IComparer<DataInstance>
    {
        private DataInstance candidate;

        public FloatListComparator(DataInstance candidate)
        {
            this.candidate = candidate;
        }

        public int Compare(DataInstance one, DataInstance two)
        {
            double dist1 = GetDist(one.normalizedDataVector);
            double dist2 = GetDist(two.normalizedDataVector);

            if (dist1 > dist2) return 1;
            if (dist1 < dist2) return -1;
            return 0;
        }

        //since only float[] are possible as parameters, this overloaded method is not needed
        //public int Compare(string x, string y) => Compare(new List<string>() { x }, new List<string>() { y });

        private double GetDist(float[] vector)
        {
            var addPow = vector.Select((t, i) => Math.Pow(t - candidate.normalizedDataVector[i], 2)).Sum();
            return Math.Sqrt(addPow);
        }
    }
}
