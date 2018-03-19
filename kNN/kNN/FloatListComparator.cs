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
            double dist1 = GetDist(one.DataVector);
            double dist2 = GetDist(two.DataVector);

            if (dist1 > dist2) return 1;
            if (dist1 < dist2) return -1;
            return 0;
        }

        //since only float[] are possible as parameters, this overloaded method is not needed
        //public int Compare(string x, string y) => Compare(new List<string>() { x }, new List<string>() { y });

        private double GetDist(float[] vector)
        {
            //var addPow = vector.Select((t, i) => Math.Pow(t - candidate.DataVector[i], 2)).Sum();
            var addPow = vector.Select((t, i) => (t - candidate.DataVector[i])*(t - candidate.DataVector[i])).Sum();
            return Math.Sqrt(addPow);
        }

        public static float SingleDist(DataInstance unknown, DataInstance known)
        {
            float addPow = 0;
            for (var i = 0; i < known.DataVector.Count(); i++)
            {
                addPow += (unknown.DataVector[i] - known.DataVector[i]) * (unknown.DataVector[i] - known.DataVector[i]);
            }
            return (float) Math.Sqrt(addPow);
        }
    }
}
