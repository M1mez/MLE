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
            bool isDebug = false;
            FileHandling.ReadFile(isDebug);
            //DataSet.PrintSimpleStructure();
            
            Algorithm algorithm = new Algorithm();

            Algorithm.ID3(DataSet.RootNode, new DataBag(DataSet.Instances));
            Printer.BeautifulBorder("ID3:");
            Printer.Print(DataSet.RootNode);
            //PrintNode(DataSet.RootNode, 0);
            var b = new Bayes();
            var con = new ConfusionMatrix();
            Printer.BeautifulBorder("Naïve Bayes:");
            con.PrintMatrix();

            if (!isDebug) Console.Read();
        }
        
    }
}
