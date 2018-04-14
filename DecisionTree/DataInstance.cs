using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree
{
    public class DataInstance
    {
        public DataInstance(List<string> row)
        {
            var index = 0;
            row.ForEach(el =>
            {
                Data.Add(DataSet.UpdateColumn(index++, el));
            });
            Qualifier = Data.Last();
        }

        public int Qualifier { get; }

        public readonly List<int> Data = new List<int>();
    }
}
