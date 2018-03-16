using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kNN
{
  class knnAlgorithm
  {
        public knnAlgorithm(DataSet data)
        {
            fullDataSet = data;
        }

        /// <summary>
        /// Mapps all values in fullDataSet between 0 and 1;
        /// Simple Rescaling is used.
        /// </summary>
        public void Normalize()
        {
            int coloumns = fullDataSet.ColoumnCount - 1;

            //min and max for each attribute coloumn.
            float[] min = new float[coloumns];
            float[] max = new float[coloumns];

            //get min and max values for each coloumn
            foreach (DataInstance row in fullDataSet.DataInstances)
            {
                for (int i = 0; i < coloumns; i++)
                {
                    if(row.DataVector[i] < min[i])
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
                    Console.WriteLine("BEFORE: {0}", row.DataVector[i]);
                    row.DataVector[i] = ((row.DataVector[i] - min[i]) / (max[i] - min[i]));
                    Console.WriteLine("AFTER: {0}\n", row.DataVector[i]);
				}
			}
        }


        private DataSet fullDataSet;
    /*
    /// <summary>
    /// Prouces an exact copy of an object.
    /// </summary>
    /// <returns>The Cloned Object</returns>
    /// <param name="obj">Any object one wishes to clone</param>
    private T DeepClone<T>(T obj)
    {
      T objResult;
      using (MemoryStream ms = new MemoryStream())
      {
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(ms, obj);
        ms.Position = 0;
        objResult = (T)bf.Deserialize(ms);
      }
      return objResult;
    }

    /// <summary>
    /// Distribute all instances of data over "k" amount of data-blocks.
    /// The instances are distributed in a way that retains
    /// the relativity in category occurences found when observing the whole set.
    /// </summary>
    /// <param name="k">Number of Blocks to distribute data over.</param>
    private void CreateKBlocks(int k)
    {
      if (train == null) throw new Exception("train Dataset was null!");
      if (test == null) throw new Exception("test Dataset was null!");
      outputDataSetInfo();
      int blockSize = DataSum / k;
      int perBlock = 0;
      int rnd;
      blocks = new List<List<string>>[k];

      Console.WriteLine("DataSum : " + DataSum + " k " + k + " blockSize: " + blockSize);

      foreach (var element in test)
      {
        perBlock = element.Value.Count / k;
        Console.WriteLine(" valueCount " + element.Value.Count + " perBlock: " + perBlock);
        for (var idx = 0; idx < k; idx++)
        {
          if (element.Value == null) continue;
          for (int per = 0; per < perBlock; per++)
          {
            rnd = Constants.randomInt(element.Value);
            if (blocks[idx] == null) blocks[idx] = new List<List<string>>() { element.Value[rnd] };
            else blocks[idx].Add(element.Value[rnd]);
            element.Value.RemoveAt(rnd);
          }
        }
      }
    }

    */
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
