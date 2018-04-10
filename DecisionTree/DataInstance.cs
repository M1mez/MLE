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
                _row.Add(DataSet.UpdateColumn(index++, el));
            });
        }

        public List<int> _row = new List<int>();
    }
}
