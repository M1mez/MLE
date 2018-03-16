using System;
using System.Collections.Generic;
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
            string FileName = "winequality-white.csv";
			//string FileName = "iris.data.txt";

            // Place data files insinde the build directory
            string DataFile = Path.Combine(Directory.GetCurrentDirectory(), FileName);

            //Read data
            var sortedCsv = CSVHandle.Read(DataFile);

            //create Dataset
            var dataSet = new DataSet(sortedCsv, 5);

            //var csvList = CSVHandle.GetSortedCSV(@"O:\FH\MLE\iris.data.csv");
            DataSet.Normalize();

            //Normalize Data.



            Console.WriteLine("That many different Types: " + sortedCsv.Count);
            Console.Read();
        }
    }
}
