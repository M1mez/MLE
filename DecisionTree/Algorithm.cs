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
            var table = DataSet.GetFrequencyTables(data.dataList);
            n.Attribute = HighestGainAttribute(table);

            for (var attributeValue = 0; attributeValue < DataSet.Attributes[n.Attribute].ValueCount; attributeValue++)
            {
                var p = new Path(level);
                var newNode = new Node(n.Attribute) {OriginEdge = attributeValue};
                var newData = new DataBag(data, n.Attribute, attributeValue);
                p.Destination = newNode;
                n.Paths.Add(p);
                ID3(newNode, newData, level+1);
            }
        }


        private static int HighestGainAttribute(List<FrequencyTable> tableList)
        {
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
