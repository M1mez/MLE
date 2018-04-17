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
                if (!Table.ContainsKey(instance.Data[attributeIndex]))
                {
                    Table[instance.Data[attributeIndex]] = new Dictionary<int, int>();
                }

                if (!Table[instance.Data[attributeIndex]].ContainsKey(instance.Qualifier))
                {
                    Table[instance.Data[attributeIndex]][instance.Qualifier] = 0;
                }

                Table[instance.Data[attributeIndex]][instance.Qualifier]++;
                QualifierCount[instance.Qualifier]++;
            }

            AllRowsLeft = instances.Count;
        }

        public int AttributeRowCount(int value)
        {
            if (!Table.ContainsKey(value)) return 0;
            return Table[value].Sum(entry => entry.Value);
        }

        public List<int> ValueQualifierSum(int value)
        {
            var qualifierCount = new List<int>();
            for (var i = 0; i < DataSet.Attributes[DataSet.QualifierIndex].ValueCount; i++)
            {
                qualifierCount.Add(Table.ContainsKey(value) && Table[value].ContainsKey(i) ? Table[value][i] : 0);
            }

            return qualifierCount;
        }

        public int AllRowsLeft;
        public int AttributeIndex;
        public Dictionary<int, Dictionary<int, int>> Table = new Dictionary<int, Dictionary<int, int>>();
        public List<int> QualifierCount = new int[DataSet.Attributes[DataSet.QualifierIndex].ValueCount].ToList();
    }
}
