using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree
{
    class Algorithm
    {
        public void ID3(Node n, DataBag data)
        {
            //Abbruchbedingungen:
            //  1. Pure Decision? 
            //      Qualifier alle selben Wert? => Node = Blatt
            //  2. keine Attribute übrig?
            //      Nach Häufigkeit der QUalifier entscheiden => Node = Blatt
            n.IsLeaf = data.IsAtomic;
            if (n.IsLeaf)
            {
                n.EndQualifier = data.HighestQualifierCount();
                return;
            }

            //Ablauf:
            // 1. neue Node erstellen TODO nicht nötig?
            // 2. Berechne HighestGain von der Tabelle, Attribut wird dann im neuen Node gesetzt
            var Table = DataSet.GetFrequencyTables(data.dataList);
            n.Attribute = HighestGainAttribute(Table);
            // 3. Pro möglichem Wert im Attribut:
            //      eine neue Node zu paths hinzufügen
            //      Alle Instanzen aus DataBag entfernen, die nicht diesen Value im Attribut haben
            //      damit ID3 neu aufrufen
            for (var attributeValue = 0; attributeValue < DataSet.Attributes[n.Attribute].ValueCount; attributeValue++)
            {
                n.paths.Add(new Path(attributeValue));
                var newNode = new Node(n.Attribute) {originEdge = attributeValue};
                var newData = new DataBag(data, n.Attribute, attributeValue);
                n.paths[attributeValue].Destination = newNode;
                ID3(newNode, newData);
            }
        }


        private static int HighestGainAttribute(List<FrequencyTable> tableList)
        {
            var attribCount = DataSet.Attributes.Count - 1;
            KeyValuePair<int, double> highestGain = new KeyValuePair<int, double>();

            // Menge aller aktuellen Instanzen festlegen:
            var sum = tableList.First().AllRowsLeft; //tableList.Sum(table => table.AllRowsLeft);
            // Aufteilung der Qualifier feststellen:
            var qualifierCount = tableList[0].QualifierCount;//DataSet.GetEmptyQualifierCount;
            /*tableList.ForEach(table =>
            {
                for (var i = 0; i < table.QualifierCount.Count; i++)
                {
                    qualifierCount[i] += table.QualifierCount[i];
                }
            });*/

            //Allgemeine Entropy für aktuelles Attribut ausrechnen
            var entropy = Entropy(sum, qualifierCount);

            //Gain für jedes Attribut durchführen und vergleichen
            for (int i = 0; i < attribCount; i++)
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

            //var splitDataBag = SplitDataBag(dataBag, attribute, attributeValueCount);

            for (var value = 0; value < attributeValueCount; value++)
            {
                var qualifierCount = table.ValueQualifierSum(value);
                var attributesRowAmount = table.AttributeRowCount(value);
                /*splitAmount = splitDataBag[index].Count();
                foreach (var attribInstance in splitDataBag[index])
                {
                    qualifierCount[attribInstance.Qualifier]++;
                }*/
                    
                entropy -= ((double)attributesRowAmount / allInstancesLeftCount) * Entropy(attributesRowAmount, qualifierCount);
            }

            return entropy;
        }

        private static double Entropy(int amount, List<int> qualifierCount)
        {
            double entropy, fraction, outcome;
            entropy = 0.0d;

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
