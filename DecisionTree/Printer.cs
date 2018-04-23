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
        private static int _longestAttributeNameLength => Attributes.Aggregate("", (max, cur) => max.Length > cur.Name.Length ? max : cur.Name).Length;
        private static int _longestValueNameLength  {
            get
            {
                var longestValues = new List<int>();
                foreach (var attribute in Attributes)
                {
                    longestValues.Add(attribute.Values.Aggregate("", (max, cur) => 
                            max.Length > cur.Length ? max : cur)
                        .Length);
                }

                return longestValues.Max();
            }
        }
        private static List<string> qualifierList => Attributes[DataSet.QualifierIndex].Values;
        private static int _longestQualifier => qualifierList.Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur).Length;

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

        public static void BeautifulBorder(string toWrite, 
            ConsoleColor fcolor = ConsoleColor.White, 
            ConsoleColor bColor = ConsoleColor.Black)
        {
            Console.WriteLine($"╔{new string('═', toWrite.Length + 2)}╗");
            Console.Write("║ ");
            ConsoleColor oldfColor = Console.ForegroundColor;
            ConsoleColor oldbColor = Console.BackgroundColor;
            Console.ForegroundColor = fcolor;
            Console.BackgroundColor = bColor;
            Console.Write(toWrite);
            Console.ForegroundColor = oldfColor;
            Console.BackgroundColor = oldbColor;
            Console.WriteLine(" ║");
            Console.WriteLine($"╚{new string('═', toWrite.Length + 2)}╝");
        }


        public static void  WriteInStyle(string toWrite, 
            ConsoleColor foreground, 
            ConsoleColor backGround = ConsoleColor.Black)
        {
            var oldForeColor = Console.ForegroundColor;
            var oldBackColor = Console.BackgroundColor;

            Console.ForegroundColor = foreground;
            Console.BackgroundColor = backGround;

            Console.Write(toWrite);

            Console.ForegroundColor = oldForeColor;
            Console.BackgroundColor = oldBackColor;
        }

        public static void PrintLikelihoodTable(Dictionary<int, Dictionary<int, Dictionary<int, KeyValuePair<int, int>>>> tables, int dataSetCount)
        {
            var longestAttribute = _longestAttributeNameLength + 3;
            var longestValue = _longestValueNameLength + 4;
            var attPlusVal = longestValue + longestAttribute;
            var longestQualifier = _longestQualifier + 12;

            BeautifulBorder("Likelihood Tables");
            foreach (var attribute in tables)
            {
                var qualifierCount = new int[qualifierList.Count].ToList();
                Console.WriteLine();
                WriteInStyle($"{GetName(attribute.Key)}".PadRight(longestAttribute),
                    ConsoleColor.DarkRed);
                Console.Write(new string(' ', longestValue));
                foreach (var qualifier in qualifierList)
                {
                    WriteInStyle(qualifier.PadRight(longestQualifier), 
                        ConsoleColor.DarkMagenta);
                }
                
                //WriteInStyle("P(X)", ConsoleColor.DarkBlue);
                foreach (var value in attribute.Value)
                {
                    var fractionLlist = value.Value;
                    Console.WriteLine();
                    Console.Write(new String(' ', longestAttribute));
                    WriteInStyle($"{GetName(attribute.Key, value.Key)}".PadRight(longestValue), 
                        ConsoleColor.DarkMagenta);

                    var pxCount = 0;
                    foreach (var fractionPair in fractionLlist)
                    {
                        var pair = fractionPair.Value;
                        Console.Write($"{pair.Key}/{pair.Value} - ");
                        WriteFraction(pair, longestQualifier -7);
                        pxCount += pair.Key;
                        qualifierCount[fractionPair.Key] += pair.Key;
                    }
                    var PxPair = new KeyValuePair<int, int>(pxCount, dataSetCount);
                    WriteFraction(PxPair, 0, ConsoleColor.DarkBlue);
                }
                Console.WriteLine('\n');
                //Console.Write(new String(' ', longestAttribute));
                //WriteInStyle($"P(C)", ConsoleColor.DarkBlue);
                Console.Write(new string(' ', attPlusVal));
                for (var index = 0; index < qualifierList.Count; index++)
                {
                    WriteFraction(new KeyValuePair<int, int>(qualifierCount[index], dataSetCount), longestQualifier, ConsoleColor.DarkBlue);
                }


                Console.WriteLine();
            }
        }

        private static void WriteFraction(KeyValuePair<int, int> fractionPair, 
            int padding, 
            ConsoleColor color = ConsoleColor.DarkGreen)
        {
            WriteInStyle((Math.Round(((float)fractionPair.Key / fractionPair.Value) * 100) / 100).ToString()
                .PadRight(padding),
                color);
        }
    }
}
