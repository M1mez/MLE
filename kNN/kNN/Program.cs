using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace kNN
{
    class Program
    {
        static void Main(string[] args)
        {
            //string FileName = "winequality-white.csv";
            string FileName = "iris.data.txt";

            // Place data files insinde the build directory
            string dataFile = Path.Combine(Directory.GetCurrentDirectory(), FileName);

            //Read data
            var sortedCsv = CSVHandle.Read(dataFile);

            //create Dataset
            var dataSet = new DataSet();
            dataSet.ReadFile(dataFile);

            //var csvList = CSVHandle.GetSortedCSV(@"O:\FH\MLE\iris.data.csv");

            //take time
            var stopwatch = new Stopwatch();
            Console.WriteLine("start timer");
            KnnAlgorithm kNNSearch = new KnnAlgorithm(dataSet);
            stopwatch.Start();
            kNNSearch.TestData();
            stopwatch.Stop();
            var ts = stopwatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}",
                ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            Console.WriteLine("time was: {0}", elapsedTime);

            ConfusionMatrix ConsoleDrawer = new ConfusionMatrix(dataSet);
            ConsoleDrawer.PrintMatrix();
            Console.WriteLine(ConsoleDrawer.Accuracy + " is accuracy");

            //Console.WriteLine("That many different Types: " + sortedCsv.Count);
            Console.Read();
        }
    }
}
