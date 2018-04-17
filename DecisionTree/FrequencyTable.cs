using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree
{
    public class FrequencyTable
    {
        public FrequencyTable(List<DataInstance> instances, int attributeIndex)
        {
            AttributeIndex = attributeIndex;
            foreach (var instance in instances)
            {
                if (!_table.ContainsKey(instance.Data[attributeIndex])) _table[instance.Data[attributeIndex]] = new Dictionary<int, int>();
                if (!_table[instance.Data[attributeIndex]].ContainsKey(instance.Qualifier)) _table[instance.Data[attributeIndex]][instance.Qualifier] = 0;
                _table[instance.Data[attributeIndex]][instance.Qualifier]++;
                QualifierCount[instance.Qualifier]++;
            }

            AllRowsLeft = instances.Count;
        }

        public int AttributeRowCount(int value)
        {
            if (!_table.ContainsKey(value)) return 0;
            return _table[value].Sum(entry => entry.Value);
        }

        public List<int> ValueQualifierSum(int value)
        {
            var qualifierCount = new List<int>();
            for (var i = 0; i < DataSet.Attributes[DataSet.QualifierIndex].ValueCount; i++)
            {
                qualifierCount.Add(_table.ContainsKey(value) && _table[value].ContainsKey(i) ? _table[value][i] : 0);
            }

            return qualifierCount;
        }

        public readonly int AllRowsLeft;
        public readonly int AttributeIndex;
        private readonly Dictionary<int, Dictionary<int, int>> _table = new Dictionary<int, Dictionary<int, int>>();
        public readonly List<int> QualifierCount = new int[DataSet.Attributes[DataSet.QualifierIndex].ValueCount].ToList();
    }
}
