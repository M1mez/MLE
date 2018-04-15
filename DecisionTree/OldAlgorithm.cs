using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace DecisionTree
{
    class AOldAlgorithm
    {
        public void ID3(Node n, List<DataInstance> dataBag)
        {
            n.Attribute = HighestGainAttribute(dataBag);
            var firstElementQualifier = dataBag.First().Qualifier;
            n.IsLeaf = dataBag.All(instance => instance.Qualifier == firstElementQualifier);
            if (n.IsLeaf) return;
            for (var index = 0; index < DataSet.Attributes[n.Attribute].Values.Count; index++)
            {
                var newNode = new Node(index);
                //n.paths.Add(newNode);
                newNode.OriginEdge = index;
                ID3(newNode, dataBag.Where(x => x.Data[n.Attribute] == index).ToList());
            }
        }


        private static int HighestGainAttribute(List<DataInstance> dataBag)
        {
            var attribCount = DataSet.Attributes.Count - 1;
            KeyValuePair<int, double> highestGain = new KeyValuePair<int, double>();
            var splitdataBag = SplitDataBag(dataBag, DataSet.QualifierIndex, DataSet.Attributes[DataSet.QualifierIndex].ValueCount);
                //dataBag.GroupBy(instance => instance.Qualifier).ToList();
            List<int> qualifierCount = new int[DataSet.Attributes.Last().ValueCount].ToList();

            for (var index = 0; index < qualifierCount.Count; index++)
            {
                qualifierCount[index] = splitdataBag[index].Count();
            }

            var entropy = Entropy(dataBag.Count, qualifierCount);

            for (int i = 0; i < attribCount; i++)
            {
                var currGain = Gain(entropy, dataBag, i);
                if (highestGain.Value < currGain)
                {
                    highestGain = new KeyValuePair<int, double>(i, currGain);
                }
            }

            return highestGain.Key;

        }

        private static List<List<DataInstance>> SplitDataBag(List<DataInstance> dataBag, int attribute, int attributeValueCount)
        {
            var splitDataBagAttribsMissing = dataBag.GroupBy(instance => instance.Data[attribute]);
            var splitDataBagComplete = new List<List<DataInstance>>();
            for (int i = 0; i < attributeValueCount; i++)
            {
                splitDataBagComplete.Add(new List<DataInstance>());
            }

            foreach (var splitList in splitDataBagAttribsMissing)
            {
                splitDataBagComplete[splitList.Key].AddRange(splitList);
            }

            return splitDataBagComplete;
        }

        private static double Gain(double entropy, List<DataInstance> dataBag, int attribute)
        {
            List<int> qualifierCount;
            var allAmount = dataBag.Count;
            int splitAmount;
            var attributeValueCount = DataSet.Attributes[attribute].ValueCount;

            var splitDataBag = SplitDataBag(dataBag, attribute, attributeValueCount);

            for (var index = 0; index < attributeValueCount; index++)
            {
                splitAmount = splitDataBag[index].Count();
                qualifierCount = new int[DataSet.Attributes.Last().ValueCount].ToList();
                foreach (var attribInstance in splitDataBag[index])
                {
                    qualifierCount[attribInstance.Qualifier]++;
                }

                entropy -= ((double) splitAmount / allAmount )* Entropy(splitAmount, qualifierCount);
            }

            return entropy;
        }

        private static double Entropy(int amount, List<int> qualifierCount)
        {
            double entropy, fraction, outcome;
            entropy = 0.0d;

            qualifierCount.ForEach(qual =>
            {
                fraction = (double) qual / amount;
                outcome = fraction * Math.Log(fraction, 2.0d);
                entropy -= double.IsNaN(outcome) ? 0 : outcome;
            });
            return entropy;
        }
    }
}
