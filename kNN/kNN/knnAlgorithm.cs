using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kNN
{
	class KnnAlgorithm
	{
		public KnnAlgorithm(DataSet data)
		{
			fullDataSet = data;
			//this.Normalize();


			//At Least one Element of each Category should be in each K
			int minOfCategoryCounter = 10;
			foreach (var categoryList in DataSet.CategoryInstances)
			{
				if (categoryList.Count < minOfCategoryCounter)
				{
					minOfCategoryCounter = (categoryList.Count);
				}
			}
			this.PrepareKFoldCrossValidation(minOfCategoryCounter);
		}

		/// <summary>
		/// Cross Validate using the prepared kFolds
		/// </summary>
		public void TestData()
		{
			foreach(var testblock in kFoldPackages)
			{
			    var restData =  fullDataSet.DataInstances.Except(testblock).ToList();
                foreach (var candidate in testblock)
			    {
                    var comp = new FloatListComparator(candidate);
			        restData.Sort(comp);
			        var first10Instances = restData.Take(10).ToList();
                    candidate.GuessedCategory = MostCommonCategory(first10Instances);
                }
			}
		}

	    private int MostCommonCategory(List<DataInstance> list)
	    {
            var mostListedCat = new Dictionary<int, int>();
            list.ForEach(i =>
            {
                if (!mostListedCat.ContainsKey(i.TrueCategory)) mostListedCat[i.TrueCategory] = 1;
                mostListedCat[i.TrueCategory]++;
            });
	        return mostListedCat.FirstOrDefault(x => x.Value == mostListedCat.Values.Max()).Key;
	    }

		/// <summary>
		/// Mapps all values in fullDataSet between 0 and 1;
		/// Simple Rescaling is used.
		/// </summary>
		private void Normalize()
		{
			int coloumns = DataSet.ColoumnCount - 1;

			//min and max for each attribute coloumn.
			float[] min = new float[coloumns];
			float[] max = new float[coloumns];

			//get min and max values for each coloumn
			foreach (DataInstance row in fullDataSet.DataInstances)
			{
				for (int i = 0; i < coloumns; i++)
				{
					if (row.DataVector[i] < min[i])
					{
						min[i] = row.DataVector[i];
					}

					else if (row.DataVector[i] > max[i])
					{
						max[i] = row.DataVector[i];
					}
				}
			}

			//normalize each value
			foreach (DataInstance row in fullDataSet.DataInstances)
			{
				for (int i = 0; i < coloumns; i++)
				{
					//Console.WriteLine("BEFORE: {0}", row.DataVector[i]);
					row.DataVector[i] = ((row.DataVector[i] - min[i]) / (max[i] - min[i]));
					//Console.WriteLine("AFTER: {0}\n", row.DataVector[i]);
				}
			}
		}

		private List<List<DataInstance>> kFoldPackages = new List<List<DataInstance>>();

		private DataSet fullDataSet;

		/// <summary>
		/// Distribute all instances of data over "k" amount of data-blocks.
		/// The instances are distributed in a way that retains
		/// the relativity in category occurences found when observing the whole set.
		/// </summary>
		/// <param name="k">Number of Blocks to distribute data over.</param>
		private void PrepareKFoldCrossValidation(int k)
		{
			Random rndNumber = new Random();
			//copy to prevent damage to original list, when removing elements later.
			List<List<DataInstance>> sortedInstances = DataSet.CategoryInstances;

			for (int i = 0; i < k; i++)
			{
				kFoldPackages.Add(new List<DataInstance>());
			}

			foreach (var categoryList in sortedInstances)
			{
				int numberOfElements = categoryList.Count;
				int elementsPerBlock = numberOfElements / k;

				for (int i = 0; i < k; i++)
				{
					for (int y = 0; y < elementsPerBlock; y++)
					{
						int elementIndex = rndNumber.Next(0, numberOfElements - y);

						kFoldPackages[i].Add(categoryList[elementIndex]);
						categoryList.RemoveAt(elementIndex);
					}
					numberOfElements -= elementsPerBlock;
				}
			}
		}

		/*
    /// <summary>
    /// Display analysed information about the data set.
    /// </summary>
    public void outputDataSetInfo()
    {
      var sortedTestData = new SortedDictionary<string, List<List<string>>>(test);
      var sum = 0;
      foreach (var category in sortedTestData)
      {
        Console.WriteLine("Elements in \"" + category.Key + "\" : " + category.Value.Count);
        sum += category.Value.Count;
      }
      Console.WriteLine("Elements in sum: " + sum);
      _dataSum = sum;
      _differentClasses = test.Count;
    }

    private Dictionary<string, List<List<string>>> train;
    private Dictionary<string, List<List<string>>> test;
    private List<List<string>>[] blocks;
    private int _dataSum = 0;
    private int _differentClasses = 0;

    private int DifferentClasses
    {
      get
      {
        if (_differentClasses <= 0) _differentClasses = test.Count;
        return _differentClasses;
      }
    }

    private int DataSum
    {
      get
      {
        if (_dataSum <= 0)
        {
          var sum = 0;
          foreach (var el in test)
          {
            sum += el.Value.Count;
          }
          _dataSum = sum;
        }
        return _dataSum;
      }
    }
        */
	}
}
