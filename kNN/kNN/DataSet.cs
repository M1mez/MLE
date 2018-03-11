using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace kNN
{
    class DataSet
    {
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

        public void outputDataSetInfo()
        {
            var sortedTestData = new SortedDictionary<string, List<List<string>>>(test);
            var sum = 0;
            foreach(var el in sortedTestData)
            {
                Console.WriteLine("Elements in \"" + el.Key + "\" : " + el.Value.Count);
                sum += el.Value.Count;
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
        private int DataSum {
            get {
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

        public DataSet(Dictionary<string, List<List<string>>> test, int k)
        {
            this.test = test;
            train = DeepClone(test);
            CreateKBlocks(k);
        }

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
    }
}
