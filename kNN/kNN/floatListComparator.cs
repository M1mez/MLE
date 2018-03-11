using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kNN
{
    public class FloatListComparator : IComparer<string>
    {
        private List<string> candidate;

        public FloatListComparator(List<string> candidate)
        {
            this.candidate = candidate.Take(candidate.Count - 1).ToList();
        }

        public int Compare(List<string> one, List<string> two)
        {
            double dist1 = getDist(one);
            double dist2 = getDist(two);

            if (dist1 > dist2) return 1;
            if (dist1 < dist2) return -1;
            return 0;
        }

        public int Compare(string x, string y) => Compare(new List<string>() { x }, new List<string>() { y });

        private double getDist(List<string> list)
        {
            double addPow = 0.0d;
            for (int i = 0; i < candidate.Count - 1; i++)
            {
                addPow += Math.Pow(float.Parse(list[i]) - float.Parse(candidate[i]), 2);
            }
            return Math.Sqrt(addPow);
        }
    }
}
