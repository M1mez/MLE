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
            var DS = new DataSet();
            //Console.WriteLine(Constants.ChooseFile());
            FileHandling.ReadFile(isDebug);
            DataSet.PrintStructure();

           
            //OldAlgorithm oldAlgorithm = new OldAlgorithm();
            Algorithm algorithm = new Algorithm();

            algorithm.ID3(DataSet.RootNode, new DataBag(DataSet.Instances));

            PrintNode(DataSet.RootNode, 0);
            if (!isDebug) Console.Read();
        }

        private static void PrintNode(Node n, int level)
        {
            if (n.IsLeaf)
            {
                Console.WriteLine($"Attribute: {n.Attribute} Value: {n.originEdge} was Leaf on level: {level}");
                return;
            }
            Console.WriteLine($"Attribute: {n.Attribute} Value: {n.originEdge} level: {level}");
            n.paths.ForEach(path => PrintNode(path.Destination, level+1));
        }
    }
}
