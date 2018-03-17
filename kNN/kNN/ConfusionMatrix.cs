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
	    public float Accuracy { get; set; }

        public void PrintMatrix()
	    {
	        for (var x = 0; x < _dimensionSize; x++)
	        {
	            for (var y = 0; y < _dimensionSize; y++)
	            {
	                Console.Write(_matrix[x,y]);
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
