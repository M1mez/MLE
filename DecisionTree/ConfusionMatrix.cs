using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree
{
    class ConfusionMatrix
    {
        private int[,] _matrix;
        private readonly int _dimensionSize = DataSet.Attributes[DataSet.QualifierIndex].ValueCount;

        // find longest string in AttributeNames to be able to display matrix properly, + 1 for better distinction between attributes
        private int _longestCatStringLength = DataSet.Attributes.Aggregate("", (max, cur) => max.Length > cur.Name.Length ? max : cur.Name).Length + 1;
        public float Accuracy { get; set; }
        public float RealAccuracy { get; set; }

        public void PrintMatrix()
        {
            var format = "{0,-" + _longestCatStringLength + "}";
            Console.Write(new string(' ', _longestCatStringLength));
            foreach (var qualifier in DataSet.Attributes[DataSet.QualifierIndex].Values) Console.Write(format, qualifier);
            Console.WriteLine();
            for (var x = 0; x < _dimensionSize; x++)
            {
                Console.Write(format, Printer.GetName(DataSet.QualifierIndex, x));
                for (var y = 0; y < _dimensionSize; y++)
                {
                    // display number with leading spaces
                    ConsoleColor newColor;
                    if (x == y) newColor = ConsoleColor.Green;
                    else if (_matrix[x, y] > 0) newColor = ConsoleColor.Red;
                    else newColor = ConsoleColor.Gray;
                    Printer.WriteInStyle(_matrix[x, y].ToString().PadLeft(_longestCatStringLength / 2).PadRight(_longestCatStringLength), newColor);
                }
                Console.WriteLine();
            }

            ConsoleColor accuracyColor = ConsoleColor.White;
            if (Accuracy < 25) accuracyColor = ConsoleColor.DarkRed;
            else if (Accuracy > 75) accuracyColor = ConsoleColor.Green;
            Console.Write("\nAccuracy: ");
            Printer.WriteInStyle($"{Accuracy.ToString("0.00") + " %"}\n\n",accuracyColor);
            //Console.WriteLine($"Real Accuracy: {RealAccuracy.ToString("0.00") + " %"}");
        }

        public ConfusionMatrix()
        {
            var bayes = new Bayes();
            _matrix = new int[_dimensionSize, _dimensionSize];
            var instanceSum = DataSet.Instances.Count;
            float truePos = 0, trueNeg = 0, falsePos = 0, falseNeg = 0;

            foreach (var instance in DataSet.Instances)
            {
                var outcome = bayes.ClassifyDataInstance(instance);
                if (outcome == instance.Qualifier)
                {
                    truePos++;
                    trueNeg += (_dimensionSize - 1);
                }
                else
                {
                    falsePos++;
                    falseNeg++;
                    trueNeg += (_dimensionSize - 2);
                    //Console.WriteLine($"Wrong: {DataSet.Instances.IndexOf(instance)}");
                }

                _matrix[instance.Qualifier, outcome]++;
            }

            RealAccuracy = (100 * truePos / (truePos + falsePos));
            Accuracy = (100 * (truePos + trueNeg) / (truePos + trueNeg + falsePos + falseNeg));
        }
    }
}
