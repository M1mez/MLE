using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kNN
{
	class ConfusionMatrix
	{
	    private int[,] _matrix;
	    private readonly int _dimensionSize = DataSet.Categories.Count;

        // find longest string in AttributeNames to be able to display matrix properly, + 1 for better distinction between attributes
	    private int _longestCatStringLength = DataSet.Categories.Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur).Length + 1;
	    public string Accuracy { get; set; }
	    public string RealAccuracy { get; set; }

        public void PrintMatrix()
	    {
	        var format = "{0,-" + _longestCatStringLength + "}";
	        //print space x times
	        Console.Write(new string(' ', _longestCatStringLength));
	        foreach (var att in DataSet.Categories) Console.Write(format, att);
	        Console.WriteLine();
	        for (var x = 0; x < _dimensionSize; x++)
	        {
	            Console.Write(format, DataSet.Categories[x]);
	            for (var y = 0; y < _dimensionSize; y++)
	            {
	                // display number with leading spaces
	                if (x == y) Console.ForegroundColor = ConsoleColor.Green;
                    else if (_matrix[x, y] > 0) Console.ForegroundColor = ConsoleColor.DarkRed;
	                else Console.ForegroundColor = ConsoleColor.Gray;
	                Console.Write(_matrix[x, y].ToString().PadLeft(_longestCatStringLength / 2).PadRight(_longestCatStringLength));
	            }
	            Console.WriteLine();
	            Console.ForegroundColor = ConsoleColor.White;
	        }
	    }

        public ConfusionMatrix(DataSet dataSet)
		{
		    _matrix = new int[_dimensionSize, _dimensionSize];
		    var instanceSum = dataSet.DataInstances.Count;
		    float truePos = 0, trueNeg = 0, falsePos = 0, falseNeg = 0;

		    foreach (var instance in dataSet.DataInstances)
		    {
		        if (instance.GuessedCategory == instance.TrueCategory)
		        {
		            truePos++;
		            trueNeg += (_dimensionSize - 1);
		        }
		        else
		        {
		            falsePos++;
		            falseNeg++;
		            trueNeg += (_dimensionSize - 2);
		        }

		        _matrix[instance.TrueCategory, instance.GuessedCategory]++;
		    }

		    RealAccuracy = (100 * truePos / (truePos + falsePos)).ToString("0.00") + " %"; 
		    Accuracy = (100 * (truePos + trueNeg) / (truePos + trueNeg + falsePos + falseNeg)).ToString("0.00") + " %";
        }
	}
}
