using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(Constants.ChooseFile());
            var DS = new DataSet();
            FileHandling.ReadFile(ref DS);
            Console.Read();
        }
    }
}
