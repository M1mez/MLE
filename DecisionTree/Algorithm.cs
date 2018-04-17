using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree
{
    class Algorithm
    {
        public static void ID3(Node n, DataBag data, int level = 1)
        {
            if (DataSet.MaxLevel < level) DataSet.MaxLevel = level;
            n.IsLeaf = data.IsAtomic;
            if (n.IsLeaf)
            {
                n.Qualifier = data.HighestQualifierCount();
                return;
            }
            n.Attribute = HighestGainAttribute(data);

            for (var attributeValue = 0; attributeValue < DataSet.Attributes[n.Attribute].ValueCount; attributeValue++)
            {
                var newNode = new Node(n.Attribute) {OriginEdge = attributeValue};
                var newData = new DataBag(data, n.Attribute, attributeValue);
                n.Paths.Add(newNode);
                ID3(newNode, newData, level+1);
            }
        }
        
        private static int HighestGainAttribute(DataBag data)
        {
            var tableList = GetFrequencyTables(data.dataList);
            if (tableList == null) throw new ArgumentNullException(nameof(tableList));

            var attribCount = DataSet.Attributes.Count - 1;
            var highestGain = new KeyValuePair<int, double>();
            
            var sum = tableList.First().AllRowsLeft;
            var qualifierCount = tableList[0].QualifierCount;
            var entropy = Entropy(sum, qualifierCount);
            
            for (var i = 0; i < attribCount; i++)
            {
                var currGain = Gain(entropy, tableList[i]);
                if (highestGain.Value < currGain)
                {
                    highestGain = new KeyValuePair<int, double>(i, currGain);
                }
            }
            return highestGain.Key;
        }

        private static List<FrequencyTable> GetFrequencyTables(List<DataInstance> instances)
        {
            var list = new List<FrequencyTable>();
            for (var index = 0; index < DataSet.Attributes.Count - 1; index++)
            {
                list.Add(new FrequencyTable(instances, index));
            }

            return list;
        }

        private static double Gain(double entropy, FrequencyTable table)
        {
            var allInstancesLeftCount = table.AllRowsLeft;
            var attributeValueCount = DataSet.Attributes[table.AttributeIndex].ValueCount;

            for (var value = 0; value < attributeValueCount; value++)
            {
                var qualifierCount = table.ValueQualifierSum(value);
                var attributesRowAmount = table.AttributeRowCount(value);
                    
                entropy -= ((double)attributesRowAmount / allInstancesLeftCount) * Entropy(attributesRowAmount, qualifierCount);
            }

            return entropy;
        }

        private static double Entropy(int amount, List<int> qualifierCount)
        {
            double fraction, outcome;
            var entropy = 0.0d;

            qualifierCount.ForEach(qual =>
            {
                fraction = (double)qual / amount;
                outcome = fraction * Math.Log(fraction, 2.0d);
                entropy -= double.IsNaN(outcome) ? 0 : outcome;
            });
            return entropy;
        }
    }
}
