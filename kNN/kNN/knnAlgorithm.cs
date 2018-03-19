using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace kNN
{
	class KnnAlgorithm
	{
		private List<List<DataInstance>> kFoldPackages = new List<List<DataInstance>>();
		private DataSet fullDataSet;

		public KnnAlgorithm(DataSet data)
		{
			fullDataSet = data;
			this.Normalize();

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
		    var taskList = new List<Thread>();
            foreach (var testblock in kFoldPackages)
            {
                taskList.Add(ComputeOneBlock(testblock));
            }

		    try
		    {
		        stopwatch.Start();
		        taskList.ForEach(x => x.Start());
		        taskList.ForEach(x => x.Join());
		        stopwatch.Stop();
            }
		    catch (Exception e)
		    {
		        Console.WriteLine(e);
		        Console.Read();
		    }
		}
        
        private Stopwatch stopwatch = new Stopwatch();
	    private int blockCount = 0;

	    public class DuplicateKeyComparer<TKey> : IComparer<TKey> where TKey : IComparable
	    {
	        public int Compare(TKey x, TKey y)
	        {
	            int result = x.CompareTo(y);

	            if (result == 0)
	                return 1;   // Handle equality as beeing greater
	            else
	                return result;
	        }
	    }

    private Thread ComputeOneBlock(List<DataInstance> testBlock)
	    {
	        var newCopy = new List<DataInstance>(fullDataSet.DataInstances).Except(testBlock).ToList();
	        var sortedCopy = new SortedList<float, DataInstance>(new DuplicateKeyComparer<float>());
	        return new Thread(() =>
	        {
	            foreach (var unknown in testBlock)
	            {
	                //var comp = new FloatListComparator(candidate);

	                //newCopy.Sort(comp);
	                foreach (var known in newCopy)
	                {
	                    sortedCopy.Add(FloatListComparator.SingleDist(known, unknown), unknown);
	                }
	                unknown.GuessedCategory = MostCommonCategorySorted(sortedCopy);
                }

	            var ts = stopwatch.Elapsed;
	            var elapsedTime = $"{ts.Minutes:00}:{ts.Seconds:00}.{ts.Milliseconds / 10:00}";
	            Console.WriteLine("Block {0} finished after: {1}", blockCount++, elapsedTime);
	        });
	    }

	    private int MostCommonCategorySorted(SortedList<float, DataInstance> list)
	    {
	        return MostCommonCategory(list.Values.ToList());

        }

	    //private Dictionary<int, int> mostListedCat = new Dictionary<int, int>();
        private int MostCommonCategory(List<DataInstance> list)
        {
            if (list.Count > 11) list = list.Take(11).ToList();
            var mostListedCat = new Dictionary<int, int>();
            list.ForEach(i =>
            {
                if (!mostListedCat.ContainsKey(i.TrueCategory)) mostListedCat[i.TrueCategory] = 1;
				//else?
				mostListedCat[i.TrueCategory]++;
            });
			//default? besser noch einen nächsten dazunehmen und danach beurteilen.
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
			double[] min = new double[coloumns];
			double[] max = new double[coloumns];

			//get min and max values for each coloumn
			foreach (DataInstance row in fullDataSet.DataInstances)
			{
				for (int i = 0; i < coloumns; i++)
				{
					if (row.fileDataVector[i] < min[i])
					{
						min[i] = row.fileDataVector[i];
					}

					else if (row.fileDataVector[i] > max[i])
					{
						max[i] = row.fileDataVector[i];
					}
				}
			}

			//normalize each value
			foreach (DataInstance row in fullDataSet.DataInstances)
			{
				for (int i = 0; i < coloumns; i++)
				{
					try
					{

					//Console.WriteLine("BEFORE: {0}", row.DataVector[i]);
					row.normalizedDataVector[i] = (float)((row.fileDataVector[i] - min[i]) / (max[i] - min[i]));
					//Console.WriteLine("AFTER: {0}\n", row.DataVector[i]);
					}
					catch (Exception e)
					{
						Console.WriteLine("Error parsing double value to float after normalizing data value {0}", e);
						row.normalizedDataVector[i] = 0.0F;
					}
				}
			}
		}

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
	}
}
