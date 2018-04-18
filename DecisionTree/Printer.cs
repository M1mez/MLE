using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree
{
    static class Printer
    {
        private static List<Attribute> Attributes => DataSet.Attributes;
        
        public static void Print(Node n, int level = 0, bool isLastPath = true)
        {
            for (var index = 0; index <= level; index++)
            {
                if (index == level)
                    if (isLastPath)
                    {
                        WasLastPath[level] = true;
                        Console.Write("└");
                    }
                    else Console.Write("├");
                else
                if (WasLastPath[index]) Console.Write(" ");
                else Console.Write("│");
                Console.Write(new string(' ', LevelWidths[index]));
            }

            string name;
            if (n.PreviousAttribute >= 0)
            {
                name = GetName(n.PreviousAttribute, n.OriginEdge);
                Console.Write($"({name}) -> ");
            }

            if (n.IsLeaf)
            {
                Console.WriteLine($"{GetName(DataSet.QualifierIndex)}: {GetName(DataSet.QualifierIndex, n.Qualifier)}");
            }
            else
            {
                name = GetName(n.Attribute);
                LevelWidths[level] = name.Length;
                if (n.PreviousAttribute >= 0) LevelWidths[level] += 10;
                Console.WriteLine($"{name} ");
            }


            for (var index = 0; index < n.Paths.Count; index++)
            {
                LevelWidths[level + 1] = 0;
                var nextNode = n.Paths[index];
                if (index == DataSet.Attributes[n.Attribute].ValueCount - 1)
                {
                    Print(nextNode, level + 1, true);
                }
                else Print(nextNode, level + 1, false);
            }
        }
        private static List<bool> _wasLastPath;
        private static List<bool> WasLastPath
        {
            get
            {
                if (_wasLastPath == null)
                {
                    _wasLastPath = new bool[DataSet.MaxLevel].ToList();
                    _wasLastPath[0] = true;
                }

                return _wasLastPath;
            }
        }
        private static List<int> _levelWidths;
        private static List<int> LevelWidths =>
            _levelWidths ?? (_levelWidths = new int[DataSet.MaxLevel].ToList());

        public static string GetName(int attributeIndex, int valueIndex = -1) => valueIndex < 0 ? DataSet.Attributes[attributeIndex].Name : DataSet.Attributes[attributeIndex].Values[valueIndex];

        public static void BeautifulBorder(string toWrite)
        {
            Console.WriteLine($"╔{new string('═', toWrite.Length + 2)}╗");
            Console.WriteLine("║ " + toWrite + " ║");
            Console.WriteLine($"╚{new string('═', toWrite.Length + 2)}╝");
        }
    }

}
