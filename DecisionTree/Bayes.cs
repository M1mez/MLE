using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree
{
    public class Bayes
    {
        public Bayes()
        {
            BuildBayesTable();
        }

        private static int _qualifierCount;
        private static int QualifierCount =>
            _qualifierCount != 0 ? _qualifierCount : (_qualifierCount = DataSet.Attributes[DataSet.QualifierIndex].ValueCount);

        private static List<int> _qualifierAmount;
        private static List<int> QualifierAmount
        {
            get
            {
                if (_qualifierAmount == null)
                {
                    _qualifierAmount = new List<int>();
                    for (var qualifierIndex = 0; qualifierIndex < QualifierCount; qualifierIndex++)
                    {
                        _qualifierAmount.Add((from instance in DataSet.Instances where instance.Qualifier == qualifierIndex select instance).Count());
                    }
                }

                return _qualifierAmount;
            }
        }

        private static double QualifierProbability(int qualifierIndex) => (double)QualifierAmount[qualifierIndex] / QualifierAmount.Sum();

        private static List<FrequencyTable> _frequency;
        private static List<FrequencyTable> Frequency =>
            _frequency ?? (_frequency = FrequencyTable.GetFrequencyTables(DataSet.Instances));


        private static List<Attribute> Attributes => DataSet.Attributes;


        public int ClassifyDataInstance(DataInstance instance)
        {
            var fractionParts = new List<double>();

            for (var qualifierIndex = 0; qualifierIndex < QualifierCount; qualifierIndex++)
            {
                if (fractionParts.Count <= qualifierIndex) fractionParts.Add(1.0d);
                for (var attributeIndex = 0; attributeIndex < Attributes.Count-1; attributeIndex++)
                {
                    var valueIndex = instance.Data[attributeIndex];
                    var tempBayesEntry = BayesTable[attributeIndex][valueIndex][qualifierIndex];

                    fractionParts[qualifierIndex] *= (double) tempBayesEntry.Key / tempBayesEntry.Value;
                }
                fractionParts[qualifierIndex] *= QualifierProbability(qualifierIndex);
            }
            
            var outcomeList = new List<double>();

            foreach (var part in fractionParts)
            {
                outcomeList.Add(part / fractionParts.Sum());
            }

            return outcomeList.IndexOf(outcomeList.Max());
        }


        private void BuildBayesTable()
        {
            foreach (var attributeFrequency in Frequency)
            {
                var attributeIndex = attributeFrequency.AttributeIndex;
                if (!BayesTable.ContainsKey(attributeIndex)) BayesTable.Add(attributeIndex, new Dictionary<int, Dictionary<int, KeyValuePair<int, int>>>());

                for (var valueIndex = 0; valueIndex < Attributes[attributeIndex].ValueCount; valueIndex++)
                {
                    if (!BayesTable[attributeIndex].ContainsKey(valueIndex)) BayesTable[attributeIndex].Add(valueIndex, new Dictionary<int, KeyValuePair<int, int>>());

                    for (var qualifierIndex = 0; qualifierIndex < QualifierCount; qualifierIndex++)
                    {
                        var newPair = new KeyValuePair<int, int>(
                            attributeFrequency.Table[valueIndex].ContainsKey(qualifierIndex)
                                ? attributeFrequency.Table[valueIndex][qualifierIndex] 
                                : 0,
                            QualifierAmount[qualifierIndex]
                        );
                        
                        
                        var zaehler = attributeFrequency.Table[valueIndex].ContainsKey(qualifierIndex) ?
                            attributeFrequency.Table[valueIndex][qualifierIndex] : 0;
                        var nenner = QualifierAmount[qualifierIndex];

                        BayesTable[attributeIndex][valueIndex][qualifierIndex] = newPair; //(double) zaehler / nenner;
                    }
                }
            }
        }

        public void PrintLikelihoodTable() => Printer.PrintLikelihoodTable(BayesTable, QualifierAmount.Sum());

        private Dictionary<int, Dictionary<int, Dictionary<int, KeyValuePair<int, int>>>> BayesTable = new Dictionary<int, Dictionary<int, Dictionary<int, KeyValuePair<int, int>>>>();

        
    }
}
