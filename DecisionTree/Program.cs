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
            DataSet.PrintStructure();
            
            Algorithm algorithm = new Algorithm();

            algorithm.ID3(DataSet.RootNode, new DataBag(DataSet.Instances));

            PrintNode(DataSet.RootNode, 0);
            if (!isDebug) Console.Read();
        }

        private static void PrintNode(Node n, int level)
        {
            if (n.IsLeaf)
            {
                Console.WriteLine($"Attribute: {DataSet.Attributes[n.PreviousAttribute].Name} " +
                                  $"Value: {DataSet.Attributes[n.PreviousAttribute].Values[n.originEdge]} " +
                                  $"was Leaf on level: {level} " +
                                  $"Qualifier: {DataSet.Attributes[DataSet.QualifierIndex].Values[n.EndQualifier]}");
                return;
            }
            if (n.PreviousAttribute != -1)
            Console.WriteLine($"Attribute: {DataSet.Attributes[n.PreviousAttribute].Name} " +
                              $"Value: {DataSet.Attributes[n.PreviousAttribute].Values[n.originEdge]} " +
                              $"level: {level}");
            n.paths.ForEach(path => PrintNode(path.Destination, level+1));
        }

        private int _longestAttribute;
        private int _longestValue;
        private int _longestQualifier;
    }
}
