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
	    private int _longestAttributeLength = DataSet.AttributeNames.Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur).Length + 1;
	    public float Accuracy { get; set; }

        public void PrintMatrix()
	    {
            //print space x times
            Console.Write(new string(' ', _longestAttributeLength));
            foreach (var att in DataSet.AttributeNames) Console.Write(att);

	        for (var x = 0; x < _dimensionSize; x++)
	        {
	            Console.Write(DataSet.AttributeNames[x]);
	            for (var y = 0; y < _dimensionSize; y++)
	            {
                    // display number with leading spaces
	                Console.Write(_matrix[x,y].ToString("D" + _longestAttributeLength.ToString()));
	            }
	            Console.WriteLine();
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
		    Accuracy = (truePos + trueNeg) / (truePos + trueNeg + falsePos + falseNeg);
        }
	}
}
