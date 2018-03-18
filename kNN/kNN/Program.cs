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

            //create Dataset
            var dataSet = new DataSet();
            dataSet.ReadFile(dataFile);
			
            //start taking time
            var stopwatch = new Stopwatch();
            Console.WriteLine("start timer");
            stopwatch.Start();

			//Prepare Algorithm
            KnnAlgorithm kNNSearch = new KnnAlgorithm(dataSet);

			//Run kFoldCrossValidation
            kNNSearch.TestData();

			//stop taking time
            stopwatch.Stop();
            var ts = stopwatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}.{2:00}",
                ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            Console.WriteLine("time was: {0}", elapsedTime);

			//Draw ConfusionMatrix
            ConfusionMatrix ConsoleDrawer = new ConfusionMatrix(dataSet);
            ConsoleDrawer.PrintMatrix();
            Console.WriteLine(ConsoleDrawer.Accuracy + " is accuracy");
			
			//Wait for Program to be terminated by the user
            Console.Read();
        }
    }
}
