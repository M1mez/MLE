using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kNN
{
    class Program
    {
        static void Main(string[] args)
        {
            var csvList = CSVHandle.GetSortedCSV(@"O:\FH\MLE\winequality-white.csv");
            //var csvList = CSVHandle.GetSortedCSV(@"O:\FH\MLE\iris.data.csv");

            var dataSet = new DataSet(csvList, 15);

            Console.WriteLine("That many different Types: " + csvList.Count);
            Console.Read();
        }
    }
}
