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
            Printer.Print(DataSet.RootNode);
            //PrintNode(DataSet.RootNode, 0);
            if (!isDebug) Console.Read();
        }

        private static void PrintNode(Node n, int level)
        {
            if (n.IsLeaf)
            {
                Console.WriteLine($"Attribute: {DataSet.Attributes[n.PreviousAttribute].Name} " +
                                  $"Level: {DataSet.Attributes[n.PreviousAttribute].Values[n.OriginEdge]} " +
                                  $"was Leaf on level: {level} " +
                                  $"Qualifier: {DataSet.Attributes[DataSet.QualifierIndex].Values[n.Qualifier]}");
                return;
            }
            if (n.PreviousAttribute != -1)
            Console.WriteLine($"Attribute: {DataSet.Attributes[n.PreviousAttribute].Name} " +
                              $"Level: {DataSet.Attributes[n.PreviousAttribute].Values[n.OriginEdge]} " +
                              $"level: {level} " +
                              $"leads to: {DataSet.Attributes[n.Attribute].Name}");
            n.Paths.ForEach(path => PrintNode(path, level+1));
        }
    }
}
